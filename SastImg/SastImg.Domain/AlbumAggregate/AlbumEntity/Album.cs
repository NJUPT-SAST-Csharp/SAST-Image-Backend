using Primitives.Entity;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Events;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using Shared.Primitives;
using Shared.Utilities;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity
{
    /// <summary>
    /// The aggregate root of the Album/Image aggregate, containing reference of images.
    /// </summary>

    public sealed class Album : EntityBase<long>, IAggregateRoot<Album>
    {
        private Album(
            long authorId,
            long categoryId,
            string title,
            string description,
            Accessibility accessibility
        )
            : base(SnowFlakeIdGenerator.NewId)
        {
            _title = title;
            _authorId = authorId;
            _categoryId = categoryId;
            _description = description;
            _accessibility = accessibility;
        }

        public static Album CreateNewAlbum(
            long authorId,
            long categoryId,
            string title,
            string description,
            Accessibility accessibility
        )
        {
            var album = new Album(authorId, categoryId, title, description, accessibility);
            album.AddDomainEvent(new CreateAlbumDomainEvent(album.Id, authorId));
            return album;
        }

        #region Fields

        private string _title = string.Empty;

        private string _description = string.Empty;

        private long _categoryId;

        private Accessibility _accessibility;

        private bool _isRemoved = false;

        private Cover _cover = new(null, true);

        private DateTime _createdAt = DateTime.UtcNow;

        private DateTime _updatedAt = DateTime.UtcNow;

        private long _authorId;

        private long[] _collaborators = [];

        private readonly List<Image> _images = [];

        #endregion

        #region Methods

        public void RemoveAlbum()
        {
            _isRemoved = true;
            // TODO: Raise domain event.
        }

        public void RestoreAlbum()
        {
            _isRemoved = false;
            // TODO: Raise domain event.
        }

        public void SetCoverAsLatestImage()
        {
            var image = _images.OrderBy(i => i.UploadedTime).FirstOrDefault();
            _cover = new(image?.ImageUrl, true);
            // TODO: Raise domain event
        }

        public void SetCoverAsContainedImage(long imageId)
        {
            var image = _images.FirstOrDefault(image => image.Id == imageId);
            _cover = new(image?.ImageUrl, false);
            // TODO: Raise domain event
        }

        public void UpdateAlbumInfo(string title, string description)
        {
            _title = title;
            _description = description;
        }

        public void ChangeAccessiblity(Accessibility accessibility)
        {
            if (_accessibility != Accessibility.Private && accessibility == Accessibility.Private)
            {
                // TODO: Raise domain event
            }
            _accessibility = accessibility;
        }

        public long AddImage(string title, Uri uri, string description, long[] tags)
        {
            var image = Image.CreateNewImage(title, uri, description, tags);

            _updatedAt = DateTime.UtcNow;
            if (_cover.IsLatestImage)
            {
                _cover = new(uri, true);
            }
            // TODO: Raise domain event
            return image.Id;
        }

        public void RemoveImage(long imageId)
        {
            var image = _images.FirstOrDefault(image => image.Id == imageId);
            if (image is not null)
            {
                image.Remove();
                // TODO: Raise domain event
            }
        }

        public void RestoreImage(long imageId)
        {
            var image = _images.FirstOrDefault(image => image.Id == imageId);
            if (image is not null)
            {
                image.Restore();
                // TODO: Raise domain event
            }
        }

        public void UpdateImage(long imageId, string title, string description, long[] tags)
        {
            var image = _images.FirstOrDefault(image => image.Id == imageId);
            if (image is not null)
            {
                image.UpdateImageInfo(title, description, tags);
                // TODO: Raise domain event
            }
        }

        #endregion
    }
}
