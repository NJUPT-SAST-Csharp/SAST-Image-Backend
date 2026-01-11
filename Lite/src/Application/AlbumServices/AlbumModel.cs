using Application.ImageServices;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Events;

namespace Application.AlbumServices;

public sealed class AlbumModel
{
    private AlbumModel() { }

    internal AlbumModel(AlbumCreatedEvent e)
    {
        Id = e.AlbumId.Value;
        Title = e.Title.Value;
        Description = e.Description.Value;
        AuthorId = e.AuthorId.Value;
        CategoryId = e.CategoryId.Value;
        AccessLevel = e.AccessLevel.Value;
        Status = AlbumStatusValue.Available;
    }

    public long Id { get; }
    public string Title { get; private set; } = null!;
    public string Description { get; private set; } = null!;
    public long AuthorId { get; private set; }
    public long CategoryId { get; private set; }
    public AccessLevelValue AccessLevel { get; private set; }
    public string[] Tags { get; private set; } = [];
    public long[] Collaborators { get; private set; } = [];
    public DateTime CreatedAt { get; } = DateTime.UtcNow;
    public DateTime UpdatedAt { get; private set; } = DateTime.UtcNow;
    public AlbumStatusValue Status { get; private set; }
    public DateTime? RemovedAt { get; private set; }
    public List<SubscribeModel> Subscribes { get; } = null!;
    public List<ImageModel> Images { get; } = null!;

    internal void UpdateAccessLevel(AlbumAccessLevelUpdatedEvent e)
    {
        AccessLevel = e.AccessLevel.Value;
        foreach (var image in Images)
        {
            image.UpdateAccessLevel(e);
        }
    }

    internal void UpdateTitle(AlbumTitleUpdatedEvent e)
    {
        Title = e.Title.Value;
    }

    internal void UpdateDescription(AlbumDescriptionUpdatedEvent e)
    {
        Description = e.Description.Value;
    }

    internal void UpdateCategory(AlbumCategoryUpdatedEvent e)
    {
        CategoryId = e.Category.Value;
    }

    internal void UpdateCollaborators(AlbumCollaboratorsUpdatedEvent e)
    {
        Collaborators = Array.ConvertAll(e.Collaborators.Value, id => id.Value);
    }

    internal void UpdateTags(AlbumTagsUpdatedEvent e)
    {
        Tags = e.Tags;
    }

    internal void Remove(AlbumRemovedEvent e)
    {
        Status = e.Status.Value;
        RemovedAt = e.Status.RemovedAt;

        foreach (var image in Images)
        {
            image.AlbumRemoved(e);
        }
    }

    internal void Restore(AlbumRestoredEvent e)
    {
        Status = e.Status.Value;
        RemovedAt = null;

        foreach (var image in Images)
        {
            image.AlbumRestored(e);
        }
    }

    internal void ImageAdded(ImageAddedEvent e)
    {
        UpdatedAt = e.CreatedAt;
    }
}

public sealed record class SubscribeModel(long Album, long User);
