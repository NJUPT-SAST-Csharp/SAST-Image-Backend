using System.Security.Claims;
using SastImg.Application.ImageServices.GetAlbumImages;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.GetRemovedImages
{
    public sealed class GetRemovedImagesQuery(long albumId, ClaimsPrincipal user)
        : IQueryRequest<IEnumerable<AlbumImageDto>>
    {
        public AlbumId AlbumId { get; } = new(albumId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
