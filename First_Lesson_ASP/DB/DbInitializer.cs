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
            context.Database.Migrate();

            if (context.Navigations.Any())
            {
                return;
            }

            SeedNavigations(context);
            SeedOptions(context);
            SeedCategories(context);
            SeedTags(context);
            SeedPosts(context);
            SeedPostCategoriesAndTags(context);
            SeedComments(context);
        }

        private static void SeedNavigations(RestDBContext context)
        {
            var navigations = new List<Navigate>
            {
                new Navigate { Id = 1, Title = "Home", Href = "/", Order = 1, ParentId = null },
                new Navigate { Id = 2, Title = "About", Href = "/About/About", Order = 2, ParentId = null },
                new Navigate { Id = 3, Title = "Menu", Href = "/Menu", Order = 3, ParentId = null },
                new Navigate { Id = 4, Title = "Blog", Href = "/Blog", Order = 4, ParentId = null },
                new Navigate { Id = 5, Title = "Contact", Href = "/About/ContactUs", Order = 5, ParentId = null },
                new Navigate { Id = 6, Title = "Services", Href = "/Services", Order = 6, ParentId = null, Childs = new List<Navigate>
                {
                    new Navigate { Id = 7, Title = "Catering", Href = "/Services/Catering", Order = 1, ParentId = 6 },
                    new Navigate { Id = 8, Title = "Events", Href = "/Services/Events", Order = 2, ParentId = 6 }
                }},
                new Navigate { Id = 9, Title = "Chefs", Href = "/Chefs", Order = 7, ParentId = null }
            };

            context.Navigations.AddRange(navigations);
            context.SaveChanges();
        }

        private static void SeedOptions(RestDBContext context)
        {
            var options = new List<Option>
            {
                new Option { Name = "title", Key = "site-title", Value = "Chefer", Relation = "general", Order = 1, IsSystem = true },
                new Option { Name = "logo", Key = "site-logo", Value = "~/img/logo.png", Relation = "general", Order = 2, IsSystem = true },
                new Option { Name = "phone", Key = "contact-phone", Value = "+012 345 6789", Relation = "contact", Order = 1 },
                new Option { Name = "email", Key = "contact-email", Value = "info@example.com", Relation = "contact", Order = 2 },
                new Option { Name = "address", Key = "contact-address", Value = "123 Street, New York, USA", Relation = "contact", Order = 3 }
            };

            context.Options.AddRange(options);
            context.SaveChanges();
        }

        private static void SeedCategories(RestDBContext context)
        {
            if (context.Categories.Any()) return;

            var categories = new List<Category>
            {
                new Category { Title = "Breakfast", Slug = "breakfast", Description = "Start your day right", ImgSrc = "~/img/category-1.jpg", ImgAlt = "Breakfast" },
                new Category { Title = "Lunch", Slug = "lunch", Description = "Midday meals", ImgSrc = "~/img/category-2.jpg", ImgAlt = "Lunch" },
                new Category { Title = "Dinner", Slug = "dinner", Description = "Evening delights", ImgSrc = "~/img/category-3.jpg", ImgAlt = "Dinner" },
                new Category { Title = "Desserts", Slug = "desserts", Description = "Sweet endings", ImgSrc = "~/img/category-4.jpg", ImgAlt = "Desserts", ParentId = null }
            };

            context.Categories.AddRange(categories);
            context.SaveChanges();
        }

        private static void SeedTags(RestDBContext context)
        {
            if (context.Tags.Any()) return;

            var tags = new List<Tag>
            {
                new Tag { Title = "Healthy", Slug = "healthy" },
                new Tag { Title = "Quick", Slug = "quick" },
                new Tag { Title = "Vegetarian", Slug = "vegetarian" },
                new Tag { Title = "Grill", Slug = "grill" },
                new Tag { Title = "Italian", Slug = "italian" }
            };

            context.Tags.AddRange(tags);
            context.SaveChanges();
        }

        private static void SeedPosts(RestDBContext context)
        {
            if (context.Posts.Any()) return;

            var posts = new List<Post>
            {
                new Post
                {
                    Title = "The Art of Perfect Steak",
                    Content = "Detailed guide on how to cook the perfect steak at home. From choosing the cut to resting time...",
                    Slogan = "Master the grill like a pro chef",
                    Slug = "the-art-of-perfect-steak",
                    Status = PostStatuses.Published,
                    ImgSrc = "~/img/menu-1.jpg",
                    ImgAlt = "Perfect grilled steak",
                    DateOFCreated = DateTime.UtcNow,
                    DateOFPublished = DateTime.UtcNow.AddDays(-10),
                    DateOFLastUpdated = DateTime.UtcNow
                },
                new Post
                {
                    Title = "Healthy Breakfast Ideas",
                    Content = "Five nutritious breakfast recipes that are quick and delicious. Perfect for busy mornings...",
                    Slogan = "Fuel your body the right way",
                    Slug = "healthy-breakfast-ideas",
                    Status = PostStatuses.Published,
                    ImgSrc = "~/img/menu-2.jpg",
                    ImgAlt = "Healthy breakfast plate",
                    DateOFCreated = DateTime.UtcNow,
                    DateOFPublished = DateTime.UtcNow.AddDays(-5),
                    DateOFLastUpdated = DateTime.UtcNow
                },
                new Post
                {
                    Title = "Classic Italian Pasta Carbonara",
                    Content = "Authentic Roman carbonara recipe without cream. Just eggs, cheese, guanciale and black pepper...",
                    Slogan = "Taste of Italy in your kitchen",
                    Slug = "classic-italian-pasta-carbonara",
                    Status = PostStatuses.Published,
                    ImgSrc = "~/img/menu-3.jpg",
                    ImgAlt = "Italian pasta carbonara",
                    DateOFCreated = DateTime.UtcNow,
                    DateOFPublished = DateTime.UtcNow.AddDays(-2),
                    DateOFLastUpdated = DateTime.UtcNow
                }
            };

            context.Posts.AddRange(posts);
            context.SaveChanges();
        }

        private static void SeedPostCategoriesAndTags(RestDBContext context)
        {
            var posts = context.Posts.ToList();
            var categories = context.Categories.ToList();
            var tags = context.Tags.ToList();

            if (!posts.Any() || !categories.Any() || !tags.Any()) return;

            var postCategories = new List<PostCategories>();
            var postTags = new List<PostTags>();

            var breakfastCat = categories.FirstOrDefault(c => c.Slug == "breakfast");
            var grillTag = tags.FirstOrDefault(t => t.Slug == "grill");
            var healthyTag = tags.FirstOrDefault(t => t.Slug == "healthy");

            if (breakfastCat != null && posts.Count > 1)
            {
                postCategories.Add(new PostCategories { PostId = posts[1].Id, CategoryId = breakfastCat.Id });
            }

            if (grillTag != null && posts.Count > 0)
            {
                postTags.Add(new PostTags { PostId = posts[0].Id, TagId = grillTag.Id });
            }

            if (healthyTag != null && posts.Count > 1)
            {
                postTags.Add(new PostTags { PostId = posts[1].Id, TagId = healthyTag.Id });
            }

            context.PostCategories.AddRange(postCategories);
            context.PostTags.AddRange(postTags);
            context.SaveChanges();
        }

        private static void SeedComments(RestDBContext context)
        {
            if (context.Comments.Any()) return;

            var posts = context.Posts.ToList();
            if (!posts.Any()) return;

            var steakPost = posts.FirstOrDefault(p => p.Slug == "the-art-of-perfect-steak");
            if (steakPost == null) return;

            var comments = new List<Comment>
            {
                new Comment
                {
                    Name = "Anna Chef",
                    Email = "anna@example.com",
                    Message = "This recipe changed my weekend BBQs forever! Thank you so much!",
                    DateOfPublished = DateTime.UtcNow.AddDays(-9),
                    IsValid = true,
                    PostId = steakPost.Id
                },
                new Comment
                {
                    Name = "Mike GrillMaster",
                    Email = "mike@example.com",
                    Message = "What internal temperature do you recommend for medium-rare?",
                    DateOfPublished = DateTime.UtcNow.AddDays(-8),
                    IsValid = true,
                    PostId = steakPost.Id
                }
            };

            context.Comments.AddRange(comments);
            context.SaveChanges();

            var question = context.Comments.FirstOrDefault(c => c.Message.Contains("temperature"));
            if (question != null)
            {
                var reply = new Comment
                {
                    Name = "John Doe",
                    Email = "john@example.com",
                    Message = "For medium-rare, aim for 130-135°F (54-57°C) internal temperature. Don't forget to rest the steak 5-10 minutes after grilling!",
                    DateOfPublished = DateTime.UtcNow.AddDays(-7),
                    IsValid = true,
                    PostId = steakPost.Id,
                    ParentId = question.Id
                };

                context.Comments.Add(reply);
                context.SaveChanges();
            }
        }
    }
}