using MovieProjectWithUmbraco.Models;
using MovieProjectWithUmbraco.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers.Search
{
    public class SearchController : SurfaceController
    {
        private const string DEFAULT_ORDER_VALUE = "Name";
        private const string FOLDER_SEARCH_PATH = "~/Views/Partials/Search/";
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public ActionResult RenderSearchResults(SearchResponse model)
        {
            var rootNodes = Umbraco.TypedContentAtRoot();
            var homeNodeByAlias = rootNodes.First(x => x.DocumentTypeAlias == "home");

            if (model.Query == null)
                return PartialView(FOLDER_SEARCH_PATH + "_SearchResults.cshtml");

            var foundResults = _searchService.GetFoundResults(model.Query.Trim());

            var finalResults = foundResults.Where(p => model.Types == null || model.Types.Count() == 0 || model.Types.Any(x => x.ToLower() == p.DocumentTypeAlias));
            var ordered = OrderFoundResults(finalResults, model.OrderBy);

            return PartialView(FOLDER_SEARCH_PATH + "_SearchResults.cshtml", ordered);
        }

        private IEnumerable<IPublishedContent> OrderFoundResults(IEnumerable<IPublishedContent> foundResults, string orderBy)
        {
            var param = orderBy ?? DEFAULT_ORDER_VALUE;
            var propertyInfo = typeof(IPublishedContent).GetProperty(param);

            return foundResults.OrderBy(x => propertyInfo.GetValue(x, null));
        }
    }
}