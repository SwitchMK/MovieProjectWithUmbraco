using MovieProjectWithUmbraco.Models;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using System.Linq;
using System.Collections.Generic;

namespace MovieProjectWithUmbraco.Controllers.Home
{
    public class HomeController : RenderMvcController
    {
        public ActionResult Home(RenderModel model)
        {
            var homeModel = GetHomePageModel(model.Content);

            return base.Index(homeModel);
        }

        private HomePage GetHomePageModel(IPublishedContent page)
        {
            return new HomePage(page)
            {
                TitleImage = page.GetCropUrl("image", "homePageImgCropper"),
                About = page.GetPropertyValue<string>("about"),
                Intro = GetIntro(page),
                News = GetNews(page)
            };
        }

        private IEnumerable<NewsItem> GetNews(IPublishedContent page)
        {
            var newsTicker = page.Children.FirstOrDefault(x => x.DocumentTypeAlias == "newsTicker");

            if (newsTicker != null)
                foreach (var item in newsTicker.Children.OrderByDescending(p => p.CreateDate))
                {
                    yield return new NewsItem(item)
                    {
                        Title = item.GetPropertyValue<string>("title"),
                        ImagePath = item.GetCropUrl("image", "homePageImgCropper"),
                        NewsContent = item.GetPropertyValue<string>("content"),
                        Url = item.Url
                    };
                }
        }

        private Intro GetIntro(IPublishedContent page)
        {
            var document = page.Children.FirstOrDefault(x => x.DocumentTypeAlias == "homePageIntro");

            return new Intro
            {
                ImagePath = document.GetCropUrl("author", "smSzImgCropper"),
                QuoteText = document.GetPropertyValue<string>("quote")
            };
        }
    }
}