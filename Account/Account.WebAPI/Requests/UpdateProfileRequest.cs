using System.Security.Claims;
using Account.Application.UserServices.UpdateProfile;
using Account.WebAPI.SeedWorks;

namespace Account.WebAPI.Requests
{
    public readonly struct UpdateProfileRequest : ICommandRequestObject<UpdateProfileCommand>
    {
        public readonly string Nickname { get; init; }
        public readonly string Biography { get; init; }
        public readonly DateOnly? Birthday { get; init; }
        public readonly Uri? Website { get; init; }

        public UpdateProfileCommand ToCommand(ClaimsPrincipal user)
        {
            return new(Nickname, Biography, Birthday, Website, user);
        }
    }
}
