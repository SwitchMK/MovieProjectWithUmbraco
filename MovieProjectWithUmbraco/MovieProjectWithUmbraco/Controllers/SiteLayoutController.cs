using MovieProjectWithUmbraco.Models;
using System.Collections.Generic;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using System.Linq;
using Umbraco.Core.Models;
using System.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers
{
    public class SiteLayoutController : SurfaceController
    {
        private const string PARTIALS_LAYOUT_PATH = "~/Views/Partials/SiteLayout/";

        public ActionResult RenderHeader()
        {
            var nav = GetNavigationModelFromDatabase();
            return PartialView(PARTIALS_LAYOUT_PATH + "_Header.cshtml", nav);
        }

        public ActionResult RenderIntro()
        {
            var intro = GetIntro();
            return PartialView(PARTIALS_LAYOUT_PATH + "_Intro.cshtml", intro);
        }

        private Intro GetIntro()
        {
            var document = CurrentPage.Children.Where(x => x.DocumentTypeAlias == "homePageIntro").FirstOrDefault();

            return new Intro
            {
                ImagePath = document.GetCropUrl("author", "smSzImgCropper"),
                QuoteText = document.GetPropertyValue<string>("quote")
            };
        }

        private IEnumerable<NavigationListItem> GetNavigationModelFromDatabase()
        {
            var homePage = CurrentPage.AncestorOrSelf(1).DescendantsOrSelf().Where(x => x.DocumentTypeAlias == "home").FirstOrDefault();

            var nav = new List<NavigationListItem>();
            nav.Add(new NavigationListItem(new NavigationLink(homePage.Url, homePage.Name)));
            nav.AddRange(GetChildNavigationList(homePage));

            return nav;
        }

        private IEnumerable<NavigationListItem> GetChildNavigationList(IPublishedContent page)
        {
            var childPages = page.Children.Where("Visible").Where(x => !x.HasValue("hideFromNavigation") || (x.HasValue("hideFromNavigation") && !x.GetPropertyValue<bool>("hideFromNavigation")));
            if (childPages != null && childPages.Any() && childPages.Count() > 0)
            {
                foreach (var childPage in childPages)
                {
                    var listItem = new NavigationListItem(new NavigationLink(childPage.Url, childPage.Name));
                    listItem.Items = GetChildNavigationList(childPage);

                    yield return listItem;
                }
            }
        }
    }
}