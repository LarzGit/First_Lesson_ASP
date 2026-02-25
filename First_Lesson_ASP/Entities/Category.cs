using System.ComponentModel.DataAnnotations.Schema;
using System.Collections.Generic;

namespace First_Lesson_ASP.Entities
{
    public class Category
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Slug { get; set; } = string.Empty;
        public string ImgSrc { get; set; } = string.Empty;
        public string ImgAlt { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;

        public int? ParentId { get; set; }

        [ForeignKey("ParentId")]
        public Category? Parent { get; set; }

        public ICollection<Category> Childs { get; set; } = new List<Category>();

       
        public ICollection<Post> Posts { get; set; } = new List<Post>();
    }
}