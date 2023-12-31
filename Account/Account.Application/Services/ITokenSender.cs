﻿namespace Account.Application.Services
{
    public interface ITokenSender
    {
        Task<bool> SendTokenAsync(
            string token,
            string email,
            CancellationToken cancellationToken = default
        );
    }
}
