using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;

namespace H.W12.Models
{
    public class NewsArticle
    {
        [Key]
        public int ArticleId { get; set; }

        [Required]
        [MaxLength(200)]
        public string Title { get; set; }

        [Required]
        public string Content { get; set; }

        public string AuthorId { get; set; } // Связь с автором (пользователем)

        [DataType(DataType.DateTime)]
        public DateTime CreatedAt { get; set; } = DateTime.Now;

        [DataType(DataType.DateTime)]
        public DateTime? UpdatedAt { get; set; }

        // Навигационное свойство для связи с пользователем
        public IdentityUser Author { get; set; }

    }
}
