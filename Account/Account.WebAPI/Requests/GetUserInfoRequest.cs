using Microsoft.AspNetCore.Mvc;

namespace Account.WebAPI.Requests
{
    public readonly struct GetUserInfoRequest
    {
        public GetUserInfoRequest() { }

        [FromQuery(Name = "username")]
        public readonly string? Username { get; init; } = null;

        [FromQuery(Name = "id")]
        public readonly long? UserId { get; init; } = null;

        [FromQuery(Name = "detailed")]
        public readonly bool IsDetailed { get; init; } = false;
    }
}
