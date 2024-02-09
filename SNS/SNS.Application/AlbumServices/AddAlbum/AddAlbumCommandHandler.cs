using Primitives;
using Primitives.Command;
using SNS.Domain.AlbumEntity;

namespace SNS.Application.AlbumServices.AddAlbum
{
    internal sealed class AddAlbumCommandHandler(
        IAlbumRepository repository,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<AddAlbumCommand>
    {
        private readonly IAlbumRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(AddAlbumCommand request, CancellationToken cancellationToken)
        {
            var album = Album.CreateNewAlbum(request.AlbumId, request.AuthorId);
            await _repository.AddNewAlbumAsync(album, cancellationToken);
            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
