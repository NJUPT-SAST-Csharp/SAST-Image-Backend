namespace SastImg.Application.ImageServices.SearchImages
{
    public sealed class SearchedImageDto
    {
        private SearchedImageDto() { }

        public long ImageId { get; }
        public long AlbumId { get; }
        public string Title { get; }
        public Uri Url { get; }
    }
}
