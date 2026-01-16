using System.Reflection;
using Domain.AlbumAggregate;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Commands;
using Domain.AlbumAggregate.Events;
using Domain.AlbumAggregate.Exceptions;
using Domain.AlbumAggregate.ImageEntity;
using Domain.AlbumAggregate.Services;
using Domain.CategoryAggregate.CategoryEntity;
using Domain.Entity;
using Domain.Shared;
using Domain.Tests;
using Domain.Tests.ImageEntity;
using Domain.UserAggregate.UserEntity;
using Moq;
using Shouldly;
using static Domain.Tests.AlbumEntity.ActorTestHelper;
using static Domain.Tests.AlbumEntity.CollaboratorsTestHelper;
using static Domain.Tests.AlbumEntity.ImageTestHelper;

namespace Domain.Tests.AlbumEntity;

[TestClass]
public class AlbumTests(TestContext context)
{
    #region Create
    [TestMethod]
    public async Task Add_New_Album_When_Created()
    {
        List<Album> db = [];
        var command = new CreateAlbumCommand(
            AlbumTitle.New,
            AlbumDescription.New,
            AccessLevel.Default,
            CategoryId.New,
            Actor.New(AuthorId)
        );
        var categoryCheckerMock = new Mock<ICategoryExistenceChecker>();
        var albumTitleCheckerMock = new Mock<IAlbumTitleUniquenessChecker>();
        var repositoryMock = new Mock<IAlbumRepository>();
        var cancellationToken = context.CancellationToken;

        categoryCheckerMock
            .Setup(c => c.CheckAsync(command.CategoryId, cancellationToken))
            .Returns(Task.CompletedTask);
        albumTitleCheckerMock
            .Setup(t => t.CheckAsync(command.Title, cancellationToken))
            .Returns(Task.CompletedTask);
        repositoryMock
            .Setup(r => r.AddAsync(It.IsAny<Album>(), cancellationToken))
            .Callback<Album, CancellationToken>((album, _) => db.Add(album))
            .Returns(Task.CompletedTask);

        var id = await Album.CreateAsync(
            command,
            categoryCheckerMock.Object,
            albumTitleCheckerMock.Object,
            repositoryMock.Object,
            cancellationToken
        );

        db.Count.ShouldBe(1);
        var album = db[0];
        album.Id.ShouldBe(id);
        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCreatedEvent>();
        categoryCheckerMock.Verify(
            c => c.CheckAsync(command.CategoryId, cancellationToken),
            Times.Once
        );
        albumTitleCheckerMock.Verify(
            t => t.CheckAsync(command.Title, cancellationToken),
            Times.Once
        );
        repositoryMock.Verify(r => r.AddAsync(It.IsAny<Album>(), cancellationToken), Times.Once);
    }

    #endregion

    #region IsRemoved

    [TestMethod]
    public void Return_True_When_Album_Removed()
    {
        var album = Album.Removed;

        bool isRemoved = album.GetValue<AlbumStatus>().IsRemoved;

        isRemoved.ShouldBeTrue();
    }

    [TestMethod]
    public void Return_False_When_Album_Not_Removed()
    {
        var album = Album.New;

        bool isRemoved = album.GetValue<AlbumStatus>().IsRemoved;

        isRemoved.ShouldBeFalse();
    }

    #endregion

    #region UpdateDescription

    [TestMethod]
    public void Throw_When_UpdateDescription_In_Immutable_Album()
    {
        var album = Album.Removed;
        UpdateAlbumDescriptionCommand command = new(
            AlbumId.New,
            AlbumDescription.New,
            Actor.Author
        );

        Should.Throw<AlbumRemovedException>(() => album.UpdateDescription(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateDescription_As_Not_Author_Or_Admin(long actorId)
    {
        var album = Album.New;
        UpdateAlbumDescriptionCommand command = new(
            AlbumId.New,
            AlbumDescription.New,
            Actor.New(actorId)
        );

        Should.Throw<NoPermissionException>(() => album.UpdateDescription(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Description_Updated(long actorId, bool isAdmin)
    {
        var album = Album.New;
        UpdateAlbumDescriptionCommand command = new(
            AlbumId.New,
            AlbumDescription.New,
            Actor.New(actorId, isAdmin)
        );

        album.UpdateDescription(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumDescriptionUpdatedEvent>();
    }

    #endregion

    #region UpdateTitle

    [TestMethod]
    public void Throw_When_UpdateTitle_In_Immutable_Album()
    {
        var album = Album.Removed;
        UpdateAlbumTitleCommand command = new(AlbumId.New, AlbumTitle.New, Actor.Author);

        Should.Throw<AlbumRemovedException>(() => album.UpdateTitle(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateTitle_As_Not_Author_Or_Admin(long actorId)
    {
        var album = Album.New;
        UpdateAlbumTitleCommand command = new(AlbumId.New, AlbumTitle.New, Actor.New(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateTitle(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Title_Updated(long actorId, bool isAdmin)
    {
        var album = Album.New;
        UpdateAlbumTitleCommand command = new(
            AlbumId.New,
            AlbumTitle.New,
            Actor.New(actorId, isAdmin)
        );

        album.UpdateTitle(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumTitleUpdatedEvent>();
    }

    #endregion

    #region UpdateCategory

    [TestMethod]
    public void Throw_When_UpdateCategory_In_Immutable_Album()
    {
        var album = Album.Removed;
        UpdateAlbumCategoryCommand command = new(AlbumId.New, CategoryId.New, Actor.Author);

        Should.Throw<AlbumRemovedException>(() => album.UpdateCategory(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateCategory_As_Not_Author_Or_Admin(long actorId)
    {
        var album = Album.New;
        UpdateAlbumCategoryCommand command = new(AlbumId.New, CategoryId.New, Actor.New(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateCategory(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Category_Updated(long actorId, bool isAdmin)
    {
        var album = Album.New;
        UpdateAlbumCategoryCommand command = new(
            AlbumId.New,
            CategoryId.New,
            Actor.New(actorId, isAdmin)
        );

        album.UpdateCategory(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCategoryUpdatedEvent>();
    }

    #endregion

    #region UpdateAccessLevel

    [TestMethod]
    public void Throw_When_UpdateAccessLevel_In_Removed_Album()
    {
        var album = Album.Removed;
        UpdateAccessLevelCommand command = new(AlbumId.New, AccessLevel.Default, Actor.Author);

        Should.Throw<AlbumRemovedException>(() => album.UpdateAccessLevel(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateAccessLevel_As_Not_Author_Or_Admin(long actorId)
    {
        var album = Album.New;
        UpdateAccessLevelCommand command = new(
            AlbumId.New,
            AccessLevel.Default,
            Actor.New(actorId)
        );

        Should.Throw<NoPermissionException>(() => album.UpdateAccessLevel(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_AccessLevel_Updated(long actorId, bool isAdmin)
    {
        var album = Album.New;
        UpdateAccessLevelCommand command = new(
            AlbumId.New,
            AccessLevel.Default,
            Actor.New(actorId, isAdmin)
        );

        album.UpdateAccessLevel(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumAccessLevelUpdatedEvent>();
    }

    #endregion

    #region UpdateCollaborators

    [TestMethod]
    public void Throw_When_UpdateCollaborators_In_Immutable_Album()
    {
        var album = Album.Removed;
        UpdateCollaboratorsCommand command = new(
            AlbumId.New,
            Collaborators.DefaultNew,
            Actor.Author
        );

        Should.Throw<AlbumRemovedException>(() => album.UpdateCollaborators(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateCollaborators_As_Not_Author_Or_Admin(long actorId)
    {
        var album = Album.New;
        var collaborators = Collaborators.DefaultNew;
        UpdateCollaboratorsCommand command = new(AlbumId.New, collaborators, Actor.New(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateCollaborators(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Collaborators_Updated(long actorId, bool isAdmin)
    {
        var album = Album.New;
        var collaborators = Collaborators.DefaultNew;
        UpdateCollaboratorsCommand command = new(
            AlbumId.New,
            collaborators,
            Actor.New(actorId, isAdmin)
        );

        album.UpdateCollaborators(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCollaboratorsUpdatedEvent>();
    }

    #endregion

    #region UpdateCover

    [TestMethod]
    public void Throw_When_UpdateCover_In_Immutable_Album()
    {
        var album = Album.Removed;
        UpdateCoverCommand command = new(AlbumId.New, null, Actor.Author);

        Should.Throw<AlbumRemovedException>(() => album.UpdateCover(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateCover_As_Not_Author_Or_Admin(long actorId)
    {
        var album = Album.New;
        UpdateCoverCommand command = new(AlbumId.New, null, Actor.New(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateCover(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Cover_Updated(long actorId, bool isAdmin)
    {
        var album = Album.New;
        UpdateCoverCommand command = new(
            AlbumId.New,
            IImageFile.Default,
            Actor.New(actorId, isAdmin)
        );

        album.UpdateCover(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCoverUpdatedEvent>();
    }

    #endregion

    #region AddImage

    [TestMethod]
    public void Throw_When_AddImage_In_Immutable_Album()
    {
        var album = Album.Removed;
        AddImageCommand command = new(
            AlbumId.New,
            ImageTitle.New,
            ImageTags.New,
            IImageFile.Default,
            Actor.Author
        );

        Should.Throw<AlbumRemovedException>(() => album.AddImage(command));
    }

    [TestMethod]
    public void Throw_When_AddImage_As_Not_Author_Or_Admin_Or_Collaborator()
    {
        var album = Album.New;
        AddImageCommand command = new(
            AlbumId.New,
            ImageTitle.New,
            ImageTags.New,
            IImageFile.Default,
            Actor.New(VisitorId)
        );

        Should.Throw<NoPermissionException>(() => album.AddImage(command));
    }

    [DataRow(AuthorId, false)]
    [DataRow(AdminId, true)]
    [DataRow(Collaborator1Id, false)]
    [DataRow(Collaborator2Id, true)]
    [TestMethod]
    public void Raise_Event_When_Image_Added(long actorId, bool isAdmin)
    {
        var album = Album.New;
        AddImageCommand command = new(
            AlbumId.New,
            ImageTitle.New,
            ImageTags.New,
            IImageFile.Default,
            Actor.New(actorId, isAdmin)
        );

        album.AddImage(command);

        album.DomainEvents.Count.ShouldBeGreaterThan(0);
        album.GetValue<List<Image>>().Count.ShouldBe(3);
    }

    [TestMethod]
    public void Raise_AlbumCoverUpdatedEvent_When_IsLatestImage_As_Image_Added()
    {
        var album = Album.New;
        AddImageCommand command = new(
            AlbumId.New,
            ImageTitle.New,
            ImageTags.New,
            IImageFile.Default,
            Actor.Author
        );

        album.AddImage(command);

        album.DomainEvents.Count.ShouldBe(2);
        album.DomainEvents.ShouldBeOfTypes(typeof(ImageAddedEvent), typeof(AlbumCoverUpdatedEvent));
    }

    [TestMethod]
    public void Not_Raise_AlbumCoverUpdatedEvent_When_Not_IsLatestImage_As_Image_Added()
    {
        var fixedCover = Cover.UserCustomCover;
        var album = Album.New;
        album.SetValue(fixedCover);
        AddImageCommand command = new(
            AlbumId.New,
            ImageTitle.New,
            ImageTags.New,
            IImageFile.Default,
            Actor.Author
        );

        album.AddImage(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<ImageAddedEvent>();
    }

    #endregion

    #region RemoveImage


    [TestMethod]
    public void Throw_When_RemoveImage_In_Immutable_Album()
    {
        var album = Album.Removed;
        RemoveImageCommand command = new(AlbumId.New, new(Image1Id), Actor.Author);

        Should.Throw<AlbumRemovedException>(() => album.RemoveImage(command));
    }

    [TestMethod]
    public void Throw_When_RemoveImage_As_Not_Author_Or_Admin_Or_Collaborator()
    {
        var album = Album.New;
        RemoveImageCommand command = new(AlbumId.New, new(Image1Id), Actor.New(VisitorId));

        Should.Throw<NoPermissionException>(() => album.RemoveImage(command));
    }

    [TestMethod]
    public void Raise_CoverUpdatedEvent_When_Image_As_LatestImage_Removed()
    {
        var album = Album.New;
        album.SetValue(new Cover(new(Image2Id), true));
        RemoveImageCommand command = new(AlbumId.New, new(Image2Id), Actor.Author);

        album.RemoveImage(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCoverUpdatedEvent>();
        album.GetValue<Cover>().Id.ShouldBe(new(Image1Id));
    }

    [TestMethod]
    public void Not_Raise_CoverUpdatedEvent_When_Image_Not_As_LatestImage_Removed()
    {
        var album = Album.New;
        RemoveImageCommand command = new(AlbumId.New, new(Image2Id), Actor.Author);

        album.RemoveImage(command);

        album.DomainEvents.Count.ShouldBe(0);
    }

    #endregion

    #region RestoreImage

    [TestMethod]
    public void Throw_When_RestoreImage_In_Immutable_Album()
    {
        var album = Album.Removed;
        RestoreImageCommand command = new(AlbumId.New, new(Image1Id), Actor.Author);

        Should.Throw<AlbumRemovedException>(() => album.RestoreImage(command));
    }

    [TestMethod]
    public void Throw_When_RestoreImage_As_Not_Author_Or_Admin_Or_Collaborator()
    {
        var album = Album.New;
        RestoreImageCommand command = new(AlbumId.New, new(Image1Id), Actor.New(VisitorId));

        Should.Throw<NoPermissionException>(() => album.RestoreImage(command));
    }

    [TestMethod]
    public void Raise_CoverUpdatedEvent_When_Image_As_LatestImage_Restored()
    {
        var album = Album.New;
        album.GetImage(Image2Id).SetValue(ImageTestsHelper.RemovedStatus);
        RestoreImageCommand command = new(AlbumId.New, new(Image2Id), Actor.Author);

        album.RestoreImage(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCoverUpdatedEvent>();
        album.GetValue<Cover>().Id.ShouldBe(new(Image2Id));
    }

    [TestMethod]
    public void Not_Raise_CoverUpdatedEvent_When_Image_Not_As_LatestImage_Restored()
    {
        var album = Album.New;
        album.SetValue(new Cover(null, false));
        RestoreImageCommand command = new(AlbumId.New, new(Image2Id), Actor.Author);

        album.RestoreImage(command);

        album.DomainEvents.Count.ShouldBe(0);
    }

    #endregion

    #region Remove

    [TestMethod]
    public void Not_Change_When_Album_Already_Removed()
    {
        var album = Album.Removed;
        RemoveAlbumCommand command = new(AlbumId.New, Actor.Author);
        var time = album.GetValue<AlbumStatus>().RemovedAt!.Value;

        album.Remove(command);

        album.DomainEvents.Count.ShouldBe(0);
        album.GetValue<AlbumStatus>().RemovedAt.ShouldBe(time);
    }

    [TestMethod]
    public void Raise_Event_When_Removed()
    {
        var album = Album.New;
        RemoveAlbumCommand command = new(AlbumId.New, Actor.Author);

        album.Remove(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumRemovedEvent>();
    }

    [TestMethod]
    public void Update_Contained_Images_Status_When_Removed()
    {
        var album = Album.New;
        RemoveAlbumCommand command = new(AlbumId.New, Actor.Author);

        album.Remove(command);

        foreach (var image in album.GetValue<List<Image>>())
        {
            image.GetValue<ImageStatus>().Value.ShouldBe(ImageStatusValue.AlbumRemoved);
        }
    }

    #endregion

    #region Restore

    [TestMethod]
    public void Not_Change_When_Album_Available()
    {
        var album = Album.New;
        RestoreAlbumCommand command = new(AlbumId.New, Actor.Author);

        album.Restore(command);

        album.DomainEvents.Count.ShouldBe(0);
        album.GetValue<AlbumStatus>().RemovedAt.ShouldBeNull();
    }

    [TestMethod]
    public void Raise_Event_When_Restored()
    {
        var album = Album.Removed;
        RestoreAlbumCommand command = new(AlbumId.New, Actor.Author);

        album.Restore(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumRestoredEvent>();
    }

    [TestMethod]
    public void Update_Contained_Images_Status_When_Restored()
    {
        var album = Album.Removed;
        RestoreAlbumCommand command = new(AlbumId.New, Actor.Author);

        album.Restore(command);

        foreach (var image in album.GetValue<List<Image>>())
        {
            image.GetValue<ImageStatus>().Value.ShouldBe(ImageStatusValue.Available);
        }
    }

    #endregion
}

file static class AlbumTestHelper
{
    extension(Album album)
    {
        public static Album New
        {
            get
            {
                var constructor = typeof(Album).GetConstructor(
                    BindingFlags.Instance | BindingFlags.NonPublic,
                    Type.EmptyTypes
                );

                Assert.IsNotNull(constructor);

                var a = (Album)constructor.Invoke([]);

                a.SetValue(Actor.Author.Id);
                a.SetValue(AlbumTitle.New);
                a.SetValue(Cover.Default);
                a.SetValue(Subscribe.Default(a.Id));
                a.SetValue(Image.Default);
                a.SetValue(Collaborators.Default.Value);

                typeof(EntityBase<AlbumId>)
                    .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
                    .First(f => f.Name.Contains("id", StringComparison.OrdinalIgnoreCase))
                    .SetValue(a, AlbumId.New);

                return a;
            }
        }

        public static Album Removed
        {
            get
            {
                var status = AlbumStatus.Removed(DateTime.UtcNow);
                var a = Album.New;
                a.SetValue(status);

                return a;
            }
        }

        public T GetValue<T>() => album.GetValue<Album, T>();

        public Image GetImage(long id)
        {
            Image? image = album
                .GetValue<List<Image>>()
                .FirstOrDefault(image => image.Id.Value == id);

            Assert.IsNotNull(image);

            return image;
        }
    }
}

file static class SubscribeTestHelper
{
    extension(Subscribe)
    {
        public static Subscribe New(AlbumId albumId, UserId userId) => new(albumId, userId);

        public static List<Subscribe> Default(AlbumId album) =>
            new([Subscribe.New(album, new(123)), Subscribe.New(album, new(321))]);
    }
}

file static class ImageTestHelper
{
    public const long Image1Id = 1111111;
    public const long Image2Id = 2222222;

    extension(Image)
    {
        public static Image New(long id) => ImageTestsHelper.CreateNewImage(id);

        public static List<Image> Default => [Image.New(Image1Id), Image.New(Image2Id)];
    }
}

file static class ActorTestHelper
{
    public const long AuthorId = 11111;
    public const long AdminId = 99999;
    public const long VisitorId = 55555;

    extension(Actor actor)
    {
        public static Actor New(long id, bool isAdmin = false) =>
            new()
            {
                Id = new(id),
                IsAdmin = isAdmin,
                IsAuthenticated = true,
            };

        public static Actor Author => Actor.New(AuthorId);
        public static Actor Visitor => Actor.New(VisitorId);
        public static Actor Admin => Actor.New(AdminId);
    }
}

file static class CollaboratorsTestHelper
{
    public const long Collaborator1Id = 1;
    public const long Collaborator2Id = 2;

    extension(Collaborators)
    {
        public static Collaborators New(params long[] userIds) =>
            new(Array.ConvertAll(userIds, i => new UserId(i)));

        public static Collaborators DefaultNew =>
            Collaborators.New([
                Collaborator1Id,
                Collaborator2Id,
                Random.Shared.NextInt64(
                    long.Max(Collaborator1Id, Collaborator2Id) + 1,
                    long.MaxValue
                ),
            ]);

        public static Collaborators Default => new([new(Collaborator1Id), new(Collaborator2Id)]);
    }
}

file static class AlbumIdTestHelper
{
    extension(AlbumId)
    {
        public static AlbumId New => AlbumId.GenerateNew();
    }
}

file static class AlbumTitleTestHelper
{
    extension(AlbumTitle)
    {
        public static AlbumTitle New =>
            new(Random.Shared.Chars(AlbumTitle.MinLength, AlbumTitle.MaxLength));
    }
}

file static class AlbumDescriptionTestHelper
{
    extension(AlbumDescription)
    {
        public static AlbumDescription New =>
            new(Random.Shared.Chars(AlbumDescription.MinLength, AlbumDescription.MaxLength));
    }
}

file static class CategoryIdTestHelper
{
    extension(CategoryId)
    {
        public static CategoryId New => new(Random.Shared.NextInt64(1, long.MaxValue));
    }
}

file static class ImageTitleTestHelper
{
    extension(ImageTitle)
    {
        public static ImageTitle New => new(Random.Shared.Chars(0, ImageTitle.MaxLength));
    }
}

file static class ImageTagsTestHelper
{
    extension(ImageTags)
    {
        public static ImageTags New =>
            new([
                .. Enumerable
                    .Repeat(
                        () => Random.Shared.Chars(default, ImageTags.MaxLength),
                        Random.Shared.Next(1, ImageTags.MaxCount)
                    )
                    .Select(f => f()),
            ]);
    }
}

file static class ImageFileTestHelper
{
    extension(IImageFile)
    {
        public static IImageFile Default => null!;
    }
}

file static class AccessLevelHelper
{
    extension(AccessLevel)
    {
        public static AccessLevel Default => AccessLevel.AuthReadOnly;
    }
}
