using Microsoft.EntityFrameworkCore;
using First_Lesson_ASP.DB;

namespace First_Lesson_ASP
{
    public class Program
    {
        public static void Main(string[] args)
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllersWithViews();

            builder.Services.AddDbContext<RestDBContext>(options =>
                options.UseSqlServer(
                    builder.Configuration.GetConnectionString("RestDBConnection")
                ));

            var app = builder.Build();

            // Запускаємо сидінг даних один раз (можна закоментувати після першого запуску)
            using (var scope = app.Services.CreateScope())
            {
                var context = scope.ServiceProvider.GetRequiredService<RestDBContext>();
                DbInitializer.Initialize(context);
            }

            if (!app.Environment.IsDevelopment())
            {
                app.UseExceptionHandler("/Home/Error");
                app.UseHsts();
            }

            app.UseHttpsRedirection();
            app.UseStaticFiles();

            app.UseRouting();

            app.UseAuthorization();

            app.MapControllerRoute(
                name: "default",
                pattern: "{controller=Home}/{action=Index}/{id?}");

            app.Run();
        }
    }
}