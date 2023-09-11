using System.Text;

namespace SastImgAPI.Models.RequestDtos
{
    public record ImageRequestDto(
        IFormFile ImageFile,
        string AlbumId,
        string Title,
        string[]? Tags,
        string Category,
        string Description = "",
        bool IsExifEnabled = false
    );
}
