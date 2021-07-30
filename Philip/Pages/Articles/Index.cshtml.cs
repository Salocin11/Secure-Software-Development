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

namespace Philip.Pages.Articles
{
    public class IndexModel : PageModel
    {
        private readonly Philip.Data.PhilipContext _context;

        public IndexModel(Philip.Data.PhilipContext context)
        {
            _context = context;
        }

        public IList<Article> Article { get;set; }
        [BindProperty(SupportsGet = true)]
        public string SearchString { get; set; }
        public SelectList Title { get; set; }
        [BindProperty(SupportsGet = true)]
        public string ArticleTitle { get; set; }
        public async Task OnGetAsync()
        {
            var articles = from m in _context.Article
                         select m;
            if (!string.IsNullOrEmpty(SearchString))
            {
                articles = articles.Where(s => s.Title.Contains(SearchString));
            }

            Article = await articles.ToListAsync();
        }
    }
}
