using Umbraco.Core.Models;
using Umbraco.Web;

namespace MovieProjectWithUmbraco.Extensions
{
    public static class Extensions
    {
        private const int DEFAULT_AVATAR_ID = 3192;

        public static string GetAvatarUrl(this IMember member, string alias)
        {
            var uHelper = new UmbracoHelper(UmbracoContext.Current);

            if (member == null)
                return null;

            var avatarId = member.GetValue<string>("avatar");

            if (avatarId == null)
                return GetDefaultAvatarUrl(alias);

            var media = uHelper.TypedMedia(avatarId);

            if (media == null)
                return GetDefaultAvatarUrl(alias);

            return media.GetCropUrl("image", alias);
        }

        private static string GetDefaultAvatarUrl(string alias)
        {
            var uHelper = new UmbracoHelper(UmbracoContext.Current);

            var defaultAvatar = uHelper.TypedMedia(DEFAULT_AVATAR_ID);

            return defaultAvatar.GetCropUrl("image", alias);
        }
    }
}