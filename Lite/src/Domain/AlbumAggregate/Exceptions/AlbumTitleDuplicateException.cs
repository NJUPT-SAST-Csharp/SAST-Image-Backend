using Domain.AlbumAggregate.AlbumEntity;
using Domain.Extensions;

namespace Domain.AlbumAggregate.Exceptions;

public sealed class AlbumTitleDuplicateException(AlbumTitle title) : DomainException
{
    public AlbumTitle Title { get; } = title;
}
