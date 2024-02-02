using System.Security.Claims;
using Primitives.Command;
using SastImg.Application.SeedWorks;
using SNS.Domain.UserEntity;

namespace SNS.Application.UserServices.Follow
{
    public sealed class FollowCommand(long userId, ClaimsPrincipal user) : ICommandRequest
    {
        public UserId FollowTarget { get; } = new(userId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
