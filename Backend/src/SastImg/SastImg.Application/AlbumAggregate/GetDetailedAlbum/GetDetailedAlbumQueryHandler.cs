using Mediator;

namespace SastImg.Application.AlbumAggregate.GetDetailedAlbum;

public sealed class GetDetailedAlbumQueryHandler(IGetDetailedAlbumRepository repository)
    : IQueryHandler<GetDetailedAlbumQuery, DetailedAlbumDto?>
{
    public async ValueTask<DetailedAlbumDto?> Handle(
        GetDetailedAlbumQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.Requester.IsAuthenticated)
        {
            if (request.Requester.IsAdmin)
            {
                return await repository.GetDetailedAlbumByAdminAsync(
                    request.AlbumId,
                    cancellationToken
                );
            }
            else
            {
                return await repository.GetDetailedAlbumByUserAsync(
                    request.AlbumId,
                    request.Requester.Id,
                    cancellationToken
                );
            }
        }
        else
        {
            return await repository.GetDetailedAlbumByAnonymousAsync(
                request.AlbumId,
                cancellationToken
            );
        }
    }
}
