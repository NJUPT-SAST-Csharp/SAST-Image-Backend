using System.Diagnostics.CodeAnalysis;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Extensions;

namespace Domain.AlbumAggregate.Exceptions;

public sealed class ImageTagsNotFoundException(ImageTags tags) : DomainException
{
    public ImageTags Tags { get; } = tags;

    [DoesNotReturn]
    public static void Throw(ImageTags tags) => throw new ImageTagsNotFoundException(tags);
}
