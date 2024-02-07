﻿using Primitives.Entity;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Events;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using SastImg.Domain.CategoryEntity;
using SastImg.Domain.TagEntity;
using Shared.Primitives;
using Shared.Utilities;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity
{
    /// <summary>
    /// The aggregate root of the Album/Image aggregate, containing reference of images.
    /// </summary>

    public sealed class Album : EntityBase<AlbumId>, IAggregateRoot<Album>
    {
        private Album(
            UserId authorId,
            CategoryId categoryId,
            string title,
            string description,
            Accessibility accessibility
        )
            : base(new(SnowFlakeIdGenerator.NewId))
        {
            _title = title;
            _authorId = authorId;
            _categoryId = categoryId;
            _description = description;
            _accessibility = accessibility;
        }

        public static Album CreateNewAlbum(
            UserId authorId,
            CategoryId categoryId,
            string title,
            string description,
            Accessibility accessibility
        )
        {
            var album = new Album(authorId, categoryId, title, description, accessibility);
            album.AddDomainEvent(new AlbumCreatedDomainEvent(album.Id, authorId));
            return album;
        }

        #region Fields

        private string _title = string.Empty;

        private string _description = string.Empty;

        private CategoryId _categoryId;

        private Accessibility _accessibility;

        private bool _isRemoved = false;

        private Cover _cover = new(null, true);

        private DateTime _createdAt = DateTime.UtcNow;

        private DateTime _updatedAt = DateTime.UtcNow;

        private UserId _authorId;

        private UserId[] _collaborators = [];

        private readonly IList<Image> _images = [];

        #endregion

        #region Properties

        public bool IsOwnedBy(UserId userId) => _authorId == userId;

        public bool IsManagedBy(UserId userId) =>
            _authorId == userId || _collaborators.Contains(userId);

        #endregion

        #region Methods


        public void Remove()
        {
            if (_isRemoved)
                return;
            _isRemoved = true;
            // TODO: Raise domain event.
        }

        public void Restore()
        {
            if (_isRemoved == false)
                return;
            _isRemoved = false;
            // TODO: Raise domain event.
        }

        public void SetCoverAsLatestImage()
        {
            var image = _images.OrderBy(i => i.UploadedTime).FirstOrDefault();
            if (image is not null)
            {
                _cover = new(image?.ImageUrl, true);
                // TODO: Raise domain event
            }
        }

        public void SetCoverAsContainedImage(ImageId imageId)
        {
            var image = _images.FirstOrDefault(image => image.Id == imageId);
            _cover = new(image?.ImageUrl, false);
            // TODO: Raise domain event
        }

        public void UpdateAlbumInfo(
            string title,
            string description,
            CategoryId categoryId,
            Accessibility accessibility
        )
        {
            _title = title;
            _description = description;
            _categoryId = categoryId;
            _accessibility = accessibility;
        }

        public ImageId AddImage(string title, Uri uri, string description, TagId[] tags)
        {
            var image = Image.CreateNewImage(title, uri, description, tags);

            _updatedAt = DateTime.UtcNow;
            if (_cover.IsLatestImage)
            {
                _cover = new(uri, true);
            }
            image.AddDomainEvent(new ImageAddedDomainEvent(Id, _authorId, image.Id));
            return image.Id;
        }

        public void RemoveImage(ImageId imageId)
        {
            var image = _images.FirstOrDefault(image => image.Id == imageId);
            if (image is not null)
            {
                image.Remove();
                // TODO: Raise domain event
            }
        }

        public void RestoreImage(ImageId imageId)
        {
            var image = _images.FirstOrDefault(image => image.Id == imageId);
            if (image is not null)
            {
                image.Restore();
                // TODO: Raise domain event
            }
        }

        public void UpdateImage(ImageId imageId, string title, string description, TagId[] tags)
        {
            var image = _images.FirstOrDefault(image => image.Id == imageId);
            if (image is not null)
            {
                image.UpdateImageInfo(title, description, tags);
                // TODO: Raise domain event
            }
        }

        public void UpdateCollaborators(UserId[] collaborators)
        {
            _collaborators = collaborators;
            //TODO: Raise domain event
        }

        #endregion
    }
}
