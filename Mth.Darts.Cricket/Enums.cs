using System;

namespace Mth.Darts.Cricket
{

    /// <summary>
    /// Section - A dartboard is made up of sections, the game of cricket recognises a subset of these
    /// </summary>
    public enum Section
    {
        //None,
        Twenty = 20,
        Nineteen = 19,
        Eighteen = 18,
        Seventeen = 17,
        Sixteen = 16,
        Fifteen = 15,
        Bull = 25
    }
    /// <summary>
    /// Bed - Each section of a dartboard consists of up to 3 beds
    /// Note that a Treble bed is not valid for the Bull section
    /// </summary>
    public enum Bed
    {
        //None = 0,
        Single = 1,
        Double = 2,
        Treble = 3
    }
    /// <summary>
    /// ScoringMode - Cricket game scoring mode influences game logic
    /// </summary>
    public enum ScoringMode
    {
        Standard,
        CutThroat
    };

    /// <summary>
    /// MatchGameScore - For a match with multiple games, score each game independently and total for overall scores and rankings
    /// </summary>
    internal enum MatchGameScore
    {
        First = 5,
        Second = 3,
        Third = 1,
        Other = 0
    }

}