using First_Lesson_ASP.DB;
using First_Lesson_ASP.Entities;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;
using System.Linq;

namespace First_Lesson_ASP.Models
{
    public class CommentsModel
    {
        private readonly RestDBContext _context;

        public CommentsModel(RestDBContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Отримує всі валідні коментарі до посту у вигляді дерева (з відповідями)
        /// </summary>
        public List<Comment> GetCommentsTree(int postId)
        {
            var allComments = _context.Comments
                .AsNoTracking()
                .Where(c => c.PostId == postId && c.IsValid)
                .Include(c => c.Childs)   // для рекурсії, але не глибоко
                .ToList();

            // Знаходимо кореневі коментарі (без батьків)
            var rootComments = allComments
                .Where(c => !c.ParentId.HasValue)
                .OrderByDescending(c => c.DateOfPublished)
                .ToList();

            // Рекурсивно заповнюємо відповіді
            foreach (var root in rootComments)
            {
                BuildCommentTree(root, allComments);
            }

            return rootComments;
        }

        private void BuildCommentTree(Comment parent, List<Comment> allComments)
        {
            var children = allComments
                .Where(c => c.ParentId == parent.Id)
                .OrderByDescending(c => c.DateOfPublished)
                .ToList();

            parent.Childs = children;

            foreach (var child in children)
            {
                BuildCommentTree(child, allComments);
            }
        }

        /// <summary>
        /// Додає новий коментар (за замовчуванням на модерації — IsValid = false)
        /// </summary>
        public Comment AddComment(int postId, string name, string email, string message, int? parentId = null)
        {
            var comment = new Comment
            {
                PostId = postId,
                Name = name,
                Email = email,
                Message = message,
                ParentId = parentId,
                IsValid = false,  // чекає модерації (можна змінити на true для тестів)
                DateOfPublished = DateTime.UtcNow
            };

            _context.Comments.Add(comment);
            _context.SaveChanges();

            return comment;
        }

        /// <summary>
        /// Повертає кількість валідних коментарів до посту
        /// </summary>
        public int GetCommentCount(int postId)
        {
            return _context.Comments
                .Count(c => c.PostId == postId && c.IsValid);
        }

        /// <summary>
        /// Змінює статус валідності коментаря (для адмінки пізніше)
        /// </summary>
        public bool SetCommentValidity(int commentId, bool isValid)
        {
            var comment = _context.Comments.Find(commentId);
            if (comment == null) return false;

            comment.IsValid = isValid;
            _context.SaveChanges();
            return true;
        }
    }
}