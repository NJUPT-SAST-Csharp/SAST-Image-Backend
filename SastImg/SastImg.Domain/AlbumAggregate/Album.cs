﻿using Shared.Primitives;
using Shared.Utilities;

namespace SastImg.Domain.AlbumAggregate
{
    /// <summary>
    /// The aggregate root of the SastImg Domain, containing references of images and authorId (user).
    /// </summary>

    public sealed class Album : Entity<long>, IAggregateRoot<Album>
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
            return new Album(authorId, categoryId, title, description, accessibility);
        }

        #region Fields

        private string _title = string.Empty;

        private string _description = string.Empty;

        private long _categoryId;

        private Accessibility _accessibility;

        private bool _isRemoved = false;

        private Cover _cover = new(null, true);

        private DateTime _createdAt = DateTime.Now;

        private DateTime _updatedAt = DateTime.Now;

        private long _authorId;

        private readonly List<long> _collaborators = [];

        private readonly List<Image> _images = [];

        #endregion

        #region Properties

        public bool IsRemoved => _isRemoved;

        #endregion

        #region Methods

        public void Remove() => _isRemoved = true;

        public void Restore() => _isRemoved = false;

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

        public void UpdateAlbumInfo(string title, string description, Accessibility accessibility)
        {
            _title = title;
            _description = description;
            _accessibility = accessibility;

            // TODO: Raise domain event
        }

        public long AddImage(string title, Uri uri, string description, IEnumerable<long> tags)
        {
            var image = Image.CreateNewImage(title, uri, description, tags);

            _updatedAt = DateTime.Now;
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
            image?.Remove();
            // TODO: Raise domain event
        }

        public void RestoreImage(long imageId)
        {
            var image = _images.FirstOrDefault(image => image.Id == imageId);
            image?.Restore();
            // TODO: Raise domain event
        }

        public void UpdateImage(
            long imageId,
            string title,
            string description,
            bool isNsfw,
            IEnumerable<long> tags
        )
        {
            var image = _images.FirstOrDefault(image => image.Id == imageId);
            if (image is not null)
            {
                image.UpdateImageInfo(title, description, isNsfw, tags);
                // TODO: Raise domain event
            }
        }

        #endregion
    }
}