using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Philip.Models;
namespace Philip.Pages.Roles
{
    public class DeleteModel : PageModel
    {
        private readonly RoleManager<ApplicationRole> _roleManager;
        private readonly Philip.Data.PhilipContext _context;
        public DeleteModel(RoleManager<ApplicationRole> roleManager,
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
        public async Task<IActionResult> OnPostAsync(string id)
        {
            if (id == null)
            {
                return NotFound();
            }
            ApplicationRole = await _roleManager.FindByIdAsync(id);
            IdentityResult roleRuslt = await _roleManager.DeleteAsync(ApplicationRole);

            // Create an auditrecord object
            var auditrecord = new AuditRecord();
            auditrecord.AuditActionType = "Delete Role";
            auditrecord.DateTimeStamp = DateTime.Now;
            auditrecord.KeyPostFieldID = 922;
            // Get current logged-in user
            var userID = User.Identity.Name.ToString();
            auditrecord.Username = userID;
            auditrecord.OldValue = oldval;

            _context.AuditRecords.Add(auditrecord);
            await _context.SaveChangesAsync();

            return RedirectToPage("./Index");
        }
    }
}