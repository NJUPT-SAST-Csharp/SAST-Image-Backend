using Primitives.Command;
using SNS.Domain.AlbumEntity;

namespace SNS.Application.AlbumServices.AddAlbum
{
    internal sealed class AddAlbumCommandHandler(IAlbumRepository repository)
        : ICommandRequestHandler<AddAlbumCommand>
    {
        private readonly IAlbumRepository _repository = repository;

        public async Task Handle(AddAlbumCommand request, CancellationToken cancellationToken)
        {
            var album = Album.CreateNewAlbum(request.AlbumId, request.AuthorId);
            await _repository.AddNewAlbumAsync(album, cancellationToken);
        }
    }
}
