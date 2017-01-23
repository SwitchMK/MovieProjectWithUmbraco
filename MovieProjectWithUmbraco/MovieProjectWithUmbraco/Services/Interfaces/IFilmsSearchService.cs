using MovieProjectWithUmbraco.Models;
using System;
using System.Collections.Generic;

namespace MovieProjectWithUmbraco.Services.Interfaces
{
    public interface IFilmsSearchService
    {
        IEnumerable<FilmInfo> SearchFilms(string query, DateTime? startDate, DateTime? endDate, double? startRating, double? endRating);
        IEnumerable<FilmInfo> GetFilms(DateTime? startDate, DateTime? endDate, double? startRating, double? endRating);
    }
}
