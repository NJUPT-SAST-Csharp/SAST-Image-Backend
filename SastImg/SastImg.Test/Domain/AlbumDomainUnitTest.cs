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

    [TestMethod]
    [DataRow(1, 1, true)]
    [DataRow(1, 2, false)]
    public void Album_OwnedBy_Author(long testAuthorId, long testUserId, bool expected)
    {
        UserId userId = new(testUserId);
        var album = AlbumUnitTestHelper.CreateAlbumWithAuthor(testAuthorId);

        bool result = album.IsOwnedBy(userId);

        Assert.AreEqual(expected, result);
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

    [TestMethod]
    [DataRow(new long[] { 4, 5, 6 }, -123435, false)]
    [DataRow(new long[] { 4, 5, 6 }, 4, true)]
    [DataRow(new long[] { authorId, 5, 6 }, authorId, true)]
    [DataRow(new long[] { }, authorId, true)]
    public void Album_ManagedBy_CollaboratorsAndAuthor(
        long[] testCollaborators,
        long testUserId,
        bool expected
    )
    {
        UserId user = new(testUserId);
        var collaborators = Array.ConvertAll(testCollaborators, id => new UserId(id));
        var album = AlbumUnitTestHelper.CreateDefaultAlbum();
        album.UpdateCollaborators(collaborators);

        bool result = album.IsManagedBy(user);

        Assert.AreEqual(expected, result);
    }
}
