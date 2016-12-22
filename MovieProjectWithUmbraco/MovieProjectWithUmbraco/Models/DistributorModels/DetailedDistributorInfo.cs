using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace MovieProjectWithUmbraco.Models
{
    public class DetailedDistributorInfo : RenderModel
    {
        public DetailedDistributorInfo(IPublishedContent content) : base(content) { }

        public string CompanyName { get; set; }
        public DateTime DateOfFoundation { get; set; }
        public string ImagePath { get; set; }
        public string History { get; set; }
        public IEnumerable<LinkResponse> Founders { get; set; }
    }
}