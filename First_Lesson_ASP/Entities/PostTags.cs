using System.ComponentModel.DataAnnotations.Schema;

namespace First_Lesson_ASP.Entities
{
    public class PostTags
    {
        public int Id { get; set; }

        [ForeignKey("PostId")]
        public int PostId { get; set; }

        [ForeignKey("TagId")]
        public int TagId { get; set; }
    }
}