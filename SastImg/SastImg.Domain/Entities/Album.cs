using SastImg.Domain.Enums;
using SastImg.Domain.ValueObjects;
using Shared.Primitives;
using Shared.Utilities;

namespace SastImg.Domain.Entities
{
    /// <summary>
    /// The aggregate root of the SastImg Domain, containing references of images and authorId (user).
    /// </summary>

    public sealed class Album : AggregateRoot<long>
    {
        private readonly ICollection<Image> images = new List<Image>();

        public Album(
            long authorId,
            string title,
            string description = "",
            Accessibility accessibility = Accessibility.Public
        )
            : base(SnowFlakeIdGenerator.NewId)
        {
            Title = title;
            AuthorId = authorId;
            Description = description;
            Accessibility = accessibility;
        }

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

        #endregion

        #region Methods

        public void UpdateAlbumInfo(string title, string description, Accessibility accessibility)
        {
            Title = title;
            Description = description;
            Accessibility = accessibility;
        }

        public void Remove() => IsRemoved = true;

        public void Restore() => IsRemoved = false;

        public void AddImage(string title, string description, Uri uri)
        {
            Image image = new(title, description) { Uri = uri };
            images.Add(image);
            UpdatedAt = DateTime.Now;
        }

        public void RemoveImageById(long id)
        {
            var image = GetImageById(id);
            if (image is not null)
                image.IsRemoved = true;
        }

        public void RestoreImageById(long id)
        {
            var image = GetImageById(id);
            if (image is not null)
                image.IsRemoved = true;
        }

        public Image? GetImageById(long id) =>
            images.Where(image => !image.IsRemoved).FirstOrDefault(image => image.Id == id);

        #endregion
    }
}
