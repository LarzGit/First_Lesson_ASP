using System.ComponentModel.DataAnnotations;

namespace First_Lesson_ASP.Entities
{
    public class Comment
    {
        public int Id { get; set; }

        [Required]
        [StringLength(100)]
        public string Name { get; set; } = string.Empty;

        [Required]
        [EmailAddress]
        [StringLength(150)]
        public string Email { get; set; } = string.Empty;

        [Required]
        [StringLength(2000, MinimumLength = 10)]
        public string Message { get; set; } = string.Empty;

        public DateTime DateOfPublished { get; set; } = DateTime.UtcNow;

        public bool IsValid { get; set; } = false;         

        public int PostId { get; set; }

        public Post? Post { get; set; }

        public int? ParentId { get; set; }                 

        public Comment? Parent { get; set; }

        public ICollection<Comment> Childs { get; set; } = new List<Comment>();
    }
}