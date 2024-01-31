using SastImg.Application.SeedWorks;
using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.GetAlbums
{
    internal sealed class GetAlbumsQueryHandler(
        IGetAlbumsRepository database,
        ICache<IEnumerable<AlbumDto>> cache
    ) : IQueryRequestHandler<GetAlbumsQuery, IEnumerable<AlbumDto>>
    {
        private readonly IGetAlbumsRepository _database = database;
        private readonly ICache<IEnumerable<AlbumDto>> _cache = cache;

        public Task<IEnumerable<AlbumDto>> Handle(
            GetAlbumsQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAuthenticated)
            {
                if (request.Requester.IsAdmin)
                {
                    return _database.GetAlbumsByAdminAsync(
                        request.Page,
                        request.AuthorId,
                        cancellationToken
                    );
                }
                else
                {
                    return _database.GetAlbumsByUserAsync(
                        request.Page,
                        request.AuthorId,
                        request.Requester.Id,
                        cancellationToken
                    );
                }
            }
            else
            {
                return _cache.GetCachingAsync(string.Empty, cancellationToken)!;
            }
        }
    }
}
