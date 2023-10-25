using SastImg.Domain.Albums;

namespace SastImg.Application.Albums.Dtos
{
    public record AlbumDto(
        long AuthorId,
        long AlbumId,
        string Title,
        Accessibility Accessibility,
        Uri CoverUri
    );
}
