using MovieProjectWithUmbraco.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web;
using Umbraco.Web.Mvc;
using System.Linq;

namespace MovieProjectWithUmbraco.Controllers.Films.Feedbacks
{
    public class FeedbackController : SurfaceController
    {
        const string PATH_TO_FEEDBACK_FOLDER = "~/Views/Partials/Feedback/";

        public ActionResult RenderFeedbackTicker()
        {
            var feedbacks = GetFeedbacksForMovie();

            return PartialView(PATH_TO_FEEDBACK_FOLDER + "_FeedbacksTicker.cshtml", feedbacks);
        }

        public ActionResult RenderFeedbackForm()
        {
            return PartialView(PATH_TO_FEEDBACK_FOLDER + "_Feedback.cshtml");
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
                    PublisherProfileUrl = member != null ? string.Format("{0}?memberId={1}", publisherProfileUrl, memberId) : null
                };
            }
        }

        private void CreateFeedbackContent(Feedback model, string filmName, int memberId)
        {
            var content = Services.ContentService.CreateContent(string.Format("{0}-{1}-Comment", memberId, filmName), CurrentPage.Id, "Feedback");
            content.SetValue("memberId", memberId);
            content.SetValue("feedbackText", model.Content);
            content.SetValue("hideFromNavigation", true);
            Services.ContentService.SaveAndPublishWithStatus(content);
        }
    }
}