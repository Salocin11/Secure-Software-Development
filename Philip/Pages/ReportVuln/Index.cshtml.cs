using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Philip.Data;
using Philip.Models;

namespace Philip.Pages.ReportVuln
{
    public class IndexModel : PageModel
    {
        private readonly Philip.Data.PhilipContext _context;

        public IndexModel(Philip.Data.PhilipContext context)
        {
            _context = context;
        }

        public IList<Feedback> Feedback { get;set; }

        public async Task OnGetAsync()
        {
            Feedback = await _context.Feedback.ToListAsync();
        }
    }
}
