using System.Security.Claims;
using SastImg.Application.SeedWorks;
using Shared.Primitives.Request;

namespace SastImg.Application.AlbumServices.GetAlbum
{
    public sealed class GetAlbumQueryRequest(long albumId, ClaimsPrincipal user)
        : IQueryRequest<DetailedAlbumDto?>
    {
        public long AlbumId { get; private init; } = albumId;
        public RequesterInfo Requester { get; private init; } = new(user);
    }
}
