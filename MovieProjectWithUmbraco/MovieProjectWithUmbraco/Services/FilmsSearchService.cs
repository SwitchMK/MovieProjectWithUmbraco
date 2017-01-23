using MovieProjectWithUmbraco.Models;
using MovieProjectWithUmbraco.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;
using System.Linq;
using System.Web.Security;
using Examine.LuceneEngine.SearchCriteria;
using Examine;
using MovieProjectWithUmbraco.Services.Interfaces;

namespace MovieProjectWithUmbraco.Services
{
    public class FilmsSearchService : IFilmsSearchService
    {
        private const float SEARCH_PRECISION = 0.7f;
        private const int FILMS_PAGE_ID = 1089;
        private readonly IFilmRatingRepository _filmRatingRepository;

        public FilmsSearchService(IFilmRatingRepository filmRatingRepository)
        {
            _filmRatingRepository = filmRatingRepository;
        }

        public IEnumerable<FilmInfo> SearchFilms(
            string query,
            DateTime? startDate,
            DateTime? endDate,
            double? startRating,
            double? endRating)
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            var Searcher = ExamineManager.Instance.SearchProviderCollection["MyFilmSearchSearcher"];
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
                .Field("distributors", query).Or()
                .Field("distributors", query.Fuzzy(SEARCH_PRECISION)).Or()
                .Field("plot", query).Or()
                .Field("plot", query.Fuzzy(SEARCH_PRECISION));

            var searchResults = Searcher.Search(operation.Compile());

            long? userId = GetUserId();

            return searchResults
                .Select(p => umbracoHelper.TypedContent(p.Fields["id"]))
                .Select(p => GetFilmInfo(p, userId))
                .Where(p => FilterFilms(p, startDate, endDate, startRating, endRating));
        }

        public IEnumerable<FilmInfo> GetFilms(
            DateTime? startDate,
            DateTime? endDate,
            double? startRating,
            double? endRating)
        {
            long? userId = GetUserId();

            return GetFilmsContent().Children
                .Select(p => GetFilmInfo(p, userId))
                .Where(p => FilterFilms(p, startDate, endDate, startRating, endRating));
        }

        private bool FilterFilms(
            FilmInfo p,
            DateTime? startDate,
            DateTime? endDate,
            double? startRating,
            double? endRating)
        {
            return (startRating == null || p.TotalRating >= startRating) &&
                (endRating == null || p.TotalRating <= endRating) &&
                (startDate == null || p.YearOfRelease >= startDate) &&
                (endDate == null || p.YearOfRelease <= endDate);
        }

        private IPublishedContent GetFilmsContent()
        {
            var umbracoHelper = new UmbracoHelper(UmbracoContext.Current);

            return umbracoHelper.TypedContent(FILMS_PAGE_ID);
        }

        private long? GetUserId()
        {
            var loggedMember = Membership.GetUser();

            if (loggedMember != null)
                return (int)loggedMember.ProviderUserKey;

            return null;
        }

        private FilmInfo GetFilmInfo(IPublishedContent film, long? userId)
        {
            return new FilmInfo
            {
                Id = film.Id,
                Title = film.GetPropertyValue<string>("title"),
                YearOfRelease = film.GetPropertyValue<DateTime>("yearOfRelease"),
                ImagePath = film.GetCropUrl("image", "smSzImgCropper"),
                Url = film.Url,
                PersonalRating = GetPersonalRating(film.Id, userId),
                TotalRating = _filmRatingRepository.GetTotalRating(film.Id)
            };
        }

        private double? GetPersonalRating(long? filmId, long? userId)
        {
            return _filmRatingRepository.GetPersonalRating(filmId, userId);
        }
    }
}