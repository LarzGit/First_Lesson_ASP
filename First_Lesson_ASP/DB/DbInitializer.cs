using Microsoft.EntityFrameworkCore;
using First_Lesson_ASP.Entities;
using System;
using System.Collections.Generic;
using System.Linq;

namespace First_Lesson_ASP.DB
{
    public static class DbInitializer
    {
        public static void Initialize(RestDBContext context)
        {
            context.Database.EnsureCreated();

            if (context.Navigations.Any())
            {
                return;
            }

            SeedNavigations(context);
            SeedOptions(context);
            SeedCategories(context);
            SeedTags(context);
            SeedPosts(context);
            SeedPostCategories(context);
            SeedPostTags(context);
        }

        private static void SeedNavigations(RestDBContext context)
        {
            var navigations = new List<Navigate>
            {
                new Navigate { Title = "Home", Href = "/", Order = 1, ParentId = null },
                new Navigate { Title = "Pages", Href = "#", Order = 2, ParentId = null },
                new Navigate { Title = "About Us", Href = "/About/About", Order = 1, ParentId = 2 },
                new Navigate { Title = "Our Chefs", Href = "/Team", Order = 2, ParentId = 2 },
                new Navigate { Title = "Blog", Href = "/Blog/Index", Order = 3, ParentId = 2 },
                new Navigate { Title = "Testimonials", Href = "/Testimonials", Order = 4, ParentId = 2 },
                new Navigate { Title = "Contact Us", Href = "/About/ContactUs", Order = 5, ParentId = 2 },
                new Navigate { Title = "Menu", Href = "/Menu", Order = 3, ParentId = null },
                new Navigate { Title = "Breakfast", Href = "/Menu/Breakfast", Order = 1, ParentId = 8 },
                new Navigate { Title = "Lunch", Href = "/Menu/Lunch", Order = 2, ParentId = 8 },
                new Navigate { Title = "Dinner", Href = "/Menu/Dinner", Order = 3, ParentId = 8 },
                new Navigate { Title = "Chefs", Href = "/Team", Order = 4, ParentId = null },
                new Navigate { Title = "Contact", Href = "/About/ContactUs", Order = 5, ParentId = null }
            };

            context.Navigations.AddRange(navigations);
            context.SaveChanges();
        }

        private static void SeedOptions(RestDBContext context)
        {
            var options = new List<Option>
            {
                new Option { Name = "SITE_TITLE", Key = "site", Value = "Chefer", Relation = "site-info", Order = 1, IsSystem = true },
                new Option { Name = "SITE_LOGO", Key = "site", Value = "/img/favicon.ico", Relation = "site-info", Order = 2, IsSystem = true },
                new Option { Name = "CONTACT_EMAIL", Key = "contact", Value = "info@example.com", Relation = "contact-info", Order = 1, IsSystem = false },
                new Option { Name = "CONTACT_PHONE", Key = "contact", Value = "+012 345 6789", Relation = "contact-info", Order = 2, IsSystem = false },
                new Option { Name = "SOCIAL_TWITTER", Key = "social", Value = "https://twitter.com", Relation = "social-links", Order = 1, IsSystem = false },
                new Option { Name = "SOCIAL_FACEBOOK", Key = "social", Value = "https://facebook.com", Relation = "social-links", Order = 2, IsSystem = false }
            };

            context.Options.AddRange(options);
            context.SaveChanges();
        }

        private static void SeedCategories(RestDBContext context)
        {
            var recipes = new Category { Title = "Recipes", Slug = "recipes", ImgSrc = "/img/category-1.jpg", ImgAlt = "Recipes", Description = "Cooking recipes", ParentId = null };
            context.Categories.Add(recipes);
            context.SaveChanges();

            var mainDishes = new Category { Title = "Main Dishes", Slug = "main-dishes", ImgSrc = "/img/category-3.jpg", ImgAlt = "Main Dishes", Description = "Main courses", ParentId = null };
            context.Categories.Add(mainDishes);
            context.SaveChanges();

            var desserts = new Category { Title = "Desserts", Slug = "desserts", ImgSrc = "/img/category-2.jpg", ImgAlt = "Desserts", Description = "Sweet desserts", ParentId = recipes.Id };
            context.Categories.Add(desserts);
            context.SaveChanges();
        }

        private static void SeedTags(RestDBContext context)
        {
            var cooking = new Tag { Title = "Cooking", Slug = "cooking" };
            context.Tags.Add(cooking);
            context.SaveChanges();

            var healthy = new Tag { Title = "Healthy", Slug = "healthy" };
            context.Tags.Add(healthy);
            context.SaveChanges();

            var quick = new Tag { Title = "Quick", Slug = "quick" };
            context.Tags.Add(quick);
            context.SaveChanges();
        }

        private static void SeedPosts(RestDBContext context)
        {
            var posts = new List<Post>
            {
                new Post { Title = "How to Make Perfect Steak", Content = "Step by step guide...", Slogan = "Perfect steak every time", Slug = "perfect-steak", ImgSrc = "/img/blog-1.jpg", ImgAlt = "Steak", DateOFCreated = DateTime.Now, DateOFPublished = DateTime.Now, DateOFLastUpdated = DateTime.Now, Status = PostStatuses.Published },
                new Post { Title = "Best Dessert Recipes", Content = "Delicious desserts...", Slogan = "Sweet treats", Slug = "dessert-recipes", ImgSrc = "/img/blog-2.jpg", ImgAlt = "Dessert", DateOFCreated = DateTime.Now, DateOFPublished = DateTime.Now, DateOFLastUpdated = DateTime.Now, Status = PostStatuses.Published },
                // ... додай інші пости, якщо потрібно, але для тесту вистачить 2
            };

            context.Posts.AddRange(posts);
            context.SaveChanges();
        }

        private static void SeedPostCategories(RestDBContext context)
        {
            var posts = context.Posts.ToList();
            var categories = context.Categories.ToList();

            var mainDishesId = categories.FirstOrDefault(c => c.Title == "Main Dishes")?.Id ?? 0;
            var dessertsId = categories.FirstOrDefault(c => c.Title == "Desserts")?.Id ?? 0;
            var recipesId = categories.FirstOrDefault(c => c.Title == "Recipes")?.Id ?? 0;

            var postCategories = new List<PostCategories>();

            // Прив’язуємо пости до категорій (можна розширити на всі 42)
            if (posts.Any())
            {
                postCategories.Add(new PostCategories { PostId = posts.First().Id, CategoryId = mainDishesId });
                postCategories.Add(new PostCategories { PostId = posts.Last().Id, CategoryId = dessertsId });
            }

            context.PostCategories.AddRange(postCategories);
            context.SaveChanges();
        }

        private static void SeedPostTags(RestDBContext context)
        {
            var posts = context.Posts.ToList();
            var tags = context.Tags.ToList();

            var cookingId = tags.FirstOrDefault(t => t.Title == "Cooking")?.Id ?? 0;
            var healthyId = tags.FirstOrDefault(t => t.Title == "Healthy")?.Id ?? 0;
            var quickId = tags.FirstOrDefault(t => t.Title == "Quick")?.Id ?? 0;

            var postTags = new List<PostTags>();

            if (posts.Any())
            {
                postTags.Add(new PostTags { PostId = posts.First().Id, TagId = cookingId });
                postTags.Add(new PostTags { PostId = posts.Last().Id, TagId = healthyId });
            }

            context.PostTags.AddRange(postTags);
            context.SaveChanges();
        }
    }
}