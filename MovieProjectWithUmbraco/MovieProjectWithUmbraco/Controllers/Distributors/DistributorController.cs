using MovieProjectWithUmbraco.Models;
using Newtonsoft.Json.Linq;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers.Distributors
{
    public class DistributorController : RenderMvcController
    {
        public ActionResult DistributorDetails(RenderModel model)
        {
            var distributorModel = GetDetailedDistributorInformation(model.Content);

            return Index(distributorModel);
        }

        private DetailedDistributorInfo GetDetailedDistributorInformation(IPublishedContent page)
        {
            return new DetailedDistributorInfo(page)
            {
                CompanyName = page.GetPropertyValue<string>("companyName"),
                DateOfFoundation = page.GetPropertyValue<DateTime>("dateOfFoundation"),
                ImagePath = page.GetCropUrl(propertyAlias: "image", imageCropMode: ImageCropMode.Max),
                History = page.GetPropertyValue<string>("history"),
                Founders = GetLinkResponses("founders"),
                DistributedMovies = GetLinkResponses("distributedMovies")
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