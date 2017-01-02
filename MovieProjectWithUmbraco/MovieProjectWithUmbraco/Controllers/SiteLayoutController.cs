﻿using MovieProjectWithUmbraco.Models;
using System.Collections.Generic;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using System.Linq;
using Umbraco.Core.Models;
using System.Web.Mvc;
using System;

namespace MovieProjectWithUmbraco.Controllers
{
    public class SiteLayoutController : SurfaceController
    {
        private const int RECENT_MOVIES = 1;
        private const int RECENT_PEOPLE = 1;
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

        public ActionResult RenderInfoSection()
        {
            var infoSection = GetInfoSection();
            return PartialView(PARTIALS_LAYOUT_PATH + "_InfoSection.cshtml", infoSection);
        }

        public ActionResult RenderSearch()
        {
            return PartialView(PARTIALS_LAYOUT_PATH + "_Search.cshtml");
        }

        [HttpPost]
        public ActionResult RenderSearchResults(Search model)
        {
            return Redirect(Uri.EscapeUriString(string.Format("/search?query={0}", model.Query)));
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

        private InfoSection GetInfoSection()
        {
            var rootNodes = Umbraco.TypedContentAtRoot();
            var homeNodeByAlias = rootNodes.First(x => x.DocumentTypeAlias == "home");
            
            return new InfoSection
            {
                RecentMovies = GetRecentlyAddedFilms(homeNodeByAlias).Take(RECENT_MOVIES),
                RecentPeople = GetRecentlyAddedPeople(homeNodeByAlias).Take(RECENT_PEOPLE)
            };
        }

        private IEnumerable<InfoItem> GetRecentlyAddedFilms(IPublishedContent page)
        {
            var filmsPage = page.Children.Where(x => x.DocumentTypeAlias == "films").FirstOrDefault();

            foreach (var item in filmsPage.Children.OrderByDescending(p => p.CreateDate))
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
            var peoplePage = page.Children.Where(x => x.DocumentTypeAlias == "people").FirstOrDefault();

            foreach (var item in peoplePage.Children.OrderByDescending(p => p.CreateDate))
            {
                yield return new InfoItem()
                {
                    Title = item.GetPropertyValue<string>("shortName"),
                    ImagePath = item.GetCropUrl("image", "homeItemSzImgCropper"),
                    Url = item.Url
                };
            }
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