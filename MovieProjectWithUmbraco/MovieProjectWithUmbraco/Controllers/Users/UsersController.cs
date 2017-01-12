using MovieProjectWithUmbraco.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web.Mvc;
using System.Linq;
using MovieProjectWithUmbraco.Extensions;
using Umbraco.Web.Models;

namespace MovieProjectWithUmbraco.Controllers.Users
{
    public class UsersController : RenderMvcController
    {
        public ActionResult Users(RenderModel model)
        {
            var usersModel = new UsersModel(model.Content)
            {
                UsersInfo = GetUsers().OrderByDescending(p => p.LastName)
            };

            return base.Index(usersModel);
        }

        private IEnumerable<UserInfo> GetUsers()
        {
            long? userId = null;
            var loggedMember = Membership.GetUser();

            if (loggedMember != null)
                userId = (int)loggedMember.ProviderUserKey;

            foreach (var member in Services.MemberService.GetAllMembers())
            {
                yield return new UserInfo
                {
                    FirstName = member.GetValue<string>("firstName"),
                    LastName = member.GetValue<string>("lastName"),
                    AvatarPath = member.GetAvatarUrl("avatarListSize"),
                    Username = member.Username,
                    Id = member.Id
                };
            }
        }
    }
}