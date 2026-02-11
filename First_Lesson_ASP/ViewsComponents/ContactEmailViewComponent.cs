using Microsoft.AspNetCore.Mvc;

namespace First_Lesson_ASP.ViewsComponents
{
    public class ContactEmailViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {
            
            return View("ContactEmail");
        }
    }
}