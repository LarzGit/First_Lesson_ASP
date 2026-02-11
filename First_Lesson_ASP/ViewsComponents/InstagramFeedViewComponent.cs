using Microsoft.AspNetCore.Mvc;

namespace First_Lesson_ASP.ViewsComponents
{
    public class InstagramFeedViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("InstagramFeed");
        }
    }
}