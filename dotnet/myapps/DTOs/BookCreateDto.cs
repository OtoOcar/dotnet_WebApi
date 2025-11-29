namespace libreria.WebApi.DTOs
{
    public record BookCreateDto(string Title, string Author, int PublishYear, string? CoverUrl);
}
