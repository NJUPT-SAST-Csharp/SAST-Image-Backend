using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Events;
using Domain.AlbumAggregate.ImageEntity;

namespace Application.ImageServices;

public sealed class ImageModel
{
    private ImageModel() { }

    public long Id { get; }
    public string Title { get; } = null!;
    public long AlbumId { get; }
    public long AuthorId { get; }
    public long UploaderId { get; }
    public string[] Tags { get; private set; } = [];
    public DateTime UploadedAt { get; } = DateTime.UtcNow;
    public AccessLevelValue AccessLevel { get; private set; }
    public ImageStatusValue Status { get; private set; }
    public DateTime? RemovedAt { get; private set; }
    public List<LikeModel> Likes { get; } = null!;
    public long[] Collaborators { get; } = null!;

    internal ImageModel(ImageAddedEvent e)
    {
        Id = e.ImageId.Value;
        AlbumId = e.Album.Value;
        AuthorId = e.AuthorId.Value;
        Title = e.Title.Value;
        Status = ImageStatusValue.Available;
        Tags = e.Tags.Value;
        Collaborators = Array.ConvertAll(e.Collaborators.Value, id => id.Value);
        AccessLevel = e.AccessLevel.Value;
        UploadedAt = e.CreatedAt;
        UploaderId = e.Uploader.Value;
    }

    internal void Remove(ImageRemovedEvent e)
    {
        Status = e.Status.Value;
        RemovedAt = e.Status.RemovedAt;
    }

    internal void Restore(ImageRestoredEvent e)
    {
        Status = e.Status.Value;
        RemovedAt = null;
    }

    internal void AlbumRestored(AlbumRestoredEvent _)
    {
        if (Status == ImageStatusValue.AlbumRemoved)
        {
            Status = ImageStatusValue.Available;
        }
    }

    internal void AlbumRemoved(AlbumRemovedEvent _)
    {
        if (Status == ImageStatusValue.Available)
        {
            Status = ImageStatusValue.AlbumRemoved;
        }
    }

    internal void UpdateAccessLevel(AlbumAccessLevelUpdatedEvent e)
    {
        AccessLevel = e.AccessLevel.Value;
    }

    internal void UpdateTags(ImageTagsUpdatedEvent e)
    {
        Tags = e.Tags;
    }
}

public sealed record class LikeModel(long Image, long User);
