using MovieProjectWithUmbraco.Models;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers.Filter
{
    public class FilterController : SurfaceController
    {
        public ActionResult RenderFilterPage()
        {
            return PartialView("~/Views/Partials/Filter/_Filter.cshtml", new SearchFilter());
        }

        public ActionResult SubmitFilterForm(SearchFilter model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var queryParam = !string.IsNullOrEmpty(model.Query) ? string.Format("?query={0}", model.Query) : string.Empty;
            var typesParam = model.Types.Length > 0 ? string.Format("&types={0}", string.Join(",", model.Types)) : string.Empty;

            TempData["CurrentModel"] = TempData["CurrentModel"] ?? model;
            TempData.Keep("CurrentModel");

            return RedirectToCurrentUmbracoPage(string.Format("{0}{1}", queryParam, typesParam));
        }
    }
}