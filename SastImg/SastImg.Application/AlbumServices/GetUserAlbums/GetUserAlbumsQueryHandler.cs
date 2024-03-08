using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.GetUserAlbums
{
    internal sealed class GetUserAlbumsQueryHandler(IGetUserAlbumsRepository database)
        : IQueryRequestHandler<GetUserAlbumsQuery, IEnumerable<UserAlbumDto>>
    {
        private readonly IGetUserAlbumsRepository _database = database;

        public Task<IEnumerable<UserAlbumDto>> Handle(
            GetUserAlbumsQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAdmin)
            {
                return _database.GetUserAlbumsByAdminAsync(request.AuthorId, cancellationToken);
            }
            else
            {
                return _database.GetUserAlbumsByUserAsync(
                    request.AuthorId,
                    request.Requester.Id,
                    cancellationToken
                );
            }
        }
    }
}
