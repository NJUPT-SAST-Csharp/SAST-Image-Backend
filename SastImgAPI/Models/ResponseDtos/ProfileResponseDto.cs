namespace SastImgAPI.Models.ResponseDtos
{
    public record ProfileResponseDto(
        string Username,
        string Nickname,
        string? Email,
        string Biography,
        string Website,
        string Avatar,
        string Header,
        DateTime RegisteredAt
    );
}
