using System.Security.Claims;
using SastImg.Application.ImageServices.GetImages;
using SastImg.Application.SeedWorks;
using Shared.Primitives.Request;

namespace SastImg.Application.ImageServices.SearchImages
{
    public sealed class SearchImagesQueryRequest(
        int page,
        IEnumerable<long> tags,
        SearchOrder order,
        ClaimsPrincipal user
    ) : IQueryRequest<IEnumerable<ImageDto>>
    {
        public int Page { get; } = page;
        public SearchOrder Order { get; } = order;
        public IEnumerable<long> Tags { get; } = tags;
        public RequesterInfo Requester { get; } = new(user);
    }
}
