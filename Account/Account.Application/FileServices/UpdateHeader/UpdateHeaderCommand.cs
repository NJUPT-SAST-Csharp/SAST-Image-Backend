using System.Security.Claims;
using Account.Application.SeedWorks;
using Microsoft.AspNetCore.Http;
using Primitives.Command;

namespace Account.Application.FileServices.UpdateHeader
{
    public sealed class UpdateHeaderCommand(IFormFile file, ClaimsPrincipal user) : ICommandRequest
    {
        public IFormFile Header { get; } = file;
        public RequesterInfo Requester { get; } = new(user);
    }
}
