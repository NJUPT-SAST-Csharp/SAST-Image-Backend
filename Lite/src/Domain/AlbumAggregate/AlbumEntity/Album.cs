using System.Collections.Immutable;
using Domain.AlbumAggregate.Commands;
using Domain.AlbumAggregate.Events;
using Domain.AlbumAggregate.Exceptions;
using Domain.AlbumAggregate.ImageEntity;
using Domain.AlbumAggregate.Services;
using Domain.Entity;
using Domain.Shared;
using Domain.UserAggregate.UserEntity;

namespace Domain.AlbumAggregate.AlbumEntity;

public sealed class Album : EntityBase<AlbumId>
{
    private Album()
        : base(default) { }

    private AlbumStatus _status = AlbumStatus.Available;

    private AlbumTitle _title;

    private Cover _cover = Cover.Default;

    private AccessLevel _accessLevel;

    private UserId[] _collaborators = [];

    private readonly UserId _author;

    private readonly List<Subscribe> _subscribes = [];

    private readonly List<Image> _images = [];

    internal Album(CreateAlbumCommand command)
        : base(AlbumId.GenerateNew())
    {
        _author = command.Actor.Id;
        _title = command.Title;
    }

    internal static async Task<AlbumId> CreateAsync(
        CreateAlbumCommand command,
        ICategoryExistenceChecker categoryChecker,
        IAlbumTitleUniquenessChecker titleChecker,
        IAlbumRepository repository,
        CancellationToken cancellationToken
    )
    {
        await Task.WhenAll(
            titleChecker.CheckAsync(command.Title, cancellationToken),
            categoryChecker.CheckAsync(command.CategoryId, cancellationToken)
        );

        Album album = new(command);

        await repository.AddAsync(album, cancellationToken);

        album.AddDomainEvent(
            new AlbumCreatedEvent(
                album.Id,
                command.Actor.Id,
                command.CategoryId,
                command.Title,
                command.Description,
                command.AccessLevel
            )
        );

        return album.Id;
    }

    public void UpdateDescription(UpdateAlbumDescriptionCommand command)
    {
        if (CanNotManage(command.Actor))
            throw new NoPermissionException();
        if (_status.IsRemoved)
            throw new AlbumRemovedException();

        AddDomainEvent(new AlbumDescriptionUpdatedEvent(Id, command.Description));
    }

    public void UpdateAccessLevel(UpdateAccessLevelCommand command)
    {
        if (CanNotManage(command.Actor))
            throw new NoPermissionException();
        if (_status.IsRemoved)
            throw new AlbumRemovedException();
        if (_accessLevel == command.AccessLevel)
            return;

        _accessLevel = command.AccessLevel;

        AddDomainEvent(new AlbumAccessLevelUpdatedEvent(Id, command.AccessLevel));
    }

    public void UpdateTitle(UpdateAlbumTitleCommand command)
    {
        if (CanNotManage(command.Actor))
            throw new NoPermissionException();
        if (_status.IsRemoved)
            throw new AlbumRemovedException();
        if (_title == command.Title)
            return;

        _title = command.Title;

        AddDomainEvent(new AlbumTitleUpdatedEvent(Id, _title));
    }

    public void UpdateCollaborators(UpdateCollaboratorsCommand command)
    {
        if (CanNotManage(command.Actor))
            throw new NoPermissionException();
        if (_status.IsRemoved)
            throw new AlbumRemovedException();

        if (_collaborators.SequenceEqual(command.Collaborators.Value))
            return;

        _collaborators = command.Collaborators.Value;

        AddDomainEvent(new AlbumCollaboratorsUpdatedEvent(Id, command.Collaborators));
    }

    public void UpdateCategory(UpdateAlbumCategoryCommand command)
    {
        if (CanNotManage(command.Actor))
            throw new NoPermissionException();
        if (_status.IsRemoved)
            throw new AlbumRemovedException();

        AddDomainEvent(new AlbumCategoryUpdatedEvent(Id, command.Category));
    }

    public void UpdateTags(UpdateAlbumTagsCommand command)
    {
        if (CanNotManage(command.Actor))
            throw new NoPermissionException();
        if (_status.IsRemoved)
            throw new AlbumRemovedException();

        AddDomainEvent(new AlbumTagsUpdatedEvent(Id, command.Tags));
    }

    public void UpdateCover(UpdateCoverCommand command)
    {
        if (CanNotManage(command.Actor))
            throw new NoPermissionException();
        if (_status.IsRemoved)
            throw new AlbumRemovedException();

        if (command.CoverImage is null)
        {
            var imageId = _images.LatestImage()?.Id;
            _cover = new(imageId, true);
            AddDomainEvent(AlbumCoverUpdatedEvent.ContainedImageOrEmpty(Id, imageId));
            return;
        }

        _cover = Cover.UserCustomCover;
        AddDomainEvent(AlbumCoverUpdatedEvent.UserCustomImage(Id, command.CoverImage));
    }

    public ImageId AddImage(AddImageCommand command)
    {
        if (CanNotManageImages(command.Actor) && _accessLevel.OthersCanNotWrite)
            throw new NoPermissionException();
        if (_status.IsRemoved)
            throw new AlbumRemovedException();

        Image image = new(command);

        _images.Add(image);

        AddDomainEvent(
            new ImageAddedEvent(
                Id,
                image.Id,
                _author,
                command.Title,
                command.Tags,
                _accessLevel,
                new(_collaborators),
                command.ImageFile,
                DateTime.UtcNow,
                command.Actor.Id
            )
        );

        if (_cover.IsLatestImage)
        {
            _cover = _cover with { Id = image.Id };
            AddDomainEvent(AlbumCoverUpdatedEvent.NewAddedImage(Id, command.ImageFile));
        }

        return image.Id;
    }

    public void RemoveImage(RemoveImageCommand command)
    {
        var image = _images.FindById(command.Image);

        if (
            CanNotManageImages(command.Actor)
            && (_accessLevel.OthersCanNotWrite || image.IsNotUploader(command.Actor))
        )
            throw new NoPermissionException();
        if (_status.IsRemoved)
            throw new AlbumRemovedException();

        image.Remove(command);

        if (_cover.Id == command.Image)
        {
            var imageId = _images.LatestImage()?.Id;
            _cover = _cover with { Id = imageId };
            AddDomainEvent(AlbumCoverUpdatedEvent.ContainedImageOrEmpty(Id, imageId));
        }
    }

    public void RestoreImage(RestoreImageCommand command)
    {
        if (CanNotManageImages(command.Actor))
            throw new NoPermissionException();
        if (_status.IsRemoved)
            throw new AlbumRemovedException();

        var image = _images.FindById(command.Image);
        image.Restore(command);

        if (_cover.IsLatestImage && image.Equals(_images.LatestImage()))
        {
            _cover = new(image.Id, true);
            AddDomainEvent(AlbumCoverUpdatedEvent.ContainedImageOrEmpty(Id, image.Id));
        }
    }

    public void DeleteImage(DeleteImageCommand command)
    {
        if (CanNotManage(command.Actor))
            throw new NoPermissionException();

        _images.DeleteImage(command.Image);

        if (_cover.Id == command.Image)
        {
            var imageId = _images.LatestImage()?.Id;
            _cover = _cover with { Id = imageId };
            AddDomainEvent(AlbumCoverUpdatedEvent.ContainedImageOrEmpty(Id, imageId));
        }
    }

    public void Remove(RemoveAlbumCommand command)
    {
        if (CanNotManage(command.Actor))
            throw new NoPermissionException();
        if (_status.IsRemoved)
            return;

        _status = AlbumStatus.Removed(DateTime.UtcNow);

        AddDomainEvent(new AlbumRemovedEvent(Id, _status));
        foreach (var image in _images)
        {
            image.AlbumRemoved(command);
        }
    }

    public void Restore(RestoreAlbumCommand command)
    {
        if (CanNotManage(command.Actor))
            throw new NoPermissionException();
        if (_status.IsAvailable)
            return;

        _status = AlbumStatus.Available;

        AddDomainEvent(new AlbumRestoredEvent(Id, _status));
        foreach (var image in _images)
        {
            image.AlbumRestored(command);
        }
    }

    public void Subscribe(SubscribeCommand command)
    {
        if (_status.IsRemoved)
            throw new AlbumRemovedException();
        if (_subscribes.ContainsUser(command.Actor.Id))
            return;
        if (_accessLevel == AccessLevel.Private && CanNotManage(command.Actor))
            throw new NoPermissionException();

        _subscribes.Add(new(Id, command.Actor.Id));

        AddDomainEvent(new AlbumSubscribedEvent(Id, command.Actor.Id));
    }

    public void Unsubscribe(UnsubscribeCommand command)
    {
        if (_status.IsRemoved)
            throw new AlbumRemovedException();
        if (_subscribes.NotContainsUser(command.Actor.Id))
            return;
        if (_accessLevel == AccessLevel.Private && CanNotManage(command.Actor))
            throw new NoPermissionException();

        _subscribes.RemoveUser(command.Actor.Id);

        AddDomainEvent(new AlbumUnsubscribedEvent(Id, command.Actor.Id));
    }

    public void LikeImage(LikeImageCommand command)
    {
        if (_status.IsRemoved)
            throw new AlbumRemovedException();
        if (_accessLevel == AccessLevel.Private && CanNotManage(command.Actor))
            throw new NoPermissionException();

        var image = _images.FindById(command.Image);

        image.Like(command);
    }

    public void UnlikeImage(UnlikeImageCommand command)
    {
        if (_status.IsRemoved)
            throw new AlbumRemovedException();
        if (_accessLevel == AccessLevel.Private && CanNotManage(command.Actor))
            throw new NoPermissionException();

        var image = _images.FindById(command.Image);

        image.Unlike(command);
    }

    public void UpdateImageTags(UpdateImageTagsCommand command)
    {
        if (_status.IsRemoved)
            throw new AlbumRemovedException();
        if (_accessLevel == AccessLevel.Private && CanNotManage(command.Actor))
            throw new NoPermissionException();

        var image = _images.FindById(command.ImageId);

        image.UpdateTags(command);
    }

    private bool IsOwnedBy(Actor actor) => _author == actor.Id;

    private bool CanManage(Actor actor) => IsOwnedBy(actor) || actor.IsAdmin;

    private bool CanNotManage(Actor actor) => !CanManage(actor);

    private bool CanManageImages(Actor actor) =>
        CanManage(actor) || _collaborators.Contains(actor.Id);

    private bool CanNotManageImages(Actor actor) => !CanManageImages(actor);
}
