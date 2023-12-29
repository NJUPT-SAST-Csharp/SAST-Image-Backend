using System.Security.Claims;
using SastImg.Application.SeedWorks;
using Shared.Primitives.Request;

namespace SastImg.Application.ImageServices.GetImage
{
    public sealed class GetImageQueryRequest(long imageId, ClaimsPrincipal user)
        : IQueryRequest<DetailedImageDto?>
    {
        public long ImageId { get; } = imageId;
        public RequesterInfo Requester { get; } = new(user);
    }
}
