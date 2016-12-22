using System.Threading.Tasks;

namespace MovieProjectWithUmbraco.Repositories.Interfaces
{
    public interface IFilmRatingRepository
    {
        double? GetPersonalRating(long? filmId, long? userId);
        double? GetTotalRating(long filmId);
        void AddRating(long filmId, long userId, double rating);
        void UpdateRating(long filmId, long userId, double rating);
    }
}
