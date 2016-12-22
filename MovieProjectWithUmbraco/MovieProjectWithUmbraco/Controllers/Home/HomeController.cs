using MovieProjectWithUmbraco.Models;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using System.Linq;

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
                Intro = GetIntro(page)
            };
        }

        private Intro GetIntro(IPublishedContent page)
        {
            var document = page.Children.Where(x => x.DocumentTypeAlias == "homePageIntro").FirstOrDefault();

            return new Intro
            {
                ImagePath = document.GetCropUrl("author", "smSzImgCropper"),
                QuoteText = document.GetPropertyValue<string>("quote")
            };
        }
    }
}