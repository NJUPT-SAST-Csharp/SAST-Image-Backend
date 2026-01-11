using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Mediator;

namespace Application.AlbumServices.Queries;

public sealed record class AlbumCoverQuery(AlbumId Id, Actor Actor) : IQuery<Stream?>;

internal sealed class AlbumCoverQueryHandler(
    ICoverStorageManager manager,
    IAlbumAvailabilityChecker checker
) : IQueryHandler<AlbumCoverQuery, Stream?>
{
    public async ValueTask<Stream?> Handle(
        AlbumCoverQuery request,
        CancellationToken cancellationToken
    )
    {
        bool available = await checker.CheckAsync(request.Id, request.Actor, cancellationToken);

        if (available == false)
            return null;

        var stream = manager.OpenReadStream(request.Id);

        return stream;
    }
}
