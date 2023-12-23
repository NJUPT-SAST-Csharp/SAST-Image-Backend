namespace SastImg.Domain.Albums
{
    internal sealed record class Cover
    {
        internal Cover(Uri? uri, bool isLatestImage)
        {
            _url = uri;
            _isLatestImage = isLatestImage;
        }

        private Uri? _url;

        private bool _isLatestImage;

        internal bool IsLatestImage => _isLatestImage;
    }
}
