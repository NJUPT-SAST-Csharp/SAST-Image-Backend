using System.Security.Claims;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.SeedWorks;
using SastImg.Domain;
using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.GetRemovedImages
{
    public sealed class GetRemovedImagesQuery(long authorId, ClaimsPrincipal user)
        : IQueryRequest<IEnumerable<AlbumImageDto>>
    {
        public UserId AuthorId { get; } = new(authorId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
