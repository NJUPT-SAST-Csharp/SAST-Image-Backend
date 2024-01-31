using SastImg.Application.AlbumServices.GetAlbums;
using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.SearchAlbums
{
    internal sealed class SearchAlbumsQueryHandler(ISearchAlbumsRepository repository)
        : IQueryRequestHandler<SearchAlbumsQuery, IEnumerable<AlbumDto>>
    {
        private readonly ISearchAlbumsRepository _repository = repository;

        public Task<IEnumerable<AlbumDto>> Handle(
            SearchAlbumsQuery request,
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
