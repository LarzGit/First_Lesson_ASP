using Microsoft.AspNetCore.Mvc;

namespace First_Lesson_ASP.ViewsComponents
{
    public class ContactNumberViewComponent : ViewComponent
    {
        public IViewComponentResult Invoke()
        {

            return View("ContactNumber");
        }
    }
}