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
        private const float SearchPrecision = 0.7f;

        public IEnumerable<IPublishedContent> GetFoundResults(string query)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var searcher = ExamineManager.Instance.SearchProviderCollection["MySearchSearcher"];
            var searchCriteria = searcher.CreateSearchCriteria(Examine.SearchCriteria.BooleanOperation.Or);

            var operation = searchCriteria
                .Field("nodeName", query).Or()
                .Field("nodeName", query.Fuzzy(SearchPrecision)).Or()
                .Field("cast", query).Or()
                .Field("cast", query.Fuzzy(SearchPrecision)).Or()
                .Field("directors", query).Or()
                .Field("directors", query.Fuzzy(SearchPrecision)).Or()
                .Field("writers", query).Or()
                .Field("writers", query.Fuzzy(SearchPrecision)).Or()
                .Field("producers", query).Or()
                .Field("producers", query.Fuzzy(SearchPrecision)).Or()
                .Field("title", query).Or()
                .Field("title", query.Fuzzy(SearchPrecision)).Or()
                .Field("content", query).Or()
                .Field("content", query.Fuzzy(SearchPrecision)).Or()
                .Field("distributors", query).Or()
                .Field("distributors", query.Fuzzy(SearchPrecision)).Or()
                .Field("plot", query).Or()
                .Field("plot", query.Fuzzy(SearchPrecision)).Or()
                .Field("history", query).Or()
                .Field("history", query.Fuzzy(SearchPrecision)).Or()
                .Field("biography", query).Or()
                .Field("biography", query.Fuzzy(SearchPrecision)).Or()
                .Field("distributedMovies", query).Or()
                .Field("distributedMovies", query.Fuzzy(SearchPrecision));

            var searchResults = searcher.Search(operation.Compile());

            return searchResults.Select(p => umbracoHelper.TypedContent(p.Fields["id"]))
                .OrderByDescending(p => p.CreateDate);
        }

        public IEnumerable<IPublishedContent> SearchFilms(string query)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var searcher = ExamineManager.Instance.SearchProviderCollection["MyFilmSearchSearcher"];
            var searchCriteria = searcher.CreateSearchCriteria(Examine.SearchCriteria.BooleanOperation.Or);

            var operation = searchCriteria
                .Field("nodeName", query).Or()
                .Field("nodeName", query.Fuzzy(SearchPrecision)).Or()
                .Field("cast", query).Or()
                .Field("cast", query.Fuzzy(SearchPrecision)).Or()
                .Field("directors", query).Or()
                .Field("directors", query.Fuzzy(SearchPrecision)).Or()
                .Field("writers", query).Or()
                .Field("writers", query.Fuzzy(SearchPrecision)).Or()
                .Field("producers", query).Or()
                .Field("producers", query.Fuzzy(SearchPrecision)).Or()
                .Field("title", query).Or()
                .Field("title", query.Fuzzy(SearchPrecision)).Or()
                .Field("distributors", query).Or()
                .Field("distributors", query.Fuzzy(SearchPrecision)).Or()
                .Field("plot", query).Or()
                .Field("plot", query.Fuzzy(SearchPrecision));

            var searchResults = searcher.Search(operation.Compile());

            return searchResults.Select(p => umbracoHelper.TypedContent(p.Fields["id"]))
                .OrderByDescending(p => p.CreateDate);
        }
    }
}