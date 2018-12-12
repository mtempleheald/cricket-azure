using System;
using System.Collections.Generic;
using System.Linq;

namespace Mth.Darts.Cricket
{
    // We're after transience for a simple serverless design, take the data with us
    // Cricket Match is the main object in our application logic containing all
    // However most of the work happens within the current game    
    public class Match
    {
        public Game currentGame{ get; set; }
        public List<Game> currentGameHistory{ get; set; }
        public List<Game> previousGames{ get; set; }
        public List<MatchScore> scores{ get; set; }
        public MatchConfig config{ get; set; }

        public Match(List<String> players, ScoringMode scoringMode, int rounds)
        {
            currentGame = new Game (players);

            currentGameHistory = new List<Game>();

            previousGames = new List<Game>();

            //scores = players.Select(p => new MatchScore(p, 0, 1)).ToList();
            scores = (
                from player in players
                select new MatchScore (player, 0, 1)
            ).ToList();

            config = new MatchConfig(scoringMode, rounds);
        }

    }
    // Config never makes sense outside the context of a Match object
    // We're interested in the values only, so use a struct instead of a class
    public struct MatchConfig
    {
        internal ScoringMode scoringMode{ get; }
        internal int rounds{ get; private set; }
        internal MatchConfig (ScoringMode scoringMode, int rounds) : this() {
            this.scoringMode = scoringMode;
            this.rounds = rounds;
        }
    };

    // Scoring Mode can only be one of a selected few values, enforce that with enum type
    public enum ScoringMode
    {
        standard,
        cutthroat
    };
    // 
    public struct MatchScore
    {
        internal String player{ get; }
        internal int points{ get; }
        internal int ranking{ get; }
        public MatchScore (String player, int points, int ranking) : this() {
            this.player = player;
            this.points = points;
            this.ranking = ranking;
        }
    }
}
