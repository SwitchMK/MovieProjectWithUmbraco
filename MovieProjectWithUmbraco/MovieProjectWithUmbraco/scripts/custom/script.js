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
        selectRateableMovie: function (filmId) {
            ratedMovieId = filmId;
            personalRating = $("#PersonalRatingValue" + filmId).val();
            $('#input-id').rating('update', personalRating);
        },
        rateMovie: function () {
            var rateRequest = {
                filmid: ratedMovieId,
                rating: rating
            }

            $.post("/umbraco/api/FilmRating/RateMovie",
                rateRequest, function (response) {
                    $("#PersonalRatingValue" + ratedMovieId).val(rating);
                    $("#TotalRating" + ratedMovieId).text((Math.ceil(response * 10) / 10).toFixed(1));
                });
        }
    };
})(jQuery);
