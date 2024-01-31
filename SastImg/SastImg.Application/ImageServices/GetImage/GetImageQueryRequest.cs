using System.Security.Claims;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.GetImage
{
    public sealed class GetImageQueryRequest(long imageId, ClaimsPrincipal user)
        : IQueryRequest<DetailedImageDto?>
    {
        public ImageId ImageId { get; } = new(imageId);
        public RequesterInfo Requester { get; } = new(user);
    }
}
