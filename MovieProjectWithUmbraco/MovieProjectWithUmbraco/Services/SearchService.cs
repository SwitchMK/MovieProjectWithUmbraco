using Examine;
using Examine.LuceneEngine.SearchCriteria;
using MovieProjectWithUmbraco.Services.Interfaces;
using System.Collections.Generic;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web;

namespace MovieProjectWithUmbraco.Services
{
    public class SearchService : ISearchService
    {
        private const float SEARCH_PRECISION = 0.7f;

        public IEnumerable<IPublishedContent> GetFoundResults(string query)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

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

            return searchResults.Select(p => umbracoHelper.TypedContent(p.Fields["id"]))
                .OrderByDescending(p => p.CreateDate);
        }
    }
}