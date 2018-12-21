using System;
using System.Collections.Generic;
using Newtonsoft.Json;

namespace Mth.Darts.Cricket
{
    public struct GameScore
    {
        
        [JsonProperty()]
        internal String player {get; set;}
        [JsonProperty()]
        internal int order {get; set;}
        [JsonProperty()]
        internal List<GameScoreSectionState> states {get; set;}
        [JsonProperty()]
        internal int points {get; set;}
        [JsonProperty()]
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