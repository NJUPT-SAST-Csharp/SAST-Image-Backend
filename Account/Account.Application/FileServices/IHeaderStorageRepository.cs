using Account.Domain.UserEntity;
using Microsoft.AspNetCore.Http;

namespace Account.Application.FileServices
{
    public interface IHeaderStorageRepository
    {
        public Task<Stream?> GetHeaderAsync(
            UserId userId,
            CancellationToken cancellationToken = default
        );
        public Task<Uri> UploadHeaderAsync(
            UserId id,
            IFormFile file,
            CancellationToken cancellationToken = default
        );
    }
}
