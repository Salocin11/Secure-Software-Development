using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Philip.Data;
using Philip.Models;
using AspNetCoreHero.ToastNotification.Abstractions;



namespace Philip.Pages.ReportVuln
{

    public class CreateModel : PageModel
    {

        private readonly Philip.Data.PhilipContext _context;
        private readonly INotyfService _notyf;

        public CreateModel(Philip.Data.PhilipContext context, INotyfService notyf)
        {
            _context = context;
            _notyf = notyf;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Feedback Feedback { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            _notyf.Success("Submission was successful!");
            _context.Feedback.Add(Feedback);
            //await _context.SaveChangesAsync();

            // Once a record is added, create an audit record
            if (await _context.SaveChangesAsync() > 0)
            {
                // Create an auditrecord object
                var auditrecord = new AuditRecord();
                auditrecord.AuditActionType = "Add Vulnerability Record";
                auditrecord.DateTimeStamp = DateTime.Now;
                auditrecord.KeyPostFieldID = Feedback.ID;
                auditrecord.Username = Feedback.Email;
                auditrecord.OldValue = "";
                auditrecord.NewValue = "Title: " + Feedback.Title +
                                       "\r\n --------Email :" + Feedback.Email +
                                       "\r\n --------Content: " + Feedback.Content;

                _context.AuditRecords.Add(auditrecord);
                await _context.SaveChangesAsync();
            }

            return RedirectToPage("/Privacy");
        }

    }
    
}
