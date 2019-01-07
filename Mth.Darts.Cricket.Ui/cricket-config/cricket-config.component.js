angular.
    module('cricketConfig').
    component('cricketConfig', {
        templateUrl: 'cricket-config/cricket-config.template.html',
        bindings: {
            maxRounds: '=',
            scoringMode: '=',
            players: '<',
            startMatch: '&',
            addPlayer: '&',
            removePlayer: '&'
        }
    });