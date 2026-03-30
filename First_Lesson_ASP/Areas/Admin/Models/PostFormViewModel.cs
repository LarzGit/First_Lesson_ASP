using Microsoft.AspNetCore.Http;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using First_Lesson_ASP.Entities;

namespace First_Lesson_ASP.Areas.Admin.Models
{
    public class PostFormViewModel
    {
        public int Id { get; set; }

        [Required(ErrorMessage = "Заголовок обов'язковий")]
        public string Title { get; set; } = string.Empty;

        [Required(ErrorMessage = "Slug обов'язковий")]
        public string Slug { get; set; } = string.Empty;

        public string Slogan { get; set; } = string.Empty;

        [Required(ErrorMessage = "Вміст обов'язковий")]
        public string Content { get; set; } = string.Empty;

        public PostStatuses Status { get; set; } = PostStatuses.New;

        // --- Зображення ---
        public IFormFile? UploadedImage { get; set; }

        // Зберігає старий шлях до картинки при редагуванні, щоб не загубити її
        public string? ExistingImagePath { get; set; }

        public string ImgAlt { get; set; } = string.Empty;

        // --- Категорії ---
        public List<int> SelectedCategoryIds { get; set; } = new List<int>();
    }
}