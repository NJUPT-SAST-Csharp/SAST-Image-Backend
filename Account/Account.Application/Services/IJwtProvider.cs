﻿using Account.Domain.UserEntity;

namespace Account.Application.Services
{
    public interface IJwtProvider
    {
        public string GetLoginJwt(UserId userId, string username, IEnumerable<Role> roles);
    }
}
