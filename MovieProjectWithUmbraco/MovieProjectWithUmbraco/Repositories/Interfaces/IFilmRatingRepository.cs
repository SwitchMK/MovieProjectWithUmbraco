using MovieProjectWithUmbraco.Entities;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace MovieProjectWithUmbraco.Repositories.Interfaces
{
    public interface IFilmRatingRepository
    {
        IEnumerable<FilmRating> GetFilmRatings(long userId);
        double? GetPersonalRating(long? filmId, long? userId);
        double? GetTotalRating(long filmId);
        void AddRating(long filmId, long userId, double rating);
        void UpdateRating(long filmId, long userId, double rating);
    }
}
