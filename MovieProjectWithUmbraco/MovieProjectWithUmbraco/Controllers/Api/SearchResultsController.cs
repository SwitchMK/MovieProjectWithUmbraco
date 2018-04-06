using System.Collections.Generic;
using System.Linq;
using Umbraco.Web.WebApi;
using System.Web.Http;
using MovieProjectWithUmbraco.Models;
using Umbraco.Web;
using MovieProjectWithUmbraco.Services.Interfaces;

namespace MovieProjectWithUmbraco.Controllers.Api
{
    public class SearchResultsController : UmbracoApiController
    {
        private const int AmountOfTakenItems = 5;
        private readonly ISearchService _searchService;

        public SearchResultsController(ISearchService searchService)
        {
            _searchService = searchService;
        }

        [HttpPost]
        public IEnumerable<AjaxSearchResponse> GetFoundResults([FromBody] SearchRequest searchRequest)
        {
            return _searchService.GetFoundResults(searchRequest.Query)
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new AjaxSearchResponse
                {
                    Name = p.Name,
                    Url = p.Url,
                    ImagePath = p.GetCropUrl("image", "smSzImgCropper")
                })
                .Take(AmountOfTakenItems);
        }

        [HttpPost]
        public IEnumerable<AjaxSearchResponse> SearchFilms([FromBody] SearchRequest searchRequest)
        {
            if (string.IsNullOrWhiteSpace(searchRequest.Query))
                return new List<AjaxSearchResponse>(); 

            var results = _searchService.SearchFilms(searchRequest.Query)
                .OrderByDescending(p => p.CreateDate)
                .Select(p => new AjaxSearchResponse
                {
                    Name = p.Name,
                    Url = p.Url,
                    ImagePath = p.GetCropUrl("image", "smSzImgCropper")
                })
                .Take(AmountOfTakenItems);

            return results;
        }
    }
}