using System.Security.Claims;
using Identity;
using Mediator;
using SastImg.Domain.AlbumTagEntity;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Application.ImageServices.SearchImages;

public sealed class SearchImagesQuery(
    int page,
    SearchOrder order,
    int categoryId,
    long[] tags,
    ClaimsPrincipal user
) : IQuery<IEnumerable<SearchedImageDto>>
{
    public int Page { get; } = page;
    public SearchOrder Order { get; } = order;
    public CategoryId CategoryId { get; } = new() { Value = categoryId };
    public ImageTagId[] Tags { get; } = tags.Select(t => new ImageTagId() { Value = t }).ToArray();
    public Requester Requester { get; } = new(user);
}
