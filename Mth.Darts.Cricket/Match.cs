using System;
using System.Collections.Generic;
using System.Linq;

namespace Mth.Darts.Cricket
{
    // We're after transience for a simple serverless design, take the data with us
    // Cricket Match is the main object in our application logic containing all
    // However most of the work happens within the current game    
    public sealed class Match
    {
        public ScoringMode scoringMode {get; set;}
        public int maxRounds {get; set;}
        public List<MatchScore> scores {get; set;}
        public Game currentGame {get; set;}
        public List<Game> currentGameHistory {get; set;}
        public List<Game> previousGames {get; set;}
        
        // Construct a new match from scratch with an empty scoreboard
        public Match(List<String> players, ScoringMode scoringMode, int maxRounds)
        {
            // Match Config
            this.scoringMode = scoringMode;
            this.maxRounds = maxRounds;
            // Initialise empty scores for the match
            scores = (
                from player in players
                select new MatchScore (player, 0, 0)
            ).ToList();
            // defer initial game creation to the Game class
            currentGame = new Game (players);
            // A new match will have no game history
            currentGameHistory = new List<Game>();
            previousGames = new List<Game>();
        }

        // Construct a new match object by deserialising an existing match
        public Match () {
            // TODO
        }

        // Throw method should trigger:
        // * Increment current turn
        // * Rotate players every 3 darts
        // * Increment rounds every 3 * playerCount throws
        // * Update game section states
        // * Update game points
        // * Update game rankings
        // * Update game completion status
        // * Update match points
        // * Update match rankings
        // * non-MVP - Data persistence
        // * non-MVP - Authentication & Authorisation check
        public Match Throw (Section? section = null, Bed? bed = null) {
            
            if (section.HasValue && bed.HasValue) {
                // A valid throw has been made and needs to be applied to the current game
                currentGame.Throw(section.Value, bed.Value);
            }
            
            return this;
        }

        // Given the current state of the 
        private void UpdateMatchPoints() {
            
        }
        private void UpdateMatchRankings() {

        }

    }

    
    
}
