(function () {
    'use strict';

    angular
        .module('umbracoMovieApp')
        .service('movieService', movieService);

    movieService.$inject = ['$http'];

    function movieService($http) {
        var url = "/umbraco/api/FilmRating";

        this.rateMovie = function (data) {
            return $http({
                method: 'POST',
                url: url + '/RateMovie',
                data: data,
                headers: { 'Content-Type': 'application/json' }
            });
        };
    }
})();