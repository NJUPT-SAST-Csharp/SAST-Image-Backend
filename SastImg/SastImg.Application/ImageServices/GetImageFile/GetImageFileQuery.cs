using System.Security.Claims;
using SastImg.Application.SeedWorks;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.GetImageFile
{
    public sealed class GetImageFileQuery(long imageId, bool isThumbnail, ClaimsPrincipal user)
        : IQueryRequest<Stream?>
    {
        public RequesterInfo Requester { get; } = new(user);
        public ImageId ImageId { get; } = new(imageId);
        public bool IsThumbnail { get; } = isThumbnail;
    }
}
