using SastImgAPI.Models.Identity;

namespace SastImgAPI.Models.ResponseDtos
{
    public class ProfileResponseDto
    {
        public ProfileResponseDto(User user)
        {
            Username = user.UserName!;
            Nickname = user.Nickname;
            Email = user.Email!;
            Biography = user.Biography;
            Avatar = user.Avatar;
            Header = user.Header;
            RegisterAt = user.RegisteredAt;
        }

        public string Username { get; init; }
        public string Nickname { get; init; }
        public string Email { get; init; }
        public string Biography { get; init; }
        public Uri? Avatar { get; init; }
        public Uri? Header { get; init; }
        public DateTime RegisterAt { get; init; }
    }
}
