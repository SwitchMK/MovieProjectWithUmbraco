using Xunit;
using Autofac.Extras.Moq;
using MovieProjectWithUmbraco.Services;
using MovieProjectWithUmbraco.Models;
using MovieProjectWithUmbraco.Services.Interfaces;
using Moq;
using System;
using System.Collections.Generic;
using MovieProjectWithUmbraco.Repositories.Interfaces;
using MovieProjectWithUmbraco.Models.Requests;

namespace MovieProjectWithUmbraco.Tests
{
    public class FilmsManagementTests
    {
        private AutoMock _autoMockContext { get; }

        public FilmsManagementTests()
        {
            _autoMockContext = AutoMock.GetLoose();
        }

        [Fact]
        public void FilmManagementService_GetFilms_VerifySearchMethodExecution()
        {
            MockFilmRepository_SearchFilms();

            var filmSearchResponse = MockFilmSearchResponse_WithQuery("potter");

            var filmsService = _autoMockContext.Create<FilmsService>();
            var actual = filmsService.GetFilms(filmSearchResponse);

            _autoMockContext.Mock<IFilmsSearchService>()
                .Verify(m => m.SearchFilms(
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<double?>(),
                    It.IsAny<double?>()));
        }

        [Fact]
        public void FilmManagementService_GetFilms_VerifyGetFilmsMethodExecution()
        {
            MockFilmRepository_GetFilms();

            var filmSearchResponse = MockFilmSearchResponse_WithQuery(string.Empty);

            var filmsService = _autoMockContext.Create<FilmsService>();
            var actual = filmsService.GetFilms(filmSearchResponse);

            _autoMockContext.Mock<IFilmsSearchService>()
                .Verify(m => m.GetFilms(
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<double?>(),
                    It.IsAny<double?>()));
        }

        [Fact]
        public void FilmManagementService_RateMovieAsync_VerifyAddRatingAsync()
        {
            long userId = 4;

            var rateRequest = new RateRequest
            {
                FilmId = 2,
                Rating = 5
            };

            MockFilmRating_GetPersonalRating_Null();

            var filmRatingService = _autoMockContext.Create<FilmRatingService>();
            filmRatingService.RateMovie(rateRequest, userId);

            _autoMockContext.Mock<IFilmRatingRepository>()
                .Verify(m => m.AddRating(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<double>()));
        }

        [Fact]
        public void FilmManagementService_RateMovie_VerifyUpdateRatingAsync()
        {
            long userId = 4;

            var rateRequest = new RateRequest
            {
                FilmId = 2,
                Rating = 5
            };

            MockFilmRating_GetPersonalRating_NotNull();

            var filmRatingService = _autoMockContext.Create<FilmRatingService>();
            filmRatingService.RateMovie(rateRequest, userId);

            _autoMockContext.Mock<IFilmRatingRepository>()
                .Verify(m => m.UpdateRating(It.IsAny<long>(), It.IsAny<long>(), It.IsAny<double>()));
        }

        private void MockFilmRating_GetPersonalRating_Null()
        {
            _autoMockContext.Mock<IFilmRatingRepository>()
                .Setup(m => m.GetPersonalRating(It.IsAny<long>(), It.IsAny<long>()))
                .Returns(default(double?));
        }

        private void MockFilmRating_GetPersonalRating_NotNull()
        {
            _autoMockContext.Mock<IFilmRatingRepository>()
                .Setup(m => m.GetPersonalRating(It.IsAny<long?>(), It.IsAny<long?>()))
                .Returns(3.6);
        }

        private void MockFilmRepository_GetFilms()
        {
            _autoMockContext.Mock<IFilmsSearchService>()
                .Setup(m => m.GetFilms(
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<double?>(),
                    It.IsAny<double?>()))
                .Returns(MockFilmInfo());
        }

        private void MockFilmRepository_SearchFilms()
        {
            _autoMockContext.Mock<IFilmsSearchService>()
                .Setup(m => m.SearchFilms(
                    It.IsAny<string>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<DateTime?>(),
                    It.IsAny<double?>(),
                    It.IsAny<double?>()))
                .Returns(MockFilmInfo());
        }

        private IEnumerable<FilmInfo> MockFilmInfo()
        {
            return new List<FilmInfo>
            {
                new FilmInfo
                {
                    Id = default(int),
                    ImagePath = string.Empty,
                    PersonalRating = default(double),
                    TotalRating = default(double),
                    Title = string.Empty,
                    Url = string.Empty,
                    YearOfRelease = DateTime.Now
                },
                new FilmInfo
                {
                    Id = default(int),
                    ImagePath = string.Empty,
                    PersonalRating = default(double),
                    TotalRating = default(double),
                    Title = string.Empty,
                    Url = string.Empty,
                    YearOfRelease = DateTime.Now
                }
            };
        }

        private FilmSearchResponse MockFilmSearchResponse_WithQuery(string query)
        {
            return new FilmSearchResponse
            {
                Query = query
            };
        }
    }
}
