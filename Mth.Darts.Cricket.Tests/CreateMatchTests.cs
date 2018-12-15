using System;
using System.Collections.Generic;
using Xunit;
using Mth.Darts.Cricket;

namespace Mth.Darts.Cricket.Tests
{
    public class CreateMatchTests
    {
        [Fact]
        public void CreateMatch_2Player_Success()
        {
            var players = new List<String>();
            players.Add("Taylor");
            players.Add("Van Gerwen");

            var match = new Match (players, ScoringMode.Standard, 20);

            // Match config
            Assert.Equal (ScoringMode.Standard, match.scoringMode);
            Assert.Equal (20, match.maxRounds);
            // Match scores
            Assert.Equal (2, match.scores.Count);
            Assert.Equal ("Taylor", match.scores[0].player);
            Assert.Equal (0, match.scores[0].points);
            Assert.Equal (0, match.scores[0].ranking);
            // Game scores
            Assert.Equal (players.Count, match.currentGame.scores.Count);
            Assert.Equal ("Taylor", match.currentGame.currentPlayer);
            Assert.Equal (1, match.currentGame.currentDart);
            Assert.Equal (1, match.currentGame.currentRound);
            Assert.False (match.currentGame.complete);
            Assert.Equal ("Taylor", match.currentGame.scores[0].player);
            Assert.Equal (0, match.currentGame.scores[0].order);
            Assert.Equal (0, match.currentGame.scores[0].points);
            Assert.Equal (0, match.currentGame.scores[0].ranking);
            // History
            Assert.Empty(match.currentGameHistory);
            Assert.Empty(match.previousGames);
        }
    }
}
