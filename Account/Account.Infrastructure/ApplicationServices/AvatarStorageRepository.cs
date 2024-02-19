using Account.Application.UserServices.UpdateAvatar;
using Account.Domain.UserEntity;
using Microsoft.AspNetCore.Http;
using Storage.Clients;

namespace Account.Infrastructure.ApplicationServices
{
    internal sealed class AvatarStorageRepository(AvatarClient client) : IAvatarStorageRepository
    {
        private readonly AvatarClient _client = client;

        public Task<Uri> UploadAvatarAsync(UserId id, IFormFile avatarFile)
        {
            return _client.UploadAvatarAsync(id.Value, avatarFile);
        }
    }
}
