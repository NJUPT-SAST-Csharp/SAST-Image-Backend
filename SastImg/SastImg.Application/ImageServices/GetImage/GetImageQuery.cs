using System.Security.Claims;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.GetImage
{
    public sealed class GetImageQuery(long albumId, long imageId, ClaimsPrincipal user)
        : IQueryRequest<DetailedImageDto?>
    {
        public AlbumId AlbumId { get; } = new(albumId);
        public ImageId ImageId { get; } = new(imageId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
