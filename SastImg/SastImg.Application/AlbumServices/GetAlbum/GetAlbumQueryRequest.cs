using System.Security.Claims;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.GetAlbum
{
    public sealed class GetAlbumQueryRequest(long albumId, ClaimsPrincipal user)
        : IQueryRequest<DetailedAlbumDto?>
    {
        public AlbumId AlbumId { get; private init; } = new(albumId);
        public RequesterInfo Requester { get; private init; } = new(user);
    }
}
