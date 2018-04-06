using MovieProjectWithUmbraco.Models;
using MovieProjectWithUmbraco.Extensions;
using System;
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

            return Index(personModel);
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
                Filmography = CurrentPage.GetLinkResponses("filmography")
            };
        }
    }
}