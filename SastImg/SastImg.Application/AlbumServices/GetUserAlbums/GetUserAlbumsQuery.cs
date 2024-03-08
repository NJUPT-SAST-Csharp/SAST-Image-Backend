using System.Security.Claims;
using SastImg.Application.SeedWorks;
using SastImg.Domain;
using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.GetAlbums
{
    public sealed class GetUserAlbumsQuery(long authorId, ClaimsPrincipal user)
        : IQueryRequest<IEnumerable<UserAlbumDto>>
    {
        public UserId AuthorId { get; } = new(authorId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
