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

        public IActionResult Index(int page = 1, string search = null, string sort = "newest", int? categoryId = null)
        {
            const int pageSize = 4;

            // Базовий запит із завантаженням зв'язків
            var query = _context.Posts
                .Include(p => p.Categories)
                .Include(p => p.Tags)
                .Where(p => p.Status == PostStatuses.Published);

            // Фільтр пошуку
            if (!string.IsNullOrEmpty(search))
            {
                var s = search.ToLower();
                query = query.Where(p => p.Title.ToLower().Contains(s) || p.Content.ToLower().Contains(s));
            }

            // Фільтр категорій
            if (categoryId.HasValue)
            {
                var categoryIds = GetCategoryAndChildrenIds(categoryId.Value);
                query = query.Where(p => p.Categories.Any(c => categoryIds.Contains(c.Id)));
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

            // Передаємо дані у View
            ViewBag.CurrentPage = page;
            ViewBag.TotalPages = (int)Math.Ceiling((double)totalPosts / pageSize);
            ViewBag.Search = search;
            ViewBag.Sort = sort;
            ViewBag.CategoryId = categoryId;

            // Тільки головні категорії для меню
            ViewBag.Categories = _context.Categories
                .Where(c => c.ParentId == null)
                .Include(c => c.Childs)
                .ToList();

            ViewBag.Tags = _context.Tags.ToList();

            return View(posts);
        }

        // Окремий метод (ТІЛЬКИ ОДИН!)
        private List<int> GetCategoryAndChildrenIds(int catId)
        {
            var all = _context.Categories.AsNoTracking().ToList();
            var ids = new List<int>();

            void Walk(int id)
            {
                ids.Add(id);
                var children = all.Where(c => c.ParentId == id).Select(c => c.Id);
                foreach (var childId in children) Walk(childId);
            }

            Walk(catId);
            return ids;
        }
    }
}