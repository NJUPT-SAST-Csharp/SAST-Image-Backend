using Account.Application.UserServices.UpdateAvatar;
using Account.WebAPI.SeedWorks;
using Microsoft.AspNetCore.Mvc;

namespace Account.WebAPI.Requests
{
    public readonly struct UpdateAvatarRequest : ICommandRequestObject<UpdateAvatarCommand>
    {
        [FromForm]
        public readonly IFormFile AvatarFile { get; init; }
    }
}
