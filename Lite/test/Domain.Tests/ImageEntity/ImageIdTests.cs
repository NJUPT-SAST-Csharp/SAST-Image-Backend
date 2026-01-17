using System.Text.Json;
using Domain.AlbumAggregate.ImageEntity;
using Shouldly;

namespace Domain.Tests.ImageEntity;

[TestClass]
public class ImageIdTests
{
    [TestMethod]
    public void Convert_From_Json()
    {
        var id = ImageId.New;

        var converted = JsonSerializer.Deserialize<ImageId>(id.Value);

        id.ShouldBe(converted);
    }
}

internal static class TestImageId
{
    extension(ImageId)
    {
        public static ImageId New => ImageId.GenerateNew();
    }
}
