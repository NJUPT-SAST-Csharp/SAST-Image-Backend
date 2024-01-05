using Account.Application.SeedWorks;
using Account.Application.Services;
using Account.Entity.UserEntity.Repositories;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.AccountEndpoints.Register.SendRegistrationCode
{
    public sealed class SendRegistrationCodeEndpointHandler(
        IAuthCodeCache cache,
        IUserCheckRepository repository,
        ITokenSender sender,
        ILogger<SendRegistrationCodeEndpointHandler> logger
    ) : IEndpointHandler<SendRegistrationCodeRequest>
    {
        private readonly IAuthCodeCache _cache = cache;
        private readonly IUserCheckRepository _repository = repository;
        private readonly ITokenSender _sender = sender;
        private readonly ILogger<SendRegistrationCodeEndpointHandler> _logger = logger;

        public async Task<IResult
        // Results<NoContent, BadRequest<BadRequestResponse>>
        > Handle(SendRegistrationCodeRequest request)
        {
            var code = Random.Shared.Next(100000, 999999);

            var result = await _sender.SendTokenAsync(code.ToString(), request.Email);

            if (result == false)
            {
                return Responses.BadRequest("Something went wrong.");
            }

            _ = _cache.StoreCodeAsync(CodeCacheKey.Registration, request.Email, code);
            return Responses.NoContent;
        }
    }
}
