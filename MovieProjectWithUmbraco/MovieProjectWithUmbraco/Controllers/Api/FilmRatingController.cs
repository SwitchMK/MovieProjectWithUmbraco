using MovieProjectWithUmbraco.Models.Requests;
using MovieProjectWithUmbraco.Services.Interfaces;
using System.Web.Http;
using System.Web.Security;
using Umbraco.Web.WebApi;

namespace MovieProjectWithUmbraco.Controllers.Api
{
    public class FilmRatingController : UmbracoApiController
    {
        private readonly IFilmRatingService _filmRatingService;

        public FilmRatingController(IFilmRatingService filmRatingService)
        {
            _filmRatingService = filmRatingService;
        }

        [HttpPost]
        public IHttpActionResult RateMovie([FromBody] RateRequest rateRequest)
        {
            var user = Membership.GetUser();

            if (user?.ProviderUserKey == null)
                return Unauthorized();

            var rating = _filmRatingService.RateMovie(rateRequest, (int)user.ProviderUserKey);

            return Ok(rating);
        }
    }
}