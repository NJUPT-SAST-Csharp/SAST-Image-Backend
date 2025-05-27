namespace Account.Application.Services;

public interface IAuthCodeSender
{
    Task SendCodeAsync(string email, int code, CancellationToken cancellationToken = default);
}
