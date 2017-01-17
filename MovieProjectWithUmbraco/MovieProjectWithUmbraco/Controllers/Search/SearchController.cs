using Examine;
using Examine.LuceneEngine.SearchCriteria;
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
        private const float SEARCH_PRECISION = 0.7f;

        public ActionResult RenderSearchResults(Models.Search model)
        {
            var rootNodes = Umbraco.TypedContentAtRoot();
            var homeNodeByAlias = rootNodes.First(x => x.DocumentTypeAlias == "home");

            if (model.Query == null)
                return PartialView(FOLDER_SEARCH_PATH + "_SearchResults.cshtml");

            var foundResults = GetFoundResults(model.Query.Trim());

            var finalResults = foundResults.Where(p => model.Types == null || model.Types.Count() == 0 || (model.Types.Any(x => x.ToLower() == p.DocumentTypeAlias)));

            var param = model.OrderBy ?? DEFAULT_ORDER_VALUE;
            var propertyInfo = typeof(IPublishedContent).GetProperty(param);
            var ordered = finalResults.OrderBy(x => propertyInfo.GetValue(x, null));

            return PartialView(FOLDER_SEARCH_PATH + "_SearchResults.cshtml", ordered);
        }

        private IEnumerable<IPublishedContent> GetFoundResults(string query)
        {
            var Searcher = ExamineManager.Instance.SearchProviderCollection["MySearchSearcher"];
            var searchCriteria = Searcher.CreateSearchCriteria(Examine.SearchCriteria.BooleanOperation.Or);

            var operation = searchCriteria
                .Field("nodeName", query).Or()
                .Field("nodeName", query.Fuzzy(SEARCH_PRECISION)).Or()
                .Field("cast", query).Or()
                .Field("cast", query.Fuzzy(SEARCH_PRECISION)).Or()
                .Field("directors", query).Or()
                .Field("directors", query.Fuzzy(SEARCH_PRECISION)).Or()
                .Field("writers", query).Or()
                .Field("writers", query.Fuzzy(SEARCH_PRECISION)).Or()
                .Field("producers", query).Or()
                .Field("producers", query.Fuzzy(SEARCH_PRECISION)).Or()
                .Field("title", query).Or()
                .Field("title", query.Fuzzy(SEARCH_PRECISION)).Or()
                .Field("content", query).Or()
                .Field("content", query.Fuzzy(SEARCH_PRECISION)).Or()
                .Field("distributors", query).Or()
                .Field("distributors", query.Fuzzy(SEARCH_PRECISION)).Or()
                .Field("plot", query).Or()
                .Field("plot", query.Fuzzy(SEARCH_PRECISION)).Or()
                .Field("history", query).Or()
                .Field("history", query.Fuzzy(SEARCH_PRECISION)).Or()
                .Field("biography", query).Or()
                .Field("biography", query.Fuzzy(SEARCH_PRECISION)).Or()
                .Field("distributedMovies", query).Or()
                .Field("distributedMovies", query.Fuzzy(SEARCH_PRECISION));

            var searchResults = Searcher.Search(operation.Compile());

            return searchResults.Select(p => Umbraco.TypedContent(p.Fields["id"]));
        }
    }
}