using System;
using System.Collections.Generic;

namespace Mth.Darts.Cricket
{
    public struct GameScore
    {
        internal String player {get; set;}
        internal int order {get; set;}
        internal List<GameScoreSectionState> states {get; set;}
        internal int points {get; set;}
        internal int ranking {get; set;}
        internal GameScore(String player, int order, List<GameScoreSectionState> states, int points = 0, int ranking = 0) : this()
        {
            this.player = player;
            this.order = order;
            this.states = states;
            this.points = points;
            this.ranking = ranking;
        }

    };
}