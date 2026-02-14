using Microsoft.AspNetCore.Mvc;

namespace First_Lesson_ASP.ViewsComponents
{
    public class FooterCopyrightViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            return View("FooterCopyright");
        }
    }
}