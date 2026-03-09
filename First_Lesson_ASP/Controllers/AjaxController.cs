using Microsoft.AspNetCore.Mvc;
using First_Lesson_ASP.DB;
using First_Lesson_ASP.Models;
using First_Lesson_ASP.Entities;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Authorization;
using System.Security.Claims;

namespace First_Lesson_ASP.Controllers
{
    public class AjaxController : Controller
    {
        private readonly RestDBContext _context;
        private readonly CommentsModel _commentsModel;

        public AjaxController(RestDBContext context, CommentsModel commentsModel)
        {
            _context = context;
            _commentsModel = commentsModel;
        }

        /// <summary>
        /// Додає новий коментар через AJAX
        /// </summary>
        [HttpPost]
        [ValidateAntiForgeryToken]  // захист від CSRF
        public async Task<IActionResult> AddComment(int postId, string name, string email, string message, int? parentId = null)
        {
            if (string.IsNullOrWhiteSpace(name) || string.IsNullOrWhiteSpace(email) || string.IsNullOrWhiteSpace(message))
            {
                return Json(new { success = false, message = "Всі поля обов'язкові" });
            }

            if (!ModelState.IsValid)
            {
                return Json(new { success = false, message = "Некоректні дані" });
            }

            // Опціонально: якщо користувач залогінений — можна взяти ім'я/емейл з профілю
            // var userId = User.FindFirstValue(ClaimTypes.NameIdentifier);

            var comment = _commentsModel.AddComment(postId, name, email, message, parentId);

            // Якщо хочеш автоматично валідувати для тестів — зміни на true
            // _commentsModel.SetCommentValidity(comment.Id, true);

            return Json(new
            {
                success = true,
                message = "Коментар додано! Чекає модерації.",
                commentId = comment.Id
            });
        }

        /// <summary>
        /// Повертає дерево коментарів для посту (використовується для оновлення після додавання)
        /// </summary>
        [HttpGet]
        public IActionResult GetComments(int postId)
        {
            var comments = _commentsModel.GetCommentsTree(postId);
            return PartialView("_CommentsTree", comments);
        }
    }
}