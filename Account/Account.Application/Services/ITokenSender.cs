namespace Account.Application.Services
{
    public interface ITokenSender
    {
        Task<bool> SendTokenAsync(string token, CancellationToken cancellationToken = default);
    }
}
