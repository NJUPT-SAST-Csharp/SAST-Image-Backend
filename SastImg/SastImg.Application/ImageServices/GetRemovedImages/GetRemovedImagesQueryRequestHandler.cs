using SastImg.Application.ImageServices.GetImages;
using SastImg.Domain;
using Shared.Primitives.Request;

namespace SastImg.Application.ImageServices.GetRemovedImages
{
    internal class GetRemovedImagesQueryRequestHandler(IGetRemovedImagesRepository repository)
        : IQueryRequestHandler<GetRemovedImagesQueryRequest, IEnumerable<AlbumImageDto>>
    {
        private readonly IGetRemovedImagesRepository _repository = repository;

        public Task<IEnumerable<AlbumImageDto>> Handle(
            GetRemovedImagesQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAdmin)
            {
                UserId id = request.AuthorId.Value == 0 ? request.Requester.Id : request.AuthorId;
                return _repository.GetImagesByAdminAsync(id, cancellationToken);
            }
            else
            {
                return _repository.GetImagesByUserAsync(request.Requester.Id, cancellationToken);
            }
        }
    }
}
