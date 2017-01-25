using System.Linq;
using System.Web;
using System.Web.Mvc;
using Umbraco.Core;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;

namespace MovieProjectWithUmbraco.Extensions
{
    public static class Extensions
    {
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

        public static IHtmlString GetBackgroundImageUrl(this HtmlHelper html)
        {
            var uHelper = new UmbracoHelper(UmbracoContext.Current);

            var rootNodes = uHelper.TypedContentAtRoot();
            var homeNodeByAlias = rootNodes.First(x => x.DocumentTypeAlias == "home");

            var backgroundUrl = homeNodeByAlias.GetCropUrl(propertyAlias: "background", imageCropMode: ImageCropMode.Max);

            return new HtmlString(backgroundUrl);
        }

        private static string GetDefaultAvatarUrl(string alias)
        {
            var uHelper = new UmbracoHelper(UmbracoContext.Current);

            var avatarsFolder = uHelper
                .TypedMediaAtRoot()
                .FirstOrDefault(m => m.Name.InvariantEquals("User Avatars"));

            var defaultImageId = avatarsFolder?.Children().FirstOrDefault(c => c.Name.InvariantEquals("Default")).Id;

            var defaultAvatar = uHelper.TypedMedia(defaultImageId);

            return defaultAvatar.GetCropUrl("image", alias);
        }
    }
}