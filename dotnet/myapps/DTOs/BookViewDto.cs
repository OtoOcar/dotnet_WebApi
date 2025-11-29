namespace libreria.WebApi.DTOs
{
    public record BookViewDto(
        int Id,
        string Title,
        string Author,
        int PublishYear,
        string? CoverUrl,
        int? Rating,
        string? Comment
    );
}
