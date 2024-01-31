using SastImg.Application.SeedWorks;
using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.GetAlbum
{
    internal sealed class GetAlbumQueryHandler(
        IGetAlbumRepository repository,
        ICache<DetailedAlbumDto> cache
    ) : IQueryRequestHandler<GetAlbumQuery, DetailedAlbumDto?>
    {
        private readonly IGetAlbumRepository _repository = repository;
        private readonly ICache<DetailedAlbumDto> _cache = cache;

        public Task<DetailedAlbumDto?> Handle(
            GetAlbumQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAuthenticated)
            {
                if (request.Requester.IsAdmin)
                {
                    return _repository.GetAlbumByAdminAsync(request.AlbumId, cancellationToken);
                }
                else
                {
                    return _repository.GetAlbumByUserAsync(
                        request.AlbumId,
                        request.Requester.Id,
                        cancellationToken
                    );
                }
            }
            else
            {
                return _cache.GetCachingAsync(request.AlbumId.ToString(), cancellationToken);
            }
        }
    }
}
