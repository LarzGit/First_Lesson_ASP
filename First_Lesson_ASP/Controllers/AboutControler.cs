using Microsoft.AspNetCore.Mvc;
using First_Lesson_ASP.Entities;

namespace First_Lesson_ASP.Controllers
{
    public class AboutController : Controller
    {
        [HttpGet]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        public IActionResult ContactUs()
        {
            return View();
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public IActionResult ContactUs(ClientMessage model)
        {
            if (ModelState.IsValid)
            {
              
                return View("Thanks", model);
            }

            return View(model);
        }
    }
}