using System.IO;
using System;
using System.Collections.Generic;

namespace Guitarmonics.SongData
{
    public class Track
    {
        public string Name { get; set; }
        public bool IsBanjoTrack { get; set; }
        public bool IsDrumsTrack { get; set; }
        public bool Is12StringedTrack { get; set; }
        public int NumberOfFrets { get; set; }
        public int HeigthOfCapo { get; set; }
        public int Color { get; set; }
        public List<int> TuningOfStrings { get; set; }

        public Track()
        {
            IsBanjoTrack = false;
            Is12StringedTrack = false;
            IsDrumsTrack = false;
            TuningOfStrings = new List<int>();
        }
    }
}
