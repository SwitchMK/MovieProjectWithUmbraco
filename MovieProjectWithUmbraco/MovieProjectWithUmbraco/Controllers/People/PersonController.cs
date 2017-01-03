using MovieProjectWithUmbraco.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers
{
    public class PersonController : RenderMvcController
    {
        public ActionResult PersonDetails(RenderModel model)
        {
            var personModel = GetDetailedPersonInformation(model.Content);

            return base.Index(personModel);
        }

        private DetailedPersonInfo GetDetailedPersonInformation(IPublishedContent page)
        {
            return new DetailedPersonInfo(page)
            {
                ShortName = page.GetPropertyValue<string>("shortName"),
                FullName = page.GetPropertyValue<string>("fullName"),
                DateOfBirth = page.GetPropertyValue<DateTime>("dateOfBirth"),
                ImagePath = page.GetCropUrl(propertyAlias: "image", imageCropMode: ImageCropMode.Max),
                Biography = page.GetPropertyValue<string>("biography"),
                Careers = page.GetPropertyValue<string>("careers").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                Countries = page.GetPropertyValue<string>("countries").Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries),
                Filmography = GetLinkResponses("filmography")
            };
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