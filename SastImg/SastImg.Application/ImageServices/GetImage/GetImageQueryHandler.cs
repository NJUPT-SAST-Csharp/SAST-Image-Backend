using SastImg.Application.SeedWorks;
using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.GetImage
{
    internal sealed class GetImageQueryHandler(
        IGetImageRepository repository,
        ICache<DetailedImageDto> cache
    ) : IQueryRequestHandler<GetImageQuery, DetailedImageDto?>
    {
        private readonly IGetImageRepository _repository = repository;
        private readonly ICache<DetailedImageDto> _cache = cache;

        public Task<DetailedImageDto?> Handle(
            GetImageQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAuthenticated)
            {
                if (request.Requester.IsAdmin)
                {
                    return _repository.GetImageByAdminAsync(
                        request.AlbumId,
                        request.ImageId,
                        cancellationToken
                    );
                }
                else
                {
                    return _repository.GetImageByUserAsync(
                        request.AlbumId,
                        request.ImageId,
                        request.Requester.Id,
                        cancellationToken
                    );
                }
            }
            else
            {
                return _cache.GetCachingAsync(request.ImageId.ToString(), cancellationToken);
            }
        }
    }
}
