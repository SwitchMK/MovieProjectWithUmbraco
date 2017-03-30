using MovieProjectWithUmbraco.Models.Requests;
using MovieProjectWithUmbraco.Repositories.Interfaces;
using MovieProjectWithUmbraco.Services.Interfaces;

namespace MovieProjectWithUmbraco.Services
{
    public class FilmRatingService : IFilmRatingService
    {
        private readonly IFilmRatingRepository _filmRatingRepository;

        public FilmRatingService(IFilmRatingRepository filmRatingRepository)
        {
            _filmRatingRepository = filmRatingRepository;
        }

        public double? RateMovie(RateRequest rateRequest, long userId)
        {
            var personalRating = _filmRatingRepository.GetPersonalRating(rateRequest.FilmId, userId);

            if (personalRating != null)
                _filmRatingRepository.UpdateRating(rateRequest.FilmId, userId, rateRequest.Rating);
            else
                _filmRatingRepository.AddRating(rateRequest.FilmId, userId, rateRequest.Rating);

            return _filmRatingRepository.GetTotalRating(rateRequest.FilmId);
        }
    }
}