using Domain.AlbumAggregate.ImageEntity;
using Domain.Shared;
using Mediator;

namespace Application.ImageServices.Queries;

public sealed record ImageFileQuery(ImageId Image, ImageKind Kind, Actor Actor) : IQuery<Stream?>;

internal sealed class ImageFileQueryHandler(
    IImageStorageManager manager,
    IImageAvailabilityChecker checker
) : IQueryHandler<ImageFileQuery, Stream?>
{
    public async ValueTask<Stream?> Handle(
        ImageFileQuery request,
        CancellationToken cancellationToken
    )
    {
        bool result = await checker.CheckAsync(request.Image, request.Actor, cancellationToken);

        if (result == false)
            return null;

        return manager.OpenReadStream(request.Image, request.Kind);
    }
}
