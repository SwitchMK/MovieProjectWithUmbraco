using MovieProjectWithUmbraco.Models;
using System.Web.Mvc;
using Umbraco.Web.Mvc;
using MovieProjectWithUmbraco.Services.Interfaces;

namespace MovieProjectWithUmbraco.Controllers
{
    public class FilmsController : SurfaceController
    {
        private const string FilmsFolderPath = "~/Views/Partials/Film/";
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

            return PartialView(FilmsFolderPath + "_Films.cshtml", filmsModel);
        }
    }
}