using Mediator;

namespace SastImg.Application.ImageServices.GetImageFile;

public sealed class GetImageFileQueryHandler(IImageStorageRepository repository)
    : IQueryHandler<GetImageFileQuery, Stream?>
{
    public async ValueTask<Stream?> Handle(
        GetImageFileQuery request,
        CancellationToken cancellationToken
    )
    {
        return await repository.GetImageAsync(
            request.ImageId,
            request.IsThumbnail,
            cancellationToken
        );
    }
}
