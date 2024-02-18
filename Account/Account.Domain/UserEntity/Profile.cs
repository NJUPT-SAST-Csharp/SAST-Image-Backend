namespace Account.Domain.UserEntity
{
    public sealed class Profile
    {
        private Profile() { }

        private string _nickname = " SASTer";
        private string _biography = string.Empty;
        private Uri? _website = null;

        private Uri? _avatar = null;
        private Uri? _header = null;

        internal void UpdateProfile(string nickname, string biography, Uri? website)
        {
            _nickname = nickname;
            _biography = biography;
            _website = website;
        }

        internal void UpdateAvatar(Uri? avatar) => _avatar = avatar;

        internal void UpdateHeader(Uri? header) => _header = header;
    }
}
