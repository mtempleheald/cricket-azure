var cricketApp = angular.module("cricketApp", ['cricketScoreboard', 'cricketHistory', 'cricketConfig']);
cricketApp.factory('dataFactory', ['$http', function ($http) {
    var urlBase = 'https://mthcricket01api.azurewebsites.net/api';
    var dataFactory = {};
    dataFactory.startMatch = function (maxRounds, scoringMode, players) {
        let params = {
            "max_rounds": maxRounds,
            "scoring_mode": scoringMode,
            "player[]": players
        };
        return $http.post(urlBase + "/matches", "", { "params": params });
    }
    dataFactory.throwDart = function (reqBody, section, bed) {
        return $http({
            "method": 'POST',
            "url": urlBase + "/matches/" + "xxx" + "/throw",
            "params": {
                "section": section,
                "bed": bed
            },
            "data": reqBody
        });
    };
    dataFactory.undoThrow = function (reqBody) {
        return $http.post(urlBase + "/matches/" + "xxx" + "/undo", reqBody);
    };
    dataFactory.newGame = function (reqBody) {
        return $http.post(urlBase + "/matches/" + "xxx" + "/newgame", reqBody);
    }
    return dataFactory;
}]);
cricketApp.controller("cricketController", function cricketController($scope, $log, dataFactory) {

    var cricket = this;
    // match settings
    cricket.matchConfigured = false;
    cricket.players = ["Player1", "Player2"];
    cricket.maxRounds = 20;
    cricket.scoringMode = "Standard";
    cricket.addPlayer = function (player) {
        $log.debug("addPlayer", player);
        if (player != "") {
            cricket.players.push(player);
        }
    }
    cricket.removePlayer = function () {
        $log.debug("removePlayer");
        cricket.players.pop();
    }
    cricket.hideConfig = function () {
        document.getElementsByClassName('Config').style.display = "none";
    }
    // full match state
    cricket.match = {};
    // scores and rankings extracted from the match, combined to a single array for lightweight presentation
    cricket.scores = [];
    // previous hits, don't need full undo history on the UI
    cricket.throws = [];
    // game state
    cricket.currentRound = 2;
    cricket.currentPlayer = "Player1";
    cricket.currentDart = 1;
    cricket.gameComplete = false;
    // current throw
    cricket.section = "";
    cricket.bed = "";

    function playAudio(sound) {
        var audio = document.getElementById(sound);
        audio.currentTime = 0;// reset in case we try to play the same sound twice in quick succession
        audio.play();
    }

    cricket.parseMatchObject = function () {
        cricket.currentPlayer = cricket.match.currentGame.currentPlayer;
        cricket.currentRound = cricket.match.currentGame.currentRound;
        cricket.currentDart = cricket.match.currentGame.currentDart;
        cricket.scores = cricket.match.currentGame.scores.map(function (score) {
            return {
                "name": score.player,
                "Twenty": score.states.filter(function (state) { return state.section == 20; })[0].count,
                "Nineteen": score.states.filter(function (state) { return state.section == 19; })[0].count,
                "Eighteen": score.states.filter(function (state) { return state.section == 18; })[0].count,
                "Seventeen": score.states.filter(function (state) { return state.section == 17; })[0].count,
                "Sixteen": score.states.filter(function (state) { return state.section == 16; })[0].count,
                "Fifteen": score.states.filter(function (state) { return state.section == 15; })[0].count,
                "Bull": score.states.filter(function (state) { return state.section == 25; })[0].count,
                "points": score.points,
                "position": cricket.match.scores.filter(function (mscore) { return mscore.player === score.player; })[0].ranking,
                "score": cricket.match.scores.filter(function (mscore) { return mscore.player === score.player; })[0].points,
            }
        });
    }

    cricket.startMatch = function () {
        $log.debug("startMatch, maxRounds=", cricket.maxRounds, " scoringMode=", cricket.scoringMode, " players=", cricket.players);
        dataFactory.startMatch(cricket.maxRounds, cricket.scoringMode, cricket.players)
            .then(function successCallback(response) {
                $log.debug("Success response: ", response);
                cricket.match = response.data;
                cricket.parseMatchObject();
            }, function errorCallback(response) {
                $log.error("Failure response: ", response);
            });
        cricket.matchConfigured = true;
    }

    cricket.hit = function (hit) {
        $log.debug("hit ", hit);
        var thrower = cricket.currentPlayer; // keep this for storing history, which we only want to do if the API call succeeds
        var section = "";
        var bed = "";
        switch (hit.substring(0, 1)) {
            case "s": // single
                bed = 1;
                section = hit.substring(1, hit.length);
                playAudio("hit1");
                break;
            case "d": // double
                bed = 2;
                section = hit.substring(1, hit.length);
                playAudio("hit2");
                break;
            case "t": // treble
                bed = 3;
                section = hit.substring(1, hit.length);
                playAudio("hit3");
                break;
            case "o": // outer bull
                bed = 1;
                section = 25;
                playAudio("hit1");
                break;
            case "i": // inner bull
                bed = 2;
                section = 25;
                playAudio("hit2");
                break;
            case "m": // miss
                playAudio("miss");
                break;
            default:
                $log.error("Unexpected hit, ", hit);
                break;
        }

        $log.debug("throwDart, section=", section, " bed=", bed);
        dataFactory.throwDart(cricket.match, section, bed)
            .then(function successCallback(response) {
                $log.debug("Success response: ", response);
                cricket.match = response.data;
                cricket.parseMatchObject();
                cricket.throws.push({
                    "name": thrower,
                    "value": hit.toUpperCase()
                });
            }, function errorCallback(response) {
                $log.error("Failure response: ", response);
            });
    }

    cricket.miss = function () {
        cricket.hit("miss");
    }
    cricket.skip = function () {
        cricket.hit("miss");
        cricket.hit("miss");
        cricket.hit("miss");
    }

    cricket.undo = function () {
        $log.debug("undoThrow initiated");
        dataFactory.undoThrow(cricket.match)
            .then(function successCallback(response) {
                $log.debug("Success response: ", response);
                cricket.match = response.data;
                cricket.parseMatchObject();
                cricket.throws.pop();
            }, function errorCallback(response) {
                $log.error("Failure response: ", response);
            })
    }

    cricket.newGame = function () {
        $log.debug("newGame initiated");
        dataFactory.newGame(cricket.match)
            .then(function successCallback(response) {
                $log.debug("Success response: ", response);
                cricket.match = response.data;
                cricket.parseMatchObject();
            }, function errorCallback(response) {
                $log.error("Failure response: ", response);
            })
    }

});