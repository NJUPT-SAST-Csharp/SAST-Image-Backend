using Shared.Primitives.Request;

namespace SastImg.Application.AlbumServices.GetAlbum
{
    internal sealed class GetAlbumQueryRequestHandler(
        IGetAlbumRepository repository,
        IGetAlbumCache cache
    ) : IQueryRequestHandler<GetAlbumQueryRequest, DetailedAlbumDto?>
    {
        private readonly IGetAlbumRepository _repository = repository;
        private readonly IGetAlbumCache _cache = cache;

        public Task<DetailedAlbumDto?> Handle(
            GetAlbumQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAuthenticated)
            {
                if (request.Requester.IsAdmin)
                    return _repository.GetDetailedAlbumByAdminAsync(
                        request.AlbumId,
                        cancellationToken
                    );
                return _repository.GetDetailedAlbumByUserAsync(
                    request.AlbumId,
                    request.Requester.Id,
                    cancellationToken
                );
            }
            else
            {
                return _cache.GetAlbumAsync(request.AlbumId, cancellationToken);
            }
        }
    }
}
