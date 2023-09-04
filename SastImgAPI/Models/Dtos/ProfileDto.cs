using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace SastImgAPI.Models.Dtos
{
    public record ProfileDto(string Nickname, string Biography = "", string Website = "");
}
