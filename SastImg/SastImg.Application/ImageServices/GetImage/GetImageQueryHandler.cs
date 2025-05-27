using Mediator;

namespace SastImg.Application.ImageServices.GetImage;

public sealed class GetImageQueryHandler(IGetImageRepository repository)
    : IQueryHandler<GetImageQuery, DetailedImageDto?>
{
    public async ValueTask<DetailedImageDto?> Handle(
        GetImageQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.Requester.IsAuthenticated)
        {
            if (request.Requester.IsAdmin)
            {
                return await repository.GetImageByAdminAsync(
                    request.AlbumId,
                    request.ImageId,
                    cancellationToken
                );
            }
            else
            {
                return await repository.GetImageByUserAsync(
                    request.AlbumId,
                    request.ImageId,
                    request.Requester.Id,
                    cancellationToken
                );
            }
        }
        else
        {
            return await repository.GetImageByAnonymousAsync(
                request.AlbumId,
                request.ImageId,
                cancellationToken
            );
        }
    }
}
