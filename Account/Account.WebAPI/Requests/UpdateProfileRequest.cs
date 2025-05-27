using System.Security.Claims;
using Account.Application.UserServices.UpdateProfile;

namespace Account.WebAPI.Requests;

public readonly struct UpdateProfileRequest
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
