using Account.Domain.UserEntity.ValueObjects;

namespace Account.Domain.UserEntity.Services;

public interface IJwtTokenGenerator
{
    public JwtToken Issue(User user);
}
