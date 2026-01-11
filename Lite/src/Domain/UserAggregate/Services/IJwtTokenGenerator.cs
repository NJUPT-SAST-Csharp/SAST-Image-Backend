using Domain.UserAggregate.UserEntity;

namespace Domain.UserAggregate.Services;

public interface IJwtTokenGenerator
{
    public JwtToken Generate(UserId id, Username username, Roles roles);
}
