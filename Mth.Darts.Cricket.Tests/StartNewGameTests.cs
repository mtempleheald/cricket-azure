using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Mth.Darts.Cricket;

namespace Mth.Darts.Cricket.Tests
{
    public class StartNewGameTests
    {

        [Fact]
        public void StartNewGame_CurrentGameSaved_CurrentGameRefreshed ()
        {
            List<String> players;
            Match match;
            players = new List<String>();
            players.Add("Van Gerwen");
            players.Add("Taylor");
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
            // Game should be complete here but not rolled over
            Assert.True(match.currentGame.complete);
            Assert.Empty (match.previousGames); 
            match.StartNewGame();
            // Now game should have been rolled over and a new one created
            Assert.NotEmpty (match.previousGames); 
            Assert.False(match.currentGame.complete);
        }

    }
}