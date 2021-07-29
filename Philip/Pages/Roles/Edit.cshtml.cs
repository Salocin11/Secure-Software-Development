using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Philip.Models;
namespace Philip.Pages.Roles
{
    public class EditModel : PageModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly Philip.Data.PhilipContext _context;
        public EditModel(RoleManager<ApplicationRole> roleManager,
            Philip.Data.PhilipContext context)
        {
            _roleManager = roleManager;
            _context = context;
        }

        [BindProperty]
        public ApplicationRole ApplicationRole { get; set; }
        static string oldval { get; set; }
        public async Task<IActionResult> OnGetAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationRole = await _roleManager.FindByIdAsync(id);

            oldval = "Name: " + ApplicationRole.Name +
                     "\r\n --------Description :" + ApplicationRole.Description;

            if (ApplicationRole == null)
            {
                return NotFound();
            }
            return Page();
        }
        public async Task<IActionResult> OnPostAsync()
        {
            if (!ModelState.IsValid)
            {
                return Page();
            }
            ApplicationRole appRole = await _roleManager.FindByIdAsync(ApplicationRole.Id);
            appRole.Id = ApplicationRole.Id;
            appRole.Name = ApplicationRole.Name;
            appRole.Description = ApplicationRole.Description;
            IdentityResult roleRuslt = await _roleManager.UpdateAsync(appRole);

            // Create an auditrecord object
            var auditrecord = new AuditRecord();
            auditrecord.AuditActionType = "Edit Role";
            auditrecord.DateTimeStamp = DateTime.Now;
            auditrecord.KeyPostFieldID = 923;
            // Get current logged-in user
            var userID = User.Identity.Name.ToString();
            auditrecord.Username = userID;
            auditrecord.OldValue = oldval;
            auditrecord.NewValue = "Name: " + ApplicationRole.Name +
                                   "\r\n --------Description :" + ApplicationRole.Description;

            _context.AuditRecords.Add(auditrecord);
            await _context.SaveChangesAsync();
            if (roleRuslt.Succeeded)
            {
                return RedirectToPage("./Index");
            }
            return RedirectToPage("./Index");
        }
    }
}
