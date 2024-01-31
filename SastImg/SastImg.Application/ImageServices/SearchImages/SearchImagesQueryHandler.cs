using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.SearchImages
{
    internal sealed class SearchImagesQueryHandler(ISearchImagesRepository repository)
        : IQueryRequestHandler<SearchImagesQuery, IEnumerable<SearchedImageDto>>
    {
        private readonly ISearchImagesRepository _repository = repository;

        public Task<IEnumerable<SearchedImageDto>> Handle(
            SearchImagesQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAdmin)
            {
                return _repository.SearchImagesByAdminAsync(
                    request.Page,
                    request.CategoryId,
                    request.Tags,
                    cancellationToken
                );
            }
            else
            {
                return _repository.SearchImagesByUserAsync(
                    request.Page,
                    request.CategoryId,
                    request.Tags,
                    request.Requester.Id,
                    cancellationToken
                );
            }
        }
    }
}
