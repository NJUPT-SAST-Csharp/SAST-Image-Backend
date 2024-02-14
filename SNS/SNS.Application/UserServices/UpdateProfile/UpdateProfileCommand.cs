using System.Security.Claims;
using Primitives.Command;
using SastImg.Application.SeedWorks;

namespace SNS.Application.UserServices.UpdateProfile
{
    public sealed class UpdateProfileCommand(
        string nickname,
        string biography,
        ClaimsPrincipal user
    ) : ICommandRequest
    {
        public string Nickname { get; } = nickname;
        public string Biography { get; } = biography;

        public RequesterInfo Requester { get; } = new(user);
    }
}
