using MovieProjectWithUmbraco.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers.Distributors
{
    public class DistributorsController : RenderMvcController
    {
        public ActionResult Distributors(RenderModel model)
        {
            var distributorsModel = new DistributorsModel(model.Content);

            distributorsModel.DistributorsInfo = GetDistributors(model.Content);

            return base.Index(distributorsModel);
        }

        private IEnumerable<DistributorInfo> GetDistributors(IPublishedContent page)
        {
            foreach (var distributor in page.Children)
            {
                yield return new DistributorInfo
                {
                    CompanyName = distributor.GetPropertyValue<string>("companyName"),
                    DateOfFoundation = distributor.GetPropertyValue<DateTime>("dateOfFoundation"),
                    ImagePath = distributor.GetCropUrl("image", "smSzImgCropper"),
                    Url = distributor.Url
                };
            }
        }
    }
}