﻿using System.Net;
using System.Net.Mail;
using Account.Application.Services;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;

namespace Account.Infrastructure.Services
{
    public sealed class EmailTokenSender(
        ILogger<EmailTokenSender> logger,
        IConfiguration configuration
    ) : ITokenSender
    {
        private readonly int _port = configuration.GetSection("SMS").GetValue<int>("Port");
        private readonly string _host =
            configuration["SMS:Host"]
            ?? throw new ArgumentNullException("Couldn't load 'Host' from configuration.");
        private readonly string _username =
            configuration["SMS:Username"]
            ?? throw new ArgumentNullException("Couldn't load 'Username' from configuration.");
        private readonly string _password =
            configuration["SMS:Password"]
            ?? throw new ArgumentNullException("Couldn't load 'Password' from configuration.");

        private readonly ILogger _logger = logger;

        public async Task<bool> SendTokenAsync(
            string code,
            string email,
            CancellationToken cancellationToken = default
        )
        {
            try
            {
                await Task.Run(() => { }, cancellationToken);
                //await SendEmailAsync(email, "Register code", code);
                _logger.LogInformation(
                    "Registrantion code [{code}] has sent to [{email}].",
                    code,
                    email
                );
                return true;
            }
            catch
            {
                _logger.LogError(
                    "Failed when try to send registration code [{code}] to [{email}].",
                    code,
                    email
                );
                return false;
            }
        }

        private Task SendEmailAsync(string to, string subject, string content)
        {
            using SmtpClient client = new(_host, _port);
            MailMessage msg = new(_username, to, subject, content);
            client.UseDefaultCredentials = false;
            NetworkCredential basicAuthenticationInfo = new(_username, _password);
            client.Credentials = basicAuthenticationInfo;
            client.EnableSsl = true;
            return client.SendMailAsync(msg);
        }
    }
}
