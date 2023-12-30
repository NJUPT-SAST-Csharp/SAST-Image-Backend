using System.Security.Claims;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Application.SeedWorks;
using Shared.Primitives.Request;

namespace SastImg.Application.AlbumServices.GetRemovedAlbums
{
    public sealed class GetRemovedAlbumsQueryRequest(long authorId, ClaimsPrincipal user)
        : IQueryRequest<IEnumerable<AlbumDto>>
    {
        public long AuthorId { get; } = authorId;

        public RequesterInfo Requester { get; } = new(user);
    }
}
