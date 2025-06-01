using Identity;
using Primitives.Entity;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Commands;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Events;
using SastImg.Domain.AlbumAggregate.AlbumEntity.Exceptions;
using SastImg.Domain.AlbumAggregate.ImageEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity.Commands;
using SastImg.Domain.AlbumTagEntity;
using SastImg.Domain.CategoryEntity;

namespace SastImg.Domain.AlbumAggregate.AlbumEntity;

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
        : base(AlbumId.GenerateNew())
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

    #region Helpers

    private bool IsOwnedBy(Requester user) => _authorId == user.Id;

    private bool CanManage(Requester user) => IsOwnedBy(user) || user.IsAdmin;

    private bool CanNotManage(Requester user) => !CanManage(user);

    private bool CanManageImages(Requester user) =>
        CanManage(user) || _collaborators.Contains(user.Id);

    private bool CanNotManageImages(Requester user) => !CanManageImages(user);

    #endregion

    #region Methods


    public void Remove(RemoveAlbumCommand command)
    {
        NoPermissionDomainException.ThrowIf(CanNotManage(command.Requester));

        if (_isRemoved)
            return;

        _isRemoved = true;

        foreach (var image in _images)
        {
            image.Remove();
        }
        // TODO: Raise domain event.
    }

    public void Restore(RestoreAlbumCommand command)
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
        AlbumRemovedDomainException.ThrowIf(_isRemoved);

        if (_isArchived)
            return;
        _isArchived = true;

        _collaborators = [];
    }

    public void SetCoverAsLatestImage()
    {
        AlbumRemovedDomainException.ThrowIf(_isRemoved);
        AlbumArchivedDomainException.ThrowIf(_isArchived);

        var image = _images
            .Where(image => image.IsRemoved == false)
            .OrderByDescending(i => i.UploadedTime)
            .FirstOrDefault();

        _cover = new(image?.Id, true);
    }

    public void SetCoverAsContainedImage(ImageId imageId)
    {
        AlbumRemovedDomainException.ThrowIf(_isRemoved);
        AlbumArchivedDomainException.ThrowIf(_isArchived);

        var image = _images.FirstOrDefault(image => image.Id == imageId);
        _cover = new(image?.Id, false);
    }

    public void UpdateAlbumInfo(
        AlbumTitle title,
        AlbumDescription description,
        CategoryId categoryId,
        Accessibility accessibility
    )
    {
        AlbumRemovedDomainException.ThrowIf(_isRemoved);
        AlbumArchivedDomainException.ThrowIf(_isArchived);

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
        ImageTagId[] tags
    )
    {
        AlbumRemovedDomainException.ThrowIf(_isRemoved);
        AlbumArchivedDomainException.ThrowIf(_isArchived);

        var image = new Image(title, description, url, tags);

        _images.Add(image);

        _updatedAt = DateTime.UtcNow;
        if (_cover.IsLatestImage)
        {
            _cover = _cover with { ImageId = image.Id };
        }

        image.AddDomainEvent(new ImageAddedDomainEvent(Id, _authorId, image.Id));
        return image.Id;
    }

    public void RemoveImage(RemoveImageCommand command)
    {
        NoPermissionDomainException.ThrowIf(CanNotManageImages(command.Requester));
        AlbumRemovedDomainException.ThrowIf(_isRemoved);
        AlbumArchivedDomainException.ThrowIf(_isArchived);

        var image = _images.FirstOrDefault(image => image.Id == command.ImageId);

        if (image is not null)
        {
            image.Remove();
            if (_cover == image)
            {
                _cover = _cover with { ImageId = null };
            }
        }
    }

    public void RestoreImage(ImageId imageId)
    {
        AlbumRemovedDomainException.ThrowIf(_isRemoved);
        AlbumArchivedDomainException.ThrowIf(_isArchived);

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
        AlbumRemovedDomainException.ThrowIf(_isRemoved);
        AlbumArchivedDomainException.ThrowIf(_isArchived);

        _collaborators = collaborators.Distinct().Where(c => c != _authorId).ToArray();
        //TODO: Raise domain event
    }

    #endregion
}
