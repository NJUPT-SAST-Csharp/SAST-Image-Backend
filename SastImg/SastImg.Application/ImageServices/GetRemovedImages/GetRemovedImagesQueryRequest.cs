using System.Security.Claims;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.SeedWorks;
using SastImg.Domain;
using Shared.Primitives.Request;

namespace SastImg.Application.ImageServices.GetRemovedImages
{
    public sealed class GetRemovedImagesQueryRequest(long authorId, ClaimsPrincipal user)
        : IQueryRequest<IEnumerable<AlbumImageDto>>
    {
        public UserId AuthorId { get; } = new(authorId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
