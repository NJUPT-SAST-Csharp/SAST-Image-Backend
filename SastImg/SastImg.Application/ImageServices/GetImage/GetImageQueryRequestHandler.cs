using SastImg.Application.SeedWorks;
using Shared.Primitives.Request;

namespace SastImg.Application.ImageServices.GetImage
{
    internal sealed class GetImageQueryRequestHandler(
        IGetImageRepository repository,
        ICache<DetailedImageDto> cache
    ) : IQueryRequestHandler<GetImageQueryRequest, DetailedImageDto?>
    {
        private readonly IGetImageRepository _repository = repository;
        private readonly ICache<DetailedImageDto> _cache = cache;

        public Task<DetailedImageDto?> Handle(
            GetImageQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAuthenticated)
            {
                if (request.Requester.IsAdmin)
                {
                    return _repository.GetImageByAdminAsync(request.ImageId, cancellationToken);
                }
                else
                {
                    return _repository.GetImageByUserAsync(
                        request.ImageId,
                        request.Requester.Id,
                        cancellationToken
                    );
                }
            }
            else
            {
                return _repository.GetImageByAnonymousAsync(
                    request.ImageId.ToString(),
                    cancellationToken
                );
            }
        }
    }
}
