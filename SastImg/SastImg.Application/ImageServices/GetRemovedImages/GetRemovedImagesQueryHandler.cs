using Mediator;
using SastImg.Application.ImageServices.GetAlbumImages;

namespace SastImg.Application.ImageServices.GetRemovedImages;

public sealed class GetRemovedImagesQueryHandler(IGetRemovedImagesRepository repository)
    : IQueryHandler<GetRemovedImagesQuery, IEnumerable<AlbumImageDto>>
{
    public async ValueTask<IEnumerable<AlbumImageDto>> Handle(
        GetRemovedImagesQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.Requester.IsAdmin)
        {
            return await repository.GetImagesByAdminAsync(request.AlbumId, cancellationToken);
        }

        return await repository.GetImagesByUserAsync(
            request.Requester.Id,
            request.AlbumId,
            cancellationToken
        );
    }
}
