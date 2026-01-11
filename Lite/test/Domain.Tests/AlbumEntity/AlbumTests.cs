using System.Reflection;
using Domain.AlbumAggregate.AlbumEntity;
using Domain.AlbumAggregate.Commands;
using Domain.AlbumAggregate.Events;
using Domain.AlbumAggregate.Exceptions;
using Domain.AlbumAggregate.ImageEntity;
using Domain.CategoryAggregate.CategoryEntity;
using Domain.Entity;
using Domain.Shared;
using Domain.Tests.ImageEntity;
using Domain.UserAggregate.UserEntity;
using Shouldly;
using static Domain.Tests.AlbumEntity.AlbumTestsHelper;

namespace Domain.Tests.AlbumEntity;

[TestClass]
public class AlbumTests
{
    #region IsRemoved

    [TestMethod]
    public void Return_True_When_Album_Removed()
    {
        var album = RemovedNewAlbum;

        bool isRemoved = album.GetValue<AlbumStatus>().IsRemoved;

        isRemoved.ShouldBeTrue();
    }

    [TestMethod]
    public void Return_False_When_Album_Not_Removed()
    {
        var album = ValidNewAlbum;

        bool isRemoved = album.GetValue<AlbumStatus>().IsRemoved;

        isRemoved.ShouldBeFalse();
    }

    #endregion

    #region UpdateDescription

    [TestMethod]
    public void Throw_When_UpdateDescription_In_Immutable_Album()
    {
        var album = RemovedNewAlbum;
        UpdateAlbumDescriptionCommand command = new(Id, NewDescription, Author);

        Should.Throw<AlbumRemovedException>(() => album.UpdateDescription(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateDescription_As_Not_Author_Or_Admin(long actorId)
    {
        var album = ValidNewAlbum;
        UpdateAlbumDescriptionCommand command = new(Id, NewDescription, NewActor(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateDescription(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Description_Updated(long actorId, bool isAdmin)
    {
        var album = ValidNewAlbum;
        UpdateAlbumDescriptionCommand command = new(Id, NewDescription, NewActor(actorId, isAdmin));

        album.UpdateDescription(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumDescriptionUpdatedEvent>();
    }

    #endregion

    #region UpdateTitle

    [TestMethod]
    public void Throw_When_UpdateTitle_In_Immutable_Album()
    {
        var album = RemovedNewAlbum;
        UpdateAlbumTitleCommand command = new(Id, NewTitle, Author);

        Should.Throw<AlbumRemovedException>(() => album.UpdateTitle(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateTitle_As_Not_Author_Or_Admin(long actorId)
    {
        var album = ValidNewAlbum;
        UpdateAlbumTitleCommand command = new(Id, NewTitle, NewActor(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateTitle(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Title_Updated(long actorId, bool isAdmin)
    {
        var album = ValidNewAlbum;
        UpdateAlbumTitleCommand command = new(Id, NewTitle, NewActor(actorId, isAdmin));

        album.UpdateTitle(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumTitleUpdatedEvent>();
    }

    #endregion

    #region UpdateCategory

    [TestMethod]
    public void Throw_When_UpdateCategory_In_Immutable_Album()
    {
        var album = RemovedNewAlbum;
        UpdateAlbumCategoryCommand command = new(Id, NewCategory, Author);

        Should.Throw<AlbumRemovedException>(() => album.UpdateCategory(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateCategory_As_Not_Author_Or_Admin(long actorId)
    {
        var album = ValidNewAlbum;
        UpdateAlbumCategoryCommand command = new(Id, NewCategory, NewActor(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateCategory(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Category_Updated(long actorId, bool isAdmin)
    {
        var album = ValidNewAlbum;
        UpdateAlbumCategoryCommand command = new(Id, NewCategory, NewActor(actorId, isAdmin));

        album.UpdateCategory(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCategoryUpdatedEvent>();
    }

    #endregion

    #region UpdateAccessLevel

    [TestMethod]
    public void Throw_When_UpdateAccessLevel_In_Removed_Album()
    {
        var album = RemovedNewAlbum;
        UpdateAccessLevelCommand command = new(Id, NewAccessLevel, Author);

        Should.Throw<AlbumRemovedException>(() => album.UpdateAccessLevel(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateAccessLevel_As_Not_Author_Or_Admin(long actorId)
    {
        var album = ValidNewAlbum;
        UpdateAccessLevelCommand command = new(Id, NewAccessLevel, NewActor(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateAccessLevel(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_AccessLevel_Updated(long actorId, bool isAdmin)
    {
        var album = ValidNewAlbum;
        UpdateAccessLevelCommand command = new(Id, NewAccessLevel, NewActor(actorId, isAdmin));

        album.UpdateAccessLevel(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumAccessLevelUpdatedEvent>();
    }

    #endregion

    #region UpdateCollaborators

    [TestMethod]
    public void Throw_When_UpdateCollaborators_In_Immutable_Album()
    {
        var album = RemovedNewAlbum;
        UpdateCollaboratorsCommand command = new(Id, new(NewCollaborators), Author);

        Should.Throw<AlbumRemovedException>(() => album.UpdateCollaborators(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateCollaborators_As_Not_Author_Or_Admin(long actorId)
    {
        var album = ValidNewAlbum;
        var collaborators = NewCollaborators;
        UpdateCollaboratorsCommand command = new(Id, new(collaborators), NewActor(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateCollaborators(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Collaborators_Updated(long actorId, bool isAdmin)
    {
        var album = ValidNewAlbum;
        var collaborators = NewCollaborators;
        UpdateCollaboratorsCommand command = new(
            Id,
            new(collaborators),
            NewActor(actorId, isAdmin)
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
        var album = RemovedNewAlbum;
        UpdateCoverCommand command = new(Id, null, Author);

        Should.Throw<AlbumRemovedException>(() => album.UpdateCover(command));
    }

    [DataRow(VisitorId)]
    [DataRow(Collaborator1Id)]
    [TestMethod]
    public void Throw_When_UpdateCover_As_Not_Author_Or_Admin(long actorId)
    {
        var album = ValidNewAlbum;
        UpdateCoverCommand command = new(Id, null, NewActor(actorId));

        Should.Throw<NoPermissionException>(() => album.UpdateCover(command));
    }

    [DataRow(AdminId, true)]
    [DataRow(AuthorId, false)]
    [TestMethod]
    public void Raise_Event_When_Cover_Updated(long actorId, bool isAdmin)
    {
        var album = ValidNewAlbum;
        UpdateCoverCommand command = new(Id, ImageFile, NewActor(actorId, isAdmin));

        album.UpdateCover(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCoverUpdatedEvent>();
    }

    #endregion

    #region AddImage

    [TestMethod]
    public void Throw_When_AddImage_In_Immutable_Album()
    {
        var album = RemovedNewAlbum;
        AddImageCommand command = new(Id, NewImageTitle, NewImageTags, ImageFile, Author);

        Should.Throw<AlbumRemovedException>(() => album.AddImage(command));
    }

    [TestMethod]
    public void Throw_When_AddImage_As_Not_Author_Or_Admin_Or_Collaborator()
    {
        var album = ValidNewAlbum;
        AddImageCommand command = new(
            Id,
            NewImageTitle,
            NewImageTags,
            ImageFile,
            NewActor(VisitorId)
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
        var album = ValidNewAlbum;
        AddImageCommand command = new(
            Id,
            NewImageTitle,
            NewImageTags,
            ImageFile,
            NewActor(actorId, isAdmin)
        );

        album.AddImage(command);

        album.DomainEvents.Count.ShouldBeGreaterThan(0);
        album.GetValue<List<Image>>().Count.ShouldBe(3);
    }

    [TestMethod]
    public void Raise_AlbumCoverUpdatedEvent_When_IsLatestImage_As_Image_Added()
    {
        var album = ValidNewAlbum;
        AddImageCommand command = new(Id, NewImageTitle, NewImageTags, ImageFile, Author);

        album.AddImage(command);

        album.DomainEvents.Count.ShouldBe(2);
        album.DomainEvents.ShouldBeOfTypes(typeof(ImageAddedEvent), typeof(AlbumCoverUpdatedEvent));
    }

    [TestMethod]
    public void Not_Raise_AlbumCoverUpdatedEvent_When_Not_IsLatestImage_As_Image_Added()
    {
        var album = ValidNewAlbum;
        album.SetValue(ImmutableCover);
        AddImageCommand command = new(Id, NewImageTitle, NewImageTags, ImageFile, Author);

        album.AddImage(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<ImageAddedEvent>();
    }

    #endregion

    #region RemoveImage


    [TestMethod]
    public void Throw_When_RemoveImage_In_Immutable_Album()
    {
        var album = RemovedNewAlbum;
        RemoveImageCommand command = new(Id, new(Image1Id), Author);

        Should.Throw<AlbumRemovedException>(() => album.RemoveImage(command));
    }

    [TestMethod]
    public void Throw_When_RemoveImage_As_Not_Author_Or_Admin_Or_Collaborator()
    {
        var album = ValidNewAlbum;
        RemoveImageCommand command = new(Id, new(Image1Id), NewActor(VisitorId));

        Should.Throw<NoPermissionException>(() => album.RemoveImage(command));
    }

    [TestMethod]
    public void Raise_CoverUpdatedEvent_When_Image_As_LatestImage_Removed()
    {
        var album = ValidNewAlbum;
        album.SetValue(new Cover(new(Image2Id), true));
        RemoveImageCommand command = new(Id, new(Image2Id), Author);

        album.RemoveImage(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCoverUpdatedEvent>();
        album.GetValue<Cover>().Id.ShouldBe(new(Image1Id));
    }

    [TestMethod]
    public void Not_Raise_CoverUpdatedEvent_When_Image_Not_As_LatestImage_Removed()
    {
        var album = ValidNewAlbum;
        RemoveImageCommand command = new(Id, new(Image2Id), Author);

        album.RemoveImage(command);

        album.DomainEvents.Count.ShouldBe(0);
    }

    #endregion

    #region RestoreImage

    [TestMethod]
    public void Throw_When_RestoreImage_In_Immutable_Album()
    {
        var album = RemovedNewAlbum;
        RestoreImageCommand command = new(Id, new(Image1Id), Author);

        Should.Throw<AlbumRemovedException>(() => album.RestoreImage(command));
    }

    [TestMethod]
    public void Throw_When_RestoreImage_As_Not_Author_Or_Admin_Or_Collaborator()
    {
        var album = ValidNewAlbum;
        RestoreImageCommand command = new(Id, new(Image1Id), NewActor(VisitorId));

        Should.Throw<NoPermissionException>(() => album.RestoreImage(command));
    }

    [TestMethod]
    public void Raise_CoverUpdatedEvent_When_Image_As_LatestImage_Restored()
    {
        var album = ValidNewAlbum;
        album.GetImage(Image2Id).SetValue(ImageTestsHelper.RemovedStatus);
        RestoreImageCommand command = new(Id, new(Image2Id), Author);

        album.RestoreImage(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumCoverUpdatedEvent>();
        album.GetValue<Cover>().Id.ShouldBe(new(Image2Id));
    }

    [TestMethod]
    public void Not_Raise_CoverUpdatedEvent_When_Image_Not_As_LatestImage_Restored()
    {
        var album = ValidNewAlbum;
        album.SetValue(new Cover(null, false));
        RestoreImageCommand command = new(Id, new(Image2Id), Author);

        album.RestoreImage(command);

        album.DomainEvents.Count.ShouldBe(0);
    }

    #endregion

    #region Remove

    [TestMethod]
    public void Not_Change_When_Album_Already_Removed()
    {
        var album = RemovedNewAlbum;
        RemoveAlbumCommand command = new(Id, Author);
        var time = album.GetValue<AlbumStatus>().RemovedAt!.Value;

        album.Remove(command);

        album.DomainEvents.Count.ShouldBe(0);
        album.GetValue<AlbumStatus>().RemovedAt.ShouldBe(time);
    }

    [TestMethod]
    public void Raise_Event_When_Removed()
    {
        var album = ValidNewAlbum;
        RemoveAlbumCommand command = new(Id, Author);

        album.Remove(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumRemovedEvent>();
    }

    [TestMethod]
    public void Update_Contained_Images_Status_When_Removed()
    {
        var album = ValidNewAlbum;
        RemoveAlbumCommand command = new(Id, Author);

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
        var album = ValidNewAlbum;
        RestoreAlbumCommand command = new(Id, Author);

        album.Restore(command);

        album.DomainEvents.Count.ShouldBe(0);
        album.GetValue<AlbumStatus>().RemovedAt.ShouldBeNull();
    }

    [TestMethod]
    public void Raise_Event_When_Restored()
    {
        var album = RemovedNewAlbum;
        RestoreAlbumCommand command = new(Id, Author);

        album.Restore(command);

        album.DomainEvents.Count.ShouldBe(1);
        album.DomainEvents.First().ShouldBeOfType<AlbumRestoredEvent>();
    }

    [TestMethod]
    public void Update_Contained_Images_Status_When_Restored()
    {
        var album = RemovedNewAlbum;
        RestoreAlbumCommand command = new(Id, Author);

        album.Restore(command);

        foreach (var image in album.GetValue<List<Image>>())
        {
            image.GetValue<ImageStatus>().Value.ShouldBe(ImageStatusValue.Available);
        }
    }

    #endregion
}

internal static class AlbumTestsHelper
{
    public static readonly AlbumId Id = AlbumId.GenerateNew();

    public static Actor NewActor(long id, bool isAdmin = false) =>
        new()
        {
            Id = new(id),
            IsAdmin = isAdmin,
            IsAuthenticated = true,
        };

    public const long Subscriber1Id = 9;
    public const long Subscriber2Id = 8;
    public const long NewSubscriberId = 7;
    public static readonly Subscribe Subscriber1 = new(Id, new(Subscriber1Id));
    public static readonly Subscribe Subscriber2 = new(Id, new(Subscriber2Id));
    public static readonly Subscribe NewSubscriber = new(Id, new(NewSubscriberId));
    public static List<Subscribe> OriginalSubscribers => [Subscriber1, Subscriber2];

    public static readonly IImageFile ImageFile = null!;

    public const long Image1Id = 1111111;
    public const long Image2Id = 2222222;
    public static List<Image> OriginalImages =>
        [ImageTestsHelper.CreateNewImage(Image1Id), ImageTestsHelper.CreateNewImage(Image2Id)];

    public static Image GetImage(this Album album, long id)
    {
        Image? image = album.GetValue<List<Image>>().FirstOrDefault(image => image.Id.Value == id);

        Assert.IsNotNull(image);

        return image;
    }

    public static readonly ImageTitle NewImageTitle = new("new_title");
    public static readonly ImageTags NewImageTags = new([new("741"), new("ywwuyi")]);

    public static readonly Cover DefaultCover = Cover.Default;
    public static readonly Cover ImmutableCover = Cover.UserCustomCover;

    public const long Collaborator1Id = 1;
    public const long Collaborator2Id = 2;
    public static readonly UserId[] OriginalCollaborators =
    [
        new(Collaborator1Id),
        new(Collaborator2Id),
    ];
    public static readonly UserId[] NewCollaborators =
    [
        new(Collaborator1Id),
        new(Collaborator2Id),
        new(3),
    ];
    public static readonly UserId[] EmptyCollaborators = [];

    public const long AuthorId = 11111;
    public const long AdminId = 99999;
    public const long VisitorId = 55555;
    public static readonly Actor Author = NewActor(AuthorId);
    public static readonly Actor Adm = NewActor(AdminId, true);
    public static readonly Actor Visitor = NewActor(VisitorId);

    public static readonly AlbumTitle OriginalTitle = new("original_title");
    public static readonly AlbumTitle NewTitle = new("new_title");
    public static readonly CategoryId NewCategory = new(2222222222);
    public static readonly AlbumDescription NewDescription = new("new_description");
    public static readonly AccessLevel NewAccessLevel = AccessLevel.AuthReadOnly;

    public static T GetProperty<T>(this Album album, string propertyName)
    {
        var property = typeof(Album)
            .GetProperties(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(p =>
                p.PropertyType == typeof(T)
                && p.Name.Contains(propertyName, StringComparison.OrdinalIgnoreCase)
            );
        object? value = property.GetValue(album);

        Assert.IsNotNull(value);

        return (T)value;
    }

    public static T GetValue<T>(this Album album)
    {
        var field = typeof(Album)
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(f => f.FieldType == typeof(T));
        object? value = field.GetValue(album);

        Assert.IsNotNull(value);

        return (T)value;
    }

    public static void SetValue<T>(this Album album, string fieldName, T value)
    {
        var field = typeof(Album)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .First(f =>
                f.FieldType == typeof(T)
                && f.Name.Contains(fieldName, StringComparison.OrdinalIgnoreCase)
            );

        Assert.IsNotNull(field);

        field.SetValue(album, value);
    }

    public static void SetValue<T>(this Album album, T value)
    {
        var field = typeof(Album)
            .GetFields(BindingFlags.NonPublic | BindingFlags.Instance)
            .First(f => f.FieldType == typeof(T));

        Assert.IsNotNull(field);

        field.SetValue(album, value);
    }

    public static Album ValidNewAlbum => CreateNewAlbumFromReflection();

    public static Album RemovedNewAlbum
    {
        get
        {
            var status = AlbumStatus.Removed(DateTime.UtcNow);
            var album = ValidNewAlbum;
            album.SetValue(status);

            return album;
        }
    }

    private static Album CreateNewAlbumFromReflection()
    {
        var constructor = typeof(Album).GetConstructor(
            BindingFlags.Instance | BindingFlags.NonPublic,
            Type.EmptyTypes
        );

        Assert.IsNotNull(constructor);

        var album = (Album)constructor.Invoke([]);

        album.SetValue(Author.Id);
        album.SetValue(OriginalTitle);
        album.SetValue(DefaultCover);
        album.SetValue(OriginalSubscribers);
        album.SetValue(OriginalImages);
        album.SetValue(OriginalCollaborators);

        typeof(EntityBase<AlbumId>)
            .GetFields(BindingFlags.Instance | BindingFlags.NonPublic)
            .First(f => f.Name.Contains("id", StringComparison.OrdinalIgnoreCase))
            .SetValue(album, Id);

        return album;
    }
}
