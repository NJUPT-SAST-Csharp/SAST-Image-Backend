using Account.Application.Services;

namespace Account.Infrastructure.Services
{
    public sealed class EmailTokenSender : ITokenSender
    {
        public Task<bool> SendTokenAsync(
            string token,
            CancellationToken cancellationToken = default
        )
        {
            throw new NotImplementedException();
        }
    }
}
