using System.Security.Claims;
using SastImg.Application.SeedWorks;
using SastImg.Domain;
using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.GetAlbums
{
    public sealed class GetAlbumsQueryRequest(int page, long authorId, ClaimsPrincipal user)
        : IQueryRequest<IEnumerable<AlbumDto>>
    {
        public int Page { get; } = page;
        public UserId AuthorId { get; } = new(authorId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
