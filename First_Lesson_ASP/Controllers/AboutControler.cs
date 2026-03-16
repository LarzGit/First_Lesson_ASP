using Microsoft.AspNetCore.Mvc;
using First_Lesson_ASP.Entities;
using First_Lesson_ASP.DB;
using Microsoft.EntityFrameworkCore;          // для можливих помилок DbUpdateException
using System.Threading.Tasks;

namespace First_Lesson_ASP.Controllers
{
    public class AboutController : Controller
    {
        private readonly RestDBContext _context;
        // private readonly IEmailSender _emailSender;   // ← додай пізніше

        public AboutController(RestDBContext context /*, IEmailSender emailSender */)
        {
            _context = context;
            // _emailSender = emailSender;
        }

        [HttpGet]
        [Route("about")]
        public IActionResult About()
        {
            return View();
        }

        [HttpGet]
        [Route("contact")]
        public IActionResult Contact()
        {
            return View();
        }

        [HttpPost]
        [Route("contact")]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Contact(ClientMessage model)
        {
            if (!ModelState.IsValid)
            {
                return View(model);
            }

            try
            {
                model.CreatedAt = DateTime.UtcNow;  // ← фіксуємо час створення

                _context.ClientMessages.Add(model);
                await _context.SaveChangesAsync();

                // TODO: відправити email адміністратору
                // await _emailSender.SendEmailAsync(
                //     to: "admin@yourdomain.com",
                //     subject: $"Нове повідомлення від {model.Name}",
                //     body: $"Тема: {model.Subject}\n\n{model.Message}\n\nВід: {model.Email}"
                // );

                TempData["SuccessMessage"] = "Дякуємо! Ваше повідомлення надіслано.";
                return RedirectToAction(nameof(Thanks));
            }
            catch (DbUpdateException dbEx)
            {
                // Логування помилки (в реальному проєкті додай ILogger)
                ModelState.AddModelError(string.Empty, "Помилка збереження в базі даних. Спробуйте пізніше.");
            }
            catch (Exception ex)
            {
                // Логування критичної помилки
                ModelState.AddModelError(string.Empty, "Сталася неочікувана помилка. Зв'яжіться з нами іншим способом.");
            }

            return View(model);
        }

        [HttpGet]
        [Route("contact/thanks")]
        public IActionResult Thanks()
        {
            return View();
        }
    }
}