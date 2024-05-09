using Primitives.Entity;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Events;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Rules;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using SastImg.Domain.CategoryEntity;
using SastImg.Domain.TagEntity;
using Shared.Primitives;
using Utilities;

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
            AlbumTitle title,
            AlbumDescription description,
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
            AlbumTitle title,
            AlbumDescription description,
            Accessibility accessibility
        )
        {
            var album = new Album(authorId, categoryId, title, description, accessibility);
            album.AddDomainEvent(new AlbumCreatedDomainEvent(album.Id, authorId));
            return album;
        }

        #region Fields

        private AlbumTitle _title;

        private AlbumDescription _description;

        private CategoryId _categoryId;

        private Accessibility _accessibility;

        private bool _isRemoved = false;

        private bool _isArchived = false;

        private Cover _cover = Cover.Empty;

        private DateTime _createdAt = DateTime.UtcNow;

        private DateTime _updatedAt = DateTime.UtcNow;

        private UserId _authorId;

        private UserId[] _collaborators = [];

        private readonly List<Image> _images = [];

        #endregion

        #region Properties

        public bool IsOwnedBy(UserId userId) => _authorId == userId;

        public bool IsManagedBy(UserId userId) =>
            _authorId == userId || Array.Exists(_collaborators, id => id == userId);

        #endregion

        #region Methods


        public void Remove()
        {
            if (_isRemoved)
                return;
            _isRemoved = true;
            foreach (var image in _images)
            {
                image.Remove();
            }
            // TODO: Raise domain event.
        }

        public void Restore()
        {
            if (_isRemoved == false)
                return;
            _isRemoved = false;
            foreach (var image in _images)
            {
                image.Restore();
            }
            // TODO: Raise domain event.
        }

        public void Archive()
        {
            if (_isArchived)
                return;
            _isArchived = true;

            _collaborators = [];
        }

        public void SetCoverAsLatestImage()
        {
            CheckRule(new ActionAllowedOnlyWhenNotArchivedRule(_isArchived));

            var image = _images
                .Where(image => image.IsRemoved == false)
                .OrderByDescending(i => i.UploadedTime)
                .FirstOrDefault();

            _cover = new(image?.Url, true);
            // TODO: Raise domain event
        }

        public void SetCoverAsContainedImage(ImageId imageId)
        {
            CheckRule(new ActionAllowedOnlyWhenNotArchivedRule(_isArchived));

            var image = _images.FirstOrDefault(image => image.Id == imageId);
            _cover = new(image?.Url, false);
            // TODO: Raise domain event
        }

        public void UpdateAlbumInfo(
            AlbumTitle title,
            AlbumDescription description,
            CategoryId categoryId,
            Accessibility accessibility
        )
        {
            CheckRule(new ActionAllowedOnlyWhenNotArchivedRule(_isArchived));

            _title = title;
            _description = description;
            _categoryId = categoryId;
            _accessibility = accessibility;

            if (_accessibility == Accessibility.Private)
            {
                _collaborators = [];
            }
        }

        public ImageId AddImage(
            ImageTitle title,
            ImageDescription description,
            ImageUrl url,
            TagId[] tags
        )
        {
            CheckRule(new ActionAllowedOnlyWhenNotArchivedRule(_isArchived));

            var image = new Image(title, description, url, tags);

            _images.Add(image);

            _updatedAt = DateTime.UtcNow;
            if (_cover.IsLatestImage)
            {
                _cover = new(url, true);
            }

            image.AddDomainEvent(new ImageAddedDomainEvent(Id, _authorId, image.Id));
            return image.Id;
        }

        public void RemoveImage(ImageId imageId)
        {
            CheckRule(new ActionAllowedOnlyWhenNotArchivedRule(_isArchived));

            var image = _images.FirstOrDefault(image => image.Id == imageId);
            if (image is not null)
            {
                image.Remove();
                if (image.Url.Thumbnail.Equals(_cover.Url))
                {
                    _cover = _cover with { Url = null };
                }
            }
        }

        public void RestoreImage(ImageId imageId)
        {
            CheckRule(new ActionAllowedOnlyWhenNotArchivedRule(_isArchived));
            CheckRule(new RestoreImageOnlyWhenAlbumNotRemovedRule(_isRemoved));

            var image = _images.FirstOrDefault(image => image.Id == imageId);
            if (image is not null)
            {
                image.Restore();
                if (_cover.IsLatestImage)
                {
                    SetCoverAsLatestImage();
                }
            }
        }

        public void UpdateCollaborators(UserId[] collaborators)
        {
            CheckRule(new ActionAllowedOnlyWhenNotArchivedRule(_isArchived));

            _collaborators = collaborators.Distinct().Where(c => c != _authorId).ToArray();
            //TODO: Raise domain event
        }

        #endregion
    }
}
