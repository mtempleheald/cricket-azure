using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Mth.Darts.Cricket;

namespace Mth.Darts.Cricket.Tests
{
    public class StartNewGameTests
    {
        private const int pointsForFirst = 5;
        private const int pointsForSecond = 3;
        private const int pointsForThird = 1;
        private const int pointsForOthers = 0;

        List<String> players;
        Match match;
        public StartNewGameTests() {
            players = new List<String>();
            players.Add("Brooks");
            players.Add("T1000");
            match = new Match (players, ScoringMode.Standard, 20);
            match.Throw (Section.Twenty, Bed.Treble);
            match.Throw (Section.Nineteen, Bed.Treble);
            match.Throw (Section.Eighteen, Bed.Treble);
            match.Throw (null, null);
            match.Throw (null, null);
            match.Throw (null, null);
            match.Throw (Section.Seventeen, Bed.Treble);
            match.Throw (Section.Sixteen, Bed.Treble);
            match.Throw (Section.Fifteen, Bed.Treble);
            match.Throw (null, null);
            match.Throw (null, null);
            match.Throw (null, null);
            match.Throw (Section.Bull, Bed.Double);
            match.Throw (Section.Bull, Bed.Double);
        }

        [Fact]
        public void StartNewGame_CurrentGameSaved_CurrentGameRefreshed ()
        {            
            // Start with game complete but not rolled over
            Assert.True(match.currentGame.complete);
            Assert.Empty (match.previousGames); 
            match.StartNewGame();
            // Now game should have been rolled over and a new one created
            Assert.NotEmpty (match.previousGames); 
            Assert.False(match.currentGame.complete);
        }

        [Fact]
        public void StartNewGame_MatchScores_Updated () {
            Assert.Equal (0, match.scores[0].points);
            Assert.Equal (0, match.scores[1].points);
            match.StartNewGame();
            Assert.Equal (pointsForFirst, match.scores[0].points);
            Assert.Equal (pointsForSecond, match.scores[1].points);
        }

        [Fact]
        public void StartNewGame_MatchRankings_Updated () {
            Assert.Equal (0, match.scores[0].ranking);
            Assert.Equal (0, match.scores[1].ranking);
            match.StartNewGame();
            Assert.Equal (1, match.scores[0].ranking);
            Assert.Equal (2, match.scores[1].ranking);
        }
    }
}