using System.Reflection;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Commands;
using Domain.AlbumAggregate.Events;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Entity;
using Domain.Shared;
using Shouldly;
using static Domain.Tests.ImageEntity.ImageTestsHelper;

namespace Domain.Tests.ImageEntity;

[TestClass]
public class ImageTests
{
    [TestMethod]
    public void Raise_Event_When_Image_Removed()
    {
        var image = ValidNewImage;
        RemoveImageCommand command = new(OuterAlbumId, Id, OuterAuthor);

        image.Remove(command);

        image.DomainEvents.Count.ShouldBe(1);
        image.DomainEvents.First().ShouldBeOfType<ImageRemovedEvent>();
    }

    [TestMethod]
    public void Raise_Event_When_Image_Restored()
    {
        var image = ValidNewImage;
        image.SetValue(RemovedStatus);
        RestoreImageCommand command = new(OuterAlbumId, Id, OuterAuthor);

        image.Restore(command);

        image.DomainEvents.Count.ShouldBe(1);
        image.DomainEvents.First().ShouldBeOfType<ImageRestoredEvent>();
    }
}

internal static class ImageTestsHelper
{
    public static readonly Actor OuterAuthor = new();
    public static readonly AlbumId OuterAlbumId = new(2333);
    public static readonly ImageId Id = new(1);
    public static readonly ImageTitle NewImageTitle = new("new_title");
    public static readonly ImageTags NewImageTags = new([new("741"), new("ywwuyi")]);

    public static ImageStatus RemovedStatus => ImageStatus.Removed(DateTime.UtcNow);

    public static Image ValidNewImage => CreateNewImage(Id.Value);

    public static Image CreateNewImage(long id)
    {
        var image = CreateNewImageFromReflection();

        typeof(EntityBase<ImageId>)
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(f => f.Name.Contains("id", StringComparison.OrdinalIgnoreCase))
            .SetValue(image, new ImageId(id));

        return image;
    }

    public static void SetValue<T>(this Image image, T value)
    {
        typeof(Image)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .First(f => f.FieldType == typeof(T))
            .SetValue(image, value);
    }

    public static T GetValue<T>(this Image image)
    {
        object? value = typeof(Image)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .First(f => f.FieldType == typeof(T))
            .GetValue(image);

        Assert.IsNotNull(value);

        return (T)value;
    }

    private static Image CreateNewImageFromReflection()
    {
        var constructor = typeof(Image).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            Type.EmptyTypes
        );
        Assert.IsNotNull(constructor);

        var image = (Image)constructor.Invoke(null);

        image.SetValue(new List<Like>());

        return image;
    }
}
