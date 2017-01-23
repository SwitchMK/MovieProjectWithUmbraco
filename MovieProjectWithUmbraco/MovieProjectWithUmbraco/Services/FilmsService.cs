using System.Collections.Generic;
using MovieProjectWithUmbraco.Models;
using MovieProjectWithUmbraco.Services.Interfaces;
using System.Linq;
using Examine.LuceneEngine.SearchCriteria;

namespace MovieProjectWithUmbraco.Services
{
    public class FilmsService : IFilmsService
    {
        private readonly IFilmsSearchService _filmsWrapperService;

        public FilmsService(IFilmsSearchService filmsWrapperService)
        {
            _filmsWrapperService = filmsWrapperService;
        }

        public IEnumerable<FilmInfo> GetFilms(FilmSearchResponse response)
        {
            var query = response.Query;
            var startRating = ParseRating(response.StartRating);
            var endRating = ParseRating(response.EndRating);

            var param = response.OrderBy ?? "TotalRating";
            var propertyInfo = typeof(FilmInfo).GetProperty(param);

            if (!string.IsNullOrWhiteSpace(query))
                return _filmsWrapperService.SearchFilms(query.Trim(), response.StartDate, response.EndDate, startRating, endRating)
                    .OrderByDescending(p => propertyInfo.GetValue(p, null));
            else
                return _filmsWrapperService.GetFilms(response.StartDate, response.EndDate, startRating, endRating)
                    .OrderByDescending(p => propertyInfo.GetValue(p, null));
        }

        private double? ParseRating(string rating)
        {
            double parsedRating;
            if (double.TryParse(rating, out parsedRating))
                return parsedRating;

            return null;
        }
    }
}