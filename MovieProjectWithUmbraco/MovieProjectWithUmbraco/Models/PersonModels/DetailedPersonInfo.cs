using System;
using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace MovieProjectWithUmbraco.Models
{
    public class DetailedPersonInfo : RenderModel
    {
        public DetailedPersonInfo(IPublishedContent content) : base(content) { }

        public string ShortName { get; set; }
        public string FullName { get; set; }
        public DateTime DateOfBirth { get; set; }
        public string ImagePath { get; set; }
        public string Biography { get; set; }
        public IEnumerable<string> Countries { get; set; }
        public IEnumerable<string> Careers { get; set; }
    }
}