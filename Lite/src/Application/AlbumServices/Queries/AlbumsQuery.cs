using Application.Shared;
using Domain.Shared;
using Mediator;

namespace Application.AlbumServices.Queries;

public sealed class AlbumDto
{
    public required long Id { get; init; }
    public required string Title { get; init; }
    public required long Author { get; init; }
    public required long Category { get; init; }
    public required DateTime UpdatedAt { get; init; }
    public required string[] Tags { get; init; }
}

public sealed record class AlbumsQuery(
    long? CategoryId,
    long? AuthorId,
    string? Title,
    long? Cursor,
    Actor Actor
) : IQuery<AlbumDto[]> { }

internal sealed class AlbumsQueryHandler(IQueryRepository<AlbumsQuery, AlbumDto[]> repository)
    : IQueryHandler<AlbumsQuery, AlbumDto[]>
{
    public async ValueTask<AlbumDto[]> Handle(
        AlbumsQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
