using System.Security.Claims;
using Microsoft.AspNetCore.Http;
using Primitives.Command;
using SastImg.Application.SeedWorks;

namespace SNS.Application.UserServices.UpdateHeader
{
    public sealed class UpdateHeaderCommand(IFormFile file, ClaimsPrincipal user) : ICommandRequest
    {
        public Stream HeaderFile { get; } = file.OpenReadStream();
        public RequesterInfo Requester { get; } = new(user);
    }
}
