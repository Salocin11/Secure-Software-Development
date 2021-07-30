using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Philip.Models;
using Microsoft.AspNetCore.Mvc;

namespace Philip.Areas.Identity.Pages.Account
{
    
    public class AccessDeniedModel : PageModel
    {
        private readonly Philip.Data.PhilipContext _context;

        public AccessDeniedModel(Philip.Data.PhilipContext context)
        {
            _context = context;
        }

        public async Task<IActionResult> OnGetAsync()
        {        
            // Create an auditrecord object
            var auditrecord = new AuditRecord();
            auditrecord.AuditActionType = "Access Denied";
            auditrecord.DateTimeStamp = DateTime.Now;
            auditrecord.KeyPostFieldID = 999401;
            // Get email of user logging in 
            var userID = User.Identity.Name.ToString();
            auditrecord.Username = userID;

            _context.AuditRecords.Add(auditrecord);
            await _context.SaveChangesAsync();
            return null;
        }
    }
}

