using System;
using System.Collections.Generic;

namespace Mth.Darts.Cricket
{
    public struct PlayerScore
    {
        internal String player {get; set;}
        internal int order {get; set;}
        internal List<SectionState> states {get; set;}
        internal PlayerScore(String player, int order, List<SectionState> states) : this()
        {
            this.player = player;
            this.order = order;
            this.states = states;
        }

    };
}