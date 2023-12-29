using System.Security.Claims;
using Shared.Primitives.Request;

namespace SastImg.Application.ImageServices.GetDetailedImage
{
    public sealed class GetImageQueryRequest(long imageId, ClaimsPrincipal user)
        : IQueryRequest<DetailedImageDto>
    {
        public long ImageId { get; } = imageId;
        public RequesterInfo Requester { get; } = new(user);
    }
}
