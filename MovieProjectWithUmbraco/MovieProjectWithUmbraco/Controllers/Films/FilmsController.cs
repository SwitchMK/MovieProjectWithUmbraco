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

namespace MovieProjectWithUmbraco.Controllers
{
    public class FilmsController : RenderMvcController
    {
        private readonly IFilmRatingRepository _filmRatingRepository;

        public FilmsController(IFilmRatingRepository filmRatingRepository)
        {
            _filmRatingRepository = filmRatingRepository;
        }

        public ActionResult Films(RenderModel model)
        {
            var filmsModel = new FilmsModel(model.Content);

            filmsModel.FilmsInfo = GetFilms(model.Content);

            return base.Index(filmsModel);
        }

        private IEnumerable<FilmInfo> GetFilms(IPublishedContent page)
        {
            long? userId = null;
            var loggedMember = Membership.GetUser();

            if (loggedMember != null)
                userId = (int)loggedMember.ProviderUserKey;

            foreach (var film in page.Children)
            {
                yield return new FilmInfo
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
        }

        private double? GetPersonalRating(long? filmId, long? userId)
        {
            return _filmRatingRepository.GetPersonalRating(filmId, userId);
        }
    }
}