using MovieProjectWithUmbraco.Extensions;
using MovieProjectWithUmbraco.Models;
using MovieProjectWithUmbraco.Repositories.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Web.Mvc;
using Umbraco.Core.Models;
using Umbraco.Web;
using Umbraco.Web.Mvc;

namespace MovieProjectWithUmbraco.Controllers.Users
{
    public class UserController : SurfaceController
    {
        private const string USER_FOLDER_PATH = "~/Views/Partials/User/";
        private const string PATH_TO_FEEDBACK_FOLDER = "~/Views/Partials/Feedback/";
        private const int FILMS_PAGE_ID = 1089;
        private const int AMOUNT_OF_COMMENTS = 5;
        private readonly IFilmRatingRepository _filmRatingRepository;

        public UserController(IFilmRatingRepository filmRatingRepository)
        {
            _filmRatingRepository = filmRatingRepository;
        }

        public ActionResult RenderUserDetails(int? memberId)
        {
            var parentUrl = CurrentPage.Parent.Url;

            if (memberId == null)
                ControllerContext.HttpContext.Response.Redirect(parentUrl);

            var userModel = GetUserModel(memberId.Value);

            if (userModel == null)
                ControllerContext.HttpContext.Response.Redirect(parentUrl);

            return PartialView(USER_FOLDER_PATH + "_UserDetails.cshtml", userModel);
        }

        private DetailedUserInfo GetUserModel(int memberId)
        {
            var member = Services.MemberService.GetById(memberId);

            if (member == null)
                return null;

            return new DetailedUserInfo
            {
                BasicInfo = GetBasicInfo(member),
                ContactInfo = GetContactInfo(member),
                FilmsInfo = GetRatedMovies(memberId),
                Comments = GetUserComments(memberId).Take(AMOUNT_OF_COMMENTS)
            };
        }

        private IEnumerable<FilmInfo> GetRatedMovies(int memberId)
        {
            var filmRatings = _filmRatingRepository.GetFilmRatings(memberId);

            var rootNodes = Umbraco.TypedContentAtRoot();
            var homeNodeByAlias = rootNodes.First(x => x.DocumentTypeAlias == "home");

            var filmList = homeNodeByAlias.Children.Where(p => p.DocumentTypeAlias == "films").First();

            foreach (var film in filmList.Children
                .Where(p => filmRatings.Any(s => s.FilmId == p.Id && s.UserId == memberId)))
            {
                yield return GetFilmInfo(film, memberId);
            }
        }

        private IEnumerable<UserComment> GetUserComments(int memberId)
        {
            var rootNodes = Umbraco.TypedContentAtRoot();
            var homeNodeByAlias = rootNodes.First(x => x.DocumentTypeAlias == "home");

            var filmsPage = homeNodeByAlias.FirstChild(p => p.Id == FILMS_PAGE_ID);

            foreach (var filmPage in filmsPage.Children())
            {
                foreach (var comment in filmPage.Children
                    .Where(p => p.DocumentTypeAlias == "feedback" && p.GetPropertyValue<int>("memberId") == memberId)
                    .OrderByDescending(p => p.CreateDate))
                {
                    var member = Services.MemberService.GetById(memberId);

                    yield return GetUserComment(filmPage, comment, member);
                }
            }
        }

        private BasicInfoModel GetBasicInfo(IMember member)
        {
            return new BasicInfoModel
            {
                UserName = member.Username,
                FirstName = member.GetValue<string>("firstName"),
                LastName = member.GetValue<string>("lastName"),
                Hometown = member.GetValue<string>("hometown"),
                Avatar = member.GetAvatarUrl("avatarNormalSize")
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
                PhoneNumber = member.GetValue<string>("phoneNumber")
            };
        }

        private double? GetPersonalRating(long? filmId, long? userId)
        {
            return _filmRatingRepository.GetPersonalRating(filmId, userId);
        }

        private FilmInfo GetFilmInfo(IPublishedContent film, int memberId)
        {
            return new FilmInfo
            {
                Title = film.GetPropertyValue<string>("title"),
                YearOfRelease = film.GetPropertyValue<DateTime>("yearOfRelease"),
                ImagePath = film.GetCropUrl("image", "smSzImgCropper"),
                Url = film.Url,
                PersonalRating = GetPersonalRating(film.Id, memberId)
            };
        }

        private UserComment GetUserComment(IPublishedContent filmPage, IPublishedContent comment, IMember member)
        {
            return new UserComment
            {
                Content = comment.GetPropertyValue<string>("feedbackText"),
                DateOfPublication = comment.CreateDate,
                Publisher = member?.Username ?? "Unknown",
                PublisherProfileUrl = member != null ? string.Format("{0}?memberId={1}", CurrentPage.Url, member.Id) : null,
                FilmName = filmPage.GetPropertyValue<string>("title"),
                FilmPageUrl = filmPage.Url
            };
        }
    }
}