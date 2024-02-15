using SNS.Domain.UserEntity;

namespace SNS.Application.UserServices.UpdateAvatar
{
    public interface IAvatarStorageClient
    {
        public Task<Uri> UploadAvatarAsync(
            UserId userId,
            Stream stream,
            CancellationToken cancellationToken = default
        );
    }
}
