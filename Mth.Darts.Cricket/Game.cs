using System;
using System.Collections.Generic;
using System.Linq;
using Newtonsoft.Json;

namespace Mth.Darts.Cricket
{
    /// <summary>
    /// Game is responsible for controlling a darts cricket game, limited to game logic and current status.
    /// Game is not exposed publicly, instead designed to operate within a public Match which it knows nothing about.
    /// </summary>
    public sealed class Game : ICloneable
    {
        [JsonProperty()]
        internal String currentPlayer { get; set; }
        [JsonProperty()]
        internal int currentDart { get; set; }
        [JsonProperty()]
        internal int currentRound { get; set; }
        [JsonProperty()]
        internal Boolean complete { get; set; }
        [JsonProperty()]
        // Scores
        internal List<GameScore> scores { get; set; }

        internal Game(List<String> players)
        {

            this.currentPlayer = players.First();
            this.currentDart = 1;
            this.currentRound = 1;
            this.complete = false;

            scores = players.AsEnumerable()
                    .Select((player, index) =>
                        new GameScore(player, index, InitGameScoreSectionStates())
                    ).ToList();

        }
        [JsonConstructor]
        internal Game() { }
        internal void Throw(Section? section, Bed? bed, ScoringMode scoringMode, int maxRounds = 0)
        {

            if (section.HasValue && bed.HasValue)
            {
                // A valid throw has been made and needs to be applied to the current game
                UpdateScores(section.Value, bed.Value, scoringMode);

                UpdateGameCompletionStatus(scoringMode, maxRounds);
            }

            // increment the current dart every throw
            currentDart = NextDart(currentDart);

            // rotate player every 3 darts
            if (currentDart == 1)
            {
                currentPlayer = NextValueFromRotatingList(currentPlayer
                                                          , (from s in scores
                                                             orderby s.order
                                                             select s.player).ToList());
            }

            // increment current round once we hit the first player again
            currentRound = (currentDart == 1 && currentPlayer == scores.First().player) ? currentRound + 1 : currentRound;

        }
        // The game is complete once a player has hit all targets and is ranked first
        // or if all rounds have completed
        private void UpdateGameCompletionStatus(ScoringMode scoringMode, int maxRounds)
        {
            if (maxRounds < currentRound && maxRounds > 0)
            {
                complete = true;
                return;
            }
            complete = (
                scores.OrderBy(s => s.ranking)
                    .First()
                    .states
                    .Sum(s => s.count * (int)s.section) == 390) ? true : false;
        }

        //   apply any extra hits as points according to the match configuration options
        private void UpdateScores(Section section, Bed bed, ScoringMode scoringMode)
        {
            // First check that any points count, that is that there is at least one player who hasn't checked off this section
            Boolean sectionClosed = 3 == scores.Min(score => score.states.Where(state => state.section == section)
                                                                         .Min(state => state.count));
            if (sectionClosed) {
                return;
            }

            // examine how many points have been scored this throw
            // (hits on score board + hits this throw - 3) * section value
            int playerHits = (from score in scores
                              where score.player == currentPlayer
                              from state in score.states
                              where state.section == section
                              select state.count).FirstOrDefault();
            int pointsScored = Math.Max(0, (playerHits + (int)bed - 3) * (int)section);
            //Console.WriteLine ("pointsScored = {0} ({1} + {2} - 3 * {3}", pointsScored, alreadyHit, (int) bed, (int) section);

            // Review all player scores
            //   update section states for the current player only
            //     update the section state for the section which was hit and only up to the maximum number of hits (3)
            scores = (
                from score in scores
                select score.player == currentPlayer
                    ? new GameScore(score.player
                                      , score.order
                                      , (from state in score.states
                                         select state.section == section
                                         ? new GameScoreSectionState(state.section
                                                            , state.count + (int)bed > 3
                                                             ? 3
                                                             : state.count + (int)bed)
                                         : state
                                       ).ToList()
                                      , (scoringMode == ScoringMode.Standard) ? score.points + pointsScored : score.points
                                      , score.ranking)
                    : new GameScore(score.player
                                      , score.order
                                      , score.states
                                      , points: (scoringMode == ScoringMode.CutThroat) ? score.points + pointsScored : score.points
                                      , ranking: score.ranking)
            ).ToList();
            // Calculate the new ranking now that the scores have been updated
            // These rankings must consider the effective points, bearing in mind states and scoring mode
            // Standard:  effective points = SUM(state.section * state.count) + points
            // CutThroat: effective points = SUM(state.section * state.count) - points
            scores = (
                from score in scores
                let hiddenPoints = score.states.Sum(s => s.count * (int)s.section)
                let effectivePoints = (scoringMode == ScoringMode.CutThroat) ? hiddenPoints - score.points : hiddenPoints + score.points
                orderby effectivePoints descending
                select score
            )//.ToList()
            .Select((s, i) => new GameScore(s.player
                                       , s.order
                                       , s.states
                                       , s.points
                                       , ranking: i + 1))
            .OrderBy(s => s.order)
            .ToList();
        }

        // General function which returns the next string from a list of strings given the current string
        // If the current value is the last entry then this method returns the first value
        private static string NextValueFromRotatingList(string currentValue, List<String> ListOfValues)
        {
            var firstValue = ListOfValues.First(); // capture in case the currentPlayer is also the last player
            var nextValue = ListOfValues.SkipWhile(p => p != currentValue).Skip(1).FirstOrDefault(); // next player or null default if we are the last player
            return string.IsNullOrEmpty(nextValue) ? firstValue : nextValue;
        }

        // Increment current dart on every throw
        private static int NextDart(int currentDart)
        {
            switch (currentDart)
            {
                case 1: return 2;
                case 2: return 3;
                default: return 1;
            }
        }


        // Helper method to generate all possible section states for a player' score
        private List<GameScoreSectionState> InitGameScoreSectionStates()
        {
            var list = new List<GameScoreSectionState>();
            list.Add(new GameScoreSectionState(Section.Bull, 0));
            list.Add(new GameScoreSectionState(Section.Twenty, 0));
            list.Add(new GameScoreSectionState(Section.Nineteen, 0));
            list.Add(new GameScoreSectionState(Section.Eighteen, 0));
            list.Add(new GameScoreSectionState(Section.Seventeen, 0));
            list.Add(new GameScoreSectionState(Section.Sixteen, 0));
            list.Add(new GameScoreSectionState(Section.Fifteen, 0));
            return list;
        }
        // Require cloning for keeping a snapshot of throws to facilitate rollback
        // This snapshot is required because the scoring process is irreversible and stateful
        internal Game Clone()
        {
            return (Game)this.MemberwiseClone();
        }
        object ICloneable.Clone()
        {
            return this.Clone();
        }
    }
}