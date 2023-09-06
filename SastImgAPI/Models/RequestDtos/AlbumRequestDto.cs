using SastImgAPI.Models.DbSet;

namespace SastImgAPI.Models.RequestDtos
{
    public record AlbumRequestDto(string Name, string Description, Accessibility Accessibility);
}
