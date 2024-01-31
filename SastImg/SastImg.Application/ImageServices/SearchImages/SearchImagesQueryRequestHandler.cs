using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.SearchImages
{
    internal sealed class SearchImagesQueryRequestHandler(ISearchImagesRepository repository)
        : IQueryRequestHandler<SearchImagesQueryRequest, IEnumerable<SearchedImageDto>>
    {
        private readonly ISearchImagesRepository _repository = repository;

        public Task<IEnumerable<SearchedImageDto>> Handle(
            SearchImagesQueryRequest request,
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
