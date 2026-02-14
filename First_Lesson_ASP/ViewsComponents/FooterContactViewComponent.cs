using Microsoft.AspNetCore.Mvc;

namespace First_Lesson_ASP.ViewsComponents
{
    public class FooterContactViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("FooterContact");
        }
    }
}