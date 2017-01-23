using MovieProjectWithUmbraco.Models;
using System.Collections.Generic;
using Umbraco.Core.Models;

namespace MovieProjectWithUmbraco.Services.Interfaces
{
    public interface IFilmsService
    {
        IEnumerable<FilmInfo> GetFilms(FilmSearchResponse response);
    }
}
