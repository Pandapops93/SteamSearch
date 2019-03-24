﻿//// todo; use '$resource' instead?

//(function () {
//    'use strict';

//    angular.module('steamSearch')
//        .factory('steamService', steamService);

//    steamService.$inject = ['$http', 'SteamApiKey','WebApiUrl'];

//    function steamService($http, SteamApiKey, WebApiUrl) {

//        var userData = {};
//        var recentGames = [];

//        function GetSteamUserInfo(steamId) {



//            $http({
//                method: 'GET',
//                url: WebApiUrl + "/api/steam/getuser/" + steamId
//            }).then(function successCallback(response) {

//                debugger;
//                userData = JSON.parse(response.data).response.players[0];


//                GetRecentPlayedGames(steamId);
//                // this callback will be called asynchronously
//                // when the response is available
//                }, function errorCallback(response) {
//                    debugger;
//                // called asynchronously if an error occurs
//                // or server returns response with an error status.
//            });


//        }
//        function GetRecentPlayedGames(steamId) {

//            $http({
//                method: 'GET',
//                url: WebApiUrl + "/api/steam/getrecentgames/" + steamId
//            }).then(function successCallback(response) {

//                debugger;
//                recentGames = JSON.parse(response.data).response.games;

//                // this callback will be called asynchronously
//                // when the response is available
//            }, function errorCallback(response) {
//                debugger;
//                // called asynchronously if an error occurs
//                // or server returns response with an error status.
//            });
//        }









//        var service = {
//            GetSteamUserInfo,
//            userData,
//            recentGames
//        }


//        return service;


//    }
//})();

﻿//////////////////////////////////////////
// Client AngularJS Service
//////////////////////////////////////////
(function () {
    'use strict';

    angular.module('steamSearch')
        .service('steamService', ['$rootScope', '$http', 'WebApiUrl', function ($rootScope, $http, WebApiUrl) {

            this.userData = {};
            this.recentGames = [];
            this.ownedGames = [];
            this.dataLoaded = false;
            this.allGames = [];

            

            this.GetRecentPlayedGames = function (steamId) {


                var service = this;
                $http({
                    method: 'GET',
                    url: WebApiUrl + "/api/steam/getrecentgames/" + steamId
                }).then(function successCallback(response) {


                    service.recentGames = JSON.parse(response.data).response.games;
                    console.log(service.recentGames);
                    service.GetOwnedGames(steamId);

                    // this callback will be called asynchronously
                    // when the response is available
                }, function errorCallback(response) {
                    service.dataLoaded = true;
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });
            }
            this.GetSteamUserInfo = function (steamId) {

                var service = this;

                $http({
                    method: 'GET',
                    url: WebApiUrl + "/api/steam/getuser/" + steamId
                }).then(function successCallback(response) {


                    service.userData = JSON.parse(response.data).response.players[0];
                    console.log(service.userData);

                    service.GetRecentPlayedGames(steamId);
                    // this callback will be called asynchronously
                    // when the response is available
                }, function errorCallback(response) {

                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });


            }
            this.GetOwnedGames = function (steamId) {

                var service = this;
                $http({
                    method: 'GET',
                    url: WebApiUrl + "/api/steam/getownedgames/" + steamId
                }).then(function successCallback(response) {


                    service.ownedGames = JSON.parse(response.data).response.games;
                    console.log(service.ownedGames);
                    service.dataLoaded = true;
                    // this callback will be called asynchronously
                    // when the response is available
                }, function errorCallback(response) {
                    service.dataLoaded = true;
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });

            }
            this.GetAllGames = function () {

                var service = this;
                $http({
                    method: 'GET',
                    url: WebApiUrl + "/api/steam/getallgames"
                }).then(function successCallback(response) {


                    service.allGames = JSON.parse(response.data).applist.apps;
                    console.log(service.allGames);
                    // this callback will be called asynchronously
                    // when the response is available
                }, function errorCallback(response) {
                    service.dataLoaded = true;
                    // called asynchronously if an error occurs
                    // or server returns response with an error status.
                });


            }

           




        }]);
})();
