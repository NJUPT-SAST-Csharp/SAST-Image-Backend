using Mediator;

namespace SastImg.Application.ImageServices.GetAlbumImages;

public sealed class GetAlbumImagesQueryHandler(IGetAlbumImagesRepository repository)
    : IQueryHandler<GetAlbumImages, IEnumerable<AlbumImageDto>>
{
    public async ValueTask<IEnumerable<AlbumImageDto>> Handle(
        GetAlbumImages request,
        CancellationToken cancellationToken
    )
    {
        if (request.Requester.IsAuthenticated)
        {
            if (request.Requester.IsAdmin)
            {
                return await repository.GetImagesByAdminAsync(
                    request.AlbumId,
                    request.Page,
                    cancellationToken
                );
            }
            else
            {
                return await repository.GetImagesByUserAsync(
                    request.AlbumId,
                    request.Requester.Id,
                    request.Page,
                    cancellationToken
                );
            }
        }
        else
        {
            return await repository.GetImagesByAnonymousAsync(request.AlbumId, cancellationToken);
        }
    }
}
