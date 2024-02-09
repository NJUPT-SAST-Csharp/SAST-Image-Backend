using SastImg.Domain.AlbumAggregate.AlbumEntity;

namespace SastImg.Test.Domain;

[TestClass]
public sealed class AlbumDomainUnitTest
{
    [TestMethod]
    public void Create_NewAlbum_RaiseEvent()
    {
        const long authorId = 1;
        const long categoryId = 0;
        const string title = "FakeTitle";
        const string description = "FakeDescription";
        const Accessibility accessibility = Accessibility.Public;
        const int expected = 1;

        Album album = Album.CreateNewAlbum(
            new(authorId),
            new(categoryId),
            title,
            description,
            accessibility
        );
        Assert.AreEqual(expected, album.DomainEvents.Count);
    }
}
