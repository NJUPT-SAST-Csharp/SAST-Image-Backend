using Shared.Primitives.Request;

namespace SastImg.Application.ImageServices.GetImages
{
    public sealed class GetImagesQueryRequestHandler
        : IQueryRequestHandler<GetImagesQueryRequest, IEnumerable<ImageDto>>
    {
        public Task<IEnumerable<ImageDto>> Handle(
            GetImagesQueryRequest request,
            CancellationToken cancellationToken
        ) { }
    }
}
