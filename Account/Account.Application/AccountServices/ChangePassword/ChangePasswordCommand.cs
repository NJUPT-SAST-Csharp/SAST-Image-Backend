using System.Security.Claims;
using Account.Application.SeedWorks;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.ChangePassword
{
    public sealed class ChangePasswordCommand(string newPassword, ClaimsPrincipal user)
        : ICommandRequest
    {
        public string NewPassword { get; } = newPassword;
        public RequesterInfo Requester { get; } = new(user);
    }
}
