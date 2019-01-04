angular.
    module('cricketScoreboard').
    component('cricketScoreboard', {
        templateUrl: 'cricket-scoreboard/cricket-scoreboard.template.html',
        bindings: {
            scores: '<',
            currentRound: '<',
            maxRounds: '<',
            currentPlayer: '<'
        }
    });