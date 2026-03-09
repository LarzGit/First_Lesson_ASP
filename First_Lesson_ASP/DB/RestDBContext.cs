using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;
using First_Lesson_ASP.Entities;

namespace First_Lesson_ASP.DB
{
    public class RestDBContext : IdentityDbContext<User>
    {
        public DbSet<ClientMessage> ClientMessages { get; set; }
        public DbSet<Navigate> Navigations { get; set; }
        public DbSet<Option> Options { get; set; }
        public DbSet<Tag> Tags { get; set; }
        public DbSet<Category> Categories { get; set; }
        public DbSet<Post> Posts { get; set; }
        public DbSet<PostTags> PostTags { get; set; }
        public DbSet<PostCategories> PostCategories { get; set; }
        public DbSet<Comment> Comments { get; set; }

        public RestDBContext(DbContextOptions<RestDBContext> options)
            : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            // Post ↔ Category
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Categories)
                .WithMany(c => c.Posts)
                .UsingEntity<PostCategories>(
                    j => j.HasOne<Category>().WithMany().HasForeignKey(pc => pc.CategoryId),
                    j => j.HasOne<Post>().WithMany().HasForeignKey(pc => pc.PostId),
                    j => { j.ToTable("PostCategories"); j.HasKey(pc => new { pc.PostId, pc.CategoryId }); });

            // Post ↔ Tag
            modelBuilder.Entity<Post>()
                .HasMany(p => p.Tags)
                .WithMany(t => t.Posts)
                .UsingEntity<PostTags>(
                    j => j.HasOne<Tag>().WithMany().HasForeignKey(pt => pt.TagId),
                    j => j.HasOne<Post>().WithMany().HasForeignKey(pt => pt.PostId),
                    j => { j.ToTable("PostTags"); j.HasKey(pt => new { pt.PostId, pt.TagId }); });

            // Коментарі
            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Post)
                .WithMany(p => p.Comments)
                .HasForeignKey(c => c.PostId)
                .OnDelete(DeleteBehavior.Cascade);

            modelBuilder.Entity<Comment>()
                .HasOne(c => c.Parent)
                .WithMany(c => c.Childs)
                .HasForeignKey(c => c.ParentId)
                .OnDelete(DeleteBehavior.NoAction);
        }
    }
}