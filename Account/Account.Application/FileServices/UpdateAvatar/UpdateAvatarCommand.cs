using System.Security.Claims;
using Account.Application.SeedWorks;
using Microsoft.AspNetCore.Http;
using Primitives.Command;

namespace Account.Application.FileServices.UpdateAvatar
{
    public sealed class UpdateAvatarCommand(IFormFile file, ClaimsPrincipal user) : ICommandRequest
    {
        public IFormFile Avatar { get; } = file;
        public RequesterInfo Requester { get; } = new(user);
    }
}
