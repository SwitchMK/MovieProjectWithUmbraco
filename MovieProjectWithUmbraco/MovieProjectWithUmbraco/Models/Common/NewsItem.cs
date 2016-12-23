using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace MovieProjectWithUmbraco.Models
{
    public class NewsItem : RenderModel
    {
        public NewsItem(IPublishedContent content) : base(content) { }

        public string ImagePath { get; set; }
        public string Title { get; set; }
        public string NewsContent { get; set; }
        public string Url { get; set; }
    }
}