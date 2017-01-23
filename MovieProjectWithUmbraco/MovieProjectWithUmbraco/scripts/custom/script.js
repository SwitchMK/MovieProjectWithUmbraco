; var MovieApp = MovieApp || {};

; (function ($) {
    var rating = null;
    var ratedMovieId = null;
    var clickTimer = false;
    var ajaxCallDelay = 500;

    $("#search-results-container").css({
        'width': ($("#search-input-group").width() + 'px')
    });

    $("#search-results-container").hide();

    $(window).resize(function () {
        $("#search-results-container").hide();
    });

    $("#input-id").rating({ size: 'xs' });

    $('#input-id').on('rating.change', function (event, value, caption) {
        rating = value;
    });

    $('#input-id').on('rating.clear', function (event) {
        rating = 0;
    });

    $(function() {
        $('#magicsuggest').magicSuggest({
            placeholder: "Site search...",
            name: "Query",
            hideTrigger: true,
            highlight: false
        });
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

    $("#search-query").on('input', function () {
        var query = $(this).val();

        if (clickTimer) {
            clearTimeout(clickTimer);
        }

        clickTimer = setTimeout(function () {
            MovieApp.Search.getResults(query);
        }, ajaxCallDelay);
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

    $("#search-results-container").on("clickoutside", function () {
        $("#search-results-container").hide();
    });

    $(document).click(function (evt) {
        if (evt.target.id !== "search-query")
            $("#search-results-container").hide();
    });

    $("#search-results-container").click(function (e) {
        e.stopPropagation();
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
                el.value = el.value.replace(el.value, '');
            }
        }
    };

    MovieApp.Search = {
        getResults: function (query) {
            var searchRequest = {
                query: query
            };

            $.post("/umbraco/api/SearchResults/GetFoundResults",
                searchRequest, function (response) {
                    $("#search-results-container").hide();

                    var searchResultsTable = $("#search-results");
                    searchResultsTable.empty();

                    if (response.length > 0) {
                        response.forEach(function (item) {
                            var node = document.createElement("tr");
                            node.innerHTML =
                                "<td class=\"col-md-2\"><img class=\"img-responsive\" src=\"" + item.ImagePath + "\" /></td>" +
                                "<td class=\"vertical-align\"><h5><a href=\"" + item.Url + "\">" + item.Name + "</a></h5></td>"
                            searchResultsTable.append(node);
                        });

                        $("#search-results-container").show();
                    }
                });
        }
    };
})(jQuery);
