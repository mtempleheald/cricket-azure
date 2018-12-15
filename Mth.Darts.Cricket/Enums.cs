using System;

namespace Mth.Darts.Cricket
{

    // There are a fixed set of sections on a dartboard
    // Cricket recognises this subset of sections
    public enum Section {
        //None,
        Twenty = 20,
        Nineteen = 19,
        Eighteen = 18,
        Seventeen = 17,
        Sixteen = 16,
        Fifteen = 15,
        Bull = 25
    }
    // A dartboard splits each section into the following beds
    // Note that Treble is not allowable for the Bull section
    public enum Bed {
        //None = 0,
        Single = 1,
        Double = 2,
        Treble = 3
    }
    // Scoring Mode has a few possible values, enum is a natural fit
    // The addition of a new mode will require new functionality
    public enum ScoringMode
    {
        Standard,
        CutThroat
    };

}