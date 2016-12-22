using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace MovieProjectWithUmbraco.Models
{
    public class DetailedFilmInfo : RenderModel
    {
        public DetailedFilmInfo(IPublishedContent content) : base(content) { }

        public string Title { get; set; }
        public DateTime YearOfRelease { get; set; }
        public string ImagePath { get; set; }
        public string Plot { get; set; }
        public decimal Budget { get; set; }
        public decimal BoxOffice { get; set; }
        public IEnumerable<LinkResponse> Stars { get; set; }
        public IEnumerable<LinkResponse> Directors { get; set; }
        public IEnumerable<LinkResponse> Producers { get; set; }
        public IEnumerable<LinkResponse> Writers { get; set; }
        public IEnumerable<LinkResponse> Composers { get; set; }
        public IEnumerable<string> Countries { get; set; }
        public IEnumerable<LinkResponse> Distributors { get; set; }
    }
}