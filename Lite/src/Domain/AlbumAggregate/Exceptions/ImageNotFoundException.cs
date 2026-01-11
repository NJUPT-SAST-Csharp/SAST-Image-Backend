using System.Diagnostics.CodeAnalysis;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Extensions;

namespace Domain.AlbumAggregate.Exceptions;

public sealed class ImageNotFoundException(ImageId imageId) : DomainException
{
    public ImageId ImageId { get; } = imageId;

    [DoesNotReturn]
    public static void Throw(ImageId imageId) => throw new ImageNotFoundException(imageId);
}
