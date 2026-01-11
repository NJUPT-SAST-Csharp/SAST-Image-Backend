using Application.Shared;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Mediator;

namespace Application.AlbumServices.Queries;

public sealed class DetailedAlbum
{
    public required long Id { get; init; }
    public required string Title { get; init; }
    public required string Description { get; init; }
    public required long Author { get; init; }
    public required long Category { get; init; }
    public required string[] Tags { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required DateTime CreatedAt { get; init; }
    public required AccessLevelValue AccessLevel { get; init; }
    public required int SubscribeCount { get; init; }
}

public sealed record class DetailedAlbumQuery(AlbumId Id, Actor Actor) : IQuery<DetailedAlbum?>;

internal sealed class DetailedAlbumQueryHandler(
    IQueryRepository<DetailedAlbumQuery, DetailedAlbum?> repository
) : IQueryHandler<DetailedAlbumQuery, DetailedAlbum?>
{
    public async ValueTask<DetailedAlbum?> Handle(
        DetailedAlbumQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
