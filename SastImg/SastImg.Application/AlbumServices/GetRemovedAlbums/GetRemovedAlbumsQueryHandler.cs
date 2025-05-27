using Mediator;

namespace SastImg.Application.AlbumServices.GetRemovedAlbums;

public sealed class GetRemovedAlbumsQueryHandler(IGetRemovedAlbumsRepository repository)
    : IQueryHandler<GetRemovedAlbumsQuery, IEnumerable<RemovedAlbumDto>>
{
    public async ValueTask<IEnumerable<RemovedAlbumDto>> Handle(
        GetRemovedAlbumsQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.Requester.IsAdmin)
        {
            var id = request.AuthorId.Value == 0 ? request.Requester.Id : request.AuthorId;
            return await repository.GetRemovedAlbumsByAdminAsync(id, cancellationToken);
        }
        else
        {
            return await repository.GetRemovedAlbumsByUserAsync(
                request.Requester.Id,
                cancellationToken
            );
        }
    }
}
