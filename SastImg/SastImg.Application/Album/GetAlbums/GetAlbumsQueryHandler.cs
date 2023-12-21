using Auth.Authorization;
using SastImg.Application.Album.Dtos;
using SastImg.Application.Services;
using Shared.Primitives.Request;

namespace SastImg.Application.Album.GetAlbums
{
    internal sealed class GetAlbumsQueryHandler(IQueryDatabase database, ICache cache)
        : IQueryRequestHandler<GetAlbumsQuery, IEnumerable<AlbumDto>>
    {
        private readonly IQueryDatabase _database = database;
        private readonly ICache _cache = cache;

        public async Task<IEnumerable<AlbumDto>> Handle(
            GetAlbumsQuery request,
            CancellationToken cancellationToken
        )
        {
            // TODO: Reconstruct.
            IEnumerable<AlbumDto>? albums;
            // When user is an auth user.
            if (request.IsAuthenticated)
            {
                if (request.Roles.Any(r => r == AuthorizationRole.User))
                {
                    var query = GetAlbumsSqlStrategy.Common(
                        request.Page,
                        request.RequesterId,
                        request.AuthorId
                    );
                    albums = await _database.QueryAsync<AlbumDto>(
                        query.SqlString,
                        query.Parameters,
                        cancellationToken
                    );
                    return albums;
                }
                // When user is an administrator.
                else if (request.Roles.Any(r => r == AuthorizationRole.Admin))
                {
                    var query = GetAlbumsSqlStrategy.Admin(request.Page, request.AuthorId);
                    albums = await _database.QueryAsync<AlbumDto>(
                        query.SqlString,
                        query.Parameters,
                        cancellationToken
                    );
                    return albums;
                }
                else
                {
                    throw new InvalidOperationException("Unable to resolve identity from request.");
                }
            }
            // When request is anonymous.
            else if (request.IsAuthenticated == false)
            {
                // Get from cache first when anonymous.
                albums = await GetCacheAsync(request.Page, request.AuthorId);
                if (albums is not null)
                    return albums;
                var query = GetAlbumsSqlStrategy.Anonymous(request.Page, request.AuthorId);
                albums = await _database.QueryAsync<AlbumDto>(
                    query.SqlString,
                    query.Parameters,
                    cancellationToken
                );
                // Cache all the anonymous request.
                _ = SetCacheAsync(albums, request.Page, request.AuthorId);
                return albums;
            }
            else
            {
                throw new InvalidOperationException("Unable to resolve identity from request.");
            }
        }

        private async Task<IEnumerable<AlbumDto>?> GetCacheAsync(int page, long authorId)
        {
            IEnumerable<AlbumDto>? albums = null;

            if (authorId == 0)
                albums = await _cache.HashGetAsync<IEnumerable<AlbumDto>>(
                    CacheKeys.AnonymousAlbums,
                    page
                );
            else
                albums = await _cache.HashGetAsync<IEnumerable<AlbumDto>>(
                    CacheKeys.AnonymousUserAlbums,
                    string.Concat('u', authorId, 'p', page)
                );

            return albums;
        }

        private Task<bool> SetCacheAsync(IEnumerable<AlbumDto> albums, int page, long userId)
        {
            if (userId == 0)
                return _cache.HashSetAsync(CacheKeys.AnonymousAlbums, page, albums);
            else
                return _cache.HashSetAsync(
                    CacheKeys.AnonymousUserAlbums,
                    string.Concat('u', userId, 'p', page),
                    albums
                );
        }
    }
}
