using Account.Domain.UserEntity;
using Shared.Primitives.Query;

namespace Account.Application.FileServices.GetHeaderFile
{
    public sealed class GetHeaderFileQuery(long userId) : IQueryRequest<Stream?>
    {
        public UserId UserId { get; } = new(userId);
    }
}
