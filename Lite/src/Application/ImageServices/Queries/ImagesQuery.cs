using Application.Shared;
using Domain.Shared;
using Mediator;

namespace Application.ImageServices.Queries;

public sealed record class ImagesQuery(long? AuthorId, long? AlbumId, long? Cursor, Actor Actor)
    : IQuery<ImageDto[]> { }

internal sealed class ImagesQueryHandler(IQueryRepository<ImagesQuery, ImageDto[]> repository)
    : IQueryHandler<ImagesQuery, ImageDto[]>
{
    public async ValueTask<ImageDto[]> Handle(
        ImagesQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
