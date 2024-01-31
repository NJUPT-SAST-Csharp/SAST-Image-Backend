using Primitives.Command;
using SastImg.Domain;
using SastImg.Domain.AlbumAggregate;
using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Application.AlbumServices.CreateAlbum
{
    public sealed class CreateAlbumCommandHandler(
        IUnitOfWork unitOfWork,
        IAlbumRepository repository
    ) : ICommandRequestHandler<CreateAlbumCommand, CreateAlbumDto>
    {
        private readonly IUnitOfWork _unitOfWork = unitOfWork;
        private readonly IAlbumRepository _repository = repository;

        public async Task<CreateAlbumDto> Handle(
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

            var id = await _repository.AddAlbumAsync(album, cancellationToken);

            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return new CreateAlbumDto(id);
        }
    }
}
