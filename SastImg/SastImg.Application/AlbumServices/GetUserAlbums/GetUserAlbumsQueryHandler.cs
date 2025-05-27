using Mediator;

namespace SastImg.Application.AlbumServices.GetUserAlbums;

public sealed class GetUserAlbumsQueryHandler(IGetUserAlbumsRepository database)
    : IQueryHandler<GetUserAlbumsQuery, IEnumerable<UserAlbumDto>>
{
    public async ValueTask<IEnumerable<UserAlbumDto>> Handle(
        GetUserAlbumsQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.Requester.IsAdmin)
        {
            return await database.GetUserAlbumsByAdminAsync(request.AuthorId, cancellationToken);
        }
        else
        {
            return await database.GetUserAlbumsByUserAsync(
                request.AuthorId,
                request.Requester.Id,
                cancellationToken
            );
        }
    }
}
