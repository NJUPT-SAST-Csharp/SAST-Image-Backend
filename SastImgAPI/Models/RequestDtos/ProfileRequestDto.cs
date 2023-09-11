using Microsoft.EntityFrameworkCore.Storage.ValueConversion.Internal;

namespace SastImgAPI.Models.RequestDtos
{
    public record ProfileRequestDto(string Nickname, string Biography = "");
}
