namespace SastImg.Storage.Services
{
    public interface IImageClient
    {
        Task<Uri> UploadImageAsync(
            string fileName,
            FileStream stream,
            CancellationToken cancellationToken = default
        );

        Task<Uri> CompressImageAsync(
            string fileName,
            CancellationToken cancellationToken = default
        );
    }
}
