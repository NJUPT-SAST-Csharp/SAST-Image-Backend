namespace SastImg.Domain.Albums
{
    public sealed class Cover
    {
        #region Properties

        public Uri? Url { get; private set; } = null;
        public long ImageId { get; private set; } = 0;
        public bool IsLatestImage { get; private set; } = true;

        #endregion

        #region Methods

        /// <summary>
        /// Set album's cover to a specific image in the album.
        /// </summary>
        /// <remarks>
        /// The image set to cover should be in the target album.
        /// </remarks>
        /// <param name="imageId">
        /// The image id set to the album cover.
        /// </param>
        public void SetCoverByImageId(long imageId)
        {
            ImageId = imageId;
            Url = null;
            IsLatestImage = false;
        }

        /// <summary>
        /// Set album's cover to a specific image with Uri.
        /// </summary>
        /// <param name="uri">
        /// The image Uri set to the album cover.
        /// </param>
        public void SetCoverByUri(Uri uri)
        {
            Url = uri;
            ImageId = 0;
            IsLatestImage = false;
        }

        #endregion
    }
}
