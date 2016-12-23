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
                RecentDistributors = GetRecentlyAddedDistributors(page).Take(3),
                RecentMovies = GetRecentlyAddedFilms(page).Take(3),
                RecentPeople = GetRecentlyAddedPeople(page).Take(3)
            };
        }

        private IEnumerable<HomePageItem> GetRecentlyAddedFilms(IPublishedContent page)
        {
            var filmsPage = page.Children.Where(x => x.DocumentTypeAlias == "films").FirstOrDefault();
            foreach (var item in filmsPage.Children)
            {
                yield return new HomePageItem()
                {
                    Title = item.GetPropertyValue<string>("title"),
                    ImagePath = item.GetCropUrl("image", "homeItemSzImgCropper"),
                    Url = item.Url
                };
            }
        }

        private IEnumerable<HomePageItem> GetRecentlyAddedPeople(IPublishedContent page)
        {
            var peoplePage = page.Children.Where(x => x.DocumentTypeAlias == "people").FirstOrDefault();
            foreach (var item in peoplePage.Children)
            {
                yield return new HomePageItem()
                {
                    Title = item.GetPropertyValue<string>("shortName"),
                    ImagePath = item.GetCropUrl("image", "homeItemSzImgCropper"),
                    Url = item.Url
                };
            }
        }

        private IEnumerable<HomePageItem> GetRecentlyAddedDistributors(IPublishedContent page)
        {
            var distributorsPage = page.Children.Where(x => x.DocumentTypeAlias == "distributors").FirstOrDefault();
            foreach (var item in distributorsPage.Children)
            {
                yield return new HomePageItem()
                {
                    Title = item.GetPropertyValue<string>("companyName"),
                    ImagePath = item.GetCropUrl("image", "homeItemSzImgCropper"),
                    Url = item.Url
                };
            }
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