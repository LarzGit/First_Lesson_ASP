using Microsoft.AspNetCore.Authentication.Cookies;
using Microsoft.AspNetCore.Mvc.Razor.RuntimeCompilation;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;
using First_Lesson_ASP.DB;
using First_Lesson_ASP.Entities;
using First_Lesson_ASP.Models;

namespace First_Lesson_ASP
{
    public class Program
    {
        // Змінено void на async Task для підтримки await
        public static async Task Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Додаємо MVC + Razor Views з підтримкою Runtime Compilation
            builder.Services.AddControllersWithViews()
                .AddRazorRuntimeCompilation();

            // Підключення до бази даних
            builder.Services.AddDbContext<RestDBContext>(options =>
                options.UseSqlServer(builder.Configuration.GetConnectionString("RestDBConnection")));

            // Додаємо сервіс для коментарів
            builder.Services.AddScoped<CommentsModel>();

            // Identity + Cookie Authentication
            builder.Services.AddIdentity<User, IdentityRole>(options =>
            {
                options.Password.RequireDigit = false;
                options.Password.RequiredLength = 6;
                options.Password.RequireNonAlphanumeric = false;
                options.Password.RequireUppercase = false;
                options.Password.RequireLowercase = false;
                options.SignIn.RequireConfirmedAccount = false;
            })
            .AddEntityFrameworkStores<RestDBContext>()
            .AddDefaultTokenProviders();

            builder.Services.AddAuthentication(CookieAuthenticationDefaults.AuthenticationScheme)
                .AddCookie(options =>
                {
                    options.LoginPath = "/Account/Login";
                    options.LogoutPath = "/Account/Logout";
                    options.AccessDeniedPath = "/Account/AccessDenied";
                    options.Cookie.Name = "FirstLessonAuth";
                    options.ExpireTimeSpan = TimeSpan.FromDays(30);
                    options.SlidingExpiration = true;
                });

            var app = builder.Build();

            // Автоматичне застосування міграцій + сидінг
            using (var scope = app.Services.CreateScope())
            {
                var services = scope.ServiceProvider;
                var context = services.GetRequiredService<RestDBContext>();

                // Застосовуємо міграції
                context.Database.Migrate();

                // Запускаємо сидінг
                DbInitializer.Initialize(context);

                var roleManager = services.GetRequiredService<RoleManager<IdentityRole>>();
                var userManager = services.GetRequiredService<UserManager<User>>();

                if (!await roleManager.RoleExistsAsync("Admin"))
                {
                    await roleManager.CreateAsync(new IdentityRole("Admin"));
                }

                var adminEmail = "admin@example.com";
                var adminUser = await userManager.FindByEmailAsync(adminEmail);
                if (adminUser == null)
                {
                    var user = new User { UserName = adminEmail, Email = adminEmail, EmailConfirmed = true };
                    var result = await userManager.CreateAsync(user, "Admin123!");
                    if (result.Succeeded)
                    {
                        await userManager.AddToRoleAsync(user, "Admin");
                    }
                }
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();
            app.UseRouting();

            app.UseAuthentication(); // Має бути ПЕРЕД UseAuthorization
            app.UseAuthorization();

            app.MapControllerRoute(
                name: "areas",
                pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Асинхронний запуск додатка
            app.MapControllerRoute(
    name: "areas",
    pattern: "{area:exists}/{controller=Home}/{action=Index}/{id?}");
            await app.RunAsync();
        }
    }
}