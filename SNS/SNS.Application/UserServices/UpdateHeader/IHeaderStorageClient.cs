using Microsoft.AspNetCore.Http;
using SNS.Domain.UserEntity;

namespace SNS.Application.UserServices.UpdateHeader
{
    public interface IHeaderStorageClient
    {
        public Task<Uri> UploadHeaderAsync(
            UserId userId,
            IFormFile file,
            CancellationToken cancellationToken = default
        );
    }
}
