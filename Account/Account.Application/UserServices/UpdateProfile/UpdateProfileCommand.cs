using Identity;
using Mediator;

namespace Account.Application.UserServices.UpdateProfile;

public sealed record class UpdateProfileCommand(
    string Nickname,
    string Biography,
    DateOnly? Birthday,
    Uri? Website,
    Requester Requester
) : ICommand;
