using MovieProjectWithUmbraco.Models;
using System.Collections.Generic;
using System.Reflection;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers.Filter
{
    public class FilterController : SurfaceController
    {
        public ActionResult RenderFilterPage()
        {
            var filter = new Models.SearchFilter();
            return PartialView("~/Views/Partials/Filter/_Filter.cshtml", filter);
        }

        [HttpGet]
        public ActionResult SubmitFilterForm(SearchFilter model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var values = new List<string>();
            foreach (PropertyInfo propertyInfo in model.DocumentTypes.GetType().GetProperties())
            {
                if (propertyInfo.CanRead)
                {
                    var value = (bool)propertyInfo.GetValue(model.DocumentTypes);
                    if (value)
                        values.Add(propertyInfo.Name);
                        
                }
            }

            var queryParam = !string.IsNullOrEmpty(model.Query) ? string.Format("?query={0}", model.Query) : string.Empty;
            var typesParam = values.Count > 0 ? string.Format("&types={0}", string.Join(",", values)) : string.Empty;

            return RedirectToCurrentUmbracoPage(string.Format("{0}{1}", queryParam, typesParam));
        }
    }
}