using System.Security.Claims;
using Shared.Primitives.Request;

namespace SastImg.Application.ImageServices.GetImages
{
    public sealed class GetImagesQueryRequest(long albumId, int page, ClaimsPrincipal user)
        : IQueryRequest<IEnumerable<ImageDto>>
    {
        public long AlbumId { get; } = albumId;
        public int Page { get; } = page;
        public RequesterInfo Requester { get; } = new(user);
    }
}
