using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Philip.Data;
using Philip.Models;

namespace Philip.Pages.ReportVuln
{
    public class DeleteModel : PageModel
    {
        private readonly Philip.Data.PhilipContext _context;

        public DeleteModel(Philip.Data.PhilipContext context)
        {
            _context = context;
        }

        [BindProperty]
        public Feedback Feedback { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Feedback = await _context.Feedback.FirstOrDefaultAsync(m => m.ID == id);

            if (Feedback == null)
            {
                return NotFound();
            }
            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Feedback = await _context.Feedback.FindAsync(id);

            if (Feedback != null)
            {
                _context.Feedback.Remove(Feedback);

                if (await _context.SaveChangesAsync() > 0)
                {
                    // Create an auditrecord object
                    var auditrecord = new AuditRecord();
                    auditrecord.AuditActionType = "Delete Vulnerability FeedBack";
                    auditrecord.DateTimeStamp = DateTime.Now;
                    auditrecord.KeyPostFieldID = Feedback.ID;
                    // Get current logged-in user
                    var userID = User.Identity.Name.ToString();
                    auditrecord.Username = userID;
                    auditrecord.OldValue = "Title: " + Feedback.Title +
                                           "\r\n --------Email :" + Feedback.Email +
                                           "\r\n --------Content: " + Feedback.Content;

                    _context.AuditRecords.Add(auditrecord);
                    await _context.SaveChangesAsync();
                }
            }

            return RedirectToPage("./Index");
        }
    }
}
