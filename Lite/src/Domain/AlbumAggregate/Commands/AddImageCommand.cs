using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Shared;
using Mediator;

namespace Domain.AlbumAggregate.Commands;

public sealed record class AddImageCommand(
    AlbumId Album,
    ImageTitle Title,
    ImageTags Tags,
    IImageFile ImageFile,
    Actor Actor
) : ICommand<ImageId> { }

internal sealed class AddImageCommandHandler(IAlbumRepository repository)
    : ICommandHandler<AddImageCommand, ImageId>
{
    public async ValueTask<ImageId> Handle(
        AddImageCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = await repository.GetAsync(request.Album, cancellationToken);

        return album.AddImage(request);
    }
}
