using System.Text.Json.Serialization;

namespace libreria.WebApi.Models
{
    public class Book
    {
        public int Id { get; set; }
        public int UserId { get; set; }
        public string Title { get; set; } = null!;
        public string Author { get; set; } = null!;
        public int PublishYear { get; set; }
        public string? CoverUrl { get; set; }

        // Campos opcionales para reseña
        public int? Rating { get; set; }   // 1 a 5
        public string? Comment { get; set; }

        [JsonIgnore] // no se serializa
        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        [JsonIgnore] // no se serializa
        public Users? User { get; set; } = null!;

    }

}