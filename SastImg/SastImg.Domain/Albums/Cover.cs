namespace SastImg.Domain.Albums
{
    public sealed class Cover(Uri? uri, long? imageId)
    {
        #region Properties

        private Uri? _uri = uri;

        private long? _imageId = imageId;

        #endregion
        public bool IsLatestImage => _imageId is not null;
    }
}
