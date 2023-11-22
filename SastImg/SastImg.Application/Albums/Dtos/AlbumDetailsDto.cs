using SastImg.Application.Images.Dtos;
using SastImg.Domain.Albums;

namespace SastImg.Application.Albums.Dtos
{
    public record AlbumDetailsDto(
        long AlbumId,
        long AuthorId,
        string Title,
        string Description,
        Accessibility Accessibility,
        DateTime CreatedAt,
        DateTime UpdatedAt,
        Uri CoverUri,
        IEnumerable<long> Collaborators,
        IEnumerable<ImageDto> Images
    );
}
