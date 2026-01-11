using Application.Shared;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.Shared;
using Mediator;

namespace Application.ImageServices.Queries;

public sealed record RemovedImagesQuery(AlbumId Album, Actor Actor) : IQuery<ImageDto[]>;

internal sealed class RemovedImagesQueryHandler(
    IQueryRepository<RemovedImagesQuery, ImageDto[]> repository
) : IQueryHandler<RemovedImagesQuery, ImageDto[]>
{
    public async ValueTask<ImageDto[]> Handle(
        RemovedImagesQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetOrDefaultAsync(request, cancellationToken);
    }
}
