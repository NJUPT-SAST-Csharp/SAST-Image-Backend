using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.GetDetailedAlbum
{
    internal sealed class GetDetailedAlbumQueryHandler(IGetDetailedAlbumRepository repository)
        : IQueryRequestHandler<GetDetailedAlbumQuery, DetailedAlbumDto?>
    {
        private readonly IGetDetailedAlbumRepository _repository = repository;

        public Task<DetailedAlbumDto?> Handle(
            GetDetailedAlbumQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAuthenticated)
            {
                if (request.Requester.IsAdmin)
                {
                    return _repository.GetDetailedAlbumByAdminAsync(
                        request.AlbumId,
                        cancellationToken
                    );
                }
                else
                {
                    return _repository.GetDetailedAlbumByUserAsync(
                        request.AlbumId,
                        request.Requester.Id,
                        cancellationToken
                    );
                }
            }
            else
            {
                return _repository.GetDetailedAlbumByAnonymousAsync(
                    request.AlbumId,
                    cancellationToken
                );
            }
        }
    }
}
