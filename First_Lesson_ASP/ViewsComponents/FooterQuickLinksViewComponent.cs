using Microsoft.AspNetCore.Mvc;

namespace First_Lesson_ASP.ViewsComponents
{
    public class FooterQuickLinksViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("FooterQuickLinks");
        }
    }
}