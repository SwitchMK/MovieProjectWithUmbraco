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
        private const string DefaultOrderValue = "Name";
        private const string FolderSearchPath = "~/Views/Partials/Search/";
        private readonly ISearchService _searchService;

        public SearchController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        public ActionResult RenderSearchResults(SearchResponse model)
        {
            if (model.Query == null)
                return PartialView(FolderSearchPath + "_SearchResults.cshtml");

            var foundResults = _searchService.GetFoundResults(model.Query.Trim());

            var finalResults = foundResults.Where(p => model.Types == null || !model.Types.Any() || model.Types.Any(x => x.ToLower() == p.DocumentTypeAlias));
            var ordered = OrderFoundResults(finalResults, model.OrderBy);

            return PartialView(FolderSearchPath + "_SearchResults.cshtml", ordered);
        }

        private IEnumerable<IPublishedContent> OrderFoundResults(IEnumerable<IPublishedContent> foundResults, string orderBy)
        {
            var param = orderBy ?? DefaultOrderValue;
            var propertyInfo = typeof(IPublishedContent).GetProperty(param);

            return foundResults.OrderBy(x => propertyInfo.GetValue(x, null));
        }
    }
}