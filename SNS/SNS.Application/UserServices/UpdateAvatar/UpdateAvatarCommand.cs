using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Primitives.Command;
using SastImg.Application.SeedWorks;

namespace SNS.Application.UserServices.UpdateAvatar
{
    public sealed class UpdateAvatarCommand(IFormFile file, ClaimsPrincipal user) : ICommandRequest
    {
        public Stream AvatarFile { get; } = file.OpenReadStream();

        public RequesterInfo Requester { get; } = new(user);
    }
}
