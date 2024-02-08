using System.Security.Claims;
using Account.Application.SeedWorks;
using Account.Entity.UserEntity.Repositories;
using Microsoft.AspNetCore.Http;
using Shared.Response.Builders;

namespace Account.Application.Endpoints.UserEndpoints.Query
{
    public sealed class QueryUserEndpointHandler(IUserQueryRepository repository)
        : IAuthEndpointHandler<QueryUserRequest>
    {
        private readonly IUserQueryRepository _repository = repository;

        public async Task<IResult> Handle(QueryUserRequest request, ClaimsPrincipal requester)
        {
            if (request.UserId is { } userId)
            {
                var user = await _repository.GetUserByIdAsync(userId);
                return Responses.DataOrNotFound(QueryUserDto.FromUser(user));
            }

            if (request.Username is { } username)
            {
                var user = await _repository.GetUserByUsernameAsync(username);
                return Responses.DataOrNotFound(QueryUserDto.FromUser(user));
            }

            return Responses.BadRequest("Invalid request.", "UserId or Username must be provided.");
        }
    }
}
