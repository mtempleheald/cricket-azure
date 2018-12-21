using System;
using Newtonsoft.Json;

namespace Mth.Darts.Cricket
{

    public struct MatchScore
    {
        [JsonProperty()]
        internal String player { get; }
        [JsonProperty()]
        internal int points { get; }
        [JsonProperty()]
        internal int ranking { get; }
        internal MatchScore(String player, int points, int ranking) : this()
        {
            this.player = player;
            this.points = points;
            this.ranking = ranking;
        }
    }
}