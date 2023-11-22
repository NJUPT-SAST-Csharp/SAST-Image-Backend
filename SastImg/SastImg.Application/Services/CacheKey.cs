namespace SastImg.Application.Services
{
    internal static class CacheKey
    {
        /// <summary>
        /// Hashtable containing the pages of all accessible albums in anonymous access level.
        /// </summary>
        public const string AnonymousAlbums = "AnonymousAlbums";

        /// <summary>
        /// Hashtable containing all accessible albums of specific user in anonymous access level.
        /// </summary>
        public const string AnonymousUserAlbums = "AnonymousUserAlbums";
    }
}
