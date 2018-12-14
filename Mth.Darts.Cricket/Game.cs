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

        internal void Throw (Section section, Bed bed) {
            // increment current round every third throw
            switch (currentDart) {
                case 3: currentRound = currentRound + 1; break;
                default: break;
            }
            // Rotate players
            switch (currentDart) {
                case 3:  {
                            var players = (from s in scores
                                           orderby s.order
                                           select s.player).ToList(); // ordered list of players
                            var firstPlayer = players.First(); // capture in case the currentPlayer is also the last player
                            var nextPlayer = players.SkipWhile(p => p != currentPlayer).Skip(1).FirstOrDefault(); // next player or null default if we are the last player
                            currentPlayer = string.IsNullOrEmpty(nextPlayer) ? firstPlayer : nextPlayer;
                         }
                         break;
                default: break;
            }
            // Incrememt current dart on every throw
            switch (currentDart) {
                case 1: currentDart = 2; break;
                case 2: currentDart = 3; break;
                default: currentDart = 1; break;
            }

            // Review all player scores
            //   only update section states for the current player only
            //     only update the state for the section which was hit and only up to the maximum number of hits (3)
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
                                       ).ToList())
                    : score
            ).ToList();
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