using MovieProjectWithUmbraco.Models;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers.Filter
{
    public class FilmFilterController : SurfaceController
    {
        private const string FOLDER_FILTER_PATH = "~/Views/Partials/Filter/";

        public ActionResult RenderFilmFilterPage(FilmSearchResponse response)
        {
            var model = ReestablishFilmFilterModel(response);
            return PartialView(FOLDER_FILTER_PATH + "_FilmFilter.cshtml", model);
        }

        private FilmFilter ReestablishFilmFilterModel(FilmSearchResponse response)
        {
            var filmFilterModel = new FilmFilter
            {
                OrderBy = new FilmOrderType[] {
                    new FilmOrderType { IsChecked = false, Name = "YearOfRelease", Label = "Year of release" },
                    new FilmOrderType { IsChecked = true, Name = "TotalRating", Label = "Total rating" },
                    new FilmOrderType { IsChecked = false, Name = "Title", Label = "Title" } }
            };

            ReestablishOrderbyFromResponse(filmFilterModel.OrderBy, response.OrderBy);

            return filmFilterModel;
        }

        private void ReestablishOrderbyFromResponse(IEnumerable<FilmOrderType> orderByCollection, string orderByString)
        {
            if (orderByString != null)
                RefreshOrderByValues(orderByCollection, orderByString);
            else
                RefreshOrderByValues(orderByCollection, orderByCollection.First().Name);
        }

        private void RefreshOrderByValues(IEnumerable<FilmOrderType> collection, string param)
        {
            foreach (var value in collection)
            {
                if (value.Name == param)
                {
                    value.IsChecked = true;
                    continue;
                }

                value.IsChecked = false;
            }
        }
    }
}