namespace Storage.Application.Model;

public interface IImageFile : IDisposable
{
    public Stream Stream { get; }
    public long Length { get; }

    public ImageFileFormat Format { get; }
    public IDictionary<string, string> MetaData { get; }
}
