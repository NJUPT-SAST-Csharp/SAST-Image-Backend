using SastImg.Application.ImageServices.GetImages;
using Shared.Primitives.Request;

namespace SastImg.Application.ImageServices.SearchImages
{
    internal sealed class SearchImagesQueryRequestHandler(ISearchImagesRepository repository)
        : IQueryRequestHandler<SearchImagesQueryRequest, IEnumerable<ImageDto>>
    {
        private readonly ISearchImagesRepository _repository = repository;

        public Task<IEnumerable<ImageDto>> Handle(
            SearchImagesQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAdmin)
            {
                return _repository.SearchImagesByAdminAsync(
                    request.Page,
                    request.CategoryId,
                    request.Order,
                    request.Tags,
                    cancellationToken
                );
            }
            else
            {
                return _repository.SearchImagesByUserAsync(
                    request.Page,
                    request.CategoryId,
                    request.Order,
                    request.Tags,
                    request.Requester.Id,
                    cancellationToken
                );
            }
        }
    }
}
