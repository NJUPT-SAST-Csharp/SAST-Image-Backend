using SastImg.Application.ImageServices.AddImage;
using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.GetImageFile
{
    internal sealed class GetImageFileQueryHandler(IImageStorageRepository repository)
        : IQueryRequestHandler<GetImageFileQuery, Stream?>
    {
        private readonly IImageStorageRepository _repository = repository;

        public Task<Stream?> Handle(GetImageFileQuery request, CancellationToken cancellationToken)
        {
            return _repository.GetImageAsync(
                request.ImageId,
                request.IsThumbnail,
                cancellationToken
            );
        }
    }
}
