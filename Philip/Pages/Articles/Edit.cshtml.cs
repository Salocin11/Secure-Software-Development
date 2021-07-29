using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Authorization;
using Philip.Data;
using Philip.Models;

namespace Philip.Pages.Articles
{
    [Authorize(Roles = "Admin")]
    public class EditModel : PageModel
    {
        private readonly Philip.Data.PhilipContext _context;

        public EditModel(Philip.Data.PhilipContext context)
        {
            _context = context;
            //if (_context.AuditRecords.Find(i => i.Username == _context.AuditRecords.Find(i => i.)))
        }

        [BindProperty]
        public Article Article { get; set; }
        //changes here
        static string oldval { get; set; }

        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id == null)
            {
                return NotFound();
            }

            Article = await _context.Article
                .AsNoTracking()
                .FirstOrDefaultAsync(m => m.ID == id);

            //get old value of article for audit logs
            oldval = "Title: " + Article.Title + 
                "\r\n --------Author :" + Article.Author +
                "\r\n --------Release Date :" + Article.ReleaseDate + 
                "\r\n --------Content: " + Article.Content;

            if (Article == null)
            {
                return NotFound();
            }
            return Page();

            
        }

        // To protect from overposting attacks, enable the specific properties you want to bind to, for
        // more details, see https://aka.ms/RazorPagesCRUD.

        public async Task<IActionResult> OnPostAsync(int id)
        {

            if (!ModelState.IsValid)
            {
                return Page();
            }
            var articleToUpdate = await _context.Article
                .FirstOrDefaultAsync(m => m.ID == id);
            if (articleToUpdate == null)
            {
                return HandleDeletedArticle();
            }

            _context.Entry(articleToUpdate)
                .Property("RowVersion").OriginalValue = Article.RowVersion;

            if (await TryUpdateModelAsync<Article>(
                articleToUpdate,
                "Article",
                s => s.Title, s => s.Author, s => s.ReleaseDate, s => s.Content))
            {
                try
                {
                    //await _context.SaveChangesAsync();

                    // Once a record is edited, create an audit record
                    if (await _context.SaveChangesAsync() > 0)
                    {
                        var auditrecord = new AuditRecord();
                        auditrecord.AuditActionType = "Edit Post Record";
                        auditrecord.DateTimeStamp = DateTime.Now;
                        auditrecord.KeyPostFieldID = Article.ID;
                        var userID = User.Identity.Name.ToString();
                        auditrecord.Username = userID;
                        //snapshot of old value and new value
                        auditrecord.OldValue = oldval;
                        auditrecord.NewValue = "Title: " + Article.Title +
                                               "\r\n --------Author :" + Article.Author +
                                               "\r\n --------Release Date :" + Article.ReleaseDate +
                                               "\r\n --------Content: " + Article.Content;
                        _context.AuditRecords.Add(auditrecord);
                        await _context.SaveChangesAsync();
                    }
                    return RedirectToPage("./Index");
                }
                catch (DbUpdateConcurrencyException ex)
                {
                    var exceptionEntry = ex.Entries.Single();
                    var clientValues = (Article)exceptionEntry.Entity;
                    var databaseEntry = exceptionEntry.GetDatabaseValues();
                    if (databaseEntry == null)
                    {
                        ModelState.AddModelError(string.Empty, "Unable to save. " +
                            "The Article was deleted by another user.");
                        return Page();
                    }
                    var dbValues = (Article)databaseEntry.ToObject();
                    await setDbErrorMessage(dbValues, clientValues, _context);
                    Article.RowVersion = (byte[])dbValues.RowVersion;
                    ModelState.Remove("Article.RowVersion");
                }

            }
            try
            {
                await _context.SaveChangesAsync();
            }

            catch (DbUpdateConcurrencyException)
            {

            }

            return Page();
        }


        private IActionResult HandleDeletedArticle()
        {
            var deletedArticle = new Article();
            ModelState.AddModelError(string.Empty,
                "Unable to save. The Article was deleted by another user.");
            return Page();
        }
        private async Task setDbErrorMessage(Article dbValues, Article clientValues, PhilipContext context)
        {
            if (dbValues.Author != clientValues.Author)
            {
                ModelState.AddModelError("Article.Author", $"Current value: {dbValues.Author}");
            }
            if (dbValues.Title != clientValues.Title)
            {
                ModelState.AddModelError("Article.Title", $"Current value: {dbValues.Title}");
            }
            if (dbValues.ReleaseDate != clientValues.ReleaseDate)
            {
                ModelState.AddModelError("Article.ReleaseDate", $"Current value: {dbValues.ReleaseDate}");
            }
            if (dbValues.Content != clientValues.Content)
            {
                ModelState.AddModelError("Article.Content", $"Current value: {dbValues.Content}");
            }
            ModelState.AddModelError(string.Empty,
                "The record you attempted to edit "
                 + "was modified by another user after you. The "
                 + "edit operation was canceled and the current values in the database "
                 + "have been displayed. If you still want to edit this record, click "
                 + "the Save button again.");
        }



        private bool ArticleExists(int id)
        {
            return _context.Article.Any(e => e.ID == id);
        }

    }
}

