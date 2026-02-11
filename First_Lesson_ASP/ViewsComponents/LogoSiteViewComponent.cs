using Microsoft.AspNetCore.Mvc;

namespace First_Lesson_ASP.ViewsComponents

{
    public class LogoSiteViewComponent : ViewComponent
    {

        public IViewComponentResult Invoke()
        {
           
            return View("LogoSite");
        }
    }
}
