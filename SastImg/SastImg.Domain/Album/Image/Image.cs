using Shared.Primitives;
using Shared.Utilities;

namespace SastImg.Domain
{
    internal sealed class Image : Entity<long>
    {
        private Image(string title, Uri uri, string description)
            : base(SnowFlakeIdGenerator.NewId)
        {
            _title = title;
            _url = uri;
            _description = description;
        }

        public static Image CreateNewImage(string title, Uri uri, string description)
        {
            return new Image(title, uri, description);
        }

        #region Fields

        private string _title = string.Empty;

        private string _description = string.Empty;

        private readonly Uri _url;

        private readonly DateTime _uploadedAt = DateTime.Now;

        private bool _isRemoved = false;

        private bool _isNsfw = false;

        private List<Tag> _tags = [];

        #endregion

        #region Properties
        internal DateTime UploadedTime => _uploadedAt;

        internal Uri ImageUrl => _url;

        #endregion

        #region Methods


        internal void UpdateImageInfo(
            string title,
            string description,
            bool isNsfw,
            IEnumerable<Tag> tags
        )
        {
            _isNsfw = isNsfw;
            _title = title;
            _description = description;
            _tags = tags.ToList();
        }

        internal void Remove() => _isRemoved = true;

        internal void Restore() => _isRemoved = false;

        internal void SetNsfw(bool isNsfw) => _isNsfw = isNsfw;

        #endregion
    }
}
