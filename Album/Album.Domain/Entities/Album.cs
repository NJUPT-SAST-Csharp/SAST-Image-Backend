using Album.Domain.Enums;
using Album.Domain.ValueObjects;
using Shared.Primitives;
using Shared.Utilities;

namespace Album.Domain.Entities
{
    /// <summary>
    /// The aggregate root of the Album Domain, containing references of images and authorId (user).
    /// </summary>

    public sealed class Album : AggregateRoot<long>
    {
        private readonly ICollection<Image> images = new List<Image>();

        public Album(long authorId, string title)
            : base(SnowFlakeIdGenerator.NewId)
        {
            Title = title;
            AuthorId = authorId;
        }

        #region Properties

        public string Title { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public Accessibility Accessibility { get; private set; }

        public Cover Cover { get; } = new Cover();

        public DateTime CreatedAt { get; } = DateTime.Now;

        public DateTime UpdatedAt { get; private set; } = DateTime.Now;

        public long AuthorId { get; private init; }

        public IEnumerable<Image> Images => images;

        public ICollection<long> Collaborators { get; } = new List<long>();

        #endregion

        #region Methods

        public void UpdateAlbumInfo(string title, string description, Accessibility accessibility)
        {
            Title = title;
            Description = description;
            Accessibility = accessibility;
        }

        public void AddImage(string title, string description, Uri uri)
        {
            Image image = new(title, description) { Uri = uri };
            images.Add(image);
            UpdatedAt = DateTime.Now;
        }

        public void RemoveImage(long id)
        {
            Image? image = images.FirstOrDefault(image => image.Id == id);
            if (image is not null)
                images.Remove(image);
        }

        #endregion
    }
}
