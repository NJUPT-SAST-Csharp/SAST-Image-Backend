using System.Reflection;
using Identity;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;

namespace SastImg.Test.Domain;

internal static class AlbumUnitTestHelper
{
    public const long AuthorId = 233;
    public const int CategoryId = 0;
    public static AlbumTitle Title = new() { Value = "FakeTitle" };
    public static AlbumDescription Description = new() { Value = "FakeDescription" };
    public const Accessibility Access = Accessibility.Public;

    public static UserId Author => new(AuthorId);
    public static UserId Category => new(CategoryId);

    public static Album CreateDefaultAlbum()
    {
        return Album.CreateNewAlbum(
            new(AuthorId),
            new() { Value = CategoryId },
            Title,
            Description,
            Access
        );
    }

    public static Album CreateAlbumWithAuthor(long authorId)
    {
        return Album.CreateNewAlbum(
            new(authorId),
            new() { Value = CategoryId },
            Title,
            Description,
            Access
        );
    }

    public static UserId[] GetCollaborators(this Album album)
    {
        object val = typeof(Album)
            .GetField("_collaborators", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(album)!;

        return (UserId[])val;
    }

    public static Image? GetFirstImageOrDefault(this Album album)
    {
        object val = typeof(Album)
            .GetField("_images", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(album)!;

        var images = (IList<Image>)val;

        return images.Count > 0 ? images[0] : null;
    }

    public static IReadOnlyList<Image> GetImages(this Album album)
    {
        object val = typeof(Album)
            .GetField("_images", BindingFlags.NonPublic | BindingFlags.Instance)!
            .GetValue(album)!;

        var images = (IList<Image>)val;

        return images.ToList();
    }
}
