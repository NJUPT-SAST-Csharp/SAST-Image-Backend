using System.Security.Claims;
using Account.Application.SeedWorks;
using Primitives.Command;

namespace Account.Application.UserServices.UpdateProfile
{
    public sealed class UpdateProfileCommand(
        string nickname,
        string biography,
        DateOnly? birthday,
        Uri? website,
        ClaimsPrincipal user
    ) : ICommandRequest
    {
        public string Nickname { get; } = nickname;
        public string Biography { get; } = biography;
        public DateOnly? Birthday { get; } = birthday;
        public Uri? Website { get; } = website;

        public RequesterInfo Requester { get; } = new(user);
    }
}
