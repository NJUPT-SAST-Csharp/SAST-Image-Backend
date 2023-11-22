using System.Security.Claims;
using SastImg.Application.Albums.Dtos;
using Shared.Primitives.Query;

namespace SastImg.Application.Albums.GetAlbums
{
    public sealed class GetAlbumsQuery(ClaimsPrincipal user, int page, long queryUserId = 0)
        : IQuery<IEnumerable<AlbumDto>>
    {
        public int Page { get; } = page;
        public long AuthorId { get; } = queryUserId;
        public ClaimsPrincipal User { get; } = user;
    }
}
