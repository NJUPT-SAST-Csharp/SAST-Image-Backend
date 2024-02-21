using Account.Domain.UserEntity;
using Microsoft.AspNetCore.Http;

namespace Account.Application.UserServices.UpdateHeader
{
    public interface IHeaderStorageRepository
    {
        public Task<Uri> UploadHeaderAsync(UserId id, IFormFile file);
    }
}
