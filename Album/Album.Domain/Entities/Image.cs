using Shared.Primitives;
using Shared.Utilities;

namespace Album.Domain.Entities
{
    public sealed class Image : Entity<long>
    {
        internal Image(string title, string description)
            : base(SnowFlakeIdGenerator.NewId)
        {
            Title = title;
            Description = description;
        }

        #region Properties

        public string Title { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public required Uri Uri { get; init; }

        public DateTime UploadedAt { get; } = DateTime.Now;

        public int ViewCount { get; private set; } = 0;

        public int CategoryId { get; private set; }

        public ICollection<long> Tags { get; } = new List<long>();

        public ICollection<long> LikedBy { get; } = new List<long>();

        #endregion

        #region Methods

        public void UpdateImageInfo(string title, string description)
        {
            Title = title;
            Description = description;
        }

        public void AddViewCount() => ViewCount++;

        public void ChangeCategory(int id) => CategoryId = id;

        #endregion
    }
}
