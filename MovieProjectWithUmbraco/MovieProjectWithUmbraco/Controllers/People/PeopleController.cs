using MovieProjectWithUmbraco.Models;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers
{
    public class PeopleController : RenderMvcController
    {
        public ActionResult People(RenderModel model)
        {
            var peopleModel = new PeopleModel(model.Content);

            peopleModel.PeopleInfo = GetPeople(model.Content);

            return base.Index(peopleModel);
        }

        private IEnumerable<PersonInfo> GetPeople(IPublishedContent page)
        {
            foreach (var person in page.Children)
            {
                yield return new PersonInfo
                {
                    ShortName = person.GetPropertyValue<string>("shortName"),
                    ImagePath = person.GetCropUrl("image", "smSzImgCropper"),
                    DateOfBirth = person.GetPropertyValue<DateTime>("dateOfBirth"),
                    Url = person.Url
                };
            }
        }
    }
}