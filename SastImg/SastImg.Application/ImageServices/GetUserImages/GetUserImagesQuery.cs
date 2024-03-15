using System.Security.Claims;
using SastImg.Application.SeedWorks;
using SastImg.Domain;
using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.GetUserImages
{
    public sealed class GetUserImagesQuery(long userId, int page, ClaimsPrincipal user)
        : IQueryRequest<IEnumerable<UserImageDto>>
    {
        public UserId UserId { get; } = new(userId);
        public int Page { get; } = page;
        public RequesterInfo Requester { get; } = new(user);
    }
}
