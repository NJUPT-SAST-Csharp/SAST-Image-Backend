using Account.Application.UserServices.UpdateHeader;
using Account.Domain.UserEntity;
using Microsoft.AspNetCore.Http;
using Storage.Clients;

namespace Account.Infrastructure.Persistence.Storages
{
    internal sealed class HeaderStorageRepository(IStorageClient client, IProcessClient processor)
        : IHeaderStorageRepository
    {
        private readonly IStorageClient _client = client;
        private readonly IProcessClient _processor = processor;

        public async Task<Uri> UploadHeaderAsync(
            UserId id,
            IFormFile file,
            CancellationToken cancellationToken = default
        )
        {
            await using var image = file.OpenReadStream();

            string extension = await _processor.GetExtensionNameAsync(image, cancellationToken);

            var key = $"headers/{id}.{extension}";

            await _client.UploadImageAsync(image, key, cancellationToken);

            Uri url = await _processor.CompressImageAsync(key, true, cancellationToken);

            return url;
        }
    }
}
