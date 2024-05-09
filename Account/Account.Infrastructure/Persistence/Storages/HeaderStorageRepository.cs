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
            string extension = await _processor.GetExtensionNameAsync(file, cancellationToken);

            var key = $"headers/{id}.{extension}";

            await _client.UploadImageAsync(file, key, cancellationToken);

            Uri url = await _processor.CompressImageAsync(key, true, cancellationToken);

            return url;
        }
    }
}
