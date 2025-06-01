using Mediator;

namespace SastImg.Application.ImageServices.SearchImages;

internal sealed class SearchImagesQueryHandler(ISearchImagesRepository repository)
    : IQueryHandler<SearchImagesQuery, IEnumerable<SearchedImageDto>>
{
    public async ValueTask<IEnumerable<SearchedImageDto>> Handle(
        SearchImagesQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.Requester.IsAdmin)
        {
            return await repository.SearchImagesByAdminAsync(
                request.Page,
                request.CategoryId,
                request.Tags,
                cancellationToken
            );
        }
        else
        {
            return await repository.SearchImagesByUserAsync(
                request.Page,
                request.CategoryId,
                request.Tags,
                request.Requester.Id,
                cancellationToken
            );
        }
    }
}
