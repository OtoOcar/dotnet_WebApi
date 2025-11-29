namespace libreria.WebApi.DTOs
{
    public record BookUpdateDto(
        string Title,
        string Author,
        int PublishYear,
        string? CoverUrl,
        int? Rating,
        string? Comment
    );
}
