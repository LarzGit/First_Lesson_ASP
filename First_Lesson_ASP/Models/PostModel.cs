using First_Lesson_ASP.DB;
using First_Lesson_ASP.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace First_Lesson_ASP.Models
{
    public class PostModel
    {
        private RestDBContext _context;

        public PostModel(RestDBContext context)
        {
            _context = context;
        }

        public List<Post> GetPublishedPosts(int page = 1, int pageSize = 9)
        {
            return _context.Posts
                .Where(p => p.Status == PostStatuses.Published)
                .OrderByDescending(p => p.DateOFPublished)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Categories)
                .Include(p => p.Tags)
                .ToList();
        }

        public int GetTotalPublishedPosts()
        {
            return _context.Posts.Count(p => p.Status == PostStatuses.Published);
        }

        public List<Post> SearchPosts(string query, int page = 1, int pageSize = 9)
        {
            return _context.Posts
                .Where(p => p.Status == PostStatuses.Published && (p.Title.Contains(query) || p.Content.Contains(query)))
                .OrderByDescending(p => p.DateOFPublished)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .Include(p => p.Categories)
                .Include(p => p.Tags)
                .ToList();
        }

        public int GetTotalSearchResults(string query)
        {
            return _context.Posts.Count(p => p.Status == PostStatuses.Published && (p.Title.Contains(query) || p.Content.Contains(query)));
        }
    }
}