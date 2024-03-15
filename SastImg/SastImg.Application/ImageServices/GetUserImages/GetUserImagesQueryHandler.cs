using Shared.Primitives.Query;

namespace SastImg.Application.ImageServices.GetUserImages
{
    internal sealed class GetUserImagesQueryHandler(IGetUserImagesRepository repository)
        : IQueryRequestHandler<GetUserImagesQuery, IEnumerable<UserImageDto>>
    {
        private readonly IGetUserImagesRepository _repository = repository;

        public Task<IEnumerable<UserImageDto>> Handle(
            GetUserImagesQuery request,
            CancellationToken cancellationToken
        )
        {
            if (request.Requester.IsAdmin)
            {
                return _repository.GetUserImagesByAdminAsync(
                    request.UserId,
                    request.Page,
                    cancellationToken
                );
            }
            else
            {
                return _repository.GetUserImagesByUserAsync(
                    request.UserId,
                    request.Requester.Id,
                    request.Page,
                    cancellationToken
                );
            }
        }
    }
}
