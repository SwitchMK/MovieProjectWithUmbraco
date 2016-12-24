using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace MovieProjectWithUmbraco.Models
{
    public class HomePage : RenderModel
    {
        public HomePage(IPublishedContent content) : base(content) { }

        public string TitleImage { get; set; }
        public string About { get; set; }
        public Intro Intro { get; set; }
        public IEnumerable<NewsItem> News { get; set; }
    }
}