﻿namespace Account.Application.Account.Login
{
    public sealed class LoginDto(string jwt)
    {
        public string Jwt { get; } = jwt;
    }
}