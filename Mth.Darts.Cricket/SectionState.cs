using System;

namespace Mth.Darts.Cricket
{
    // An individual section state is meaningless on its own
    // It is defined by its data so a struct is a natural fit
    public struct SectionState
    {
        internal Section section {get; set;}
        internal int count {get; set;}
        internal SectionState(Section section, int count) : this()
        {
            this.section = section;
            this.count = count;
        }
    }
}