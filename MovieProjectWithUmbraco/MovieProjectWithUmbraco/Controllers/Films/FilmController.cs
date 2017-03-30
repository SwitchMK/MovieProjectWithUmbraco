using MovieProjectWithUmbraco.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Text.RegularExpressions;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers
{
    public class FilmController : RenderMvcController
    {
        public ActionResult FilmDetails(RenderModel model)
        {
            var filmDetailsModel = GetDetailedFilmInformation(model.Content);

            return base.Index(filmDetailsModel);
        }

        private DetailedFilmInfo GetDetailedFilmInformation(IPublishedContent page)
        {
            return new DetailedFilmInfo(page)
            {
                Title = page.GetPropertyValue<string>("title"),
                YearOfRelease = page.GetPropertyValue<DateTime>("yearOfRelease"),
                ImagePath = page.GetCropUrl(propertyAlias: "image", imageCropMode: ImageCropMode.Max),
                BoxOffice = page.GetPropertyValue<decimal>("boxOffice"),
                Budget = page.GetPropertyValue<decimal>("budget"),
                Plot = page.GetPropertyValue<string>("plot"),
                Trailer = GetTrailerUrl(page.GetPropertyValue<string>("trailer")),
                Countries = page.GetPropertyValue<string>("countries").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                Distributors = GetLinkResponses("distributors"),
                Stars = GetLinkResponses("cast"),
                Directors = GetLinkResponses("directors"),
                Producers = GetLinkResponses("producers"),
                Writers = GetLinkResponses("writers"),
                Composers = GetLinkResponses("composers")
            };
        }

        private string GetTrailerUrl(string html)
        {
            var matchdec = Regex.Match(html, @"\ssrc=""\b(\S*)\b", RegexOptions.IgnorePatternWhitespace | RegexOptions.IgnoreCase);

            if (matchdec.Success)
            {
                if (matchdec.Groups.Count > 1)
                {
                    return matchdec.Groups[1].Value;
                }
            }

            return null;
        }

        private IEnumerable<LinkResponse> GetLinkResponses(string alias)
        {
            if (CurrentPage.HasValue(alias))
            {
                foreach (var item in CurrentPage.GetPropertyValue<JArray>(alias))
                {
                    var linkCaption = item.Value<string>("caption");
                    var linkUrl = (item.Value<bool>("isInternal")) ? Umbraco.NiceUrl(item.Value<int>("internal")) : item.Value<string>("link");
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
    }
}