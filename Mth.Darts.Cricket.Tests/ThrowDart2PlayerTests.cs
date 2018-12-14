using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Mth.Darts.Cricket;

namespace Mth.Darts.Cricket.Tests
{
    public class ThrowDart2PlayerTests
    {
        List<String> players;
        Match match;
        public ThrowDart2PlayerTests() {
            players = new List<String>();
            players.Add("Brooks");
            players.Add("T1000");
            match = new Match (players, ScoringMode.Standard, 20);
        }

        [Fact]
        public void ThrowDart_CurrentDart_Incremented()
        {
            Assert.Equal (1, match.currentGame.currentDart);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal (2, match.currentGame.currentDart);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal (3, match.currentGame.currentDart);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal (1, match.currentGame.currentDart);          
        }

        [Fact]
        public void ThrowDart_CurrentRound_Incremented()
        {
            Assert.Equal (1, match.currentGame.currentRound);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal (1, match.currentGame.currentRound);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal (1, match.currentGame.currentRound);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal (1, match.currentGame.currentRound);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal (1, match.currentGame.currentRound);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal (1, match.currentGame.currentRound);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal (2, match.currentGame.currentRound);
        }
        [Fact]
        public void ThrowDart_currentPlayer_Rotated() {
            Assert.Equal(players.First(), match.currentGame.currentPlayer);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal(players.First(), match.currentGame.currentPlayer);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal(players.First(), match.currentGame.currentPlayer);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal(players.Last(), match.currentGame.currentPlayer);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal(players.Last(), match.currentGame.currentPlayer);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal(players.Last(), match.currentGame.currentPlayer);
            match.Throw (Section.Twenty, Bed.Treble);
            Assert.Equal(players.First(), match.currentGame.currentPlayer);
        }

        [Fact]
        public void ThrowDart_SectionStates_Updated() {
            
            match.Throw (Section.Twenty, Bed.Treble);

            var countTwenties = (
                from playerScore in match.currentGame.scores
                where playerScore.player == match.currentGame.currentPlayer
                from sectionStates in playerScore.states
                where sectionStates.section == Section.Twenty
                select sectionStates.count
            ).First();

            Assert.Equal (3, countTwenties);
        }

        [Fact]
        public void ThrowDart_Points_Updated () {
            
            match.Throw (Section.Twenty, Bed.Double);
            Assert.Equal (0, match.currentGame.scores.First().points);
            match.Throw (Section.Twenty, Bed.Double);
            Assert.Equal (20, match.currentGame.scores.First().points);
            match.Throw (null, null);
            Assert.Equal (20, match.currentGame.scores.First().points);
            
        }
    }
}