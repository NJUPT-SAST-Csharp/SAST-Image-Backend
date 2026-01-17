using System.Text.Json;
using Domain.AlbumAggregate.AlbumEntity;
using Shouldly;

namespace Domain.Tests.AlbumEntity;

[TestClass]
public class AlbumIdTests
{
    [TestMethod]
    public void Convert_From_Json()
    {
        var id = AlbumId.New;

        var converted = JsonSerializer.Deserialize<AlbumId>(id.Value);

        id.ShouldBe(converted);
    }
}

internal static class TestAlbumId
{
    extension(AlbumId)
    {
        public static AlbumId New => AlbumId.GenerateNew();
    }
}
