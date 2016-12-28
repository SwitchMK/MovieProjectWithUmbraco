using MovieProjectWithUmbraco.Models;
using System.Web.Mvc;
using System.Web.Security;
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

            Members.Login(model.Login, model.Password);

            return Redirect("/");
        }
    }
}