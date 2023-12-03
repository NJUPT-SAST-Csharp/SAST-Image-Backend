using System.Net;
using System.Net.Mail;

namespace Utilities
{
    public sealed class EmailSender(string host, int port, string username, string password)
    {
        private readonly string _host = host;
        private readonly int _port = port;
        private readonly string _username = username;
        private readonly string _password = password;

        public async Task<bool> SendEmailAsync(string email, string subject, string content)
        {
            try
            {
                using SmtpClient client = new(_host, _port);
                MailMessage msg = new(_username, email, subject, content);
                client.UseDefaultCredentials = false;
                NetworkCredential basicAuthenticationInfo = new(_username, _password);
                client.Credentials = basicAuthenticationInfo;
                client.EnableSsl = true;
                await client.SendMailAsync(msg);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
