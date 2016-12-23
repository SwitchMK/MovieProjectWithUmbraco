using MovieProjectWithUmbraco.Models;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers.News
{
    public class NewsController : RenderMvcController
    {
        public ActionResult News(RenderModel model)
        {
            var homeModel = GetNewsItem(model.Content);

            return base.Index(homeModel);
        }

        private NewsItem GetNewsItem(IPublishedContent page)
        {
            return new NewsItem(page)
            {
                Title = page.GetPropertyValue<string>("title"),
                ImagePath = page.GetCropUrl("image", "homePageImgCropper"),
                NewsContent = page.GetPropertyValue<string>("content")
            };
        }
    }
}