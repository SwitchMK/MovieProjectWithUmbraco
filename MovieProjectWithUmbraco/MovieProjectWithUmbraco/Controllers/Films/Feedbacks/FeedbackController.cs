using MovieProjectWithUmbraco.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using System.Linq;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers.Films.Feedbacks
{
    public class FeedbackController : SurfaceController
    {
        private const string PathToFeedbackFolder = "~/Views/Partials/Feedback/";

        public ActionResult RenderFeedbackTicker()
        {
            var feedbacks = GetFeedbacksForMovie();

            return PartialView(PathToFeedbackFolder + "_FeedbacksTicker.cshtml", feedbacks);
        }

        public ActionResult RenderFeedbackForm()
        {
            return PartialView(PathToFeedbackFolder + "_Feedback.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitFeedback(Feedback model)
        {
            if (!ModelState.IsValid)
            {
                return CurrentUmbracoPage();
            }

            var user = Membership.GetUser();

            if (user == null)
                return Redirect("~/Views/Login.cshtml");

            var member = Services.MemberService.GetByUsername(user.UserName);

            CreateFeedbackContent(model, CurrentPage.Name, member.Id);

            return RedirectToCurrentUmbracoUrl();
        }

        private IEnumerable<Feedback> GetFeedbacksForMovie()
        {
            var rootNodes = Umbraco.TypedContentAtRoot();
            var homeNodeByAlias = rootNodes.First(x => x.DocumentTypeAlias == "home");

            var publisherProfileUrl = homeNodeByAlias.Descendant("profile").Url;

            foreach (var comment in CurrentPage.Children.Where(d => d.DocumentTypeAlias == "feedback").OrderByDescending(p => p.CreateDate))
            {
                var memberId = comment.GetPropertyValue<int>("memberId");
                var member = Services.MemberService.GetById(memberId);

                yield return new Feedback
                {
                    Content = comment.GetPropertyValue<string>("feedbackText"),
                    DateOfPublication = comment.CreateDate,
                    Publisher = member?.Username ?? "Unknown",
                    PublisherProfileUrl = member != null ? $"{publisherProfileUrl}?memberId={memberId}" : null
                };
            }
        }

        private void CreateFeedbackContent(Feedback model, string filmName, int memberId)
        {
            var content = Services.ContentService.CreateContent($"{memberId}-{filmName}-Comment", CurrentPage.Id, "Feedback");
            content.SetValue("memberId", memberId);
            content.SetValue("feedbackText", model.Content);
            content.SetValue("hideFromNavigation", true);
            Services.ContentService.SaveAndPublishWithStatus(content);
        }
    }
}