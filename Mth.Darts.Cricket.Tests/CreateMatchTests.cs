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
            players.Add("Brooks");
            players.Add("T1000");

            var match = new Match (players, ScoringMode.Standard, 20);

            Assert.Equal (players.Count, match.currentGame.scores.Count) ;

            Assert.NotEqual (ScoringMode.CutThroat, match.scoringMode);
            Assert.True (ScoringMode.Standard == match.scoringMode);
            Assert.False (ScoringMode.CutThroat == match.scoringMode);

            Assert.Empty(match.currentGameHistory);
            Assert.NotEmpty(match.currentGame.scores);

            Assert.NotNull(match.scores[0].player);
            Assert.IsType(Type.GetType("System.Int32"), match.scores[0].ranking);
        }
    }
}
