using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.Extensions.Logging;
using Philip.Models;

namespace Philip.Areas.Identity.Pages.Account
{
    [AllowAnonymous]
    public class LogoutModel : PageModel
    {
        private readonly SignInManager<ApplicationUser> _signInManager;
        private readonly ILogger<LogoutModel> _logger;
        private readonly Philip.Data.PhilipContext _context;
        public LogoutModel(SignInManager<ApplicationUser> signInManager, 
            ILogger<LogoutModel> logger, 
            Philip.Data.PhilipContext context)
        {
            _signInManager = signInManager;
            _logger = logger;
            _context = context;
        }

        public void OnGet()
        {
        }

        public async Task<IActionResult> OnPost(string returnUrl = null)
        {
            await _signInManager.SignOutAsync();
            
            // Create an auditrecord object
            var auditrecord = new AuditRecord();
            auditrecord.AuditActionType = "Logout";
            auditrecord.DateTimeStamp = DateTime.Now;
            auditrecord.KeyPostFieldID = 913;
            // Get current logged-in user
            var userID = User.Identity.Name.ToString();
            auditrecord.Username = userID;

            //add logs to audit record
            _context.AuditRecords.Add(auditrecord);
            await _context.SaveChangesAsync();
            
            _logger.LogInformation("User logged out.");
            if (returnUrl != null)
            {
                return LocalRedirect(returnUrl);
            }
            else
            {
                return RedirectToPage();
            }
        }
    }
}
