using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Philip.Data;
using Philip.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;


namespace Philip.Pages.Articles
{
    public class DetailsModel : PageModel
    {
        private readonly Philip.Data.PhilipContext _context;
        private readonly UserManager<ApplicationUser> _userManager;

        public DetailsModel(Philip.Data.PhilipContext context)
        {
            _context = context;
        }


        public Article Article { get; set; }
        public bool isAuth { get; set; }
        public FinalComment Comment { get; set; }
        public IList<FinalComment> Comments { get; set; }
        public bool Comentable { get; set; }

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

          

            try
            {
                Comment = await _context.FinalComment.FirstOrDefaultAsync(m => m.ArticleID == id);
                if (Comment == null)
                {
                    FinalComment NullComment = new FinalComment
                    {
                        CommentID = 0,
                        ArticleID = (int)id,
                        CommentName = "No Comments",
                        CommentWords = "No Comments"
                    };
                    Comments = new List<FinalComment> { NullComment };
                }
                else
                {
                    var commentQuery = from s in _context.FinalComment
                                      where s.ArticleID == Article.ID
                                      select s;
                    Comments = await commentQuery.ToListAsync();
                }
            }
            catch
            {
                FinalComment NullComment = new FinalComment
                {
                    CommentID = 0,
                    ArticleID = (int)id,
                    CommentName = "No Comments",
                    CommentWords = "No Comments"
                };
                Comments = new List<FinalComment> { NullComment };
            }

            //if (User.IsInRole("Staff"))
            //{
            //    isAuth = true;
            //}
            return Page();
        }
    }
}