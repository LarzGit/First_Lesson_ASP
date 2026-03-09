using Microsoft.AspNetCore.Mvc;
using First_Lesson_ASP.Models;
using System.Threading.Tasks;

namespace First_Lesson_ASP.ViewComponents
{
    public class CommentsTreeViewComponent : ViewComponent
    {
        private readonly CommentsModel _commentsModel;

        public CommentsTreeViewComponent(CommentsModel commentsModel)
        {
            _commentsModel = commentsModel;
        }

        public async Task<IViewComponentResult> InvokeAsync(int postId)
        {
            var comments = _commentsModel.GetCommentsTree(postId);
            return View(comments); // Views/Shared/Components/CommentsTree/Default.cshtml
        }
    }
}