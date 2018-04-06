using MovieProjectWithUmbraco.Extensions;
using MovieProjectWithUmbraco.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Linq;
using Umbraco.Core.Models;
using Umbraco.Web.Models;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers.Users
{
    public class UsersController : RenderMvcController
    {
        public ActionResult Users(RenderModel model)
        {
            var usersModel = new UsersModel(model.Content)
            {
                UsersInfo = GetUsers()
            };

            return Index(usersModel);
        }

        private IEnumerable<UserInfo> GetUsers()
        {
            foreach (var member in Services.MemberService.GetAllMembers().OrderByDescending(p => p.CreateDate))
            {
                yield return GetUserInfo(member);
            }
        }

        private UserInfo GetUserInfo(IMember member)
        {
            return new UserInfo
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