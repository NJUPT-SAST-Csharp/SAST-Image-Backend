using Account.Application.UserServices.UpdateHeader;
using Account.Domain.UserEntity;
using Microsoft.AspNetCore.Http;
using Storage.Clients;

namespace Account.Infrastructure.ApplicationServices
{
    internal sealed class HeaderStorageRepository(HeaderClient client) : IHeaderStorageRepository
    {
        private readonly HeaderClient _client = client;

        public Task<Uri> UploadHeaderAsync(UserId id, IFormFile file)
        {
            return _client.UploadHeaderAsync(id.Value, file);
        }
    }
}
