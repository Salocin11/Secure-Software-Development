using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Authorization;
using Philip.Data;
using Philip.Models;

namespace Philip.Pages.Roles
{
    [Authorize(Roles = "Admin")]
    public class CreateModel : PageModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly Philip.Data.PhilipContext _context;

        public CreateModel(RoleManager<ApplicationRole> roleManager,
            Philip.Data.PhilipContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public ApplicationRole ApplicationRole { get; set; }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }

            ApplicationRole.CreatedDate = DateTime.UtcNow;

            IdentityResult roleResult = await _roleManager.CreateAsync(ApplicationRole);

            // Create an auditrecord object
            var auditrecord = new AuditRecord();
            auditrecord.AuditActionType = "Create Role";
            auditrecord.DateTimeStamp = DateTime.Now;
            auditrecord.KeyPostFieldID = 921;
            // Get current logged-in user
            var userID = User.Identity.Name.ToString();
            auditrecord.Username = userID;
            auditrecord.NewValue = "Role Name: " + ApplicationRole.Name +
                                   "\r\n --------Description :" + ApplicationRole.Description;

            _context.AuditRecords.Add(auditrecord);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}
