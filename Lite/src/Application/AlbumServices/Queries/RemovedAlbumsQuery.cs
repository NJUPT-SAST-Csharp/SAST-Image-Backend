using Application.Shared;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Mediator;

namespace Application.AlbumServices.Queries;

public sealed class RemovedAlbumDto
{
    public required long Id { get; init; }
    public required string Title { get; init; }
    public required long Category { get; init; }
    public required AccessLevelValue AccessLevel { get; init; }
    public required DateTime RemovedAt { get; init; }
}

public sealed record class RemovedAlbumsQuery(Actor Actor) : IQuery<RemovedAlbumDto[]> { }

internal sealed class RemovedAlbumsQueryHandler(
    IQueryRepository<RemovedAlbumsQuery, RemovedAlbumDto[]> repository
) : IQueryHandler<RemovedAlbumsQuery, RemovedAlbumDto[]>
{
    public async ValueTask<RemovedAlbumDto[]> Handle(
        RemovedAlbumsQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
