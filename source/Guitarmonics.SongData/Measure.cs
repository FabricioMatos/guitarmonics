using System.IO;
using System;
using System.Collections.Generic;

namespace Guitarmonics.SongData
{
    public enum Keys
    {

        C_FLAT, G_FLAT, D_FLAT, A_FLAT, E_FLAT,
        B_FLAT = 254,
        F,
        C = 0,
        G,
        D,
        A,
        E,
        B, F_SHARP, C_SHARP
    }

    public class Measure
    {
        public bool Presence_of_a_double_bar = false;
        public bool Beginning_of_repeat = false;
        public byte? KeySignatureNumerator { get; set; }
        public byte? KeySignatureDenominator { get; set; }
        public byte? EndOfRepeat { get; set; }
        public byte? NumberOfAlternateEnding { get; set; }
        public string MarkerName { get; set; }
        public int? MarkerColor { get; set; }
        public byte? Tonality { get; set; }

        public IList<MeasureTrackPair> Pairs;

        public Measure()
        {
            KeySignatureNumerator = null;
            KeySignatureDenominator = null;
            EndOfRepeat = null;
            NumberOfAlternateEnding = null;
            MarkerColor = null;
            Tonality = null;
            MarkerName = "";
            Pairs = new List<MeasureTrackPair>();
        }
    }
}
