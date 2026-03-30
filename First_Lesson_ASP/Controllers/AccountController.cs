using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using First_Lesson_ASP.Entities;
using First_Lesson_ASP.Models;
using System.Security.Claims;
using System.Threading.Tasks;
using System.Linq;

namespace First_Lesson_ASP.Controllers
{
    [AllowAnonymous]
    public class AccountController : Controller
    {
        private readonly UserManager<User> _userManager;
        private readonly SignInManager<User> _signInManager;

        public AccountController(
            UserManager<User> userManager,
            SignInManager<User> signInManager)
        {
            _userManager = userManager;
            _signInManager = signInManager;
        }

        // GET: /Account/Login
        [HttpGet]
        public IActionResult Login(string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;
            return View(new LoginViewModel());
        }

        // POST: /Account/Login
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Login(LoginViewModel model, string? returnUrl = null)
        {
            ViewData["ReturnUrl"] = returnUrl;

            if (!ModelState.IsValid) return View(model);

            var result = await _signInManager.PasswordSignInAsync(
                model.Email,
                model.Password,
                model.RememberMe,
                lockoutOnFailure: false);

            if (result.Succeeded) return LocalRedirect(returnUrl ?? Url.Content("~/"));

            if (result.IsLockedOut)
            {
                ModelState.AddModelError(string.Empty, "Обліковий запис заблоковано. Спробуйте пізніше.");
            }
            else
            {
                ModelState.AddModelError(string.Empty, "Неправильний email або пароль.");
            }

            return View(model);
        }

        // GET: Початок зовнішньої авторизації (Google)
        [HttpGet]
        public IActionResult ExternalLogin(string provider = "Google", string? returnUrl = null)
        {
            returnUrl ??= Url.Content("~/");
            var redirectUrl = Url.Action(nameof(ExternalLoginCallback), "Account", new { returnUrl });
            var properties = _signInManager.ConfigureExternalAuthenticationProperties(provider, redirectUrl);
            return Challenge(properties, provider);
        }

        // Callback після Google
        [HttpGet]
        public async Task<IActionResult> ExternalLoginCallback(string? returnUrl = null, string? remoteError = null)
        {
            returnUrl ??= Url.Content("~/");

            if (remoteError != null)
            {
                ModelState.AddModelError(string.Empty, $"Помилка від Google: {remoteError}");
                return View(nameof(Login), new LoginViewModel());
            }

            var info = await _signInManager.GetExternalLoginInfoAsync();
            if (info == null)
            {
                ModelState.AddModelError(string.Empty, "Не вдалося отримати інформацію від провайдера.");
                return View(nameof(Login), new LoginViewModel());
            }

            var signInResult = await _signInManager.ExternalLoginSignInAsync(
                info.LoginProvider, info.ProviderKey, isPersistent: false, bypassTwoFactor: true);

            if (signInResult.Succeeded) return LocalRedirect(returnUrl);
            if (signInResult.IsLockedOut) return RedirectToAction(nameof(AccessDenied));

            var email = info.Principal.FindFirstValue(ClaimTypes.Email);
            if (string.IsNullOrEmpty(email))
            {
                ModelState.AddModelError(string.Empty, "Провайдер не повернув email.");
                return View(nameof(Login), new LoginViewModel());
            }

            var user = await _userManager.FindByEmailAsync(email);
            if (user == null)
            {
                user = new User
                {
                    UserName = email,
                    Email = email,
                    EmailConfirmed = true,
                    FullName = info.Principal.FindFirstValue(ClaimTypes.Name) ?? email.Split('@')[0]
                };

                var createResult = await _userManager.CreateAsync(user);
                if (!createResult.Succeeded)
                {
                    foreach (var error in createResult.Errors) ModelState.AddModelError(string.Empty, error.Description);
                    return View(nameof(Login), new LoginViewModel());
                }
            }

            var addLoginResult = await _userManager.AddLoginAsync(user, info);
            if (!addLoginResult.Succeeded)
            {
                foreach (var error in addLoginResult.Errors) ModelState.AddModelError(string.Empty, error.Description);
                return View(nameof(Login), new LoginViewModel());
            }

            await _signInManager.SignInAsync(user, isPersistent: false);
            return LocalRedirect(returnUrl);
        }

        // GET: /Account/Register
        [HttpGet]
        public IActionResult Register()
        {
            return View(new RegisterViewModel());
        }

        // POST: /Account/Register
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Register(RegisterViewModel model)
        {
            if (!ModelState.IsValid) return View(model);

            var user = new User
            {
                UserName = model.Email,
                Email = model.Email,
                FullName = model.FullName
            };

            var result = await _userManager.CreateAsync(user, model.Password);

            if (result.Succeeded)
            {
                await _signInManager.SignInAsync(user, isPersistent: false);
                return RedirectToAction("Index", "Home");
            }

            foreach (var error in result.Errors) ModelState.AddModelError(string.Empty, error.Description);
            return View(model);
        }

        // POST: /Account/Logout
        [HttpPost]
        [ValidateAntiForgeryToken]
        public async Task<IActionResult> Logout()
        {
            await _signInManager.SignOutAsync();
            return RedirectToAction("Index", "Home");
        }

        // GET: /Account/AccessDenied
        [HttpGet]
        public IActionResult AccessDenied()
        {
            return View();
        }

        // 🚨 ТИМЧАСОВИЙ МЕТОД ДЛЯ НАДАННЯ ПРАВ АДМІНА 🚨
        [HttpGet]
        [AllowAnonymous]
        public async Task<IActionResult> MakeMeAdmin([FromServices] RoleManager<IdentityRole> roleManager)
        {
            string myEmail = "larzofficialgames@gmail.com";

            var user = await _userManager.FindByEmailAsync(myEmail);
            if (user == null) return Content("Користувача не знайдено!");

            if (!await roleManager.RoleExistsAsync("Admin"))
            {
                await roleManager.CreateAsync(new IdentityRole("Admin"));
            }

            if (!await _userManager.IsInRoleAsync(user, "Admin"))
            {
                var result = await _userManager.AddToRoleAsync(user, "Admin");
                if (!result.Succeeded)
                {
                    return Content("Помилка видачі ролі: " + string.Join(", ", result.Errors.Select(e => e.Description)));
                }
            }

            return Content($@"
                <h1>Готово!</h1>
                <p>Користувач <b>{myEmail}</b> тепер Адміністратор!</p>
                <b style='color:red;'>ВАЖЛИВО: Перейдіть на сайт, натисніть кнопку 'Вийти' (Logout), а потім зайдіть знову, щоб права запрацювали.</b>
            ", "text/html");
        }
    }
}