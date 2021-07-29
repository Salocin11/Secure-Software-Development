using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Authorization;
using Philip.Data;
using Philip.Models;

namespace Philip.Pages.Articles
{
    [Authorize(Roles = "Admin,Member")]
    public class CreateModel : PageModel
    {
        private readonly Philip.Data.PhilipContext _context;

        public CreateModel(Philip.Data.PhilipContext context)
        {
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Article Article { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            _context.Article.Add(Article);
            //await _context.SaveChangesAsync();

            // Once a record is added, create an audit record
            if (await _context.SaveChangesAsync() > 0)
            {
                // Create an auditrecord object
                var auditrecord = new AuditRecord();
                auditrecord.AuditActionType = "Add Article";
                auditrecord.DateTimeStamp = DateTime.Now;
                auditrecord.KeyPostFieldID = Article.ID;
                // Get current logged-in user
                var userID = User.Identity.Name.ToString();
                auditrecord.Username = userID;
                auditrecord.OldValue = "";
                auditrecord.NewValue = "Title: " + Article.Title +
                                       "\r\n --------Author :" + Article.Author +
                                       "\r\n --------Release Date :" + Article.ReleaseDate +
                                       "\r\n --------Content: " + Article.Content;

                _context.AuditRecords.Add(auditrecord);
                await _context.SaveChangesAsync();
            }


            return RedirectToPage("./Index");
        }
    }
}
