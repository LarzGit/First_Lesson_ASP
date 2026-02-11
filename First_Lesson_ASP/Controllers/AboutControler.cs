using Microsoft.AspNetCore.Mvc;

namespace First_Lesson_ASP.Controllers
{
    public class AboutController : Controller
    {


        public IActionResult ContactUs()
        {
            return View();
        }

        public IActionResult About()
        {
            return View();
        }
    }
}
