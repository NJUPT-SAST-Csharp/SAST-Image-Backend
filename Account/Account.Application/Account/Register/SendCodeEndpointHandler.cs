using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.User.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Response.Builders;

namespace Account.Application.Account.Register
{
    public sealed class SendCodeEndpointHandler(
        IAuthCache cache,
        IUserCheckRepository repository,
        ITokenSender sender,
        ILogger<SendCodeEndpointHandler> logger
    ) : IEndpointHandler<SendCodeRequest>
    {
        private readonly IAuthCache _cache = cache;
        private readonly IUserCheckRepository _repository = repository;
        private readonly ITokenSender _sender = sender;
        private readonly ILogger<SendCodeEndpointHandler> _logger = logger;

        public async Task<IResult
        // Results<NoContent, BadRequest<BadRequestResponse>>
        > Handle(SendCodeRequest request)
        {
            var isExist = await _repository.CheckEmailExistenceAsync(request.Email);
            if (isExist)
            {
                _logger.LogInformation(
                    "Tried to register with duplicated email \"{email}\".",
                    request.Email
                );
                return Responses.Conflict(nameof(request.Email), request.Email);
            }
            var code = Random.Shared.Next(100000, 999999).ToString();
            var result = await _sender.SendTokenAsync(code);
            if (result)
            {
                _logger.LogInformation(
                    "Registration code {code} has sent to {email}.",
                    code,
                    request.Email
                );
                _ = _cache.StoreCodeAsync(request.Email, code);
                return Responses.NoContent;
            }
            else
            {
                _logger.LogError(
                    "Failed when try to send registration code to {email}.",
                    request.Email
                );
                return TypedResults.Empty;
            }
        }
    }
}
