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

    function readURL(input) {
        if (input.files && input.files[0]) {
            var reader = new FileReader();

            reader.onload = function (e) {
                $('#original-picture').attr('src', e.target.result);
                $('#cropped-picture').attr('src', e.target.result);
            }

            reader.readAsDataURL(input.files[0]);
        }
    }

    $('.datepicker').datepicker();

    $("#imgInp").change(function () {
        readURL(this);
    });

    $('.type-filter').each(function () {
        var self = $(this);
        label = self.nextAll('label:first');
        label_text = label.text();

        label.remove();
        self.iCheck({
            checkboxClass: 'icheckbox_line-green',
            insert: '<div class="icheck_line-icon"></div>' + label_text
        });
    });

    $('.sort-filter').iCheck({
        radioClass: 'iradio_square-green'
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
            };

            $.post("/umbraco/api/FilmRating/RateMovie",
                rateRequest, function (response) {
                    $("#PersonalRatingValue" + ratedMovieId).val(rating);
                    $("#TotalRating" + ratedMovieId).text(response.toFixed(1));
                });
        },
        checkDec: function (el) {
            var ex = /^\d*\.?\d{0,1}$/;
            if(ex.test(el.value)==false) {
                el.value = el.value.substring(0,el.value.length - 1);
            }
        }
    };
})(jQuery);
