using Mediator;

namespace SastImg.Application.AlbumAggregate.GetAlbums;

public sealed class GetAlbumsQueryHandler(IGetAlbumsRepository repository)
    : IQueryHandler<GetAlbumsQuery, IEnumerable<AlbumDto>>
{
    public async ValueTask<IEnumerable<AlbumDto>> Handle(
        GetAlbumsQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.Requester.IsAdmin)
        {
            return await repository.GetAlbumsByAdminAsync(
                request.CategoryId,
                request.Page,
                cancellationToken
            );
        }
        else if (request.Requester.IsAuthenticated)
        {
            return await repository.GetAlbumsByUserAsync(
                request.CategoryId,
                request.Page,
                request.Requester.Id,
                cancellationToken
            );
        }
        else
        {
            return await repository.GetAlbumsAnonymousAsync(request.CategoryId, cancellationToken);
        }
    }
}
