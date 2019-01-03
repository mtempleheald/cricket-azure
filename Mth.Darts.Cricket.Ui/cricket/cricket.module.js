angular.module("cricketApp", ['cricketScoreboard', 'cricketHistory'])
    .factory('dataFactory', ['$http', function ($http) { // back-end api services
        var urlBase = 'https://mthcricket01api.azurewebsites.net/api';
        var dataFactory = {};
        dataFactory.startMatch = function () {
            return $http.post(urlBase + "/matches");
        }
        dataFactory.throwDart = function (reqBody, config) {
            return $http.put(urlBase + "/matches/" + "xxx" + "/throw", reqBody, config);
        };
        dataFactory.undoThrow = function (reqBody) {
            return $http.delete(urlBase + "/matches/" + "xxx" + "/undo", reqBody);
        };
        dataFactory.newGame = function (reqBody) {
            return $http.delete(urlBase + "/matches/" + "xxx" + "/newgame", reqBody);
        }
        return dataFactory;
    }])
    .controller("cricketController", function cricketController($scope, dataFactory) { // front-end app controller

        var cricket = this;

        cricket.scores = [
            {
                "name": "Player1",
                "Twenty": 3,
                "Nineteen": "2",
                "Eighteen": "1",
                "Seventeen": "0",
                "Sixteen": "0",
                "Fifteen": "0",
                "Bull": "0",
                "points": "0",
                "position": "0",
                "score": "0",
                "current": true
            },
            {
                "name": "Player2",
                "Twenty": 0,
                "Nineteen": "0",
                "Eighteen": "0",
                "Seventeen": "0",
                "Sixteen": "0",
                "Fifteen": "0",
                "Bull": "3",
                "points": "25",
                "position": "0",
                "score": "0",
                "current": false
            }
        ];
        cricket.throws = [
            {
                "name": "Player2",
                "value": "Inner"
            },
            {
                "name": "Player2",
                "value": "-"
            },
            {
                "name": "Player2",
                "value": "Inner"
            },
            {
                "name": "Player1",
                "value": "S18"
            },
            {
                "name": "Player1",
                "value": "D19"
            },
            {
                "name": "Player1",
                "value": "T20"
            }
        ];
        cricket.currentRound = 2;
        cricket.maxRounds = 20;

        // models for this app
        cricket.allScores = []; /* Static copy of the latest game state retrieved from the server */
        //cricket.scores = []; /* Extracted first element of the game state, the latest version of the scores for display purposes */
        cricket.rankings = []; /* Static copy of the match rankings retrieved from the server */
        cricket.currentPlayer = ''; /* Static, retrieved from server, used for display purposes */
        //cricket.currentTurn = 0; /* Static, retrieved from server, used to manage traversal to next player */
        //cricket.moves = []; /* Dynamic, decoupled from server representation of the same data */
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