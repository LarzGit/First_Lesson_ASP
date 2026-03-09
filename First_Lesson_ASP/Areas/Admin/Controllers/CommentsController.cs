using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using First_Lesson_ASP.DB;
using System.Threading.Tasks;

namespace First_Lesson_ASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class CommentsController : Controller
    {
        private readonly RestDBContext _context;

        public CommentsController(RestDBContext context)
        {
            _context = context;
        }

        // Метод, який відкриває список коментарів (asp-action="Index")
        public async Task<IActionResult> Index()
        {
            var comments = await _context.Comments
                .Include(c => c.Post)
                .OrderByDescending(c => c.DateOfPublished)
                .ToListAsync();

            return View(comments);
        }

        // Метод, який обробляє видалення коментаря (asp-action="Delete")
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Delete(int id)
        {
            var comment = await _context.Comments.FindAsync(id);
            if (comment != null)
            {
                _context.Comments.Remove(comment);
                await _context.SaveChangesAsync();
            }

            // Після успішного видалення повертаємо назад до списку
            return RedirectToAction(nameof(Index));
        }
    }
}