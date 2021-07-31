using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Philip.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;

namespace Philip.Pages.Comments
{
    [Authorize]
    public class ManageCommentsModel : PageModel
    {
        private readonly Philip.Data.PhilipContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ManageCommentsModel(Philip.Data.PhilipContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public Article Article { get; set; }
        public FinalComment Comment { get; set; }
        public IList<FinalComment> Comments { get; set; }
        public bool deleteable { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Article.FirstOrDefaultAsync(m => m.ID == id);
            if (Article == null)
            {
                return NotFound();
            }

            Comment = await _context.FinalComment.FirstOrDefaultAsync(m => m.CommentID == id);
            deleteable = true;
            if (Article == null)
            {
                FinalComment NullComment = new FinalComment
                {
                    CommentID = 0,
                    ArticleID = (int)id,
                    CommentName = "No Comments",
                    CommentWords = "No Comments"
                };
                deleteable = false;
                List<FinalComment> Commentss = new List<FinalComment> { NullComment };
            }
            else
            {
                var Commentss = from s in _context.FinalComment
                                select s;
                Commentss = (IQueryable<FinalComment>)await Commentss.ToListAsync();
                deleteable = true;
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int commentId, int articleId)
        {
            Article = await _context.Article.FirstOrDefaultAsync(m => m.ID == articleId);
            if (Article == null)
            {
                return Page();
            }

            Article = await _context.Article.FirstOrDefaultAsync(m => m.ID == articleId);
            if (Article == null)
            {
                return Page();
            }
            _context.FinalComment.Remove(Comment);
            await _context.SaveChangesAsync();


            return RedirectToPage("ManageComments", new { id = commentId });
        }
    }
}