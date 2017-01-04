using MovieProjectWithUmbraco.Models;
using System;
using System.Security.Cryptography;
using System.Text;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Core.Models;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers.Account
{
    public class AccountController : SurfaceController
    {
        const string PATH_TO_ACCOUNT_PAGES = "~/Views/Partials/Account/";

        public ActionResult RenderRegisterForm()
        {
            return PartialView(PATH_TO_ACCOUNT_PAGES + "_Register.cshtml");
        }

        public ActionResult RenderLoginForm()
        {
            return PartialView(PATH_TO_ACCOUNT_PAGES +  "_Login.cshtml");
        }

        public ActionResult RenderProfile()
        {
            var profileModel = GetProfileModel();

            return PartialView(PATH_TO_ACCOUNT_PAGES + "_Profile.cshtml", profileModel);
        }

        public ActionResult ChangePassword()
        {
            return PartialView(PATH_TO_ACCOUNT_PAGES + "_ChangePassword.cshtml");
        }

        public ActionResult MemberLogOut()
        {
            Session.Clear();
            FormsAuthentication.SignOut();

            return Redirect("/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitRegisterForm(RegisterModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var memberService = Services.MemberService;
            if (memberService.GetByEmail(model.Email) != null)
            {
                ModelState.AddModelError("", "Member already exists");
                return CurrentUmbracoPage();
            }

            var member = memberService.CreateMemberWithIdentity(model.Login, model.Email, model.FullName, "Member");

            memberService.Save(member);

            memberService.SavePassword(member, model.Password);

            Members.Login(model.Login, model.Password);

            return Redirect("/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitLoginForm(LoginModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var memberService = Services.MemberService;
            if (memberService.GetByUsername(model.Login) == null)
            {
                ModelState.AddModelError("", "This member doesn't exist or locked out.");
                return CurrentUmbracoPage();
            }

            var result = Members.Login(model.Login, model.Password);

            if (!result)
            {
                ModelState.AddModelError("", "Incorrect password.");
                return CurrentUmbracoPage();
            }

            return Redirect("/");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult ChangePassword(ProfileModel model)
        {
            if (!ModelState.IsValid)
                return CurrentUmbracoPage();

            var user = Membership.GetUser();

            var memberService = Services.MemberService;
            var member = memberService.GetByUsername(user?.UserName);

            if (member == null)
                return Redirect("/login");

            if (HashPassword(model.ChangePassword.OldPassword) != member.RawPasswordValue)
            {
                ModelState.AddModelError("", "Old password was incorrect.");
                return CurrentUmbracoPage();
            }

            try
            {
                memberService.SavePassword(member, model.ChangePassword.NewPassword);
            }
            catch
            {
                ModelState.AddModelError("", "Password doesn't satisfy the requirements.");
                return CurrentUmbracoPage();
            }

            return RedirectToCurrentUmbracoPage();
        }

        private string HashPassword(string password)
        {
            HMACSHA1 hash = new HMACSHA1();
            hash.Key = Encoding.Unicode.GetBytes(password);
            string encodedPassword = Convert.ToBase64String(hash.ComputeHash(Encoding.Unicode.GetBytes(password)));

            return encodedPassword;
        }

        private ProfileModel GetProfileModel()
        {
            var user = Membership.GetUser();

            if (user != null)
            {
                var member = Services.MemberService.GetByUsername(user.UserName);

                return new ProfileModel
                {
                    BasicInfo = GetBasicInfo(member),
                    ContactInfo = GetContactInfo(member),
                    ChangePassword = new ChangePassword()
                };
            }

            return null;
        }

        private BasicInfoModel GetBasicInfo(IMember member)
        {
            return new BasicInfoModel
            {
                UserName = member.Username,
                FirstName = member.GetValue<string>("firstName"),
                LastName = member.GetValue<string>("lastName"),
                Hometown = member.GetValue<string>("hometown")
            };
        }

        private ContactInfoModel GetContactInfo(IMember member)
        {
            return new ContactInfoModel
            {
                City = member.GetValue<string>("city"),
                Country = member.GetValue<string>("country"),
                Skype = member.GetValue<string>("skype"),
                Website = member.GetValue<string>("website"),
                PhoneNumber = member.GetValue<string>("phoneNumber"),
                Email = member.Email
            };
        }
    }
}