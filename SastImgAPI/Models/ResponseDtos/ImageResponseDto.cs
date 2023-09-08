using SastImgAPI.Models.DbSet;

namespace SastImgAPI.Models.ResponseDtos
{
    public record ImageResponseDto(
        int Id,
        string Url,
        string Title,
        string? Description,
        string Category,
        DateTime CreatedAt,
        string Author,
        Accessibility Accessibility,
        ICollection<string> Tags
    );
}
