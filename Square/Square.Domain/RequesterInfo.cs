﻿using System.Security.Claims;
using Auth.Authentication;
using Auth.Authorization;

namespace Square.Domain
{
    public readonly struct RequesterInfo
    {
        public RequesterInfo(ClaimsPrincipal user)
        {
            IsAuthenticated = user.TryFetchId(out long id);
            Id = new(id);
            if (IsAuthenticated)
            {
                IsAdmin = user.HasRole(AuthorizationRole.ADMIN);
            }
        }

        public readonly bool IsAuthenticated { get; }
        public readonly UserId Id { get; }
        public readonly bool IsAdmin { get; }
    }
}
