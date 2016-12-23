(function () {
    'use strict';

    angular
        .module('umbracoMovieApp')
        .controller('movieController', movieController);

    movieController.$inject = ['$scope', 'movieService'];

    function movieController($scope, movieService) {
        var ratedMovieId = null;

        $scope.rateMovie = function () {
            var rateRequest = {
                FilmId: ratedMovieId,
                Rating: $scope.rating
            };

            $scope.rateMoviePromise = movieService
                .rateMovie(rateRequest)
                .then(function (response) {
                    $scope.films = response.data;
                });
        };

        $scope.selectRateableMovie = function (filmId, personalRating) {
            ratedMovieId = film.id;
            $('#input-id').rating('update', film.personalRating);
        };

        $("#input-id").rating({ size: 'xs' });

        $('#input-id').on('rating.change', function (event, value, caption) {
            $scope.rating = value;
        });

        $('#input-id').on('rating.clear', function (event) {
            $scope.rating = 0;
        });
    }
})();
