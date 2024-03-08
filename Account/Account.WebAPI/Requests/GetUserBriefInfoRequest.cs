using Microsoft.AspNetCore.Mvc;

namespace Account.WebAPI.Requests
{
    public readonly struct GetUserBriefInfoRequest
    {
        [FromRoute(Name = "username")]
        public readonly string Username { get; init; }
    }
}
