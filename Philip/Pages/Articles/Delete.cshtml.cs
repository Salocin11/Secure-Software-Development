using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;
using Philip.Data;
using Philip.Models;
using Microsoft.AspNetCore.Authorization;

namespace Philip.Pages.Articles
{
    [Authorize(Roles = "Admin")]
    public class DeleteModel : PageModel
    {
        private readonly Philip.Data.PhilipContext _context;

        public DeleteModel(Philip.Data.PhilipContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Article Article { get; set; }
        public string ConcurrencyErrorMessage { get; set; }
        static string oldval { get; set; }

        public async Task<IActionResult> OnGetAsync(int id, bool? concurrencyError)
        {
            Article = await _context.Article
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            oldval = "Title: " + Article.Title +
                "\r\n --------Author :" + Article.Author +
                "\r\n --------Release Date :" + Article.ReleaseDate +
                "\r\n --------Content: " + Article.Content;
            if (Article == null)
            {
                return NotFound();
            }
            if (concurrencyError.GetValueOrDefault())
            {
                ConcurrencyErrorMessage = "The record you attempted to delete "
                  + "was modified by another user after you selected delete. "
                  + "The delete operation was canceled and the current values in the "
                  + "database have been displayed. If you still want to delete this "
                  + "record, click the Delete button again.";

            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {
            try
            {
                if (await _context.Article.AnyAsync(m => m.ID == id))
                {
                    if (Article != null)
                    {
                        _context.Article.Remove(Article);
                        //await _context.SaveChangesAsync();

                        // Once a record is deleted, create an audit record
                        if (await _context.SaveChangesAsync() > 0)
                        {
                            // Create an auditrecord object
                            var auditrecord = new AuditRecord();
                            auditrecord.AuditActionType = "Delete Post Record";
                            auditrecord.DateTimeStamp = DateTime.Now;
                            auditrecord.KeyPostFieldID = Article.ID;
                            // Get current logged-in user
                            var userID = User.Identity.Name.ToString();
                            auditrecord.Username = userID;
                            auditrecord.OldValue = oldval;
                            auditrecord.NewValue = "";
                            _context.AuditRecords.Add(auditrecord);
                            await _context.SaveChangesAsync();
                        }
                        
                    }
                }
               
                return RedirectToPage("./Index");
            }
            catch (DbUpdateConcurrencyException)
            {
                return RedirectToPage("./Delete",
                    new { concurrencyError = true, id = id });
            }
            
        }
    }
}
