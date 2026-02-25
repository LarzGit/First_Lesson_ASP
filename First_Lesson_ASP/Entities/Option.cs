namespace First_Lesson_ASP.Entities
{
    public class Option
    {
        public int Id { get; set; }
        public string Name { get; set; } = string.Empty;
        public string Key { get; set; } = string.Empty;
        public string Value { get; set; } = string.Empty;
        public string Relation { get; set; } = string.Empty;
        public int Order { get; set; }
        public bool IsSystem { get; set; } = false;
    }
}