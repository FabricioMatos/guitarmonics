using System.IO;
using System;
using System.Collections.Generic;

namespace Guitarmonics.SongData
{
    public class MeasureTrackPair
    {
        public IList<Beat> Beats { get; set; }
        public MeasureTrackPair()
        {
            Beats = new List<Beat>();
        }
    }
}
