using SastImg.Domain.AlbumAggregate;

namespace SastImg.Test.Domain;

[TestClass]
public sealed class AlbumDomainUnitTest
{
    [TestMethod]
    public void CreateNewAlbumShouldRaiseEvent()
    {
        const long authorId = 1;
        const long categoryId = 0;
        const string title = "FakeTitle";
        const string description = "FakeDescription";
        const Accessibility accessibility = Accessibility.Public;
        const int expected = 1;

        Album album = Album.CreateNewAlbum(authorId, categoryId, title, description, accessibility);
        Assert.AreEqual(expected, album.DomainEvents.Count);
    }
}
