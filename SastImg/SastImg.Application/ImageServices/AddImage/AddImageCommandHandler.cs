using Primitives;
using Primitives.Command;
using SastImg.Domain.AlbumAggregate;

namespace SastImg.Application.ImageServices.AddImage
{
    internal sealed class AddImageCommandHandler(
        IAlbumRepository repository,
        IImageStorageClient client,
        IUnitOfWork unitOfWork
    ) : ICommandRequestHandler<AddImageCommand, ImageInfo>
    {
        private readonly IAlbumRepository _repository = repository;
        private readonly IImageStorageClient _client = client;
        private readonly IUnitOfWork _unitOfWork = unitOfWork;

        public async Task<ImageInfo> Handle(
            AddImageCommand request,
            CancellationToken cancellationToken
        )
        {
            var url = await _client.UploadImageAsync(
                request.FileName,
                request.ImageFile,
                cancellationToken
            );

            var album = await _repository.GetAlbumAsync(request.AlbumId, cancellationToken);

            album.AddImage(request.Title, url, request.Description, request.Tags);

            await _unitOfWork.CommitChangesAsync(cancellationToken);

            return new ImageInfo(url);
        }
    }
}
