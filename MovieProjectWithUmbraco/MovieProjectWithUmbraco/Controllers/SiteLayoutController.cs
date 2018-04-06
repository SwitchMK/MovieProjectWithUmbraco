using MovieProjectWithUmbraco.Models;
using System.Collections.Generic;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using System.Linq;
using Umbraco.Core.Models;
using System.Web.Mvc;
using System.Web.Security;
using MovieProjectWithUmbraco.Extensions;

namespace MovieProjectWithUmbraco.Controllers
{
    public class SiteLayoutController : SurfaceController
    {
        private const int RecentMovies = 1;
        private const int RecentPeople = 1;
        private const string PartialsLayoutPath = "~/Views/Partials/SiteLayout/";

        public ActionResult RenderHeader()
        {
            var layoutModel = GetNavigationModelFromDatabase();
            return PartialView(PartialsLayoutPath + "_Header.cshtml", layoutModel);
        }

        public ActionResult RenderIntro()
        {
            var intro = GetIntro();
            return PartialView(PartialsLayoutPath + "_Intro.cshtml", intro);
        }

        public ActionResult RenderInfoSection()
        {
            var infoSection = GetInfoSection();
            return PartialView(PartialsLayoutPath + "_InfoSection.cshtml", infoSection);
        }

        public ActionResult RenderSearch()
        {
            return PartialView(PartialsLayoutPath + "_Search.cshtml");
        }

        private Intro GetIntro()
        {
            var document = CurrentPage.Children.FirstOrDefault(x => x.DocumentTypeAlias == "homePageIntro");

            return new Intro
            {
                ImagePath = document.GetCropUrl("author", "smSzImgCropper"),
                QuoteText = document.GetPropertyValue<string>("quote")
            };
        }

        private InfoSection GetInfoSection()
        {
            var rootNodes = Umbraco.TypedContentAtRoot();
            var homeNodeByAlias = rootNodes.First(x => x.DocumentTypeAlias == "home");
            
            return new InfoSection
            {
                RecentMovies = GetRecentlyAddedFilms(homeNodeByAlias),
                RecentPeople = GetRecentlyAddedPeople(homeNodeByAlias)
            };
        }

        private IEnumerable<InfoItem> GetRecentlyAddedFilms(IPublishedContent page)
        {
            var filmsPage = page.Children.FirstOrDefault(x => x.DocumentTypeAlias == "films");

            foreach (var item in filmsPage.Children.OrderByDescending(p => p.CreateDate).Take(RecentMovies))
            {
                yield return new InfoItem()
                {
                    Title = item.GetPropertyValue<string>("title"),
                    ImagePath = item.GetCropUrl("image", "homeItemSzImgCropper"),
                    Url = item.Url
                };
            }
        }

        private IEnumerable<InfoItem> GetRecentlyAddedPeople(IPublishedContent page)
        {
            var peoplePage = page.Children.FirstOrDefault(x => x.DocumentTypeAlias == "people");

            foreach (var item in peoplePage.Children.OrderByDescending(p => p.CreateDate).Take(RecentPeople))
            {
                yield return new InfoItem()
                {
                    Title = item.GetPropertyValue<string>("shortName"),
                    ImagePath = item.GetCropUrl("image", "homeItemSzImgCropper"),
                    Url = item.Url
                };
            }
        }

        private Layout GetNavigationModelFromDatabase()
        {
            var homePage = CurrentPage.AncestorOrSelf(1).DescendantsOrSelf().FirstOrDefault(x => x.DocumentTypeAlias == "home");

            var nav = new List<NavigationListItem>
            {
                new NavigationListItem(new NavigationLink(homePage.Url, homePage.Name))
            };
            nav.AddRange(GetChildNavigationList(homePage));

            IMember member = null;
            var user = Membership.GetUser();

            if (user != null)
                member = Services.MemberService.GetByUsername(user.UserName);

            var layoutModel = new Layout
            {
                Links = nav,
                UserImage = member?.GetAvatarUrl("avatarSmallSize"),
                UserName = member?.Username
            };

            return layoutModel;
        }

        private IEnumerable<NavigationListItem> GetChildNavigationList(IPublishedContent page)
        {
            var childPages = page.Children.Where("Visible").Where(x => !x.HasValue("hideFromNavigation") || 
                x.HasValue("hideFromNavigation") && !x.GetPropertyValue<bool>("hideFromNavigation"));

            if (childPages.Any())
            {
                foreach (var childPage in childPages)
                {
                    var listItem =
                        new NavigationListItem(new NavigationLink(childPage.Url, childPage.Name))
                        {
                            Items = GetChildNavigationList(childPage)
                        };

                    yield return listItem;
                }
            }
        }
    }
}