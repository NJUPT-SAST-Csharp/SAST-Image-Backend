using Mediator;

namespace SastImg.Application.AlbumServices.SearchAlbums;

internal sealed class SearchAlbumsQueryHandler(ISearchAlbumsRepository repository)
    : IQueryHandler<SearchAlbumsQuery, IEnumerable<SearchAlbumDto>>
{
    public async ValueTask<IEnumerable<SearchAlbumDto>> Handle(
        SearchAlbumsQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.Requester.IsAdmin)
        {
            return await repository.SearchAlbumsByAdminAsync(
                request.CategoryId,
                request.SearchTitle,
                request.Page,
                cancellationToken
            );
        }
        else
        {
            return await repository.SearchAlbumsByUserAsync(
                request.CategoryId,
                request.SearchTitle,
                request.Page,
                request.Requester.Id,
                cancellationToken
            );
        }
    }
}
