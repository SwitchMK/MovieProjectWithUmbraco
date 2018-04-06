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
        private static readonly UmbracoHelper UHelper = new UmbracoHelper(UmbracoContext.Current);

        public static string GetAvatarUrl(this IMember member, string alias)
        {
            if (member == null)
                return null;

            var avatarId = member.GetValue<string>("avatar");

            if (avatarId == null)
                return GetDefaultAvatarUrl(alias);

            var media = UHelper.TypedMedia(avatarId);

            return media == null ? GetDefaultAvatarUrl(alias) : media.GetCropUrl("image", alias);
        }

        public static IHtmlString GetBackgroundImageUrl(this HtmlHelper html)
        {
            var rootNodes = UHelper.TypedContentAtRoot();
            var homeNodeByAlias = rootNodes.First(x => x.DocumentTypeAlias == "home");

            var backgroundUrl = homeNodeByAlias.GetCropUrl(propertyAlias: "background", imageCropMode: ImageCropMode.Max);

            return new HtmlString(backgroundUrl);
        }

        private static string GetDefaultAvatarUrl(string alias)
        {
            var avatarsFolder = UHelper
                .TypedMediaAtRoot()
                .FirstOrDefault(m => m.Name.InvariantEquals("User Avatars"));

            var defaultImageId = avatarsFolder?.Children().FirstOrDefault(c => c.Name.InvariantEquals("Default"))?.Id;

            var defaultAvatar = UHelper.TypedMedia(defaultImageId);

            return defaultAvatar.GetCropUrl("image", alias);
        }
    }
}