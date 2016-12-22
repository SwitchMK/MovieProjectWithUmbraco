using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace MovieProjectWithUmbraco.Models
{
    public class DistributorsModel : RenderModel
    {
        public DistributorsModel(IPublishedContent content) : base(content) { }

        public IEnumerable<DistributorInfo> DistributorsInfo { get; set; }
    }
}