using Exceptions.Exceptions;
using Mediator;
using Primitives;
using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.ImageServices.AddImage;

public sealed class AddImageCommandHandler(
    IAlbumRepository repository,
    IImageStorageRepository client,
    IUnitOfWork unitOfWork
) : ICommandHandler<AddImageCommand, ImageInfoDto>
{
    public async ValueTask<ImageInfoDto> Handle(
        AddImageCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAlbumAsync(request.AlbumId, cancellationToken);

        if (request.Requester.IsAdmin == false && album.IsManagedBy(request.Requester.Id) == false)
        {
            throw new NoPermissionException();
        }

        var url = await client.UploadImageAsync(request.ImageFile, cancellationToken);

        var imageId = album.AddImage(request.Title, request.Description, url, request.Tags);

        await unitOfWork.CommitChangesAsync(cancellationToken);

        return new ImageInfoDto(imageId);
    }
}
