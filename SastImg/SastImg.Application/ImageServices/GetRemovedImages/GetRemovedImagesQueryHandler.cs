using SastImg.Application.ImageServices.GetImages;
using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.GetRemovedImages
{
    internal class GetRemovedImagesQueryHandler(IGetRemovedImagesRepository repository)
        : IQueryRequestHandler<GetRemovedImagesQuery, IEnumerable<AlbumImageDto>>
    {
        private readonly IGetRemovedImagesRepository _repository = repository;

        public Task<IEnumerable<AlbumImageDto>> Handle(
            GetRemovedImagesQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAdmin)
            {
                return _repository.GetImagesByAdminAsync(request.AlbumId, cancellationToken);
            }

            return _repository.GetImagesByUserAsync(
                request.Requester.Id,
                request.AlbumId,
                cancellationToken
            );
        }
    }
}
