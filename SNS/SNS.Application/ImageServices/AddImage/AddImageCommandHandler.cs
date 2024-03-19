using Primitives;
using Primitives.Command;
using SNS.Domain.ImageAggregate.ImageEntity;

namespace SNS.Application.ImageServices.AddImage
{
    internal class AddImageCommandHandler(IImageRepository repository, IUnitOfWork unitOfWork)
        : ICommandRequestHandler<AddImageCommand>
    {
        private readonly IImageRepository _repository = repository;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task Handle(AddImageCommand request, CancellationToken cancellationToken)
        {
            var image = Image.CreateNewImage(request.ImageId, request.AuthorId, request.AlbumId);
            await _repository.AddNewImageAsync(image, cancellationToken);
            await _unitOfWork.CommitChangesAsync(cancellationToken);
        }
    }
}
