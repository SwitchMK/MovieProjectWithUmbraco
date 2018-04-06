using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using MovieProjectWithUmbraco.Models;
using Newtonsoft.Json.Linq;
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

        public static IEnumerable<LinkResponse> GetLinkResponses(this IPublishedContent content, string alias)
        {
            if (content.HasValue(alias))
            {
                foreach (var item in content.GetPropertyValue<JArray>(alias))
                {
                    var linkCaption = item.Value<string>("caption");
                    var linkUrl = (item.Value<bool>("isInternal")) ? UHelper.NiceUrl(item.Value<int>("internal")) : item.Value<string>("link");
                    var linkTarget = item.Value<bool>("newWindow") ? "_blank" : null;

                    yield return new LinkResponse
                    {
                        Caption = linkCaption,
                        Url = linkUrl,
                        Target = linkTarget
                    };
                }
            }
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