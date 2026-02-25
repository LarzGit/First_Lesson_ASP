using First_Lesson_ASP.DB;
using First_Lesson_ASP.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace First_Lesson_ASP.Models
{
    public class CategoryModel
    {
        private RestDBContext _context;

        public CategoryModel(RestDBContext context)
        {
            _context = context;
        }

        public List<Category> GetTopLevelCategories()
        {
            return _context.Categories
                .Where(c => c.ParentId == null)
                .OrderBy(c => c.Title)
                .Include(c => c.Childs)
                .ToList();
        }
    }
}