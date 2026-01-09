using Mediator;

namespace SastImg.Application.ImageServices.GetUserImages;

public sealed class GetUserImagesQueryHandler(IGetUserImagesRepository repository)
    : IQueryHandler<GetUserImagesQuery, IEnumerable<UserImageDto>>
{
    public async ValueTask<IEnumerable<UserImageDto>> Handle(
        GetUserImagesQuery request,
        CancellationToken cancellationToken
    )
    {
        if (request.Requester.IsAdmin)
        {
            return await repository.GetUserImagesByAdminAsync(
                request.UserId,
                request.Page,
                cancellationToken
            );
        }
        else
        {
            return await repository.GetUserImagesByUserAsync(
                request.UserId,
                request.Requester.Id,
                request.Page,
                cancellationToken
            );
        }
    }
}
