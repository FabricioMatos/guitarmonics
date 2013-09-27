using System.IO;
using System;
using System.Collections.Generic;

namespace Guitarmonics.SongData
{
    public class NoteClass
    {
        public bool HasEffects { get; set; }
        public int? Type { get; set; }
        public byte? Duration { get; set; }
        public byte? NTuplet { get; set; }
        public byte? NoteDynamic { get; set; }
        public byte? Fret { get; set; }
        public byte? Fingering1 { get; set; }
        public byte? Fingering2 { get; set; }
        public byte? StringPlayed;
    }
}
