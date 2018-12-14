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

        [Fact]
        public void ThrowDart_Ranking_Updated () {
            Assert.Equal (0, match.currentGame.scores.First().ranking);
            Assert.Equal (0, match.currentGame.scores.Last().ranking);
            PrintCurrentGame(match);
            match.Throw (Section.Twenty, Bed.Double);
            PrintCurrentGame(match);
            Assert.Equal (1, match.currentGame.scores.First().ranking);
            Assert.Equal (2, match.currentGame.scores.Last().ranking);
        }

        [Fact]
        public void ThrowDart_Game_Completed () {
            // Perfect game by player 1, nothing hit by player 2
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
            // game ongoing
            Assert.False(match.currentGame.complete);
            match.Throw (Section.Bull, Bed.Double);
            // game throw made and the crowd goes wild!
            Assert.True(match.currentGame.complete);
        }

        private static void PrintCurrentGame (Match match) {
            foreach (PlayerScore score in match.currentGame.scores) {
                Console.WriteLine("Score for player {0}, order {1}, points {2}, ranking {3}", score.player, score.order, score.points, score.ranking);
                var ss = "{";
                foreach (SectionState state in score.states) {
                    ss = String.Format("{0} ({1}, {2}),", ss, state.section, state.count);
                }
                Console.WriteLine("Section state {0}", ss);
            }
        }
    }
}