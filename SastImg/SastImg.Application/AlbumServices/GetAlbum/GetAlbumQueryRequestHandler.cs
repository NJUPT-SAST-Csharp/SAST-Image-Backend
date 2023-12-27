using Shared.Primitives.Request;

namespace SastImg.Application.AlbumServices.GetAlbum
{
    internal sealed class GetAlbumQueryRequestHandler(IGetAlbumRepository repository)
        : IQueryRequestHandler<GetAlbumQueryRequest, DetailedAlbumDto?>
    {
        private readonly IGetAlbumRepository _repository = repository;

        public async Task<DetailedAlbumDto?> Handle(
            GetAlbumQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            return await _repository.GetDetailedAlbumAsync(request.AlbumId, cancellationToken);
        }
    }
}
