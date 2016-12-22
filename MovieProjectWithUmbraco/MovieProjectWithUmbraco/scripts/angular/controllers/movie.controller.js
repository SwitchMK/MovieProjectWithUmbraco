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
                .then(function (rating) {
                    $scope.totalRating = response.data;
                });
        };

        $scope.selectRateableMovie = function (filmId, personalRating) {
            ratedMovieId = filmId;
            $('#input-id').rating('update', personalRating);
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
