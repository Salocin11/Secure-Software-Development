using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Mvc.Rendering;
using Philip.Data;
using Philip.Models;
using Microsoft.AspNetCore.Authorization;

namespace Philip.Pages.Audit
{
    [Authorize(Roles = "Admin")]
    public class IndexModel : PageModel
    {
        private readonly Philip.Data.PhilipContext _context;

        public IndexModel(Philip.Data.PhilipContext context)
        {
            _context = context;
        }

        public IList<AuditRecord> AuditRecord { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public SelectList Audits { get; set; }
        [BindProperty(SupportsGet = true)]
        public string AuditAction { get; set; }
        public async Task OnGetAsync()
        {
            var audits = from m in _context.AuditRecords
                         select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                audits = audits.Where(s => s.AuditActionType.Contains(SearchString));
            }
            AuditRecord = await _context.AuditRecords.ToListAsync();

            // Create an auditrecord object
            var auditrecord = new AuditRecord();
            auditrecord.AuditActionType = "View Audit Logs";
            auditrecord.DateTimeStamp = DateTime.Now;
            auditrecord.KeyPostFieldID = 999;
            // Get email of user logging in 
            var userID = User.Identity.Name.ToString();
            auditrecord.Username = userID;

            _context.AuditRecords.Add(auditrecord);
            await _context.SaveChangesAsync();
        }
    }
   
}
