using System;

namespace Mth.Darts.Cricket
{

    public struct MatchScore
    {
        internal String player { get; }
        internal int points { get; }
        internal int ranking { get; }
        public MatchScore(String player, int points, int ranking) : this()
        {
            this.player = player;
            this.points = points;
            this.ranking = ranking;
        }
    }
}