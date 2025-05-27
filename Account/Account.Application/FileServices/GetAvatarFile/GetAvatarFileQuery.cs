using Identity;
using Mediator;

namespace Account.Application.FileServices.GetAvatarFile;

public sealed class GetAvatarFileQuery(long userId) : IQuery<Stream?>
{
    public UserId UserId { get; } = new() { Value = userId };
}
