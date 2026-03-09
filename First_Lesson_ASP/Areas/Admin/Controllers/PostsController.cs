using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using First_Lesson_ASP.DB;
using First_Lesson_ASP.Entities;       // ← ЦЕЙ РЯДОК БУВ ВІДСУТНІЙ — ДОДАЙ ЙОГО
using System.Threading.Tasks;

namespace First_Lesson_ASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PostsController : Controller
    {
        private readonly RestDBContext _context;

        public PostsController(RestDBContext context)
        {
            _context = context;
        }

        // Список постів
        public async Task<IActionResult> Index()
        {
            var posts = await _context.Posts
                .Include(p => p.Categories)
                .Include(p => p.Tags)
                .OrderByDescending(p => p.DateOFCreated)
                .ToListAsync();

            return View(posts);
        }

        // Створити пост — форма
        public IActionResult Create()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(Post post)
        {
            if (ModelState.IsValid)
            {
                post.DateOFCreated = DateTime.UtcNow;
                post.DateOFLastUpdated = DateTime.UtcNow;
                post.Status = PostStatuses.Draft;

                _context.Add(post);
                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }
            return View(post);
        }

        // Редагувати пост — форма
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post == null) return NotFound();
            return View(post);
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, Post post)
        {
            if (id != post.Id) return NotFound();

            if (ModelState.IsValid)
            {
                try
                {
                    post.DateOFLastUpdated = DateTime.UtcNow;
                    _context.Update(post);
                    await _context.SaveChangesAsync();
                    return RedirectToAction(nameof(Index));
                }
                catch (DbUpdateConcurrencyException)
                {
                    if (!await _context.Posts.AnyAsync(e => e.Id == post.Id))
                        return NotFound();
                    throw;
                }
            }
            return View(post);
        }

        // Видалити пост
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var post = await _context.Posts.FindAsync(id);
            if (post != null)
            {
                _context.Posts.Remove(post);
                await _context.SaveChangesAsync();
            }
            return RedirectToAction(nameof(Index));
        }
    }
}