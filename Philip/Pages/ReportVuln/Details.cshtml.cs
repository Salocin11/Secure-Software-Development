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
    public class DetailsModel : PageModel
    {
        private readonly Philip.Data.PhilipContext _context;

        public DetailsModel(Philip.Data.PhilipContext context)
        {
            _context = context;
        }

        public Feedback Feedback { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Feedback = await _context.Feedback.FirstOrDefaultAsync(m => m.ID == id);

            if (Feedback == null)
            {
                return NotFound();
            }
            return Page();
        }
    }
}
