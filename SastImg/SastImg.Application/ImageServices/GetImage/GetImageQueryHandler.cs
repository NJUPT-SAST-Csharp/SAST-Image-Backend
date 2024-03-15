using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.GetImage
{
    internal sealed class GetImageQueryHandler(IGetImageRepository repository)
        : IQueryRequestHandler<GetImageQuery, DetailedImageDto?>
    {
        private readonly IGetImageRepository _repository = repository;

        public Task<DetailedImageDto?> Handle(
            GetImageQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAuthenticated)
            {
                if (request.Requester.IsAdmin)
                {
                    return _repository.GetImageByAdminAsync(
                        request.AlbumId,
                        request.ImageId,
                        cancellationToken
                    );
                }
                else
                {
                    return _repository.GetImageByUserAsync(
                        request.AlbumId,
                        request.ImageId,
                        request.Requester.Id,
                        cancellationToken
                    );
                }
            }
            else
            {
                return _repository.GetImageByAnonymousAsync(
                    request.AlbumId,
                    request.ImageId,
                    cancellationToken
                );
            }
        }
    }
}
