function playAudio(sound) {
    var audio = document.getElementById(sound);
    audio.currentTime = 0;// reset in case we try to play the same sound twice in quick succession
    audio.play();
}
function throwDart(hit) {
    // scope = angular.element("#cricket-controller").scope();
    // scope.cricket.hit = hit
    // scope.cricket.throwDart();
    // scope.$apply();
    switch (hit.substring(0, 1)) {
        case "s": // single
            playAudio("hit1");
            break;
        case "d": // double
            playAudio("hit2");
            break;
        case "t": // treble
            playAudio("hit3");
            break;
        case "o": // outer bull
            playAudio("hit1");
            break;
        case "b": // inner bull
            playAudio("hit2");
            break;
        case "m": // miss
            playAudio("miss");
            break;
        default:
            alert("error " + hit.substring(0, 1));
            break;
    }
};
function undo() {
    scope = angular.element("#cricket-controller").scope();
    scope.cricket.undo();
    scope.$apply();
};
function skipTurn() {
    throwDart("miss");
    throwDart("miss");
    throwDart("miss");
}

// set up triggers on dartboard elements, be sure not to propagate to parent elements
document.getElementById("dartboard").addEventListener("click", function (event) {
    throwDart("miss");
    event.stopPropagation();
});
document.getElementById("inner").addEventListener("click", function (event) {
    throwDart("bull");
    event.stopPropagation();
});
scoreables = document.getElementsByTagName("path");
for (i = 0; i < scoreables.length; i++) {
    if (scoreables[i].id != "labels") {
        // record click action
        scoreables[i].addEventListener("click", function (event) {
            throwDart(this.id);
            event.stopPropagation();
        });
    }
}
// window.onload = function () {
//     var k = angular.element("#cricket-controller").scope();
//     k.cricket.createGame();
//     k.$apply();
// }