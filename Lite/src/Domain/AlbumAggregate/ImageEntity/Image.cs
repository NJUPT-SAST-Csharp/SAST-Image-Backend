using Domain.AlbumAggregate.Commands;
using Domain.AlbumAggregate.Events;
using Domain.AlbumAggregate.Exceptions;
using Domain.Entity;
using Domain.Shared;
using Domain.UserAggregate.UserEntity;

namespace Domain.AlbumAggregate.ImageEntity;

internal sealed class Image : EntityBase<ImageId>
{
    private Image()
        : base(default) { }

    private readonly UserId _uploader;
    private readonly List<Like> _likes = [];
    private ImageStatus _status = ImageStatus.Available;

    public Image(AddImageCommand command)
        : base(ImageId.GenerateNew())
    {
        _uploader = command.Actor.Id;
    }

    public void Like(LikeImageCommand command)
    {
        CheckAvailability();

        if (_likes.ContainsUser(command.Actor.Id))
            return;

        _likes.Add(new(Id, command.Actor.Id));

        AddDomainEvent(new ImageLikedEvent(Id, command.Actor.Id));
    }

    public void Unlike(UnlikeImageCommand command)
    {
        CheckAvailability();

        if (_likes.NotContainsUser(command.Actor.Id))
            return;

        _likes.RemoveUser(command.Actor.Id);

        AddDomainEvent(new ImageUnlikedEvent(Id, command.Actor.Id));
    }

    public void UpdateTags(UpdateImageTagsCommand command)
    {
        CheckAvailability();

        AddDomainEvent(new ImageTagsUpdatedEvent(Id, command.Tags));
    }

    public void Remove(RemoveImageCommand command)
    {
        if (_status.IsRemoved || _status.IsAlbumRemoved)
            return;

        _status = ImageStatus.Removed(DateTime.UtcNow);
        AddDomainEvent(new ImageRemovedEvent(Id, _status));
    }

    public void Restore(RestoreImageCommand command)
    {
        if (_status.IsAvailable || _status.IsAlbumRemoved)
            return;

        _status = ImageStatus.Available;
        AddDomainEvent(new ImageRestoredEvent(Id, _status));
    }

    public void AlbumRemoved(RemoveAlbumCommand _)
    {
        if (_status.IsRemoved || _status.IsAlbumRemoved)
            return;

        _status = ImageStatus.AlbumRemoved;
    }

    public void AlbumRestored(RestoreAlbumCommand _)
    {
        if (_status.IsRemoved || _status.IsAvailable)
            return;

        _status = ImageStatus.Available;
    }

    internal bool IsRemoved => _status.IsRemoved;
    internal bool IsAvailable => _status.IsAvailable;

    internal bool IsUploader(Actor actor) => _uploader == actor.Id;

    internal bool IsNotUploader(Actor actor) => !IsUploader(actor);

    private void CheckAvailability()
    {
        if (_status.IsRemoved || _status.IsAlbumRemoved)
            throw new ImageRemovedException();
    }
}

internal static class ImageListExtensions
{
    public static Image FindById(this List<Image> images, ImageId id)
    {
        Image? image = images.FirstOrDefault(image => image.Id == id);
        if (image is null)
            ImageNotFoundException.Throw(id);

        return image;
    }

    public static bool Contains(this List<Image> images, ImageId id)
    {
        return images.Any(image => image.Id == id);
    }

    public static bool NotContains(this List<Image> images, ImageId id)
    {
        return !images.Contains(id);
    }

    public static Image? LatestImage(this List<Image> images)
    {
        return images
            .Where(image => image.IsAvailable)
            .OrderByDescending(image => image.Id.Value)
            .FirstOrDefault();
    }

    public static void DeleteImage(this List<Image> images, ImageId id)
    {
        Image? image = images.FirstOrDefault(image => image.Id == id);

        if (image is null)
            return;

        image.AddDomainEvent(new ImageDeletedEvent(image.Id));
        images.Remove(image);
    }

    public static int DeleteAllRemovedImages(this List<Image> images)
    {
        var imagesToBeRemoved = images.Where(image => image.IsRemoved).ToList();

        foreach (var image in imagesToBeRemoved)
        {
            DeleteImage(images, image.Id);
        }

        return imagesToBeRemoved.Count;
    }
}
