using MovieProjectWithUmbraco.Models.Requests;

namespace MovieProjectWithUmbraco.Services.Interfaces
{
    public interface IFilmRatingService
    {
        double? RateMovie(RateRequest rateRequest, long userId);
    }
}
