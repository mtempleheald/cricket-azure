using System;
using System.Collections.Generic;
using System.Linq;

namespace Mth.Darts.Cricket
{
    public sealed class Game {
        // Game Status
        internal String currentPlayer {get; set;}
        internal int currentDart {get; set;}
        internal int currentRound {get; set;}
        internal Boolean complete {get; set;}
        // Scores
        public List<PlayerScore> scores {get; set;}
        
        public Game (List<String> players) {

            this.currentPlayer = players.First();
            this.currentDart = 1;
            this.currentRound = 1;
            this.complete = false;

            scores = players.AsEnumerable()
                    .Select((player, index) => 
                        new PlayerScore (player, index, InitSectionStates())
                    ).ToList();

        }

        internal void Throw (Section section, Bed bed, ScoringMode scoringMode) {
            
            UpdateScores(section, bed, scoringMode);

            // increment the current dart every throw
            currentDart = NextDart(currentDart);

            // rotate player every 3 darts
            if (currentDart == 1) {
                currentPlayer = NextValueFromRotatingList (currentPlayer
                                                          ,(from s in scores
                                                            orderby s.order
                                                            select s.player).ToList());
            }

            // increment current round once we hit the first player again
            currentRound = (currentDart == 1 && currentPlayer == scores.First().player) ? currentRound + 1 : currentRound;    

        }


        //   apply any extra hits as points according to the match configuration options
        private void UpdateScores(Section section, Bed bed, ScoringMode scoringMode) {
            // examine how many points have been scored this throw
            // (hits on score board + hits this throw - 3) * section value
            int alreadyHit = (from score in scores
                     where score.player == currentPlayer
                     from state in score.states
                     where state.section == section
                     select state.count).FirstOrDefault();
            int pointsScored = Math.Max (0, (alreadyHit + (int) bed - 3) * (int) section);
            Console.WriteLine ("pointsScored = {0} ({1} + {2} - 3 * {3}", pointsScored, alreadyHit, (int) bed, (int) section);

            // Review all player scores
            //   update section states for the current player only
            //     update the section state for the section which was hit and only up to the maximum number of hits (3)
            scores = (
                from score in scores
                select score.player == currentPlayer 
                    ? new PlayerScore (score.player
                                      ,score.order
                                      ,(from state in score.states
                                        select state.section == section
                                        ? new SectionState (state.section
                                                           ,state.count + (int) bed > 3
                                                            ? 3
                                                            : state.count + (int) bed)
                                        : state
                                       ).ToList()
                                      ,(scoringMode == ScoringMode.Standard) ? score.points + pointsScored : score.points
                                      ,score.ranking)
                    : new PlayerScore (score.player
                                      ,score.order
                                      ,score.states
                                      ,points: (scoringMode == ScoringMode.CutThroat) ? score.points + pointsScored : score.points 
                                      ,ranking: score.ranking)
            ).ToList();
            // Calculate the new ranking now that the scores have been updated
            // These rankings must consider the effective points, bearing in mind states and scoring mode
            // Standard:  effective points = SUM(state.section * state.count) + points
            // CutThroat: effective points = SUM(state.section * state.count) - points
            scores = (
                from score in scores
                let hiddenPoints = score.states.Sum(s => s.count * (int) s.section)
                let effectivePoints = (scoringMode == ScoringMode.CutThroat) ? hiddenPoints - score.points : hiddenPoints + score.points
                orderby effectivePoints
                select score
            )//.ToList()
            .Select((s, i) => new PlayerScore (s.player
                                       ,s.order
                                       ,s.states
                                       ,s.points
                                       ,i))
            .OrderBy(s => s.order)
            .ToList();
        }

        // General function which returns the next string from a list of strings given the current string
        // If the current value is the last entry then this method returns the first value
        private static string NextValueFromRotatingList (string currentValue, List<String> ListOfValues) { 
            var firstValue = ListOfValues.First(); // capture in case the currentPlayer is also the last player
            var nextValue = ListOfValues.SkipWhile(p => p != currentValue).Skip(1).FirstOrDefault(); // next player or null default if we are the last player
            return string.IsNullOrEmpty(nextValue) ? firstValue : nextValue;
        }

        // Increment current dart on every throw
        private static int NextDart (int currentDart) {             
            switch (currentDart) {
                case 1: return 2;
                case 2: return 3;
                default: return 1;
            }
        }


        // Helper method to generate all possible section states for a player' score
        private List<SectionState> InitSectionStates () {
            var list = new List<SectionState>();
            list.Add(new SectionState(Section.Bull, 0));
            list.Add(new SectionState(Section.Twenty, 0));
            list.Add(new SectionState(Section.Nineteen, 0));
            list.Add(new SectionState(Section.Eighteen, 0));
            list.Add(new SectionState(Section.Seventeen, 0));
            list.Add(new SectionState(Section.Sixteen, 0));
            list.Add(new SectionState(Section.Fifteen, 0));
            return list;
        }
    }
}