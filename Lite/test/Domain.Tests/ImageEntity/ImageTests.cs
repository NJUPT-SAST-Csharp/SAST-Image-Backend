using System.Runtime.CompilerServices;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Commands;
using Domain.AlbumAggregate.Events;
using Domain.AlbumAggregate.ImageEntity;
using Domain.Shared;
using Domain.Tests.AlbumEntity;
using Shouldly;

namespace Domain.Tests.ImageEntity;

[TestClass]
public class ImageTests
{
    [TestMethod]
    public void Raise_Event_When_Image_Removed()
    {
        var image = Image.New(ImageId.New);
        var album = Album.New;
        RemoveImageCommand command = new(album.Id, image.Id, Actor.New(album.Author));

        image.Remove(command);

        image.DomainEvents.Count.ShouldBe(1);
        image.DomainEvents.First().ShouldBeOfType<ImageRemovedEvent>();
    }

    [TestMethod]
    public void Raise_Event_When_Image_Restored()
    {
        var image = Image.New(ImageId.New);
        var album = Album.New;
        image.Status = ImageStatus.NewRemoved;
        RestoreImageCommand command = new(album.Id, image.Id, Actor.New(album.Author));

        image.Restore(command);

        image.DomainEvents.Count.ShouldBe(1);
        image.DomainEvents.First().ShouldBeOfType<ImageRestoredEvent>();
    }
}

internal static class TestImage
{
    [UnsafeAccessor(UnsafeAccessorKind.Constructor)]
    private static extern Image Constructor();

    extension(Image image)
    {
        private T GetValue<T>() => image.GetValue<Image, T>();

        public ImageStatus Status
        {
            get => image.GetValue<ImageStatus>();
            set => image.SetValue(value);
        }

        public static Image New(ImageId id)
        {
            var i = Constructor();

            i.SetId(id);
            i.SetValue(new List<Like>());

            return i;
        }

        public static Image Removed(ImageId id)
        {
            var i = Image.New(id);
            i.Status = ImageStatus.NewRemoved;
            return i;
        }
    }
}
