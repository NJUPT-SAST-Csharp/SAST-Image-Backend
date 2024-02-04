using Primitives.Command;
using SNS.Domain.ImageAggregate;
using SNS.Domain.ImageAggregate.ImageEntity;

namespace SNS.Application.ImageServices.AddImage
{
    internal class AddImageCommandHandler(IImageRepository repository)
        : ICommandRequestHandler<AddImageCommand>
    {
        private readonly IImageRepository _repository = repository;

        public async Task Handle(AddImageCommand request, CancellationToken cancellationToken)
        {
            var image = Image.CreateNewImage(request.ImageId, request.AuthorId, request.AlbumId);
            await _repository.AddNewImageAsync(image, cancellationToken);
        }
    }
}
