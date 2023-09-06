using System.ComponentModel.DataAnnotations;

namespace SastImgAPI.Models.RequestDtos
{
    public record LoginRequestDto(string Username, string Password);
}
