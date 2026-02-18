using Microsoft.AspNetCore.Mvc;
using First_Lesson_ASP.Entities;
using First_Lesson_ASP.DB;           // ← додай using

namespace First_Lesson_ASP.Controllers
{
    public class AboutController : Controller
    {
        private readonly RestDBContext _context;     // ← додай поле

        public AboutController(RestDBContext context)   // ← додай конструктор
        {
            _context = context;
        }

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
        public async Task<IActionResult> ContactUs(ClientMessage model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                _context.ClientMessages.Add(model);
                await _context.SaveChangesAsync();         

                return View("Thanks", model);
            }
            catch (Exception ex)
            {
               
                ModelState.AddModelError("", "Виникла помилка при збереженні. Спробуйте пізніше.");
                return View(model);
            }
        }
    }
}