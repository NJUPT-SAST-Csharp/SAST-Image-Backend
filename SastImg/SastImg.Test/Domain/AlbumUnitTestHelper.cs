using System.Reflection;
using SastImg.Domain;
using SastImg.Domain.AlbumAggregate.AlbumEntity;
using SastImg.Domain.AlbumAggregate.ImageEntity;

namespace SastImg.Test.Domain
{
    internal static class AlbumUnitTestHelper
    {
        public const long AuthorId = 233;
        public const long CategoryId = 0;
        public const string Title = "FakeTitle";
        public const string Description = "FakeDescription";
        public const Accessibility Access = Accessibility.Public;

        public static UserId Author => new(AuthorId);
        public static UserId Category => new(CategoryId);

        public static Album CreateDefaultAlbum()
        {
            return Album.CreateNewAlbum(new(AuthorId), new(CategoryId), Title, Description, Access);
        }

        public static Album CreateAlbumWithAuthor(long authorId)
        {
            return Album.CreateNewAlbum(new(authorId), new(CategoryId), Title, Description, Access);
        }

        public static UserId[] GetCollaborators(this Album album)
        {
            var val = typeof(Album)
                .GetField("_collaborators", BindingFlags.NonPublic | BindingFlags.Instance)!
                .GetValue(album)!;

            return (UserId[])val;
        }

        public static Image? GetFirstImageOrDefault(this Album album)
        {
            var val = typeof(Album)
                .GetField("_images", BindingFlags.NonPublic | BindingFlags.Instance)!
                .GetValue(album)!;

            var images = (IList<Image>)val;

            return images.Count > 0 ? images[0] : null;
        }

        public static IReadOnlyList<Image> GetImages(this Album album)
        {
            var val = typeof(Album)
                .GetField("_images", BindingFlags.NonPublic | BindingFlags.Instance)!
                .GetValue(album)!;

            var images = (IList<Image>)val;

            return images.ToList();
        }
    }
}
