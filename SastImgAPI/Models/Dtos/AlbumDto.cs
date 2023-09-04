using SastImgAPI.Models.DbSet;

namespace SastImgAPI.Models.Dtos
{
    public record AlbumDto(string Name, string Description, Accessibility Accessibility);
}
