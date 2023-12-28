using System.Security.Claims;
using Shared.Primitives.Request;

namespace SastImg.Application.AlbumServices.GetAlbums
{
    public class GetAlbumsQueryRequest(
        int page,
        long authorId,
        long categoryId,
        ClaimsPrincipal user
    ) : IQueryRequest<IEnumerable<AlbumDto>>
    {
        public int Page { get; } = page;
        public long CategoryId { get; } = categoryId;
        public long AuthorId { get; } = authorId;
        public RequesterInfo Requester { get; } = new(user);
    }
}
