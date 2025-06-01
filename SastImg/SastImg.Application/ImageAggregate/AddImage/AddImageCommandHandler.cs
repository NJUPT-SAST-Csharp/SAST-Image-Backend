using Mediator;
using SastImg.Application.ImageServices.AddImage;
using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.ImageAggregate.AddImage;

public sealed class AddImageCommandHandler(IAlbumRepository repository)
    : ICommandHandler<AddImageCommand, ImageInfoDto>
{
    public async ValueTask<ImageInfoDto> Handle(
        AddImageCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAlbumAsync(request.AlbumId, cancellationToken);

        var imageId = album.AddImage(
            request.Title,
            request.Description,
            new(new("TODO"), new("TODO")),
            request.Tags
        );

        throw new NotImplementedException("Image upload logic is not implemented yet.");

        //return new ImageInfoDto(imageId);
    }
}
