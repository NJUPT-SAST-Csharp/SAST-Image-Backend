namespace Storage.Application.Model;

public interface IImageFile
{
    public Stream Stream { get; }
    public long Length { get; }
}
