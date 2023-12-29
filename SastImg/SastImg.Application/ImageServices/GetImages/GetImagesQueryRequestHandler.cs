using SastImg.Application.SeedWorks;
using Shared.Primitives.Request;

namespace SastImg.Application.ImageServices.GetImages
{
    public sealed class GetImagesQueryRequestHandler(
        IGetImagesRepository repository,
        ICache<IEnumerable<ImageDto>> cache
    ) : IQueryRequestHandler<GetImagesQueryRequest, IEnumerable<ImageDto>>
    {
        private readonly IGetImagesRepository _repository = repository;
        private readonly ICache<IEnumerable<ImageDto>> _cache = cache;

        public Task<IEnumerable<ImageDto>> Handle(
            GetImagesQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAuthenticated)
            {
                if (request.Requester.IsAdmin)
                {
                    return _repository.GetImagesByAdminAsync(
                        request.AlbumId,
                        request.Page,
                        cancellationToken
                    );
                }
                else
                {
                    return _repository.GetImagesByUserAsync(
                        request.AlbumId,
                        request.Page,
                        cancellationToken
                    );
                }
            }
            else
            {
                return _cache.GetCachingAsync(request.AlbumId.ToString(), cancellationToken)!;
            }
        }
    }
}
