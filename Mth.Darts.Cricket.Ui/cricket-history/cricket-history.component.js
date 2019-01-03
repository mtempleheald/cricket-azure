angular.
    module('cricketHistory').
    component('cricketHistory', {
        templateUrl: 'cricket-history/cricket-history.template.html',
        bindings: {
            throws: '<'
        }
    });