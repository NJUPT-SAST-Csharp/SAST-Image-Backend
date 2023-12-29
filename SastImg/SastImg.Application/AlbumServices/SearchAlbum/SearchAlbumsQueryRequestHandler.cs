using SastImg.Application.AlbumServices.GetAlbums;
using Shared.Primitives.Request;

namespace SastImg.Application.AlbumServices.SearchAlbum
{
    internal sealed class SearchAlbumsQueryRequestHandler(ISearchAlbumsRepository repository)
        : IQueryRequestHandler<SearchAlbumsQueryRequest, IEnumerable<AlbumDto>>
    {
        private readonly ISearchAlbumsRepository _repository = repository;

        public Task<IEnumerable<AlbumDto>> Handle(
            SearchAlbumsQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAdmin)
            {
                return _repository.SearchAlbumsByAdminAsync(
                    request.CategoryId,
                    request.Title,
                    request.Page
                );
            }
            else
            {
                return _repository.SearchAlbumsByUserAsync(
                    request.CategoryId,
                    request.Title,
                    request.Page,
                    request.Requester.Id
                );
            }
        }
    }
}
