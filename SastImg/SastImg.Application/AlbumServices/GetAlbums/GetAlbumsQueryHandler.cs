using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.GetAlbums
{
    internal sealed class GetAlbumsQueryHandler(IGetAlbumsRepository repository)
        : IQueryRequestHandler<GetAlbumsQuery, IEnumerable<AlbumDto>>
    {
        private readonly IGetAlbumsRepository _repository = repository;

        public Task<IEnumerable<AlbumDto>> Handle(
            GetAlbumsQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAdmin)
            {
                return _repository.GetAlbumsByAdminAsync(
                    request.CategoryId,
                    request.Page,
                    cancellationToken
                );
            }
            else if (request.Requester.IsAuthenticated)
            {
                return _repository.GetAlbumsByUserAsync(
                    request.CategoryId,
                    request.Page,
                    request.Requester.Id,
                    cancellationToken
                );
            }
            else
            {
                return _repository.GetAlbumsAnonymousAsync(request.CategoryId, cancellationToken);
            }
        }
    }
}
