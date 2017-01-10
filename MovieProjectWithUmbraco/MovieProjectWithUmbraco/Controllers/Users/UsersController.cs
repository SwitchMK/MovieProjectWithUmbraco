using MovieProjectWithUmbraco.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using System.Linq;
using MovieProjectWithUmbraco.Extensions;

namespace MovieProjectWithUmbraco.Controllers.Users
{
    public class UsersController : SurfaceController
    {
        private const string USER_FOLDER_PATH = "~/Views/Partials/User/";

        public ActionResult RenderUsers()
        {
            var usersModel = new UsersModel
            {
                UsersInfo = GetUsers().OrderByDescending(p => p.LastName)
            };

            return PartialView(USER_FOLDER_PATH + "_Users.cshtml", usersModel);
        }

        private IEnumerable<UserInfo> GetUsers()
        {
            long? userId = null;
            var loggedMember = Membership.GetUser();

            if (loggedMember != null)
                userId = (int)loggedMember.ProviderUserKey;

            foreach (var member in Services.MemberService.GetAllMembers().Where(p => p.Id != userId))
            {
                yield return new UserInfo
                {
                    FirstName = member.GetValue<string>("firstName"),
                    LastName = member.GetValue<string>("lastName"),
                    AvatarPath = member.GetAvatarUrl("avatarNormalSize"),
                    Id = member.Id
                };
            }
        }
    }
}