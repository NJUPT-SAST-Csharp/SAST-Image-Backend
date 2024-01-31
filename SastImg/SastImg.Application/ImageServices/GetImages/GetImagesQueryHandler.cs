using SastImg.Application.SeedWorks;
using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.GetImages
{
    public sealed class GetImagesQueryHandler(
        IGetImagesRepository repository,
        ICache<IEnumerable<AlbumImageDto>> cache
    ) : IQueryRequestHandler<GetImagesQuery, IEnumerable<AlbumImageDto>>
    {
        private readonly IGetImagesRepository _repository = repository;
        private readonly ICache<IEnumerable<AlbumImageDto>> _cache = cache;

        public Task<IEnumerable<AlbumImageDto>> Handle(
            GetImagesQuery request,
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
