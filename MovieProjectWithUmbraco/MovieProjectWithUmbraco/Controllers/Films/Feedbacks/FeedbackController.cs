using MovieProjectWithUmbraco.Models;
using System.Collections.Generic;
using System.Web.Mvc;
using System.Web.Security;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers.Films.Feedbacks
{
    public class FeedbackController : SurfaceController
    {
        public ActionResult RenderFeedbackTicker()
        {
            var feedbacks = GetFeedbacksForMovie();

            return PartialView("~/Views/Partials/Feedback/_FeedbacksTicker.cshtml", feedbacks);
        }

        public ActionResult RenderFeedbackForm()
        {
            return PartialView("~/Views/Partials/Feedback/_Feedback.cshtml");
        }

        [HttpPost]
        [ValidateAntiForgeryToken]
        public ActionResult SubmitFeedback(Feedback model)
        {
            var user = Membership.GetUser();

            if (user == null)
                return Redirect("~/Views/Login.cshtml");

            CreateFeedbackContent(model, CurrentPage.Name, user.UserName);

            return RedirectToCurrentUmbracoUrl();
        }

        private IEnumerable<Feedback> GetFeedbacksForMovie()
        {
            foreach (var comment in CurrentPage.Children)
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