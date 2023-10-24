using SastImg.Domain.Albums.Images;
using Shared.Primitives;
using Shared.Utilities;

namespace SastImg.Domain.Albums
{
    /// <summary>
    /// The aggregate root of the SastImg Domain, containing references of images and authorId (user).
    /// </summary>

    public sealed class Album : AggregateRoot<long>
    {
        private Album(long authorId, string title, string description, Accessibility accessibility)
            : base(SnowFlakeIdGenerator.NewId)
        {
            Title = title;
            AuthorId = authorId;
            Description = description;
            Accessibility = accessibility;
        }

        public static Album CreateNewAlbum(
            long authorId,
            string title,
            string description,
            Accessibility accessibility
        )
        {
            return new Album(authorId, title, description, accessibility);
        }

        private readonly ICollection<Image> images = new List<Image>();

        #region Properties

        public string Title { get; private set; } = string.Empty;

        public string Description { get; private set; } = string.Empty;

        public Accessibility Accessibility { get; private set; }

        public bool IsRemoved { get; private set; } = false;

        public Cover Cover { get; } = new Cover();

        public DateTime CreatedAt { get; } = DateTime.Now;

        public DateTime UpdatedAt { get; private set; } = DateTime.Now;

        public long AuthorId { get; private init; }

        public ICollection<long> Collaborators { get; } = new List<long>();

        public IEnumerable<Image> Images => images;

        #endregion

        #region Methods

        public void Remove() => IsRemoved = true;

        public void Restore() => IsRemoved = false;

        public void UpdateAlbumInfo(string title, string description, Accessibility accessibility)
        {
            Title = title;
            Description = description;
            Accessibility = accessibility;
        }

        public long AddImage(string title, Uri uri, string description)
        {
            var image = Image.CreateNewImage(title, uri, description);
            UpdatedAt = DateTime.Now;
            return image.Id;
        }

        public void RemoveImage(long imageId)
        {
            var image = images.FirstOrDefault(image => image.Id == imageId);
            image?.SetRemoval(true);
        }

        public void RestoreImage(long imageId)
        {
            var image = images.FirstOrDefault(image => image.Id == imageId);
            image?.SetRemoval(false);
        }

        public void HideImage(long imageId)
        {
            var image = images.FirstOrDefault(image => image.Id == imageId);
            image?.SetVisibility(false);
        }

        public void ShowImage(long imageId)
        {
            var image = images.FirstOrDefault(image => image.Id == imageId);
            image?.SetVisibility(true);
        }

        public void UpdateImage(
            long imageId,
            string title,
            string description,
            IEnumerable<long> tags
        ) { }

        #endregion
    }
}
