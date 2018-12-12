angular.module("cricket-app", [])
    .factory('dataFactory', ['$http', function ($http) { // back-end api services

        var urlBase = '/cricket/api';
        var dataFactory = {};
        // Initial action required to create a game (collection)
        // Need to know the number of players, game settings ...
        dataFactory.createGame = function () {
            return $http.post(urlBase);
        }
        // This is a very simple application in terms of IO.  
        // There's only one action that can be taken (element to be posted)
        // This will move the player one if necessary, update history, recalculate scores...
        dataFactory.throwDart = function (reqBody, config) {
            return $http.put(urlBase, reqBody, config);
        };
        dataFactory.undoThrow = function (reqBody) {
            return $http.delete(urlBase, reqBody);
        }
        return dataFactory;
    }])
    .controller("cricket-controller", function ($scope, dataFactory) { // front-end app controller
        //alias for this app
        var cricket = this;

        // models for this app
        cricket.allScores = []; /* Static copy of the latest game state retrieved from the server */
        cricket.scores = []; /* Extracted first element of the game state, the latest version of the scores for display purposes */
        cricket.rankings = []; /* Static copy of the match rankings retrieved from the server */
        cricket.currentPlayer = ''; /* Static, retrieved from server, used for display purposes */
        cricket.currentTurn = 0; /* Static, retrieved from server, used to manage traversal to next player */
        cricket.moves = []; /* Dynamic, decoupled from server representation of the same data */
        cricket.hit = ''; /* Dynamic, triggered from click events, the only parameter for backend calls besides the game state */

        // methods for this app
        cricket.createGame = function () {
            console.log("createGame");
            dataFactory.createGame()
                .then(function (response) {
                    console.log('Response returned from createGame', response);
                    cricket.allScores = response.data.game_scores;
                    cricket.scores = response.data.game_scores[0].player_scores;
                    cricket.rankings = response.data.match_rankings;
                    cricket.currentPlayer = response.data.current_player;
                    cricket.currentTurn = 1;
                }, function (error) {
                    console.log('Error returned from createGame', error);
                });
        };
        cricket.undo = function () {
            console.log("undo");
            var req = {
                "game_state": {
                    "game_scores": cricket.scores,
                    "match_rankings": cricket.rankings,
                    "prev_throws": cricket.throws,
                    "current_player": cricket.currentPlayer,
                    "current_turn": cricket.currentTurn
                }
            };
            dataFactory.undoThrow(req)
                .then(function (response) {
                    console.log('Response returned from undoThrow', response);
                    cricket.allScores = response.data.game_scores;
                    cricket.scores = response.data.game_scores[0].player_scores;
                    cricket.rankings = response.data.match_rankings;
                    cricket.currentPlayer = response.data.current_player;
                    cricket.currentTurn = response.data.current_turn;
                    cricket.moves.shift();// pop
                }, function (error) {
                    console.log('Error returned from undoThrow', error);
                });
        };
        cricket.throwDart = function () {
            console.log("throwDart: ", cricket.currentPlayer, " ", cricket.hit);
            var req = {
                "game_scores": cricket.scores,
                "match_rankings": cricket.rankings,
                "prev_throws": cricket.throws,
                "current_player": cricket.currentPlayer,
                "current_turn": cricket.currentTurn
            };
            // game_state": {
            //},"throw": cricket.hit
            var cfg = { "params": { "throw": cricket.hit } };
            dataFactory.throwDart(req, cfg)
                .then(function (response) {
                    console.log('Response returned from throwDart', response);
                    cricket.allScores = response.data.game_scores;
                    cricket.scores = response.data.game_scores[0].player_scores;
                    cricket.rankings = response.data.match_rankings;
                    cricket.currentPlayer = response.data.current_player;
                    cricket.currentTurn = response.data.current_turn;
                    cricket.moves.unshift({ player: cricket.currentPlayer, hit: cricket.hit }); // push
                }, function (error) {
                    console.log('Error returned from throwDart', error);
                });
        };
    });