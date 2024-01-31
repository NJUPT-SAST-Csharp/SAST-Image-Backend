using System.Security.Claims;
using SastImg.Application.AlbumServices.GetAlbums;
using SastImg.Application.SeedWorks;
using SastImg.Domain;
using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.GetRemovedAlbums
{
    public sealed class GetRemovedAlbumsQuery(long authorId, ClaimsPrincipal user)
        : IQueryRequest<IEnumerable<AlbumDto>>
    {
        public UserId AuthorId { get; } = new(authorId);

        public RequesterInfo Requester { get; } = new(user);
    }
}
