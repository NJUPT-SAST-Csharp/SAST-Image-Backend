using MediatR.Pipeline;

namespace SastImg.Application.ImageServices.SearchImages
{
    public sealed class SearchImagesQueryRequestInterceptor
        : IRequestPreProcessor<SearchImagesQueryRequest>
    {
        public async Task Process(
            SearchImagesQueryRequest request,
            CancellationToken cancellationToken
        )
        {
            request.Ids = request.Order switch
            {
                SearchOrder.ByDate => [],
                SearchOrder.ByLikes => [],
                SearchOrder.ByViews => [],
                _ => throw new ArgumentOutOfRangeException(string.Empty),
            };
        }
    }
}
