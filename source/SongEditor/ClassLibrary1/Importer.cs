using System.IO;
using System;
using System.Collections.Generic;
using System.Collections;
using Guitarmonics.SongData;

namespace Guitarmonics.Importer
{
    public class Importer
    {
        private string ErrorMessage;
        private const int BYTES_TO_SKIP_LYRICS = 40;
        private const int BYTES_TO_SKIP_AFTER_OCTAVE = 3;

        private GP4Reader Reader;
        public string CheckVersion(string versionInfo)
        {
            if (versionInfo.Equals(Song.STR_V4_06))
            {
                return "";
            }

            return VersionInfoError(versionInfo);
        }


        private static string VersionInfoError(string versionInfo)
        {
            foreach (string reconizedConstant in new string[] { Song.STR_V1, Song.STR_V1_01, Song.STR_V1_02, Song.STR_V1_03, Song.STR_V1_04, Song.STR_V2_20, Song.STR_V2_21, Song.STR_V3_00, Song.STR_V4_00, Song.STR_L4_06 })
            {
                if (versionInfo.Equals(reconizedConstant))
                {
                    return "This file was reconized as a file for GuitarPRO " + reconizedConstant.Replace("FICHIER GUITAR PRO ", "") + " and couldn't be open.";
                }
            }

            return "This file was not reconized as GuitarPRO.";
        }
        public Importer()
        {
            Reader = new GP4Reader();
        }

        public string Load(string sFilePath, Song song)
        {
            try
            {

                string load2 = Load2(sFilePath, song);
                Reader.AnnotatedOutput.WriteLine("error = " + load2);
                return load2;
            }
            finally
            {

                Reader.Close();
            }
        }

        private string Load2(string sFilePath, Song song)
        {
            ErrorMessage = "";

            Reader.ReadAllBytes(sFilePath);

            {
                int iLen = Reader.readByte("Length of VersionInfo");
                string versionInfo = "";

                for (int i = 0; i < iLen; i++)
                {
                    versionInfo += (char)Reader.readByte("VersionInfo char #" + i);
                }
                string errorMessage = CheckVersion(versionInfo);
                if (errorMessage != "")
                {
                    return errorMessage;
                }
                for (int i = iLen; i < 30; i++)
                {
                    readByte("skip " + i);
                }
            }

            ReadBasicSongData(song);


            //ignoring lyrics session
            int trackLyricsFor = readInteger("");

            for (int i = 0; i < BYTES_TO_SKIP_LYRICS; i++)
            {
                readByte("skip lyrics " + i);
            }

            song.Tempo = readInteger("");

            song.Key = readByte("Key");

            song.Octave = readByte("Octave");

            for (int i = 0; i < BYTES_TO_SKIP_AFTER_OCTAVE; i++)
            {
                readByte("skip after octave " + i);
            }

            ReadMidiChannelsTable(song);
            int numberOfMeasures = readInteger("");
            int numberOfTracks = readInteger("");

            Measure previousMeasure = null;
            bool insideRepeat = false;
            for (int i = 0; i < numberOfMeasures; i++)
            {
                var newMeasure = readMeasure(previousMeasure, i);

                song.Measures.Add(newMeasure);

                if (newMeasure.Tonality == null)
                {
                    throw new Exception();
                }
                 
                if (insideRepeat && (newMeasure.EndOfRepeat != null))
                {
                    insideRepeat = false;
                }
                else
                {
                    if (!insideRepeat && newMeasure.Beginning_of_repeat)
                    {
                        insideRepeat = true;
                    }
                }
                previousMeasure = newMeasure;
            }
            //strange extra mesure
            readMeasure(new Measure(), numberOfMeasures);


            for (int i = 0; i < numberOfTracks; i++)
            {
                song.Tracks.Add(readTrack(i));
            }

            for (int m = 0; m < numberOfMeasures; m++)
            {
                var measure = song.Measures[m];
                for (int t = 0; t < numberOfTracks; t++)
                {
                    measure.Pairs.Add(readPair(m, t));
                }
            }
            if (Reader.currentByte >= Reader.FileStreamLength)
            {
                ErrorMessage = "passed the end";
            }
            return ErrorMessage;
        }

        private Measure readMeasure(Measure previousMeasure, int i)
        {
            byte bHeader = readByte("Measure " + i + " header");

            var ba = new BitArray(new byte[] { bHeader });

            var newMeasure = new Measure();

            
            bool Presence_of_a_double_bar = ba[7];
            newMeasure.Beginning_of_repeat = ba[2];
            /////////////////////////
            if (previousMeasure != null)
            {
                newMeasure.KeySignatureDenominator = previousMeasure.KeySignatureDenominator;
                newMeasure.KeySignatureNumerator = previousMeasure.KeySignatureNumerator;
                newMeasure.Tonality = previousMeasure.Tonality;
            }

            if (ba[0])
            {
                newMeasure.KeySignatureNumerator = readByte("Measure " + i + " KeySignatureNumerator");
            }
            if (ba[1])
            {
                newMeasure.KeySignatureDenominator = readByte("Measure " + i + " KeySignatureDenominator");
            }

            if (ba[3])
            {
                newMeasure.EndOfRepeat = readByte("Measure " + i + " EndOfRepeat");
            }

            if (ba[4])
            {
                newMeasure.NumberOfAlternateEnding = readByte("Measure " + i + " NumberOfAlternateEnding");
            }

            if (ba[5])
            {
                //readInteger();
                newMeasure.MarkerName = readString();
                newMeasure.MarkerColor = readInteger("");
            }

            if (ba[6])
            {
                newMeasure.Tonality = readByte("Measure " + i + " Tonality");
            }
            return newMeasure;
        }

        private void ReadMidiChannelsTable(Song song)
        {

            for (int i = 0; i < song.oMidiChannelsTable.Ports.Length; i++)
            {
                for (int j = 0; j < song.oMidiChannelsTable.Ports[i].Channels.Length; j++)
                {
                    var channel = song.oMidiChannelsTable.Ports[i].Channels[j];
                    channel.Instrument = readInteger("");
                    channel.Volume = readByte(string.Format("Port {0} Channel {1} Volume", i, j));
                    channel.Balance = readByte(string.Format("Port {0} Channel {1} Balance", i, j));
                    channel.Chorus = readByte(string.Format("Port {0} Channel {1} Chorus", i, j));
                    channel.Reverb = readByte(string.Format("Port {0} Channel {1} Reverb", i, j));
                    channel.Phaser = readByte(string.Format("Port {0} Channel {1} Phaser", i, j));
                    channel.Tremolo = readByte(string.Format("Port {0} Channel {1} Tremolo", i, j));

                    Reader.readByte("skip");
                    Reader.readByte("skip");
                }
            }
        }

        private void ReadBasicSongData(Song song)
        {
            song.Name = readString();
            song.Subtitle = readString();
            song.Interpret = readString();
            song.Album = readString();
            song.Author = readString();
            song.Copyright = readString();
            song.TablatureAuthor = readString();
            song.InstructionalLine = readString();

            int noticeLinesCount = readInteger("");
            for (int i = 0; i < noticeLinesCount; i++)
            {
                song.NoticeLines.Add(readString());
            }

            song.TripletFeelActivated = (Reader.readByte("TripletFeelActivated") == 1);
        }

        private Track readTrack(int i)
        {
            var newTrack = new Track();
            if (ErrorMessage != "")
            {
                return newTrack;
            }
            if (Reader.currentByte >= Reader.FileStreamLength)
            {
                return newTrack;
            }

            byte header = readByte("Track " + i + " header");

            newTrack.Name = readString2();
            int numberOfStrings = readInteger("Track " + i + " numberOfStrings");
            if (numberOfStrings > 7)
            {
                ErrorMessage = "numberOfStrings=" + numberOfStrings;
                return newTrack;
            }

            for (int j = 0; j < numberOfStrings; j++)
            {
                newTrack.TuningOfStrings.Add(readInteger("string " + j));
            }
            for (int j = numberOfStrings; j < 7; j++)
            {
                readInteger("no string at " + j);
            }
            readInteger("skip");
            readInteger("skip");
            readInteger("skip");
            newTrack.NumberOfFrets = readInteger("NumberOfFrets");
            newTrack.HeigthOfCapo = readInteger("HeigthOfCapo");
            newTrack.Color = readInteger("Color");
            return newTrack;
        }

        public int readShortInteger()
        {
            if (Reader.currentByte + 2 >= Reader.FileStreamLength)
            {
                return 0;
            }
            var bs = new byte[2];
            bs[0] = readByte("short int byte 0");
            bs[1] = readByte("short int byte 1");

            return BitConverter.ToInt16(bs, 0);
        }


        public string readString2()
        {
            if (Reader.currentByte >= Reader.FileStreamLength)
            {
                return "";
            }
            string s = "";

            int iLen = readByte("string2 length");

            for (int i = 0; i < iLen; i++)
            {
                s += (char)readByte("string 2 char " + i);
            }

            for (int i = iLen; i < 40; i++)
            {
                readByte("skip");
            }

            return s;

        }

        public int readInteger(string extraComment)
        {
            if (Reader.currentByte + 4 >= Reader.FileStreamLength)
            {
                return 0;
            }
            var bs = new byte[4];
            bs[0] = readByte("int byte 0");
            bs[1] = readByte("int byte 1");
            bs[2] = readByte("int byte 2");
            //bs[3] = 
            readByte("int byte 3 ==> " + extraComment);

            int integerRead = BitConverter.ToInt32(bs, 0); ;

            Reader.AnnotatedOutput.Write(" --> " + integerRead);

            return integerRead;
        }

        public string readString()
        {
            string s = "";
            int nextInt = readInteger("string length - 1");
            byte nextByte = readByte("string next byte");

            if (nextInt == 0 && nextByte == 0)
            {
                ;
            }
            else
            {
                int iLen = nextInt - 1;

                if (Reader.currentByte + iLen >= Reader.FileStreamLength)
                {
                    return "estourou";
                }

                for (int i = 0; i < iLen; i++)
                {
                    s += (char)readByte("string char " + i);
                }
            }
            return s;
        }

        public byte readByte(string extraComment)
        {
            return Reader.readByte(ErrorMessage + extraComment);
        }

        private MeasureTrackPair readPair(int m, int p)
        {

            var newPair = new MeasureTrackPair();
            if (ErrorMessage != "")
            {
                return newPair;
            }
            if (Reader.currentByte >= Reader.FileStreamLength)
            {
                return newPair;
            }
            int numberOfBeats = readInteger("Measure " + m + " Pair " + p + " numberOfBeats");
            if (numberOfBeats > 16)
            {
                ErrorMessage = "TOO MANY BEATS: " + numberOfBeats;
                return newPair;
            }

            for (int b = 0; b < numberOfBeats; b++)
            {
                newPair.Beats.Add(readBeat(p, b, m));
            }

            return newPair;
        }

        private Beat readBeat(int p, int b, int m)
        {
            var newBeat = new Beat();

            if (Reader.currentByte >= Reader.FileStreamLength)
            {
                return newBeat;
            }

            string beatDesc = "Measure " + m + " Pair " + p + " Beat " + b;
            string noteDesc = beatDesc + " Note ";
            byte bHeader = readByte(beatDesc + " header");
            {
                var ba = new BitArray(new byte[] { bHeader });

                var h = new
                {
                    HasStatus = ba[6],
                    IsNtuplet = ba[5],
                    Presence_of_a_mix_table_change_event = ba[4],
                    Presence_of_effects = ba[3],
                    Presence_of_a_text = ba[2],
                    Presence_of_a_chord_diagram = ba[1],
                    Dotted_notes = ba[0],
                };
                if (h.HasStatus)
                {
                    newBeat.Status = readByte(beatDesc + " status");
                }
                newBeat.Duration = readByte(beatDesc + " duration");
                if (h.Presence_of_a_mix_table_change_event)
                {
                    string newVariable = "Mix table change event ";
                    readByte(newVariable + "Instrument");
                    readByte(newVariable + "Volume");
                    readByte(newVariable + "Pan");
                    readByte(newVariable + "Chorus");
                    readByte(newVariable + "Reverb");
                    readByte(newVariable + "Phaser");
                    readByte(newVariable + "Tremolo");
                    readByte(newVariable + "Tempo");
                    readByte(newVariable + "Volume change duration");
                    readByte(newVariable + "Pan change duration");
                    readByte(newVariable + "Chorus change duration");
                    readByte(newVariable + "Reverb change duration");
                    readByte(newVariable + "Phaser change duration");
                    readByte(newVariable + "Tremolo change duration");
                    readByte(newVariable + "Tempo change duration");
                    readByte(newVariable + "indicates chage");
                    /*The next byte precises if the changes apply only to the current track (if the 
                    matching bit is 0), or to every track (if it is 1).*/
                }
                if (h.IsNtuplet)
                {
                    newBeat.NTuplet = readInteger(beatDesc + " NTuplet ");
                }
                if (h.Presence_of_a_text)
                {
                    newBeat.Text = readString();
                }
                readNotes(newBeat, noteDesc);
            }
            return newBeat;
        }

        private void readNotes(Beat newBeat, string noteDesc)
        {
            var headerStrings = readByte("strings played");
            var ba = new BitArray(new byte[] { headerStrings });
            for (byte i = 0; i < ba.Length; i++)
            {
                if (ba[i])
                {
                    Reader.AnnotatedOutput.Write(" note at string " + i);
                    var note = new NoteClass();
                    note.StringPlayed = i;
                    newBeat.Notes.Add(note);
                    readNote(noteDesc, note);
                }
            }

        }

        private void readNote(string noteDesc, NoteClass note)
        {
            byte noteHeader = readByte(noteDesc + " header");
            var ba = new BitArray(new byte[] { noteHeader });

            if (ba[5])
            {
                note.Type = readByte("era pra ser um short int");// readShortInteger();
            }
            if (ba[0])
            {
                note.Duration = readByte(noteDesc + " Duration");
                note.NTuplet = readByte(noteDesc + " NTuplet");
            }
            if (ba[3])
            {
                readByte(noteDesc + " Effects First byte");
                readByte(noteDesc + " Effects Second byte");

            }

            if (ba[4])
            {
                note.NoteDynamic = readByte(noteDesc + " Dynamic");
            }
            else
            {
                note.NoteDynamic = 7;// 6;
            }

            if (ba[5])
            {
                note.Fret = readByte(noteDesc + " Fret");
            }
            if (ba[7])
            {
                note.Fingering1 = readByte(noteDesc + " Fingering1");
                note.Fingering2 = readByte(noteDesc + " Fingering2");
            }
        }

    }
}