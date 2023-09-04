using Microsoft.Extensions.Localization;
using System.Resources;

namespace LanguageResources
{
    public class IdentityLanguage : IStringLocalizer<IdentityLanguage>
    {
        private readonly ResourceManager _resourceManager;

        public IdentityLanguage()
        {
            _resourceManager = new ResourceManager(typeof(IdentityLanguage));
        }

        public LocalizedString this[string name] => new(name, _resourceManager.GetString(name)!);

        public LocalizedString this[string name, params object[] arguments] =>
            new(name, string.Format(_resourceManager.GetString(name)!, arguments));

        public IEnumerable<LocalizedString> GetAllStrings(bool includeParentCultures)
        {
            throw new NotImplementedException();
        }
    }
}
