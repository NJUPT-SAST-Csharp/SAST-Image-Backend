using Account.Application.Services;
using Account.Domain.UserEntity.Services;
using Primitives.Command;

namespace Account.Application.Endpoints.AccountEndpoints.ForgetAccount.SendForgetCode
{
    public sealed class SendForgetCodeCommandHandler(
        IAuthCodeSender sender,
        IAuthCodeCache cache,
        IUserUniquenessChecker checker
    ) : ICommandRequestHandler<SendForgetCodeCommand>
    {
        private readonly IAuthCodeSender _sender = sender;
        private readonly IAuthCodeCache _cache = cache;
        private readonly IUserUniquenessChecker _checker = checker;

        public async Task Handle(SendForgetCodeCommand request, CancellationToken cancellationToken)
        {
            var isEmailExist = await _checker.CheckEmailExistenceAsync(
                request.Email,
                cancellationToken
            );

            if (isEmailExist is false)
                return;

            var code = Random.Shared.Next(100000, 999999);

            await _sender.SendCodeAsync(request.Email, code, cancellationToken);

            await _cache.StoreCodeAsync(
                CodeCacheKey.ForgetAccount,
                request.Email,
                code,
                cancellationToken
            );
        }
    }
}
