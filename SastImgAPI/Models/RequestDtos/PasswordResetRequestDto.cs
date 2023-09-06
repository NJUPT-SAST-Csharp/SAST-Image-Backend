namespace SastImgAPI.Models.RequestDtos
{
    public record PasswordResetRequestDto(string NewPassword, string ConfirmPassword);
}
