using Microsoft.EntityFrameworkCore;
using First_Lesson_ASP.DB;  // ? обов'язково додай цей using

namespace First_Lesson_ASP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            // Додаємо підтримку контролерів та представлень (MVC)
            builder.Services.AddControllersWithViews();

            // Реєстрація контексту бази даних
            // Використовуємо існуючий ключ "RestDBConection" з appsettings.json
            builder.Services.AddDbContext<RestDBContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("RestDBConection")
                ));

            var app = builder.Build();

            // Налаштування пайплайну обробки запитів
            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                // The default HSTS value is 30 days. You may want to change this for production scenarios.
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            // Маршрутизація для MVC
            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            // Запуск додатка
            app.Run();
        }
    }
}