using Examine;
using Examine.LuceneEngine.SearchCriteria;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers.Search
{
    public class SearchController : SurfaceController
    {
        private const int SEARCH_PAGE_ID = 2160;

        public ActionResult RenderSearchResults(Models.Search model)
        {
            var rootNodes = Umbraco.TypedContentAtRoot();
            var homeNodeByAlias = rootNodes.First(x => x.DocumentTypeAlias == "home");

            var query = model.Query.Trim();

            var Searcher = ExamineManager.Instance.SearchProviderCollection["MySearchSearcher"];

            var searchCriteria = Searcher.CreateSearchCriteria(Examine.SearchCriteria.BooleanOperation.Or);

            var operation = searchCriteria
                .Field("nodeName", query).Or()
                .Field("nodeName", query.Fuzzy(0.7f)).Or()
                .Field("cast", query).Or()
                .Field("cast", query.Fuzzy(0.7f)).Or()
                .Field("directors", query).Or()
                .Field("directors", query.Fuzzy(0.7f)).Or()
                .Field("writers", query).Or()
                .Field("writers", query.Fuzzy(0.7f)).Or()
                .Field("producers", query).Or()
                .Field("producers", query.Fuzzy(0.7f)).Or()
                .Field("plot", query).Or()
                .Field("plot", query.Fuzzy(0.7f)).Or()
                .Field("history", query).Or()
                .Field("history", query.Fuzzy(0.7f)).Or()
                .Field("biography", query).Or()
                .Field("biography", query.Fuzzy(0.7f));

            var searchResults = Searcher.Search(operation.Compile());

            var nodes = searchResults.Select(p => Umbraco.TypedContent(p.Fields["id"]));

            var types = model.Types != null ? model.Types.Split(new[] { ',' }, StringSplitOptions.RemoveEmptyEntries).ToList() : new List<string>();

            var finalResults = nodes.Where(p => types.Count() == 0 || (types.Any(x => x.ToLower() == p.DocumentTypeAlias)));

            var param = model.OrderBy ?? "Name";
            var propertyInfo = typeof(IPublishedContent).GetProperty(param);
            var ordered = finalResults.OrderBy(x => propertyInfo.GetValue(x, null));

            return PartialView("~/Views/Partials/Search/_SearchResults.cshtml", ordered);
        }
    }
}