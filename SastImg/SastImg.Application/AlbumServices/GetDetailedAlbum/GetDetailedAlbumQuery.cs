using System.Security.Claims;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.GetDetailedAlbum
{
    public sealed class GetDetailedAlbumQuery(long albumId, ClaimsPrincipal user)
        : IQueryRequest<DetailedAlbumDto?>
    {
        public AlbumId AlbumId { get; private init; } = new(albumId);
        public RequesterInfo Requester { get; private init; } = new(user);
    }
}
