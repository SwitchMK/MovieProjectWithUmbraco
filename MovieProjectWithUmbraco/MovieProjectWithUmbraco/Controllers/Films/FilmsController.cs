using MovieProjectWithUmbraco.Models;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using MovieProjectWithUmbraco.Services.Interfaces;

namespace MovieProjectWithUmbraco.Controllers
{
    public class FilmsController : SurfaceController
    {
        private const float SEARCH_PRECISION = 0.7f;
        private const string FILMS_FOLDER_PATH = "~/Views/Partials/Film/";
        private readonly IFilmsService _filmsService;

        public FilmsController(IFilmsService filmsService)
        {
            _filmsService = filmsService;
        }

        public ActionResult RenderFilms(FilmSearchResponse response)
        {
            var filmsModel = new FilmsModel
            {
                FilmsInfo = _filmsService.GetFilms(response)
            };

            return PartialView(FILMS_FOLDER_PATH + "_Films.cshtml", filmsModel);
        }
    }
}