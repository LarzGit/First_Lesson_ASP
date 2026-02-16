using System.ComponentModel.DataAnnotations;

namespace First_Lesson_ASP.Entities
{
    public class ClientMessage
    {
        [Required(ErrorMessage = "Вкажіть ім'я")]
        [StringLength(100, MinimumLength = 2, ErrorMessage = "Ім'я від 2 до 100 символів")]
        [Display(Name = "Ім'я")]
        public string Name { get; set; } = "";

        [Required(ErrorMessage = "Вкажіть email")]
        [EmailAddress(ErrorMessage = "Неправильний email")]
        [Display(Name = "Email")]
        public string Email { get; set; } = "";

        [Required(ErrorMessage = "Вкажіть тему")]
        [StringLength(150, MinimumLength = 3, ErrorMessage = "Тема від 3 до 150 символів")]
        [Display(Name = "Тема")]
        public string Subject { get; set; } = "";

        [Required(ErrorMessage = "Напишіть повідомлення")]
        [MinLength(20, ErrorMessage = "Повідомлення мінімум 20 символів")]
        [MaxLength(2000, ErrorMessage = "Повідомлення максимум 2000 символів")]
        [Display(Name = "Повідомлення")]
        public string Message { get; set; } = "";

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    }
}