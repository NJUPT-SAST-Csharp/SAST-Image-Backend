using Account.Application.Services;
using Account.Domain.UserEntity.Services;
using Mediator;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode;

public sealed class SendForgetCodeCommandHandler(
    IAuthCodeSender sender,
    IAuthCodeCache cache,
    IUserUniquenessChecker checker
) : ICommandHandler<SendForgetCodeCommand>
{
    public async ValueTask<Unit> Handle(
        SendForgetCodeCommand request,
        CancellationToken cancellationToken
    )
    {
        bool isEmailExist = await checker.CheckEmailExistenceAsync(
            request.Email,
            cancellationToken
        );

        if (isEmailExist is false)
            return Unit.Value;

        int code = Random.Shared.Next(100000, 999999);

        await sender.SendCodeAsync(request.Email, code, cancellationToken);

        await cache.StoreCodeAsync(
            CodeCacheKey.ForgetAccount,
            request.Email,
            code,
            cancellationToken
        );

        return Unit.Value;
    }
}
