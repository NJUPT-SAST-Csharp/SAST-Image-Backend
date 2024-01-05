namespace SastImg.Application.ImageServices.GetImages
{
    public sealed class AlbumImageDto
    {
        private AlbumImageDto() { }

        public long ImageId { get; }
        public long AlbumId { get; }
        public string Title { get; }
        public Uri Url { get; }
    }
}
