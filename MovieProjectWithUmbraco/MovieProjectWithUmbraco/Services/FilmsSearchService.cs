using MovieProjectWithUmbraco.Models;
using MovieProjectWithUmbraco.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web;
using System.Linq;
using System.Web.Security;
using MovieProjectWithUmbraco.Services.Interfaces;

namespace MovieProjectWithUmbraco.Services
{
    public class FilmsSearchService : IFilmsSearchService
    {
        private const float SEARCH_PRECISION = 0.7f;
        private readonly IFilmRatingRepository _filmRatingRepository;
        private readonly ISearchService _searchService;

        public FilmsSearchService(
            IFilmRatingRepository filmRatingRepository,
            ISearchService searchService)
        {
            _filmRatingRepository = filmRatingRepository;
            _searchService = searchService;
        }

        public IEnumerable<FilmInfo> SearchFilms(
            string query,
            DateTime? startDate,
            DateTime? endDate,
            double? startRating,
            double? endRating)
        {
            var searchResults = _searchService.SearchFilms(query);

            long? userId = GetUserId();

            return searchResults
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

            var rootNodes = umbracoHelper.TypedContentAtRoot();
            var homeNode = rootNodes.First(x => x.DocumentTypeAlias == "home");

            return homeNode.Descendant("films");
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