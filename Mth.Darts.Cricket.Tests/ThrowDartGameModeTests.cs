using System;
using System.Collections.Generic;
using System.Linq;
using Xunit;
using Mth.Darts.Cricket;

namespace Mth.Darts.Cricket.Tests
{
    public class ThrowDartGameModeTests
    {

        [Theory]
        [InlineData(ScoringMode.Standard, 0, Section.Bull)]        
        [InlineData(ScoringMode.Standard, 20, Section.Bull)]        
        [InlineData(ScoringMode.CutThroat, 0, Section.Bull)]
        [InlineData(ScoringMode.CutThroat, 20, Section.Bull)]
        [InlineData(ScoringMode.Standard, 0, Section.Twenty)]
        [InlineData(ScoringMode.Standard, 20, Section.Twenty)]
        [InlineData(ScoringMode.CutThroat, 0, Section.Twenty)]
        [InlineData(ScoringMode.CutThroat, 20, Section.Twenty)]
        public void ThrowDart_2Players_SectionComplete_ScoresNotUpdated(ScoringMode mode, int rounds, Section section)
        {
            List<String> players = new List<String>();
            players.Add("Player1");
            players.Add("Player2");
            var match = new Match(players, mode, rounds);

            match.Throw(section, Bed.Single);
            match.Throw(section, Bed.Single);
            match.Throw(section, Bed.Single);
            match.Throw(section, Bed.Single);
            match.Throw(section, Bed.Single);
            match.Throw(section, Bed.Single);
            // check that the section is complete (6 for a 2 player game)
            var countTicks = match.currentGame
                                .scores
                                .Aggregate(0, (total, score) => total + score.states
                                                                            .Where(state => state.section == section)
                                                                            .Aggregate(0, (t, state) => t + state.count));
            Assert.Equal(6, countTicks);
            // and that no unexpected points were captured
            Assert.Equal(0, match.currentGame.scores[0].points);
            Assert.Equal(0, match.currentGame.scores[1].points);
            // Finally, check that a further hit doesn't result in a score update, regardless of game mode
            match.Throw(section, Bed.Single);
            Assert.Equal(0, match.currentGame.scores[0].points);
            Assert.Equal(0, match.currentGame.scores[1].points);   
        }    

        [Theory]
        [InlineData(ScoringMode.Standard, 0, Section.Bull)]        
        [InlineData(ScoringMode.Standard, 20, Section.Bull)]        
        [InlineData(ScoringMode.CutThroat, 0, Section.Bull)]
        [InlineData(ScoringMode.CutThroat, 20, Section.Bull)]
        [InlineData(ScoringMode.Standard, 0, Section.Twenty)]
        [InlineData(ScoringMode.Standard, 20, Section.Twenty)]
        [InlineData(ScoringMode.CutThroat, 0, Section.Twenty)]
        [InlineData(ScoringMode.CutThroat, 20, Section.Twenty)]
        public void ThrowDart_2Players_SectionCompleteWithinThrow_ScoresNotUpdated(ScoringMode mode, int rounds, Section section) {
            List<String> players = new List<String>();
            players.Add("Player1");
            players.Add("Player2");
            var match = new Match(players, mode, rounds);
            match.Throw(section, Bed.Single);
            match.Throw(section, Bed.Single);
            match.Throw(section, null); 
            // first player not completed section
            match.Throw(section, Bed.Single);
            match.Throw(section, Bed.Single);
            match.Throw(section, Bed.Single); 
            // second player completed section but no scores yet
            Assert.Equal(0, match.currentGame.scores[0].points);
            Assert.Equal(0, match.currentGame.scores[1].points);
            // first player scores multiple, this should not result in a score update, regardless of game mode
            match.Throw(section, Bed.Double);
            Assert.Equal(0, match.currentGame.scores[0].points);
            Assert.Equal(0, match.currentGame.scores[1].points);  

        }

    }
}