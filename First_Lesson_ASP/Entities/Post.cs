namespace First_Lesson_ASP.Entities
{
    public enum PostStatuses
    {
        New,
        Draft,
        Published,
        Archived
    }

    public class Post
    {
        public int Id { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public string Slogan { get; set; } = string.Empty;

        // ──────────────── НОВЕ ────────────────
        public string Slug { get; set; } = string.Empty;
        public PostStatuses Status { get; set; } = PostStatuses.New;
        public string ImgSrc { get; set; } = string.Empty;
        public string ImgAlt { get; set; } = string.Empty;

        public DateTime DateOFCreated { get; set; } = DateTime.Now;
        public DateTime? DateOFPublished { get; set; } = null;
        public DateTime DateOFLastUpdated { get; set; } = DateTime.Now;

        public ICollection<Category> Categories { get; set; } = new List<Category>();
        public ICollection<Tag> Tags { get; set; } = new List<Tag>();

        // ──────────────── НОВЕ ────────────────
        public ICollection<Comment> Comments { get; set; } = new List<Comment>();
    }
}