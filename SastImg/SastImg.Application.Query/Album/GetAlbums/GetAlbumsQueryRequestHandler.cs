using Shared.Primitives.Request;

namespace SastImg.Application.Query.Album.GetAlbums
{
    public sealed class GetAlbumsQueryRequestHandler(
        IGetAlbumsRepository repository,
        IGetAlbumsAnonymousCache cache
    ) : IQueryRequestHandler<GetAlbumsQueryRequest, IEnumerable<AlbumDto>>
    {
        private readonly IGetAlbumsRepository _repository = repository;
        private readonly IGetAlbumsAnonymousCache _cache = cache;

        public async Task<IEnumerable<AlbumDto>> Handle(
            GetAlbumsQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAuthenticated)
            {
                if (request.Requester.IsAdmin)
                {
                    return await _repository.GetAlbumsByAdminAsync(request.Page, request.AuthorId);
                }
                else
                {
                    return await _repository.GetAlbumsByUserAsync(
                        request.Page,
                        request.AuthorId,
                        request.Requester.Id
                    );
                }
            }
            else
            {
                IEnumerable<AlbumDto>? dtos = null;
                dtos = await _cache.GetAlbumsAsync();
                if (dtos is null)
                {
                    dtos = await _repository.GetAlbumsAnonymousAsync(
                        request.Page,
                        request.AuthorId
                    );
                    _ = _cache.SetAlbumsAsync(dtos);
                    return dtos;
                }
                else
                {
                    return dtos;
                }
            }
        }
    }
}
