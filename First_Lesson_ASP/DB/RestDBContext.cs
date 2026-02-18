using Microsoft.EntityFrameworkCore;
using First_Lesson_ASP.Entities;

namespace First_Lesson_ASP.DB
{
    public class RestDBContext : DbContext
    {
        public DbSet<ClientMessage> ClientMessages { get; set; }

        public RestDBContext(DbContextOptions<RestDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.Entity<ClientMessage>()
                .Property(m => m.CreatedAt)
                .HasDefaultValueSql("GETDATE()");   // ← найбезпечніший вибір для SQL Server Express
        }
    }
}