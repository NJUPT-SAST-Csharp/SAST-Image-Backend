using Microsoft.AspNetCore.Http;
using Primitives.Command;
using SastImg.Application.SeedWorks;
using System.Security.Claims;

namespace SNS.Application.UserServices.UpdateAvatar
{
    public sealed class UpdateAvatarCommand(IFormFile file, ClaimsPrincipal user) : ICommandRequest
    {
        public IFormFile AvatarFile { get; } = file;

        public RequesterInfo Requester { get; } = new(user);
    }
}
