namespace First_Lesson_ASP.Entities
{
    public class Navigate
    {
        public int Id { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Href { get; set; } = string.Empty;

        public int Order {  get; set; }

        public int? ProductId {  get; set; } = null;

        public ICollection<Navigate> Childs { get; set; } = new List<Navigate>();
    }
}
