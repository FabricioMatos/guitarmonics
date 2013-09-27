using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;

namespace Guitarmonics.Importer.Tests
{
    [TestFixture]
    public class ImportTests2
    {
        public const string BASE_PATH = @"D:\_GuitarMonics\svn-guitarmonics\trunk\Fontes\SongEditor\GuitarProImport.Tests\gpFiles";

        [Test, TestCaseSource(typeof(TestTablaturesFactoryClass), "SongDataTestCases")]
        public void SongData(GpFileTestCase tc)
        {
            var readSong = tc.ReadSong;
            new Importer().Load(BASE_PATH + @"\" + tc.FileTried, readSong);


            Assert.AreEqual(tc.ExpectedSong.Name, readSong.Name);
            Assert.AreEqual(tc.ExpectedSong.Subtitle, readSong.Subtitle);
            Assert.AreEqual(tc.ExpectedSong.Interpret, readSong.Interpret);
            Assert.AreEqual(tc.ExpectedSong.InstructionalLine, readSong.InstructionalLine);
            Assert.AreEqual(tc.ExpectedSong.Album, readSong.Album);
            Assert.AreEqual(tc.ExpectedSong.Author, readSong.Author);
            Assert.AreEqual(tc.ExpectedSong.Copyright, readSong.Copyright);
            Assert.AreEqual(tc.ExpectedSong.TablatureAuthor, readSong.TablatureAuthor);

            Assert.AreEqual(tc.ExpectedSong.NoticeLines, readSong.NoticeLines);
            Assert.AreEqual(tc.ExpectedSong.TripletFeelActivated, readSong.TripletFeelActivated);

            Assert.AreEqual(tc.ExpectedSong.TrackThatLyricsAreFor, readSong.TrackThatLyricsAreFor);

            Assert.AreEqual(tc.ExpectedSong.Tempo, readSong.Tempo);

            Assert.AreEqual(tc.ExpectedSong.Key, readSong.Key);
            Assert.AreEqual(tc.ExpectedSong.Octave, readSong.Octave);

        }


        [Test, TestCaseSource(typeof(TestTablaturesFactoryClass), "MeasuresTestCases")]
        public void Measures(GpFileTestCase tc)
        {
            var readSong = tc.ReadSong;
            new Importer().Load(BASE_PATH + @"\" + tc.FileTried, readSong);

            Assert.AreEqual(tc.ExpectedSong.Measures.Count, readSong.Measures.Count);
            for (int m = 0; m < tc.ExpectedSong.Measures.Count; m++)
            {
                var eMeasure = tc.ExpectedSong.Measures[m];
                var rMeasure = readSong.Measures[m];
                string sMessage = "Measure " + m;
                Assert.AreEqual(eMeasure.KeySignatureDenominator, rMeasure.KeySignatureDenominator, sMessage);
                Assert.AreEqual(eMeasure.KeySignatureNumerator, rMeasure.KeySignatureNumerator, sMessage);
                Assert.AreEqual(eMeasure.Beginning_of_repeat, rMeasure.Beginning_of_repeat, sMessage);
                Assert.AreEqual(eMeasure.EndOfRepeat, rMeasure.EndOfRepeat, sMessage);
                Assert.AreEqual(eMeasure.MarkerName, rMeasure.MarkerName, sMessage);
                Assert.AreEqual(eMeasure.NumberOfAlternateEnding, rMeasure.NumberOfAlternateEnding, sMessage);
                Assert.AreEqual(eMeasure.Tonality, rMeasure.Tonality, sMessage);
                Assert.AreEqual(eMeasure.MarkerColor, rMeasure.MarkerColor, sMessage);
            }

        }

        private static string TrimOrNull(string s)
        {
            if (null == s)
            {
                return null;
            }
            return s.Trim();
        }

        [Test, TestCaseSource(typeof(TestTablaturesFactoryClass), "TracksTestCases")]
        public void Tracks(GpFileTestCase tc)
        {
            var readSong = tc.ReadSong;
            new Importer().Load(BASE_PATH + @"\" + tc.FileTried, readSong);
            bool writeTracks = false;
            if (writeTracks)
            {
                Console.WriteLine("---------" + readSong.Name);
            }

            for (int t = 0; t < readSong.Tracks.Count; t++)
            {
                var rTrack = readSong.Tracks[t];
                if (writeTracks)
                {
                    Console.WriteLine(
                        "song.Tracks.Add(new Track() {" +
                        string.Format(@"                        
                             Name = {0},
                             NumberOfFrets = {1},
                             HeigthOfCapo= {2},
                             TuningOfStrings = new List<int>() {3} ,              
                            ",
                                 "\"" + rTrack.Name + "\"",
                                 rTrack.NumberOfFrets,
                                 rTrack.HeigthOfCapo,
                                 "{" + Mount(rTrack.TuningOfStrings) + "}"
                                 ) +
                                 " }); \n");
                }
            }
            Assert.AreEqual(tc.ExpectedSong.Tracks.Count, readSong.Tracks.Count);
            for (int t = 0; t < tc.ExpectedSong.Tracks.Count; t++)
            {
                var eTrack = tc.ExpectedSong.Tracks[t];
                var rTrack = readSong.Tracks[t];
                string sMessage = "Track " + t;
                Assert.AreEqual(TrimOrNull(eTrack.Name), TrimOrNull(rTrack.Name), sMessage);
                Assert.AreEqual(eTrack.NumberOfFrets, rTrack.NumberOfFrets, sMessage);
                Assert.AreEqual(eTrack.HeigthOfCapo, rTrack.HeigthOfCapo, sMessage);
                Assert.AreEqual(eTrack.IsBanjoTrack, rTrack.IsBanjoTrack, sMessage);
                Assert.AreEqual(eTrack.IsDrumsTrack, rTrack.IsDrumsTrack, sMessage);
                Assert.AreEqual(eTrack.Is12StringedTrack, rTrack.Is12StringedTrack, sMessage);
                //Assert.AreEqual(eTrack.Color, rTrack.Color, sMessage);
                Assert.AreEqual(eTrack.TuningOfStrings, rTrack.TuningOfStrings, sMessage);


            }

        }

        private string Mount(List<int> list)
        {
            string s = "";
            for (int i = 0; i < list.Count; i++)
            {
                if (i > 0)
                {
                    s += ", ";
                }
                s += list[i];
            }
            return s;
        }

        [Test, TestCaseSource(typeof(TestTablaturesFactoryClass), "MidiChannelsTableTestCases")]
        public void MidiChannelsTable(GpFileTestCase tc)
        {
            var readSong = tc.ReadSong;
            new Importer().Load(BASE_PATH + @"\" + tc.FileTried, readSong);
            var ePorts = tc.ExpectedSong.oMidiChannelsTable.Ports;
            var rPorts = readSong.oMidiChannelsTable.Ports;
            for (int p = 0; p < ePorts.Length; p++)
            {
                var eChannels = ePorts[p].Channels;
                var rChannels = rPorts[p].Channels;
                for (int ch = 0; ch < eChannels.Length; ch++)
                {
                    var expectedMidiChannel = eChannels[ch];
                    var readMidiChannel = rChannels[ch];

                    Assert.AreEqual(expectedMidiChannel.Instrument, readMidiChannel.Instrument);

                    Assert.AreEqual(expectedMidiChannel.Volume, readMidiChannel.Volume);
                    Assert.AreEqual(expectedMidiChannel.Balance, readMidiChannel.Balance);
                    Assert.AreEqual(expectedMidiChannel.Chorus, readMidiChannel.Chorus);
                    Assert.AreEqual(expectedMidiChannel.Reverb, readMidiChannel.Reverb);
                    Assert.AreEqual(expectedMidiChannel.Phaser, readMidiChannel.Phaser);
                    Assert.AreEqual(expectedMidiChannel.Tremolo, readMidiChannel.Tremolo);
                }

            }
        }

        [Test, TestCaseSource(typeof(TestTablaturesFactoryClass), "MidiChannelsTableTestCases")]
        public void MidiChannelsTableAsString(GpFileTestCase tc)
        {
            var readSong = tc.ReadSong;
            new Importer().Load(BASE_PATH + @"\" + tc.FileTried, readSong);
            string e = tc.ExpectedSong.oMidiChannelsTable.AsText();
            string r = readSong.oMidiChannelsTable.AsText();
            Assert.AreEqual(e, r);
        }

        [Test, TestCaseSource(typeof(TestTablaturesFactoryClass), "MeasureTrackPairsTestCases")]
        public void MeasureTrackPairs(GpFileTestCase tc)
        {
            var readSong = tc.ReadSong;

            Assert.AreEqual(tc.ExpectedSong.Measures.Count, readSong.Measures.Count);

            int m = tc.MeasureToTest.Value;
            int p = tc.PairToTest.Value;

            {
                var eMeasure = tc.ExpectedSong.Measures[m];
                var rMeasure = readSong.Measures[m];
                Assert.AreEqual(eMeasure.Pairs.Count, rMeasure.Pairs.Count);

                var ePair = eMeasure.Pairs[p];
                var rPair = readSong.Measures[m].Pairs[p];
                string sMessage = "Measure " + m + " Pair " + p;
                Assert.AreEqual(ePair.Beats.Count, rPair.Beats.Count, sMessage);

                for (int b = 0; b < ePair.Beats.Count / ePair.Beats.Count; b++)
                {
                    var eBeat = ePair.Beats[b];
                    var rBeat = rPair.Beats[b];
                    sMessage = "Measure " + m + " Pair " + p + " Beat " + b;
                    Assert.AreEqual(eBeat.Status, rBeat.Status, sMessage);
                    Assert.AreEqual(eBeat.Duration, rBeat.Duration, sMessage);
                    Assert.AreEqual(eBeat.NTuplet, rBeat.NTuplet, sMessage);

                    Assert.AreEqual(eBeat.Notes.Count, rBeat.Notes.Count, sMessage);

                    for (int n = 0; n < eBeat.Notes.Count; n++)
                    {
                        var eNote = eBeat.Notes[n];
                        var rNote = rBeat.Notes[n];
                        Assert.AreEqual(eNote.Type, rNote.Type, sMessage);
                        Assert.AreEqual(eNote.Duration, rNote.Duration, sMessage);
                        Assert.AreEqual(eNote.NTuplet, rNote.NTuplet, sMessage);
                        Assert.AreEqual(eNote.Fret, rNote.Fret, sMessage);

                        Assert.AreEqual(eNote.NoteDynamic, rNote.NoteDynamic, sMessage);
                        Assert.AreEqual(eNote.Fingering1, rNote.Fingering1, sMessage);
                        Assert.AreEqual(eNote.Fingering2, rNote.Fingering2, sMessage);
                        Assert.AreEqual(eNote.HasEffects, rNote.HasEffects, sMessage);
                    }
                }
            }

        }

        [Test]
        public void bitconverter()
        {
            {
                byte[] bs = new byte[10];
                bs[3] = 13;
                int i = BitConverter.ToInt32(bs, 3);
                Assert.AreEqual(13, i);
            }
            {
                byte[] bs = new byte[10];
                bs[3] = 13;
                bs[4] = 10;
                int i = BitConverter.ToInt32(bs, 3);
                Assert.AreEqual(2573, i);
            }
            {
                byte[] bs = new byte[10];
                bs[3] = 10;
                bs[4] = 13;
                bs[5] = 1;
                bs[6] = 1;
                int i = BitConverter.ToInt32(bs, 3);
                Assert.AreEqual(16846090, i);
            }
            {
                byte[] bs = new byte[10];
                bs[2] = 1;
                bs[3] = 10;
                bs[4] = 13;
                bs[5] = 1;
                bs[6] = 1;
                bs[7] = 1;
                int i = BitConverter.ToInt32(bs, 3);
                Assert.AreEqual(16846090, i);
            }
        }

    }
}
