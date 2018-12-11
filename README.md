# cricket-azure

Darts scoreboard app written using Azure serverless components

# Cricket

Cricket is a darts game played by 2 or more people.  
The idea is to tick off 3 of each of the numbers 15-20 plus the bull (centre bull counts double).  
Hitting a number which you've already completed but others haven't scores you those points.  
The winner is the first to complete this checklist and also have the best score overall.  

There are additional extended rules too, notably:
* Cutthroat - scores go on everybody else, not you, the winner has the lowest score at the end
* Require 3 - If all 3 hits on a number aren't completed within a single visit they aren't saved for the next visit
* Unlimited vs round-based, usually 20 rounds


# Application Architecture

2 key actions:
* create match
    * requires list of players
    * returns new match, initialised with zero scores/ hits
* throw dart
    * requires current match state
    * returns updated match state

Web based, because why wouldn't it be?  Mobile-friendly progressive web app.  

Improved design (full dart board) for desktop and large tablets.  

