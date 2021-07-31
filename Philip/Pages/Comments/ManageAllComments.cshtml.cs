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
    public class ManageAllCommentsModel : PageModel
    {
        private readonly Philip.Data.PhilipContext _context;
        private readonly UserManager<ApplicationUser> _userManager;
        public ManageAllCommentsModel(Philip.Data.PhilipContext context, UserManager<ApplicationUser> userManager)
        {
            _context = context;
            _userManager = userManager;
        }
        public Article Article { get; set; }
        public List<Article> Articles { get; set; }
        public FinalComment Comment { get; set; }
        public List<FinalComment> Comments { get; set; }
        public List<List<FinalComment>> CommentsList { get; set; }
        public List<bool> IsDeleteableList { get; set; }
        public bool IsDisplayable { get; set; }
        public async Task<IActionResult> OnGetAsync()
        {

            
            if (Articles == null)
            {
                IsDisplayable = true;
                return RedirectToPage("../Manage");
            }

            IsDeleteableList = new List<bool>();
            CommentsList = new List<List<FinalComment>>();
            for (int i = 0; i < Articles.Count; i++)
            {
                Comment = await _context.FinalComment.FirstOrDefaultAsync(m => m.ArticleID == Articles[i].ID);
                IsDeleteableList.Add(true);
                if (Comment == null)
                {
                    FinalComment NullComment = new FinalComment
                    {
                        CommentID = 0,
                        ArticleID = Articles[i].ID,
                        CommentName = "No Reviews",
                        CommentWords = "No Reviews"
                    };
                    IsDeleteableList[i] = false;
                    Comments = new List<FinalComment> { NullComment };
                    CommentsList.Add(Comments);
                }
                else
                {
                    var CommentsQ = from s in _context.FinalComment
                                   where s.ArticleID == Articles[i].ID
                                   select s;
                    CommentsList.Add(CommentsQ.ToList());
                }
            }

            return Page();
        }

        public async Task<IActionResult> OnPostAsync(int commentId, int articleId)
        {
            Article = await _context.Article.FirstOrDefaultAsync(m => m.ID == articleId);
            if (Article == null)
            {
                IsDisplayable = true;
                return RedirectToPage("../Manage");
            }

            Comment = await _context.FinalComment.FirstOrDefaultAsync(m => m.CommentID == commentId);
            if (Comment == null)
            {
                return Page();
            }
            _context.FinalComment.Remove(Comment);
            await _context.SaveChangesAsync();

     
            return RedirectToPage("../Comments/ManageAllComments");
        }
    }
}