namespace SastImgAPI.Models.Dtos
{
    public record ImageDto(
        IFormFile ImageFile,
        int AlbumId,
        string Title,
        string[] Tags,
        string Description = "",
        bool IsExifEnabled = false,
        int CategoryId = 1
    );
}
