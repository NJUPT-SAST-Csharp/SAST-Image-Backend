using Microsoft.AspNetCore.Mvc;

namespace Account.WebAPI.Requests
{
    public readonly struct UpdateAvatarRequest
    {
        [FromForm]
        public readonly IFormFile AvatarFile { get; init; }
    }
}
