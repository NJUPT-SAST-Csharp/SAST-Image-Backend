using Primitives.Common;
using SastImg.Application.Albums.Dtos;
using SastImg.Application.Services;
using Shared.Primitives.Query;
using Utilities;

namespace SastImg.Application.Albums.GetAlbums
{
    internal sealed class GetAlbumsQueryHandler(IQueryDatabase database, ICache cache)
        : IQueryHandler<GetAlbumsQuery, IEnumerable<AlbumDto>>
    {
        private readonly IQueryDatabase _database = database;
        private readonly ICache _cache = cache;

        public async Task<IEnumerable<AlbumDto>> Handle(
            GetAlbumsQuery request,
            CancellationToken cancellationToken
        )
        {
            // When request is anonymous.
            IEnumerable<AlbumDto>? albums = null;
            if (request.User.Identity is null || !request.User.Identity.IsAuthenticated)
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
            // When user is an administrator.
            else if (request.User.IsInRole(AuthorizationRoles.Admin))
            {
                var query = GetAlbumsSqlStrategy.Admin(request.Page, request.AuthorId);
                albums = await _database.QueryAsync<AlbumDto>(
                    query.SqlString,
                    query.Parameters,
                    cancellationToken
                );
                return albums;
            }
            // When user is an auth common user.
            else
            {
                var result = AuthenticationHelper.TryFetchId(request.User, out long requesterId);
                if (!result)
                    throw new Exception(
                        "Successfully validated user identity but failed when fetching user id."
                    );
                var query = GetAlbumsSqlStrategy.Common(
                    request.Page,
                    requesterId,
                    request.AuthorId
                );
                albums = await _database.QueryAsync<AlbumDto>(
                    query.SqlString,
                    query.Parameters,
                    cancellationToken
                );
                return albums;
            }
        }

        private async Task<IEnumerable<AlbumDto>?> GetCacheAsync(int page, long authorId)
        {
            IEnumerable<AlbumDto>? albums = null;

            if (authorId == 0)
                albums = await _cache.HashGetAsync<IEnumerable<AlbumDto>>(
                    CacheKey.AnonymousAlbums,
                    page
                );
            else
                albums = await _cache.HashGetAsync<IEnumerable<AlbumDto>>(
                    CacheKey.AnonymousUserAlbums,
                    string.Concat('u', authorId, 'p', page)
                );

            return albums;
        }

        private Task<bool> SetCacheAsync(IEnumerable<AlbumDto> albums, int page, long userId)
        {
            if (userId == 0)
                return _cache.HashSetAsync(CacheKey.AnonymousAlbums, page, albums);
            else
                return _cache.HashSetAsync(
                    CacheKey.AnonymousUserAlbums,
                    string.Concat('u', userId, 'p', page),
                    albums
                );
        }
    }
}
