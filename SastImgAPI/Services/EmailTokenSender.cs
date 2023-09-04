using Microsoft.Extensions.Options;
using SastImgAPI.Options;
using System.Net.Mail;

namespace SastImgAPI.Services
{
    public class EmailTokenSender
    {
        private readonly EmailSendOption _option;

        public EmailTokenSender(IOptionsSnapshot<EmailSendOption> option)
        {
            _option = option.Value;
        }

        public async Task<bool> SendEmailTokenAsync(string email, string subject, string content)
        {
            try
            {
                using SmtpClient client = new(_option.Host, _option.Port);
                MailMessage msg = new(_option.Username, email, subject, content);
                client.UseDefaultCredentials = false;
                System.Net.NetworkCredential basicAuthenticationInfo =
                    new(_option.Username, _option.Password);
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
