using System.Diagnostics.CodeAnalysis;

namespace Domain.AlbumAggregate.AlbumEntity;

public sealed record AlbumStatus
{
    public AlbumStatusValue Value { get; private init; }
    public DateTime? RemovedAt { get; private init; }

    [MemberNotNullWhen(true, nameof(RemovedAt))]
    public bool IsRemoved => Value == AlbumStatusValue.Removed;

    [MemberNotNullWhen(false, nameof(RemovedAt))]
    public bool IsAvailable => Value == AlbumStatusValue.Available;

    public static AlbumStatus Available => new() { Value = AlbumStatusValue.Available };

    public static AlbumStatus Removed(DateTime removedAt) =>
        new() { Value = AlbumStatusValue.Removed, RemovedAt = removedAt };
}

public enum AlbumStatusValue
{
    Available,
    Removed,
}
