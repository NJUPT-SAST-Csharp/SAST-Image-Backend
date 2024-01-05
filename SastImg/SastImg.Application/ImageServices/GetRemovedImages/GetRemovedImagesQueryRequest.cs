using System.Security.Claims;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.SeedWorks;
using Shared.Primitives.Request;

namespace SastImg.Application.ImageServices.GetRemovedImages
{
    public sealed class GetRemovedImagesQueryRequest(long authorId, ClaimsPrincipal user)
        : IQueryRequest<IEnumerable<AlbumImageDto>>
    {
        public long AuthorId { get; } = authorId;
        public RequesterInfo Requester { get; } = new(user);
    }
}
