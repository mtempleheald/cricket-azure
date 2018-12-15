using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Mth.Darts.Cricket;

namespace Mth.Darts.Cricket.Tests
{
    public class UndoThrowTests
    {

        [Fact]
        public void UndoThrow_GameRolledBack ()
        {
            List<String> players;
            Match match;
            players = new List<String>();
            players.Add("Taylor");
            players.Add("Van Gerwen");
            match = new Match (players, ScoringMode.Standard, 20);
            // fresh game with no history
            match.Throw (Section.Twenty, Bed.Treble);            
            Assert.Equal (2, match.currentGame.currentDart);
            Assert.Single (match.currentGameHistory);
            // A throw has been made and stored in history
            // The wrong value was entered, undo this throw
            match.UndoThrow();
            Assert.Empty (match.currentGameHistory);
            Assert.Equal (1, match.currentGame.currentDart);
        }

    }
}