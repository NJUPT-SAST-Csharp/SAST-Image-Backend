using Storage.Application.Model;

namespace Storage.Infrastructure.Models;

internal sealed class ManagedImageFile : IImageFile
{
    private ManagedImageFile() { }

    public static async Task<ManagedImageFile> CreateAsync(
        Stream stream,
        CancellationToken cancellationToken
    )
    {
        MemoryStream m = new();
        await stream.CopyToAsync(m, cancellationToken);
        m.Position = 0;
        m.Seek(0, SeekOrigin.Begin);
        return new()
        {
            Stream = m,
            Format = ImageFile.TryGetFormat(m, out var format)
                ? format.Value
                : ImageFileFormat.NONE,
        };
    }

    public Stream Stream { get; private init; } = new MemoryStream();
    public long Length => Stream.Length;
    public ImageFileFormat Format { get; private init; }
    public IDictionary<string, string> MetaData { get; private set; } =
        new Dictionary<string, string>();

    public void Dispose()
    {
        Stream.Dispose();
    }
}
