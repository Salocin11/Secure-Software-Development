using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Philip.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;

namespace Philip.Pages.Comments
{
    [Authorize]
    public class GiveCommentModel : PageModel
    {
 
    private readonly Philip.Data.PhilipContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public GiveCommentModel(Philip.Data.PhilipContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        [BindProperty]
        public FinalComment Comment { get; set; }
        public Article Article { get; set; }
        public int Id { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Id = (int)id;

            Article = await _context.Article.FirstOrDefaultAsync(m => m.ID == id);
            if (Article == null)
            {
                return NotFound();
            }

            var user = await _userManager.GetUserAsync(User);

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }

            
            _context.FinalComment.Add(Comment);
            await _context.SaveChangesAsync();
            return RedirectToPage("../Articles/Details/", new { id = id });
        }
    }
}