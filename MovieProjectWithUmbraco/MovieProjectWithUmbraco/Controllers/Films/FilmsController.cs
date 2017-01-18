using MovieProjectWithUmbraco.Models;
using MovieProjectWithUmbraco.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;
using System.Linq;
using Examine;
using Examine.LuceneEngine.SearchCriteria;

namespace MovieProjectWithUmbraco.Controllers
{
    public class FilmsController : RenderMvcController
    {
        private const float SEARCH_PRECISION = 0.7f;
        private readonly IFilmRatingRepository _filmRatingRepository;

        public FilmsController(IFilmRatingRepository filmRatingRepository)
        {
            _filmRatingRepository = filmRatingRepository;
        }

        public ActionResult Films(RenderModel model, FilmSearchResponse response)
        {
            var filmsModel = new FilmsModel(model.Content);

            IEnumerable<FilmInfo> results = null;

            var query = response.Query;
            var rating = ParseRating(response.Rating);

            if (!string.IsNullOrEmpty(query) && !string.IsNullOrEmpty(query.Trim()))
                results = SearchFilms(query.Trim(), response.StartDate, response.EndDate, rating).ToList();
            else
                results = GetFilms(model.Content, response.StartDate, response.EndDate, rating);

            var param = response.OrderBy ?? "TotalRating";
            var propertyInfo = typeof(FilmInfo).GetProperty(param);

            filmsModel.FilmsInfo = results.OrderByDescending(p => propertyInfo.GetValue(p, null)).ToList();

            return base.Index(filmsModel);
        }

        private double? ParseRating(string rating)
        {
            double parsedRating;
            if (double.TryParse(rating, out parsedRating))
                return parsedRating;
            return null;
        }

        private IEnumerable<FilmInfo> SearchFilms(string query, DateTime? startDate, DateTime? endDate, double? rating)
        {
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
                .Select(p => Umbraco.TypedContent(p.Fields["id"]))
                .Select(p => GetFilmInfo(p, userId))
                .Where(p => FilterFilms(p, startDate, endDate, rating));
        }

        private bool FilterFilms(
            FilmInfo p,
            DateTime? startDate,
            DateTime? endDate,
            double? rating)
        {
            return (rating == null || p.TotalRating == rating) &&
                (startDate == null || p.YearOfRelease >= startDate) && 
                (endDate == null || p.YearOfRelease <= endDate);
        }

        private IEnumerable<FilmInfo> GetFilms(
            IPublishedContent page,
            DateTime? startDate,
            DateTime? endDate,
            double? rating)
        {
            long? userId = GetUserId();

            return page.Children
                .Select(p => GetFilmInfo(p, userId))
                .Where(p => FilterFilms(p, startDate, endDate, rating));
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