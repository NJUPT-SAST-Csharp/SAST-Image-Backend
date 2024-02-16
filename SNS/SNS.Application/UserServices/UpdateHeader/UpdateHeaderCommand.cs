using Microsoft.AspNetCore.Http;
using Primitives.Command;
using SastImg.Application.SeedWorks;
using System.Security.Claims;

namespace SNS.Application.UserServices.UpdateHeader
{
    public sealed class UpdateHeaderCommand(IFormFile file, ClaimsPrincipal user) : ICommandRequest
    {
        public IFormFile HeaderFile { get; } = file;
        public RequesterInfo Requester { get; } = new(user);
    }
}
