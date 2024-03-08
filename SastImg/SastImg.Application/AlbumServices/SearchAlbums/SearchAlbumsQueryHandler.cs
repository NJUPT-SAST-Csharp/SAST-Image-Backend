using Shared.Primitives.Query;

namespace SastImg.Application.AlbumServices.SearchAlbums
{
    internal sealed class SearchAlbumsQueryHandler(ISearchAlbumsRepository repository)
        : IQueryRequestHandler<SearchAlbumsQuery, IEnumerable<SearchAlbumDto>>
    {
        private readonly ISearchAlbumsRepository _repository = repository;

        public Task<IEnumerable<SearchAlbumDto>> Handle(
            SearchAlbumsQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAdmin)
            {
                return _repository.SearchAlbumsByAdminAsync(
                    request.CategoryId,
                    request.Title,
                    request.Page,
                    cancellationToken
                );
            }
            else
            {
                return _repository.SearchAlbumsByUserAsync(
                    request.CategoryId,
                    request.Title,
                    request.Page,
                    request.Requester.Id,
                    cancellationToken
                );
            }
        }
    }
}
