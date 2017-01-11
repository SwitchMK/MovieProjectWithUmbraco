using System.Collections.Generic;
using Umbraco.Core.Models;
using Umbraco.Web.Models;

namespace MovieProjectWithUmbraco.Models
{
    public class UsersModel : RenderModel
    {
        public UsersModel(IPublishedContent content) : base (content) { }

        public IEnumerable<UserInfo> UsersInfo { get; set; }
    }
}