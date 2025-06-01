using Identity;

namespace SastImg.Test.Domain;

[TestClass]
public sealed class AlbumDomainUnitTest
{
    [TestMethod]
    public void Create_NewAlbum_RaiseEvent()
    {
        const int expected = 1;

        var album = AlbumUnitTestHelper.CreateDefaultAlbum();

        Assert.AreEqual(expected, album.DomainEvents.Count);
    }

    private const long authorId = AlbumUnitTestHelper.AuthorId;

    [TestMethod]
    [DataRow(new long[] { })]
    [DataRow(new long[] { 1, 2, 3 })]
    [DataRow(new long[] { authorId, 2, 3 })]
    [DataRow(new long[] { authorId, authorId, authorId })]
    public void Update_AlbumCollaborators_NotIncludeAuthor(long[] testCollaborators)
    {
        var collaborators = Array.ConvertAll(testCollaborators, id => new UserId(id));
        var album = AlbumUnitTestHelper.CreateDefaultAlbum();

        album.UpdateCollaborators(collaborators);
        collaborators = album.GetCollaborators();
        bool containsAuthor = Array.Exists(collaborators, id => id.Value == authorId);

        Assert.IsFalse(containsAuthor);
    }
}
