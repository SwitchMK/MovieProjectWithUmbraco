using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace MovieProjectWithUmbraco.Models
{
    public class FilmsModel : RenderModel
    {
        public FilmsModel(IPublishedContent content) : base(content) { }

        public IEnumerable<FilmInfo> FilmsInfo { get; set; }
    }
}