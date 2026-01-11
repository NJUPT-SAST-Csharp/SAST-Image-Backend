using System.Diagnostics.CodeAnalysis;

namespace Domain.AlbumAggregate.ImageEntity;

public sealed record ImageStatus
{
    public ImageStatusValue Value { get; private init; }
    public DateTime? RemovedAt { get; private init; }

    [MemberNotNullWhen(true, nameof(RemovedAt))]
    public bool IsRemoved => Value == ImageStatusValue.Removed;

    [MemberNotNullWhen(false, nameof(RemovedAt))]
    public bool IsAvailable => Value == ImageStatusValue.Available;
    public bool IsAlbumRemoved => Value == ImageStatusValue.AlbumRemoved;

    public static ImageStatus Removed(DateTime removedAt) =>
        new() { Value = ImageStatusValue.Removed, RemovedAt = removedAt };

    public static ImageStatus Available => new() { Value = ImageStatusValue.Available };

    public static ImageStatus AlbumRemoved => new() { Value = ImageStatusValue.AlbumRemoved };
}

public enum ImageStatusValue
{
    Available,
    Removed,
    AlbumRemoved,
}
