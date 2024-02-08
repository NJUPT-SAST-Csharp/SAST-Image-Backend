using Account.Application.SeedWorks;
using Microsoft.AspNetCore.Mvc;

namespace Account.Application.Endpoints.UserEndpoints.Query
{
    public readonly struct QueryUserRequest : IRequest
    {
        [FromQuery]
        public readonly long? UserId { get; init; }

        [FromQuery]
        public readonly string? Username { get; init; }
    }
}
