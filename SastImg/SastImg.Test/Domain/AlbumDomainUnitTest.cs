using SastImg.Domain.AlbumAggregate;

namespace SastImg.Test.Domain;

[TestClass]
public sealed class AlbumDomainUnitTest
{
    [TestMethod]
    public void CreateNewAlbumShouldRaiseEvent()
    {
        const long authorId = 1;
        const string title = "fakeTitle";
        const string description = "fakeDescription";
        const Accessibility accessibility = Accessibility.Public;
        const int expected = 1;

        Album album = Album.CreateNewAlbum(authorId, title, description, accessibility);
        Assert.AreEqual(expected, album.DomainEvents.Count);
    }
}
