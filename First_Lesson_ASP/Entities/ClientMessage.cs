using System.ComponentModel.DataAnnotations;

namespace First_Lesson_ASP.Entities
{
    public class ClientMessage
    {
        [Key]
        public int Id { get; set; }                    

        [Required(ErrorMessage = "Вкажіть ім'я")]
        [StringLength(100, MinimumLength = 2)]
        [Display(Name = "Ім'я")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Вкажіть email")]
        [EmailAddress]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Вкажіть тему")]
        [StringLength(150, MinimumLength = 3)]
        [Display(Name = "Тема")]
        public string Subject { get; set; } = "";

        [Required(ErrorMessage = "Напишіть повідомлення")]
        [MinLength(20)]
        [MaxLength(2000)]
        [Display(Name = "Повідомлення")]
        public string Message { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}