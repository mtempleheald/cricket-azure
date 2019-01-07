var cricketApp = angular.module("cricketApp", ['cricketScoreboard', 'cricketHistory', 'cricketConfig']);
cricketApp.factory('dataFactory', ['$http', function ($http) {
    var urlBase = 'https://mthcricket01api.azurewebsites.net/api';
    var dataFactory = {};
    dataFactory.startMatch = function (maxRounds, scoringMode, ...players) {
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
cricketApp.controller("cricketController", function cricketController($scope, dataFactory) {

    var cricket = this;
    // match settings
    cricket.matchConfigured = false;
    cricket.players = ["Player1", "Player2"];
    cricket.maxRounds = 20;
    cricket.scoringMode = "Standard";
    cricket.addPlayer = function (player) {
        console.log("addPlayer", player);
        if (player != "") {
            cricket.players.push(player);
        }
    }
    cricket.removePlayer = function () {
        console.log("removePlayer");
        cricket.players.pop();
    }
    cricket.hideConfig = function() {
        document.getElementsByClassName ('Config').style.display = "none";
    }
    // full match state
    cricket.match = {};
    // scores and rankings extracted from the match, combined to a single array for lightweight presentation
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
            "score": "0"
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
            "score": "0"
        }
    ];
    // previous hits, don't need full undo history on the UI
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
    // game state
    cricket.currentRound = 2;
    cricket.currentPlayer = "Player1";
    cricket.currentDart = 1;
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
                "Twenty": score.states.filter(function (state) { return state.section == 20; }).count,
                "Nineteen": score.states.filter(function (state) { return state.section == 19; }).count,
                "Eighteen": score.states.filter(function (state) { return state.section == 18; }).count,
                "Seventeen": score.states.filter(function (state) { return state.section == 17; }).count,
                "Sixteen": score.states.filter(function (state) { return state.section == 16; }).count,
                "Fifteen": score.states.filter(function (state) { return state.section == 15; }).count,
                "Bull": score.states.filter(function (state) { return state.section == 25; }).count,
                "points": score.points,
                "position": cricket.match.scores.filter(function (mscore) { return mscore.player === score.player; }).ranking,
                "score": cricket.match.scores.filter(function (mscore) { return mscore.player === score.player; }).points,
            }
        });
    }

    cricket.startMatch = function () {
        console.log("startMatch initiated");
        cricket.matchConfigured = true;
        dataFactory.startMatch(cricket.maxRounds, cricket.scoringMode, cricket.players)
            .then(function successCallback(response) {
                console.log("Success response: ", response);
                cricket.match = response.data;
                cricket.parseMatchObject();
            }, function errorCallback(response) {
                console.log("Failure response: ", response);
            });
        
        console.log("startMatch completed");
    }

    cricket.hit = function (hit) {
        console.log("throwDart initiated");
        var section;
        switch (hit.substring(0, 1)) {
            case "s": // single
                bed = 1;
                section = hit.substring(1, null);
                playAudio("hit1");
                break;
            case "d": // double
                bed = 2;
                section = hit.substring(1, null);
                playAudio("hit2");
                break;
            case "t": // treble
                bed = 3;
                section = hit.substring(1, null);
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
                alert("error " + hit.substring(0, 1));
                break;
        }

        dataFactory.throwDart(cricket.match, cricket.section, cricket.bed)
            .then(function successCallback(response) {
                console.log("Success response: ", response);
                cricket.match = response.data;
                cricket.parseMatchObject();
            }, function errorCallback(response) {
                console.log("Failure response: ", response);
            })
        console.log("throwDart completed");
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
        console.log("undoThrow initiated");
        dataFactory.undoThrow()
            .then(function successCallback(response) {
                console.log("Success response: ", response);
                cricket.match = response.data;
                cricket.parseMatchObject();
            }, function errorCallback(response) {
                console.log("Failure response: ", response);
            })
        console.log("undoThrow completed");
    }

    cricket.newGame = function () {
        console.log("newGame initiated");
        dataFactory.newGame()
            .then(function successCallback(response) {
                console.log("Success response: ", response);
                cricket.match = response.data;
                cricket.parseMatchObject();
            }, function errorCallback(response) {
                console.log("Failure response: ", response);
            })
        console.log("newGame completed");
    }

});