using System.Security.Claims;
using SastImg.Application.SeedWorks;
using Shared.Primitives.Request;

namespace SastImg.Application.ImageServices.SearchImages
{
    public sealed class SearchImagesQueryRequest(
        int page,
        SearchOrder order,
        long categoryId,
        long[] tags,
        ClaimsPrincipal user
    ) : IQueryRequest<IEnumerable<SearchedImageDto>>
    {
        public int Page { get; } = page;
        public SearchOrder Order { get; } = order;
        public long CategoryId { get; } = categoryId;
        public long[] Tags { get; } = tags;
        public RequesterInfo Requester { get; } = new(user);
    }
}
