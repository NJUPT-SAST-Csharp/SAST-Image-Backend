using Identity;
using Mediator;

namespace Account.Application.Endpoints.AccountEndpoints.ChangePassword;

public sealed record class ChangePasswordCommand(string NewPassword, Requester Requester)
    : ICommand;
