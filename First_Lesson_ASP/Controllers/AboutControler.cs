using Microsoft.AspNetCore.Mvc;

namespace First_Lesson_ASP.Controllers
{
    public class AboutController : Controller
    {

    


        public IActionResult About()
        {
            return View();
        }
    }
}
