using Microsoft.AspNetCore.Mvc;

namespace First_Lesson_ASP.ViewsComponents

{
    public class LogoStatsViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
            return View("LogoSite");
        }
    }
}
