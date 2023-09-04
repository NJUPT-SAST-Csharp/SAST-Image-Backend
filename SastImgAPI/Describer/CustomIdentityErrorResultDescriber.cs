using Microsoft.AspNetCore.Identity;
using Microsoft.Extensions.Localization;
using LanguageResources;

namespace SastImgAPI.Describer
{
    public class CustomIdentityErrorResultDescriber : IdentityErrorDescriber
    {
        private readonly IStringLocalizer<IdentityLanguage> _localizer;

        public CustomIdentityErrorResultDescriber(IStringLocalizer<IdentityLanguage> localizer)
        {
            _localizer = localizer;
        }

        public override IdentityError DuplicateUserName(string userName)
        {
            return new IdentityError { Description = _localizer[nameof(DuplicateUserName)] };
        }

        public override IdentityError DuplicateEmail(string email)
        {
            return new IdentityError { Description = _localizer[nameof(DuplicateEmail)] };
        }

        public override IdentityError DefaultError()
        {
            return new IdentityError { Description = _localizer[nameof(DefaultError)] };
        }
    }
}
