﻿using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.Login
{
    public sealed class LoginCommand(string username, string password) : ICommandRequest<LoginDto>
    {
        public string Username { get; } = username;
        public string Password { get; } = password;
    }
}
