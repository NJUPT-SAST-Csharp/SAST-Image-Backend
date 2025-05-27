using Identity;
using Mediator;

namespace Account.Application.FileServices.GetHeaderFile;

public sealed class GetHeaderFileQuery(long userId) : IQuery<Stream?>
{
    public UserId UserId { get; } = new(userId);
}
