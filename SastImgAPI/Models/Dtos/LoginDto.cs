using System.ComponentModel.DataAnnotations;

namespace SastImgAPI.Models.Dtos
{
    public record LoginDto(string Username, string Password);
}
