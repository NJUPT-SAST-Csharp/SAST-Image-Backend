using Account.Domain.UserEntity;
using Microsoft.AspNetCore.Http;

namespace Account.Application.FileServices
{
    public interface IAvatarStorageRepository
    {
        public Task<Stream?> GetAvatarAsync(
            UserId userId,
            CancellationToken cancellationToken = default
        );

        public Task<Uri> UploadAvatarAsync(
            UserId userId,
            IFormFile avatarFile,
            CancellationToken cancellationToken = default
        );
    }
}
