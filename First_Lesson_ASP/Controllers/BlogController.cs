using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using First_Lesson_ASP.DB;
using First_Lesson_ASP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace First_Lesson_ASP.Controllers
{
    public class BlogController : Controller
    {
        private readonly RestDBContext _context;

        public BlogController(RestDBContext context)
        {
            _context = context;
        }

        // Список постів з пагінацією, пошуком, сортуванням, фільтрами за категорією та тегом
        public IActionResult Index(
            int page = 1,
            string? search = null,
            string? sort = "newest",
            int? categoryId = null,
            int? tagId = null)
        {
            const int pageSize = 4;

            var query = _context.Posts
                .Include(p => p.Categories)
                .Include(p => p.Tags)
                .Where(p => p.Status == PostStatuses.Published);

            // Пошук за заголовком або вмістом
            if (!string.IsNullOrWhiteSpace(search))
            {
                var s = search.Trim().ToLower();
                query = query.Where(p =>
                    p.Title.ToLower().Contains(s) ||
                    p.Content.ToLower().Contains(s));
            }

            // Фільтр за категорією (включаючи підкатегорії)
            if (categoryId.HasValue)
            {
                var categoryIds = GetCategoryAndChildrenIds(categoryId.Value);
                query = query.Where(p => p.Categories.Any(c => categoryIds.Contains(c.Id)));
            }

            // Фільтр за тегом
            if (tagId.HasValue)
            {
                query = query.Where(p => p.Tags.Any(t => t.Id == tagId.Value));
            }

            // Сортування
            query = sort?.ToLower() switch
            {
                "oldest" => query.OrderBy(p => p.DateOFPublished),
                "az" => query.OrderBy(p => p.Title),
                "za" => query.OrderByDescending(p => p.Title),
                _ => query.OrderByDescending(p => p.DateOFPublished)
            };

            var totalPosts = query.Count();

            var posts = query
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .AsNoTracking()
                .ToList();

            // Дані для ViewBag (для фільтрів і пагінації)
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
            ViewBag.Search = search;
            ViewBag.Sort = sort;
            ViewBag.CategoryId = categoryId;
            ViewBag.TagId = tagId;

            ViewBag.Categories = _context.Categories
                .Where(c => c.ParentId == null)
                .Include(c => c.Childs)
                .AsNoTracking()
                .ToList();

            ViewBag.Tags = _context.Tags
                .AsNoTracking()
                .ToList();

            return View(posts);
        }

        // Деталі посту — тепер підтримує як Id, так і slug (для красивих URL)
        public IActionResult Details(string? id)
        {
            if (string.IsNullOrWhiteSpace(id))
            {
                return NotFound();
            }

            Post? post;

            // Спробуємо спочатку як Id (число)
            if (int.TryParse(id, out int parsedId))
            {
                post = _context.Posts
                    .Include(p => p.Categories)
                    .Include(p => p.Tags)
                    .Include(p => p.Comments.Where(c => c.IsValid)) // ← готуємося до коментарів
                    .FirstOrDefault(p => p.Id == parsedId);
            }
            else
            {
                // Якщо не число — вважаємо slug
                post = _context.Posts
                    .Include(p => p.Categories)
                    .Include(p => p.Tags)
                    .Include(p => p.Comments.Where(c => c.IsValid))
                    .FirstOrDefault(p => p.Slug.ToLower() == id.ToLower());
            }

            if (post == null)
            {
                return NotFound();
            }

            return View(post);
        }

        // Рекурсивний метод для отримання всіх ID підкатегорій
        private List<int> GetCategoryAndChildrenIds(int catId)
        {
            var all = _context.Categories
                .AsNoTracking()
                .ToList();

            var ids = new List<int>();

            void Walk(int currentId)
            {
                ids.Add(currentId);
                var children = all.Where(c => c.ParentId == currentId).Select(c => c.Id).ToList();
                foreach (var childId in children) Walk(childId);
            }

            Walk(catId);
            return ids;
        }
    }
}