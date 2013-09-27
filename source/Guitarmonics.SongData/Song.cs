using System.IO;
using System;
using System.Collections.Generic;

namespace Guitarmonics.SongData
{
    public class Song
    {
        public const string STR_V1 = "FICHIER GUITAR PRO v1";
        public const string STR_V1_01 = "FICHIER GUITAR PRO v1.01";
        public const string STR_V1_02 = "FICHIER GUITAR PRO v1.02";
        public const string STR_V1_03 = "FICHIER GUITAR PRO v1.03";
        public const string STR_V1_04 = "FICHIER GUITAR PRO v1.04";
        public const string STR_V2_20 = "FICHIER GUITAR PRO v2.20";
        public const string STR_V2_21 = "FICHIER GUITAR PRO v2.21";
        public const string STR_V3_00 = "FICHIER GUITAR PRO v3.00";
        public const string STR_V4_00 = "FICHIER GUITAR PRO v4.00";
        public const string STR_V4_06 = "FICHIER GUITAR PRO v4.06";
        public const string STR_L4_06 = "FICHIER GUITAR PRO L4.06";

        public string Name { get; set; }
        public string Subtitle { get; set; }
        public string Interpret { get; set; }
        public string Album { get; set; }
        public string Author { get; set; }
        public string Copyright { get; set; }
        public string TablatureAuthor { get; set; }
        public string InstructionalLine { get; set; }

        public IList<string> NoticeLines { get; set; }
        public bool TripletFeelActivated { get; set; }

        public int Tempo { get; set; }
        public int Key { get; set; }
        public int Octave { get; set; }


        public int TrackThatLyricsAreFor { get; set; }

        public MidiChannelsTable oMidiChannelsTable { get; private set; }

        public IList<Measure> Measures { get; private set; }
        public IList<Track> Tracks { get; private set; }

        public string MeasuresToString()
        {
            string result = "";
            for (int i = 0; i < Measures.Count; i++)
            {
                string sLine = "";
                var m = Measures[i];
                if (m.KeySignatureNumerator != 4)
                {
                    sLine += "song.Measures[" + i + "].KeySignatureNumerator = " + m.KeySignatureNumerator + ";\n";
                }
                if (m.KeySignatureDenominator != 4)
                {
                    sLine += "song.Measures[" + i + "].KeySignatureDenominator = " + m.KeySignatureDenominator + ";\n";
                }
                if (m.Beginning_of_repeat)
                {
                    sLine += "song.Measures[" + i + "].Beginning_of_repeat= true;\n";
                }
                if (m.EndOfRepeat != null)
                {
                    sLine += "song.Measures[" + i + "].EndOfRepeat = " + m.EndOfRepeat + ";\n";
                }
                if (m.MarkerName != "")
                {
                    sLine += "song.Measures[" + i + "].MarkerName= \"" + m.MarkerName + "\";\n";
                }

                if (sLine != "")
                {
                    result += "\n" + sLine;
                }
            }
            return result;
        }
        public Song()
        {
            Name = "";
            Subtitle = "";
            Interpret = "";
            Album = "";
            Author = "";
            Copyright = "";
            InstructionalLine = "";
            TablatureAuthor = "";
            NoticeLines = new List<string>();
            TripletFeelActivated = false;
            Tempo = 0;

            oMidiChannelsTable = new MidiChannelsTable();
            Measures = new List<Measure>();
            Tracks = new List<Track>();
        }

    }
}
