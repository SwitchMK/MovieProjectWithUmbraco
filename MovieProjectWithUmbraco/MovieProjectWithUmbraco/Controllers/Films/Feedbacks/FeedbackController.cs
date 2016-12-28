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

            CreateFeedbackContent(model, CurrentPage.Name, user.UserName);

            return RedirectToCurrentUmbracoUrl();
        }

        private IEnumerable<Feedback> GetFeedbacksForMovie()
        {
            foreach (var comment in CurrentPage.Children.OrderByDescending(p => p.CreateDate))
            {
                yield return new Feedback
                {
                    Content = comment.GetPropertyValue<string>("feedbackText"),
                    DateOfPublication = comment.CreateDate,
                    Publisher = Membership.GetUser(comment.GetPropertyValue<string>("member")).UserName
                };
            }
        }

        private void CreateFeedbackContent(Feedback model, string filmName, string userEmail)
        {
            var content = Services.ContentService.CreateContent(string.Format("{0}-{1}-Comment", userEmail, filmName), CurrentPage.Id, "Feedback");
            content.SetValue("member", userEmail);
            content.SetValue("feedbackText", model.Content);
            content.SetValue("hideFromNavigation", true);
            Services.ContentService.SaveAndPublishWithStatus(content);
        }
    }
}