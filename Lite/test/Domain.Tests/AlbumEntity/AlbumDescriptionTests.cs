using Domain.AlbumAggregate.AlbumEntity;
using Shouldly;

namespace Domain.Tests.AlbumEntity;

[TestClass]
public class AlbumDescriptionTests
{
    [DataRow(null)]
    [DataRow("")]
    [DataRow("   ")]
    [DataRow("                                                                            ")]
    [TestMethod]
    public void Return_False_When_Create_From_NullOrWhitespace(string value)
    {
        bool result = AlbumDescription.TryCreateNew(value, out var _);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void Return_False_When_Create_From_Too_Long()
    {
        string value = new('a', AlbumDescription.MaxLength + 1);

        bool result = AlbumDescription.TryCreateNew(value, out var _);

        result.ShouldBeFalse();
    }

    [TestMethod]
    public void Return_False_When_Create_From_Too_Short()
    {
        string value = new('a', AlbumDescription.MinLength - 1);

        bool result = AlbumDescription.TryCreateNew(value, out var _);

        result.ShouldBeFalse();
    }

    [DataRow(AlbumDescription.MinLength)]
    [DataRow(AlbumDescription.MaxLength)]
    [TestMethod]
    public void Return_True_When_Create_From_Ends(int length)
    {
        string value = new('a', length);

        bool result = AlbumDescription.TryCreateNew(value, out var _);

        result.ShouldBeTrue();
    }

    [TestMethod]
    public void Return_True_When_Create_From_Valid()
    {
        const string value = "I'm a album description";

        bool result = AlbumDescription.TryCreateNew(value, out var _);

        result.ShouldBeTrue();
    }

    [DataRow("album_description  ")]
    [DataRow("    album_description")]
    [DataRow("   album_description   ")]
    [TestMethod]
    public void Should_Trim_Whitespace_When_Create(string input_value)
    {
        const string actual_value = "album_description";

        AlbumDescription.TryCreateNew(input_value, out var album);

        album.Value.ShouldBe(actual_value);
    }

    [TestMethod]
    public void Should_Set_Value_When_Create_From_Valid()
    {
        const string value = "album_description";

        AlbumDescription.TryCreateNew(value, out var album);

        album.Value.ShouldBe(value);
    }

    [TestMethod]
    public void Should_Be_Equal_When_Same_Value()
    {
        const string value = "album_description";

        AlbumDescription.TryCreateNew(value, out var album1);
        AlbumDescription.TryCreateNew(value, out var album2);

        album1.ShouldBe(album2);
    }

    [DataRow("album_description  ")]
    [DataRow("    album_description")]
    [DataRow("   album_description   ")]
    [TestMethod]
    public void Should_Be_Equal_When_Same_Value_With_Whitespace(string value_with_whitespace)
    {
        const string value = "album_description";

        AlbumDescription.TryCreateNew(value, out var album1);
        AlbumDescription.TryCreateNew(value_with_whitespace, out var album2);

        album1.ShouldBe(album2);
    }

    [TestMethod]
    public void Should_Not_Be_Equal_When_Different_Value()
    {
        const string value1 = "album_description1";
        const string value2 = "album_description2";

        AlbumDescription.TryCreateNew(value1, out var album1);
        AlbumDescription.TryCreateNew(value2, out var album2);

        album1.ShouldNotBe(album2);
    }
}
