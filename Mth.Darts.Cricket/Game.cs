using System;
using System.Collections.Generic;
using System.Linq;

namespace Mth.Darts.Cricket
{
    public class Game {
        public List<GameScore> scores;
        public GameStatus status;

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
        public Game (List<String> players) {
            scores = players.AsEnumerable()
                    .Select((player, index) => 
                        new GameScore (player, index, InitSectionStates())
                    ).ToList();

            status = new GameStatus (players.First(), 1, 1, false);
        }
    }
    // There are a fixed set of sections on a dartboard
    // only these values are valid for the game of Cricket
    public enum Section {
        Twenty,
        Nineteen,
        Eighteen,
        Seventeen,
        Sixteen,
        Fifteen,
        Bull
    }
    // The status of a game is not self-contained, it depends upon ...
    // current scores and future throws, managed in Game class
    public struct GameStatus {
        internal String currentPlayer{ get; }
        internal int currentDart{ get; }
        internal int currentRound{ get; }
        internal Boolean complete{ get; }
        internal GameStatus (String currentPlayer, int currentDart, int currentRound, Boolean complete) : this() {
            this.currentPlayer = currentPlayer;
            this.currentDart = currentDart;
            this.currentRound = currentRound;
            this.complete = complete;
        }
    };
    // An individual section state is meaningless on its own
    // Managed within the Game class
    public struct SectionState {
        internal Section section{ get; }
        internal int count{ get; }
        internal SectionState (Section section, int count) : this() {
            this.section = section;
            this.count = count;
        }
    }
    public struct GameScore {
        internal String player{ get; }
        internal int order{ get; }
        internal List<SectionState> states{ get; }
        internal GameScore (String player, int order, List<SectionState> states) : this() {
            this.player = player;
            this.order = order;
            this.states = states;
        }

    };
}