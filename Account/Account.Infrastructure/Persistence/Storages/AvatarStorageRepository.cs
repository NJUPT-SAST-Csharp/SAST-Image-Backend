using Account.Application.UserServices.UpdateAvatar;
using Account.Domain.UserEntity;
using Microsoft.AspNetCore.Http;
using Storage.Clients;

namespace Account.Infrastructure.Persistence.Storages
{
    internal sealed class AvatarStorageRepository(IStorageClient client, IProcessClient processor)
        : IAvatarStorageRepository
    {
        private readonly IStorageClient _client = client;
        private readonly IProcessClient _processor = processor;

        public async Task<Uri> UploadAvatarAsync(
            UserId id,
            IFormFile avatarFile,
            CancellationToken cancellationToken = default
        )
        {
            await using var image = avatarFile.OpenReadStream();

            string extension = await _processor.GetExtensionNameAsync(image, cancellationToken);

            var key = $"avatars/{id}.{extension}";

            var url = await _client.UploadImageAsync(image, key, cancellationToken);

            return url;
        }
    }
}
