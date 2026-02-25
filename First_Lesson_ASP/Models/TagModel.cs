using First_Lesson_ASP.DB;
using First_Lesson_ASP.Entities;
using System.Collections.Generic;
using System.Linq;

namespace First_Lesson_ASP.Models
{
    public class TagModel
    {
        private RestDBContext _context;

        public TagModel(RestDBContext context)
        {
            _context = context;
        }

        public List<Tag> GetAllTags()
        {
            return _context.Tags.OrderBy(t => t.Title).ToList();
        }
    }
}