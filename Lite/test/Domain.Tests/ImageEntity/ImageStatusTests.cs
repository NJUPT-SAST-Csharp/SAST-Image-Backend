using Domain.AlbumAggregate.ImageEntity;

namespace Domain.Tests.ImageEntity;

internal class ImageStatusTests { }

internal static class TestImageStatus
{
    extension(ImageStatus status)
    {
        private T GetValue<T>() => status.GetValue<ImageStatus, T>();

        public static ImageStatus NewRemoved => ImageStatus.Removed(DateTime.UtcNow);
    }
}
