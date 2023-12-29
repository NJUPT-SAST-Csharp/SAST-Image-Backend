using System.Security.Claims;
using SastImg.Application.SeedWorks;
using Shared.Primitives.Request;

namespace SastImg.Application.AlbumServices.GetAlbums
{
    public sealed class GetAlbumsQueryRequest(int page, long authorId, ClaimsPrincipal user)
        : IQueryRequest<IEnumerable<AlbumDto>>
    {
        public int Page { get; } = page;
        public long AuthorId { get; } = authorId;
        public RequesterInfo Requester { get; } = new(user);
    }
}
