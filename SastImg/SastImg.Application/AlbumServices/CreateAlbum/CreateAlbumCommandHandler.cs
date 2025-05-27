using Mediator;
using Primitives;
using SastImg.Domain.AlbumAggregate;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.CreateAlbum;

public sealed class CreateAlbumCommandHandler(IUnitOfWork unitOfWork, IAlbumRepository repository)
    : ICommandHandler<CreateAlbumCommand, CreateAlbumDto>
{
    public async ValueTask<CreateAlbumDto> Handle(
        CreateAlbumCommand request,
        CancellationToken cancellationToken
    )
    {
        var album = Album.CreateNewAlbum(
            request.Requester.Id,
            request.CategoryId,
            request.Title,
            request.Description,
            request.Accessibility
        );

        var id = await repository.AddAlbumAsync(album, cancellationToken);

        await unitOfWork.CommitChangesAsync(cancellationToken);

        return new CreateAlbumDto(id);
    }
}
