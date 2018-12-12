using System;
using System.Collections.Generic;
using Xunit;
using Mth.Darts.Cricket;

namespace Mth.Darts.Cricket.Tests
{
    public class CreateMatch
    {
        [Fact]
        public void CreateMatch_2Player_Success()
        {
            var players = new List<String>();
            players.Add("Brooks");
            players.Add("T1000");

            var match = new Match (players, ScoringMode.standard, 20);

            Assert.Equal (ScoringMode.standard, match.config.scoringMode);
            Assert.NotEqual (ScoringMode.cutthroat, match.config.scoringMode);
            Assert.True (ScoringMode.standard == match.config.scoringMode);
            Assert.False (ScoringMode.cutthroat == match.config.scoringMode);

            Assert.Empty(match.currentGameHistory);
            Assert.NotEmpty(match.currentGame.scores);

            Assert.NotNull(match.scores[0].ranking);
            Assert.IsType(Type.GetType("System.Int32"), match.scores[0].ranking);
        }
    }
}
