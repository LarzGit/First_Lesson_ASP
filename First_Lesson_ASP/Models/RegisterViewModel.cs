using System.ComponentModel.DataAnnotations;

namespace First_Lesson_ASP.Models
{
    public class RegisterViewModel
    {
        [Required(ErrorMessage = "Вкажіть Email")]
        [EmailAddress]
        public string Email { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вкажіть пароль")]
        [DataType(DataType.Password)]
        [MinLength(6, ErrorMessage = "Пароль має бути не менше 6 символів")]
        public string Password { get; set; } = string.Empty;

        public string? FullName { get; set; } // Для твого поля в Register.cshtml
    }
}