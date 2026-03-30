using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Hosting;
using First_Lesson_ASP.DB;
using First_Lesson_ASP.Entities;
using First_Lesson_ASP.Areas.Admin.Models;
using System.Threading.Tasks;
using System.IO;
using System;
using System.Linq;

namespace First_Lesson_ASP.Areas.Admin.Controllers
{
    [Area("Admin")]
    [Authorize(Roles = "Admin")]
    public class PostsController : Controller
    {
        private readonly RestDBContext _context;
        private readonly IWebHostEnvironment _webHostEnvironment;

        public PostsController(RestDBContext context, IWebHostEnvironment webHostEnvironment)
        {
            _context = context;
            _webHostEnvironment = webHostEnvironment;
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

        // GET: Створити пост
        public IActionResult Create()
        {
            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Title");
            return View(new PostFormViewModel());
        }

        // POST: Створити пост
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Create(PostFormViewModel model)
        {
            if (ModelState.IsValid)
            {
                string uniqueFileName = string.Empty;

                // 1. Завантаження картинки
                if (model.UploadedImage != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    uniqueFileName = Guid.NewGuid().ToString() + "_" + model.UploadedImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.UploadedImage.CopyToAsync(fileStream);
                    }
                }

                // 2. Створення поста
                var post = new Post
                {
                    Title = model.Title,
                    Slug = model.Slug,
                    Slogan = model.Slogan,
                    Content = model.Content,
                    Status = model.Status,
                    ImgAlt = model.ImgAlt,
                    ImgSrc = string.IsNullOrEmpty(uniqueFileName) ? "" : "/uploads/" + uniqueFileName,
                    DateOFCreated = DateTime.UtcNow,
                    DateOFLastUpdated = DateTime.UtcNow
                };

                _context.Posts.Add(post);
                await _context.SaveChangesAsync(); // Зберігаємо, щоб отримати Id поста

                // 3. Збереження категорій
                if (model.SelectedCategoryIds != null && model.SelectedCategoryIds.Any())
                {
                    foreach (var catId in model.SelectedCategoryIds)
                    {
                        _context.PostCategories.Add(new PostCategories
                        {
                            PostId = post.Id,
                            CategoryId = catId
                        });
                    }
                    await _context.SaveChangesAsync();
                }

                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Title");
            return View(model);
        }

        // GET: Редагувати пост
        public async Task<IActionResult> Edit(int id)
        {
            var post = await _context.Posts
                .Include(p => p.Categories) // Підтягуємо категорії
                .FirstOrDefaultAsync(p => p.Id == id);

            if (post == null) return NotFound();

            // Перекладаємо дані з бази у ViewModel
            var model = new PostFormViewModel
            {
                Id = post.Id,
                Title = post.Title,
                Slug = post.Slug,
                Slogan = post.Slogan,
                Content = post.Content,
                Status = post.Status,
                ImgAlt = post.ImgAlt,
                ExistingImagePath = post.ImgSrc, // Зберігаємо старий шлях
                SelectedCategoryIds = post.Categories.Select(c => c.Id).ToList()
            };

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Title");
            return View("Create", model); // Використовуємо ту саму View
        }

        // POST: Редагувати пост
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Edit(int id, PostFormViewModel model)
        {
            if (id != model.Id) return NotFound();

            if (ModelState.IsValid)
            {
                var post = await _context.Posts
                    .Include(p => p.Categories)
                    .FirstOrDefaultAsync(p => p.Id == id);

                if (post == null) return NotFound();

                // 1. Оновлення полів
                post.Title = model.Title;
                post.Slug = model.Slug;
                post.Slogan = model.Slogan;
                post.Content = model.Content;
                post.Status = model.Status;
                post.ImgAlt = model.ImgAlt;
                post.DateOFLastUpdated = DateTime.UtcNow;

                // 2. Оновлення картинки (тільки якщо завантажили нову)
                if (model.UploadedImage != null)
                {
                    string uploadsFolder = Path.Combine(_webHostEnvironment.WebRootPath, "uploads");
                    if (!Directory.Exists(uploadsFolder)) Directory.CreateDirectory(uploadsFolder);

                    string uniqueFileName = Guid.NewGuid().ToString() + "_" + model.UploadedImage.FileName;
                    string filePath = Path.Combine(uploadsFolder, uniqueFileName);

                    using (var fileStream = new FileStream(filePath, FileMode.Create))
                    {
                        await model.UploadedImage.CopyToAsync(fileStream);
                    }
                    post.ImgSrc = "/uploads/" + uniqueFileName;
                }

                // 3. Оновлення категорій
                // Спочатку видаляємо старі зв'язки
                var existingCategories = _context.PostCategories.Where(pc => pc.PostId == id);
                _context.PostCategories.RemoveRange(existingCategories);

                // Потім додаємо нові
                if (model.SelectedCategoryIds != null && model.SelectedCategoryIds.Any())
                {
                    foreach (var catId in model.SelectedCategoryIds)
                    {
                        _context.PostCategories.Add(new PostCategories
                        {
                            PostId = post.Id,
                            CategoryId = catId
                        });
                    }
                }

                await _context.SaveChangesAsync();
                return RedirectToAction(nameof(Index));
            }

            ViewBag.Categories = new SelectList(_context.Categories, "Id", "Title");
            return View("Create", model);
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