using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.GetAlbumImages
{
    public sealed class GetAlbumImagesQueryHandler(IGetAlbumImagesRepository repository)
        : IQueryRequestHandler<GetAlbumImages, IEnumerable<AlbumImageDto>>
    {
        private readonly IGetAlbumImagesRepository _repository = repository;

        public Task<IEnumerable<AlbumImageDto>> Handle(
            GetAlbumImages request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAuthenticated)
            {
                if (request.Requester.IsAdmin)
                {
                    return _repository.GetImagesByAdminAsync(
                        request.AlbumId,
                        request.Page,
                        cancellationToken
                    );
                }
                else
                {
                    return _repository.GetImagesByUserAsync(
                        request.AlbumId,
                        request.Requester.Id,
                        request.Page,
                        cancellationToken
                    );
                }
            }
            else
            {
                return _repository.GetImagesByAnonymousAsync(request.AlbumId, cancellationToken);
            }
        }
    }
}
