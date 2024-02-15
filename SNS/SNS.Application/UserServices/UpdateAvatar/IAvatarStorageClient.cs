using Microsoft.AspNetCore.Http;

namespace SNS.Application.UserServices.UpdateAvatar
{
    public interface IAvatarStorageClient
    {
        public Task<Uri> UploadAvatarAsync(
            IFormFile file,
            CancellationToken cancellationToken = default
        );
    }
}
