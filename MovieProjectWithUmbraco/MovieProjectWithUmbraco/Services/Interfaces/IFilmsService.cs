using MovieProjectWithUmbraco.Models;
using System.Collections.Generic;

namespace MovieProjectWithUmbraco.Services.Interfaces
{
    public interface IFilmsService
    {
        IEnumerable<FilmInfo> GetFilms(FilmSearchResponse response);
    }
}
