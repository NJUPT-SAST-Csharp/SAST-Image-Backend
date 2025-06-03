namespace Storage.Entity;

public interface IImageFile
{
    public Stream Stream { get; }

    public long Length { get; }

    public ImageFormat Format { get; }
}
