using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using H.W12.Data;
using H.W12.Models;

namespace H.W12.Controllers
{
    public class NewsArticlesController : Controller
    {
        private readonly ApplicationDbContext _context;

        public NewsArticlesController(ApplicationDbContext context)
        {
            _context = context;
        }

        // GET: NewsArticles
        public async Task<IActionResult> Index()
        {
            var applicationDbContext = _context.NewsArticles.Include(n => n.Author);
            return View(await applicationDbContext.ToListAsync());
        }

        // GET: NewsArticles/Details/5
        public async Task<IActionResult> Details(int? id)
        {
            if (id == null || _context.NewsArticles == null)
            {
                return NotFound();
            }

            var newsArticle = await _context.NewsArticles
                .Include(n => n.Author)
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

        // GET: NewsArticles/Create
        public IActionResult Create()
        {
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id");
            return View();
        }

        // POST: NewsArticles/Create
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create([Bind("ArticleId,Title,Content,AuthorId")] NewsArticle newsArticle)
        {
            newsArticle.CreatedAt = DateTime.Now;
            newsArticle.UpdatedAt = DateTime.Now;

            if (!string.IsNullOrEmpty(newsArticle.AuthorId))
            {
                newsArticle.Author = _context.Users.Find(newsArticle.AuthorId);
            }
            else
            {
                ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", newsArticle.AuthorId);
                return View(newsArticle);
            }

            if (!string.IsNullOrEmpty(newsArticle.Title) && !string.IsNullOrEmpty(newsArticle.Content))
            {
                _context.Add(newsArticle);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", newsArticle.AuthorId);
            return View(newsArticle);
        }

        // GET: NewsArticles/Edit/5
        public async Task<IActionResult> Edit(int? id)
        {
            if (id == null || _context.NewsArticles == null)
            {
                return NotFound();
            }

            var newsArticle = await _context.NewsArticles.FindAsync(id);
            if (newsArticle == null)
            {
                return NotFound();
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", newsArticle.AuthorId);
            return View(newsArticle);
        }

        // POST: NewsArticles/Edit/5
        // To protect from overposting attacks, enable the specific properties you want to bind to.
        // For more details, see http://go.microsoft.com/fwlink/?LinkId=317598.
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, [Bind("ArticleId,Title,Content,AuthorId")] NewsArticle newsArticle)
        {
            if (id != newsArticle.ArticleId)
            {
                return NotFound();
            }

            newsArticle.UpdatedAt = DateTime.Now;
            if (!string.IsNullOrEmpty(newsArticle.AuthorId))
            {
                newsArticle.Author = _context.Users.Find(newsArticle.AuthorId);
            }
            else
            {
                ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", newsArticle.AuthorId);
                return View(newsArticle);
            }

            if (!string.IsNullOrEmpty(newsArticle.Title) && !string.IsNullOrEmpty(newsArticle.Content))
            {
                try
                {
                    _context.Update(newsArticle);
                    await _context.SaveChangesAsync();
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!NewsArticleExists(newsArticle.ArticleId))
                    {
                        return NotFound();
                    }
                    else
                    {
                        throw;
                    }
                }
                return RedirectToAction(nameof(Index));
            }
            ViewData["AuthorId"] = new SelectList(_context.Users, "Id", "Id", newsArticle.AuthorId);
            return View(newsArticle);
        }

        // GET: NewsArticles/Delete/5
        public async Task<IActionResult> Delete(int? id)
        {
            if (id == null || _context.NewsArticles == null)
            {
                return NotFound();
            }

            var newsArticle = await _context.NewsArticles
                .Include(n => n.Author)
                .FirstOrDefaultAsync(m => m.ArticleId == id);
            if (newsArticle == null)
            {
                return NotFound();
            }

            return View(newsArticle);
        }

        // POST: NewsArticles/Delete/5
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> DeleteConfirmed(int id)
        {
            if (_context.NewsArticles == null)
            {
                return Problem("Entity set 'ApplicationDbContext.NewsArticles'  is null.");
            }
            var newsArticle = await _context.NewsArticles.FindAsync(id);
            if (newsArticle != null)
            {
                _context.NewsArticles.Remove(newsArticle);
            }

            await _context.SaveChangesAsync();
            return RedirectToAction(nameof(Index));
        }

        private bool NewsArticleExists(int id)
        {
            return (_context.NewsArticles?.Any(e => e.ArticleId == id)).GetValueOrDefault();
        }
    }
}
