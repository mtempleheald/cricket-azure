# cricket-azure

Darts scoreboard app written using Azure serverless components

# Cricket

Cricket is a darts game played by 2 or more people.  
The idea is to tick off 3 of each of the numbers 15-20 plus the bull (centre bull counts double).  
Hitting a number which you've already completed but others haven't scores you those points.  
The winner is the first to complete this checklist and also have the best score overall.  

| Player   | Bull | 20  | 19  | 18  | 17  | 16  | 15  | Points | Ranking |
| Player 1 | XXX  | XXX | XXX | XXX | XXX | XXX | XXX | 0      | 1       |
| Player 2 | XXX  | XXX | XXX | XXX | XXX | XXX | XXX | 100    | 2       | 

There are additional extended rules too, notably:
* Cutthroat - scores go on everybody else, not you, the winner has the lowest score at the end
* Require 3 - If all 3 hits on a number aren't completed within a single visit they aren't saved for the next visit
* Unlimited vs round-based, usually 20 rounds

# Game Logic

Game logic can be split into 2 distinct parts:
* Current game
    * Initialise new game
    * Increment current turn (every throw)
    * Rotate players (every 3 throws)
    * Track rounds within the current game (when all players have had 3 throws)
    * Update points (every throw)
        * standard (points on thrower), highest score wins
        * cut-throat (points on everyone else), lowest score wins
    * Update game rankings when points are updated, inferred from points - outstanding state values = SUM(section * bed)
    * Update game completion status (every time scores are updated)
    * Undo previous move (allow during the current game, drop history before new game)
* Match
    * Initialise new match containing a single game
    * Track all games within the current match
    * When a game is marked as complete, initialise a new game in the current match.  Player scores to be ordered as reverse of previous game rankings.
    * Update match points and rankings - triggered on game completion 
    * Push to persistent data store

# Application Architecture

3 key triggers:
* create match
    * requires list of players
    * returns new match, initialised with zero scores/ hits
* throw dart
    * requires current match state
    * returns updated match state
* undo throw
    * requires current match state including history of throws
    * returns updated match state

## Frontend
Mobile-friendly progressive web app for frontend UI.  Improved design (full dart board) for desktop and large tablets.  

## Backend
Serverless hosting (Azure functions) for backend service.  Securing these services is MVP but second phase.  

## Repository 
No persistent data store initially, PaaS DB eventually, probably Azure SQL Database.  



