using Shared.Primitives;
using Shared.Utilities;

namespace SastImg.Domain.Albums.Images
{
    public sealed class Image : Entity<long>
    {
        private Image(string title, Uri uri, string description)
            : base(SnowFlakeIdGenerator.NewId)
        {
            Title = title;
            Uri = uri;
            Description = description;
        }

        internal static Image CreateNewImage(string title, Uri uri, string description)
        {
            return new Image(title, uri, description);
        }

        #region Properties

        public string Title { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public Uri Uri { get; private init; }

        public DateTime UploadedAt { get; } = DateTime.Now;

        public bool IsRemoved { get; private set; } = false;

        public bool IsHidden { get; private set; } = false;

        public int ViewCount { get; private set; } = 0;

        public int CategoryId { get; private set; }

        public ICollection<long> Tags { get; } = new List<long>();

        #endregion

        #region Methods

        internal void UpdateImageInfo(string title, string description)
        {
            Title = title;
            Description = description;
        }

        internal void SetRemoval(bool isRemoved)
        {
            IsRemoved = isRemoved;
        }

        internal void SetVisibility(bool isVisible)
        {
            IsHidden = !isVisible;
        }

        internal void AddViewCount() => ViewCount++;

        internal void ChangeCategory(int id) => CategoryId = id;

        #endregion
    }
}
