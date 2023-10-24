using Shared.Primitives;
using Shared.Utilities;

namespace SastImg.Domain.Albums.Images
{
    public sealed class Image : Entity<long>
    {
        private Image(string title, Uri uri, string description)
            : base(SnowFlakeIdGenerator.NewId)
        {
            this.title = title;
            this.uri = uri;
            this.description = description;
        }

        internal static Image CreateNewImage(string title, Uri uri, string description)
        {
            return new Image(title, uri, description);
        }

        #region Properties

        private string title = string.Empty;

        private string description = string.Empty;

        private Uri uri;

        private DateTime uploadedAt = DateTime.Now;

        private bool isRemoved = false;

        private int viewCount = 0;

        private int categoryId;

        private ICollection<long> tags = new List<long>();

        #endregion

        #region Methods

        internal void UpdateImageInfo(string title, string description)
        {
            this.title = title;
            this.description = description;
        }

        internal void Remove() => isRemoved = true;

        internal void Restore() => isRemoved = false;

        internal void AddViewCount() => viewCount++;

        internal void ChangeCategory(int id) => categoryId = id;

        #endregion
    }
}
