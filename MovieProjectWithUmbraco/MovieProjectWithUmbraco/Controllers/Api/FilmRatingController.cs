using MovieProjectWithUmbraco.Models.Requests;
using MovieProjectWithUmbraco.Repositories.Interfaces;
using System.Web.Http;
using System.Web.Security;
using Umbraco.Web.WebApi;

namespace MovieProjectWithUmbraco.Controllers.Api
{
    public class FilmRatingController : UmbracoApiController
    {
        private readonly IFilmRatingRepository _filmRatingRepository;

        public FilmRatingController(IFilmRatingRepository filmRatingRepository)
        {
            _filmRatingRepository = filmRatingRepository;
        }

        [HttpPost]
        public void RateMovie([FromBody] RateRequest rateRequest)
        {
            var user = Membership.GetUser();

            if (user != null)
                RateMovie(rateRequest, (int)user.ProviderUserKey);
        }

        private void RateMovie(RateRequest rateRequest, long userId)
        {
            var personalRating = _filmRatingRepository.GetPersonalRating(rateRequest.FilmId, userId);

            if (personalRating != null)
                _filmRatingRepository.UpdateRating(rateRequest.FilmId, userId, rateRequest.Rating);
            else
                _filmRatingRepository.AddRating(rateRequest.FilmId, userId, rateRequest.Rating);
        }
    }
}