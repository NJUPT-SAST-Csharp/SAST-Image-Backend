namespace SastImg.Application.ImageServices.SearchImages
{
    public interface ISearchImagesRepository
    {
        public Task<IEnumerable<SearchedImageDto>> SearchImagesByAdminAsync(
            int page,
            long categoryId,
            long[] tags,
            CancellationToken cancellationToken = default
        );

        public Task<IEnumerable<SearchedImageDto>> SearchImagesByUserAsync(
            int page,
            long categoryId,
            long[] tags,
            long requesterId,
            CancellationToken cancellationToken = default
        );
    }
}
