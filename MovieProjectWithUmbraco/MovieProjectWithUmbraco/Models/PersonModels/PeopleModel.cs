using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace MovieProjectWithUmbraco.Models
{
    public class PeopleModel : RenderModel
    {
        public PeopleModel(IPublishedContent content) : base(content) { }

        public IEnumerable<PersonInfo> PeopleInfo { get; set; }
    }
}