using System.Collections.Generic;
using Umbraco.Core.Models;

namespace MovieProjectWithUmbraco.Services.Interfaces
{
    public interface ISearchService
    {
        IEnumerable<IPublishedContent> GetFoundResults(string query);
        IEnumerable<IPublishedContent> SearchFilms(string query);
    }
}
