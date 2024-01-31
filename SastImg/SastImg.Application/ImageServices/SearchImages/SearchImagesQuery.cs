using System.Security.Claims;
using SastImg.Application.SeedWorks;
using SastImg.Domain.CategoryEntity;
using SastImg.Domain.TagEntity;
using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.SearchImages
{
    public sealed class SearchImagesQuery(
        int page,
        SearchOrder order,
        long categoryId,
        long[] tags,
        ClaimsPrincipal user
    ) : IQueryRequest<IEnumerable<SearchedImageDto>>
    {
        public int Page { get; } = page;
        public SearchOrder Order { get; } = order;
        public CategoryId CategoryId { get; } = new(categoryId);
        public TagId[] Tags { get; } = tags.Select(t => new TagId(t)).ToArray();
        public RequesterInfo Requester { get; } = new(user);
    }
}
