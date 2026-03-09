using Microsoft.AspNetCore.Identity;

namespace First_Lesson_ASP.Entities
{
    public class User : IdentityUser
    {
        public string? FullName { get; set; }
        public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;
    }
}