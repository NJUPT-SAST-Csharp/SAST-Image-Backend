using Utilities.Validators;

namespace SNS.WebAPI.Requests
{
    public readonly struct UpdateAvatarRequest
    {
        [FileValidator(10)]
        public readonly IFormFile Avatar { get; init; }
    }
}
