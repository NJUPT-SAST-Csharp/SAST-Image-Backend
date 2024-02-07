namespace SastImg.Application.ImageServices.AddImage
{
    public interface IImageStorageClient
    {
        public Task<Uri> UploadImageAsync(
            string fileName,
            Stream stream,
            CancellationToken cancellationToken = default
        );
    }
}
