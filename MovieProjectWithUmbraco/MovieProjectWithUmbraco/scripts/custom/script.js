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
            var obj = $("#TotalRatingValue" + filmId);
            rating = $("#TotalRatingValue" + filmId).val();
            $('#input-id').rating('update', rating);
        },
        rateMovie: function () {
            var rateRequest = {
                filmid: ratedMovieId,
                rating: rating
            }

            $.post("/umbraco/api/FilmRating/RateMovie",
                rateRequest, function (response) {
                    $("#TotalRatingValue" + ratedMovieId).val(response);
                    $("#TotalRating" + ratedMovieId).text((Math.ceil(response * 10) / 10).toFixed(1));
                });
        }
    };
})(jQuery);
