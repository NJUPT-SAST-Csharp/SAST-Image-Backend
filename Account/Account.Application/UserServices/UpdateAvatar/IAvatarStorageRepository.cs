using Account.Domain.UserEntity;
using Microsoft.AspNetCore.Http;

namespace Account.Application.UserServices.UpdateAvatar
{
    public interface IAvatarStorageRepository
    {
        public Task<Uri> UploadAvatarAsync(UserId id, IFormFile avatarFile);
    }
}
