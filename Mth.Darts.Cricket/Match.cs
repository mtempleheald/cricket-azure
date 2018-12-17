using System;
using System.Collections.Generic;
using System.Linq;

namespace Mth.Darts.Cricket
{
    /// <summary>
    /// Match is responsible for controlling a darts cricket match which may last several games.
    /// Match exposes all public endpoints, reports on current match scores and facilitates rollback.
    /// Match is not responsible for game logic, this is deferred to Game.
    /// </summary>
    public sealed class Match
    {
        public ScoringMode scoringMode {get; set;}
        public int maxRounds {get; set;}
        public List<MatchScore> scores {get; set;}
        public Game currentGame {get; set;}
        public Stack<Game> currentGameHistory {get; set;}
        public List<Game> previousGames {get; set;}

        /// <summary>
        /// Start a new match from scratch with an empty scoreboard
        /// </summary>
        public Match(List<String> players, ScoringMode scoringMode, int maxRounds)
        {
            this.scoringMode = scoringMode;
            this.maxRounds = maxRounds;
            
            scores = (
                from player in players
                select new MatchScore (player, 0, 0)
            ).ToList();
            
            currentGame = new Game (players);// defer initial game creation logic to the Game class
            
            currentGameHistory = new Stack<Game>();
            previousGames = new List<Game>();
        }
        
        /// <summary>
        /// Start a new match by deserialising an existing match.static
        /// Required when hosted in a stateless environment, e.g. Azure Functions.
        /// </summary>
        public Match () {
            // TODO
        }

        /// <summary>
        /// Throw - entry point which triggers all game logic
        /// </summary>
        public Match Throw (Section? section = null, Bed? bed = null) {
            
            currentGameHistory.Push((Game)currentGame.Clone());
            currentGame.Throw(section, bed, scoringMode, maxRounds);
            
            return this;
        }
        
        /// <summary>
        /// StartNewGame - Entry point for opting to continue the match after game completion.
        /// Update match points and rankings, archive the current game and finally start a new game.
        /// </summary>
        public Match StartNewGame () {
            if (currentGame.complete) {
                UpdateMatchScores();
                previousGames.Add(currentGame);
                var players = (from score in currentGame.scores
                               orderby score.ranking descending
                               select score.player).ToList();
                currentGame = new Game (players);
            }
            return this;
        }
        
        /// <summary>
        /// UndoThrow - Entry point for undoing the last throw and returning the previous game state.
        /// </summary>
        public Match UndoThrow () {
            if (currentGameHistory.Any()) {
                currentGame = currentGameHistory.Pop();
            }
            return this;
        }
        
        private void UpdateMatchScores() {
            scores = (from matchscore in scores
                      join gamescore in currentGame.scores on matchscore.player equals gamescore.player
                      let newpoints = matchscore.points + gamescore.ranking==1 ? (int) MatchGameScore.First
                                                        : gamescore.ranking==2 ? (int) MatchGameScore.Second
                                                        : gamescore.ranking==3 ? (int) MatchGameScore.Third
                                                        :                        (int) MatchGameScore.Other
                      orderby newpoints descending
                      select (player: matchscore.player, points: newpoints)
                     ).ToList()
                      .Select((s, i) => new MatchScore( player: s.player
                                                      , points: s.points
                                                      , ranking: i + 1))
                      .OrderBy(s => s.ranking)
                      .ToList();
        }

    }  
}