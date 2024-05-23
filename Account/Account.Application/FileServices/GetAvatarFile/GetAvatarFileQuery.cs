using Account.Domain.UserEntity;
using Shared.Primitives.Query;

namespace Account.Application.FileServices.GetAvatarFile
{
    public sealed class GetAvatarFileQuery(long userId) : IQueryRequest<Stream?>
    {
        public UserId UserId { get; } = new(userId);
    }
}
