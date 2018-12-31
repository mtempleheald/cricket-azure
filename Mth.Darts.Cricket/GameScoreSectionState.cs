using System;
using Newtonsoft.Json;

namespace Mth.Darts.Cricket
{
    // An individual section state is meaningless on its own
    // It is defined by its data so a struct is a natural fit
    public struct GameScoreSectionState
    {
        [JsonProperty()]
        internal Section section { get; set; }
        [JsonProperty()]
        internal int count { get; set; }

        [JsonConstructor]
        internal GameScoreSectionState(Section section, int count) : this()
        {
            this.section = section;
            this.count = count;
        }
    }
}