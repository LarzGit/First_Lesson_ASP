using Microsoft.AspNetCore.Mvc;

namespace First_Lesson_ASP.ViewsComponents
{
    public class FooterNewsletterViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("FooterNewsletter");
        }
    }
}