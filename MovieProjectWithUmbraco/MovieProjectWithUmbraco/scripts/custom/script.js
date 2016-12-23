; var MovieApp = MovieApp || {};

; (function ($) {
    var rating = null;
    var ratedMovieId = null;

    $("#input-id").rating({ size: 'xs' });

    $('#input-id').on('rating.change', function (event, value, caption) {
        rating = value;
    });

    $('#input-id').on('rating.clear', function (event) {
        rating = 0;
    });

    MovieApp.Rating = {
        selectRateableMovie: function (filmId, personalRating) {
            ratedMovieId = filmId;
            rating = personalRating;
            $('#input-id').rating('update', personalRating);
        },
        rateMovie: function () {
            var rateRequest = {
                filmid: ratedMovieId,
                rating: rating
            }

            $.post("/umbraco/api/FilmRating/RateMovie",
                rateRequest, function () { });
        }
    };
})(jQuery);
