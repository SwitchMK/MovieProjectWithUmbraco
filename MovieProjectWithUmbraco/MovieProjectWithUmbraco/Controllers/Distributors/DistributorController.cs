using MovieProjectWithUmbraco.Models;
using MovieProjectWithUmbraco.Extensions;
using System;
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
                Founders = CurrentPage.GetLinkResponses("founders"),
                DistributedMovies = CurrentPage.GetLinkResponses("distributedMovies")
            };
        }
    }
}