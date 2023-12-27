using Shared.Primitives.Request;

namespace SastImg.Application.AlbumServices.GetAlbums
{
    internal sealed class GetAlbumsQueryRequestHandler(
        IGetAlbumsRepository database,
        IGetAlbumsAnonymousCache cache
    ) : IQueryRequestHandler<GetAlbumsQueryRequest, IEnumerable<AlbumDto>>
    {
        private readonly IGetAlbumsRepository _database = database;
        private readonly IGetAlbumsAnonymousCache _cache = cache;

        public Task<IEnumerable<AlbumDto>> Handle(
            GetAlbumsQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAuthenticated == false)
            {
                return _cache.GetAlbumsAsync(request.Page, request.AuthorId, cancellationToken);
            }
            else
            {
                if (request.Requester.IsAdmin)
                {
                    return _database.GetAlbumsByAdminAsync(
                        request.Page,
                        request.AuthorId,
                        request.CategoryId,
                        cancellationToken
                    );
                }
                else
                {
                    return _database.GetAlbumsByUserAsync(
                        request.Page,
                        request.AuthorId,
                        request.CategoryId,
                        request.Requester.Id,
                        cancellationToken
                    );
                }
            }
        }
    }
}
