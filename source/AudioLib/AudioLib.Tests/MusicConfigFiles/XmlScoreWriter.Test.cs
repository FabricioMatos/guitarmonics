using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.AudioLib.MusicConfigFiles;
using Toub.Sound.Midi;
using Guitarmonics.AudioLib.Midi;
using System.IO;
using System.Xml;
using Guitarmonics.AudioLib.Tests;
using Guitarmonics.SongData;

namespace Guitarmonics.AudioLib.MusicConfigFiles.Tests
{
    [TestFixture]
    public class XmlScoreWriterTest
    {
        private const string MIDI_for_whom_the_bell_tolls = TestConfig.ConstAudioPath + "metallica-for_whom_the_bell_tolls.mid";
        private const string SongFile_for_whom_the_bell_tolls = TestConfig.ConstAudioPath + "metallica-for_whom_the_bell_tolls.song.xml";
        private const string LinkedSongFile_for_whom_the_bell_tolls = TestConfig.ConstAudioPath + "metallica-for_whom_the_bell_tolls(linked).song.xml";
        private const string SongFile_TesteOk = TestConfig.ConstAudioPath + "TesteOk.song.xml";
        private const string MIDI_when_you_where_young = TestConfig.ConstAudioPath + "The Killers - When You Were Young.mid";
        private const string SongFile_when_you_where_young = TestConfig.ConstAudioPath + "The Killers - When You Were Young (mid).song.xml";



        //[TestCase(MIDI_for_whom_the_bell_tolls, SongFile_for_whom_the_bell_tolls, "Metallica", "For Whom the Bell Tolls", 2, 120, 0)]
        [TestCase(MIDI_when_you_where_young, SongFile_when_you_where_young, "The Killers", "When You Were Young", 1, 130, 0)]
        public void ImportMidiAndSaveToFile(string pMidiFile, string pXmlFile, string pArtist,
            string pTitle, int pTrack, int pBpm, int pSkipBeats)
        {
            var sequence = MidiSequence.Import(pMidiFile);

            var track = sequence.GetTracks()[pTrack];

            MidiEventCollection midiEvents = track.Events;

            var midiImporter = new GuitarMidiImporter(midiEvents, pBpm, pSkipBeats);
            var song = new Song()
            {
                Author = pArtist,
                Name = pTitle,
            };
            var xmlScoreWriter = new XmlScoreWriter(song, PlayingMode.EletricGuitarScore, midiImporter.ScoreNotes);

            if (File.Exists(pXmlFile))
                File.Delete(pXmlFile);

            xmlScoreWriter.SaveXmlNotesToFile(pXmlFile);

            Assert.IsTrue(File.Exists(pXmlFile));
        }


        [Test]
        public void SongHeader()
        {
            var notes = new SortedList<ScoreNote, ScoreNote>();

            var artist = "Metallica";
            var title = "For Whom the Bell Tolls";

            var xmlScoreWriter = new XmlScoreWriter(artist, title, PlayingMode.EletricGuitarScore, notes);

            string songHeader = string.Format(
                "<Song Artist=\"{0}\" Title=\"{1}\" PlayingMode=\"{2}\">",
                artist, title, PlayingMode.EletricGuitarScore);

            Assert.AreEqual(songHeader, xmlScoreWriter.GenerateSongHeader());
        }

        [Test]
        public void SongSyncHeader()
        {
            var notes = new SortedList<ScoreNote, ScoreNote>();

            var artist = "Metallica";
            var title = "For Whom the Bell Tolls";

            var xmlScoreWriter = new XmlScoreWriter(artist, title, PlayingMode.EletricGuitarScore, notes);

            string songSyncHeader = string.Format(
                "<SongSync Artist=\"{0}\" Title=\"{1}\" PlayingMode=\"{2}\">",
                artist, title, PlayingMode.EletricGuitarScore);

            Assert.AreEqual(songSyncHeader, xmlScoreWriter.GenerateSongSyncHeader());
        }

        [Test]
        public void EmptyScoreListToXmlNotes()
        {
            var notes = new SortedList<ScoreNote, ScoreNote>();

            var artist = "Artist X";
            var title = "Song Y";

            var xmlScoreWriter = new XmlScoreWriter(artist, title, PlayingMode.EletricGuitarScore, notes);

            Assert.IsNotNull(xmlScoreWriter);

            string emptyXml = string.Format(
                "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + "\r\n" +
                "<Song Artist=\"{0}\" Title=\"{1}\" PlayingMode=\"{2}\">" + "\r\n" +
                "</Song>" + "\r\n",
                artist, title, PlayingMode.EletricGuitarScore);


            Assert.AreEqual(emptyXml, xmlScoreWriter.ToXmlNotes());
        }

        [Test]
        public void EmptyScoreListToXmlSync()
        {
            var notes = new SortedList<ScoreNote, ScoreNote>();

            var artist = "Artist X";
            var title = "Song Y";

            var xmlScoreWriter = new XmlScoreWriter(artist, title, PlayingMode.EletricGuitarScore, notes);

            Assert.IsNotNull(xmlScoreWriter);

            string emptyXml = string.Format(
                "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + "\r\n" +
                "<SongSync Artist=\"{0}\" Title=\"{1}\" PlayingMode=\"{2}\">" + "\r\n" +
                "</SongSync>" + "\r\n",
                artist, title, PlayingMode.EletricGuitarScore);


            Assert.AreEqual(emptyXml, xmlScoreWriter.ToXmlSync());
        }

        [Test]
        public void GenerateScoreNoteElementForOneScoreNote()
        {
            var notes = new SortedList<ScoreNote, ScoreNote>();

            var artist = "Metallica";
            var title = "For Whom the Bell Tolls";

            var xmlScoreWriter = new XmlScoreWriter(artist, title, PlayingMode.EletricGuitarScore, notes);

            var scoreNote = new ScoreNote("A4", 1, 0, 10, 62250);

            string xmlScoreNote = "\t<ScoreNote Beat=\"1\" Tick=\"0\" NoteId=\"A4\" Duration=\"10\"/>";

            Assert.AreEqual(xmlScoreNote, xmlScoreWriter.GenerateScoreNoteElement(scoreNote));
        }

        [Test]
        public void GenerateSyncElementForOneScoreNote()
        {
            var notes = new SortedList<ScoreNote, ScoreNote>();

            var artist = "Metallica";
            var title = "For Whom the Bell Tolls";

            var xmlScoreWriter = new XmlScoreWriter(artist, title, PlayingMode.EletricGuitarScore, notes);

            var scoreNote = new ScoreNote("A4", 1, 0, 10, 62250);

            string xmlScoreNote = "\t<ScoreNote Beat=\"1\" Tick=\"0\" SyncSongPin=\"1:2:250\"/>";

            Assert.AreEqual(xmlScoreNote, xmlScoreWriter.GenerateSyncElement(scoreNote));
        }

        [Test]
        public void GenerateScoreNoteElementForOneGuitarScoreNote()
        {
            var notes = new SortedList<GuitarScoreNote, GuitarScoreNote>();

            var artist = "Metallica";
            var title = "For Whom the Bell Tolls";

            var xmlScoreWriter = new XmlScoreWriter(artist, title, PlayingMode.EletricGuitarScore, notes);

            var scoreNote = new GuitarScoreNote("A4", 1, 0, 10, null);

            string xmlScoreNote = "\t<ScoreNote Beat=\"1\" Tick=\"0\" NoteId=\"A4\" Duration=\"10\" String=\"3\" Fret=\"2\" RemarkOrChordName=\"\"/>";

            Assert.AreEqual(xmlScoreNote, xmlScoreWriter.GenerateScoreNoteElement(scoreNote));
        }

        [Test]
        public void GenerateScoreNoteElementFor3GuitarScoreNote()
        {
            var notes = new SortedList<GuitarScoreNote, GuitarScoreNote>();

            {
                var n = new GuitarScoreNote("A4", 1, 0, 240, 0);
                notes.Add(n, n);
            }
            {
                var n = new GuitarScoreNote("B4", 1, 240, 240, 250);
                notes.Add(n, n);
            }
            {
                var n = new GuitarScoreNote("C5", 2, 0, 960, 500);
                notes.Add(n, n);
            }

            var artist = "Artist X";
            var title = "Song Y";

            var xmlScoreWriter = new XmlScoreWriter(artist, title, PlayingMode.EletricGuitarScore, notes);

            var expectedXml = new StringBuilder();

            //Prepera the expected XML
            expectedXml.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            expectedXml.AppendLine(string.Format("<Song Artist=\"{0}\" Title=\"{1}\" PlayingMode=\"{2}\">",
                artist, title, PlayingMode.EletricGuitarScore));
            expectedXml.AppendLine(string.Format("\t<ScoreNote Beat=\"{0}\" Tick=\"{1}\" NoteId=\"{2}\" Duration=\"{3}\" String=\"3\" Fret=\"2\" RemarkOrChordName=\"\"/>",
                1, 0, "A4", 240));
            expectedXml.AppendLine(string.Format("\t<ScoreNote Beat=\"{0}\" Tick=\"{1}\" NoteId=\"{2}\" Duration=\"{3}\" String=\"2\" Fret=\"0\" RemarkOrChordName=\"\"/>",
                1, 240, "B4", 240));
            expectedXml.AppendLine(string.Format("\t<ScoreNote Beat=\"{0}\" Tick=\"{1}\" NoteId=\"{2}\" Duration=\"{3}\" String=\"2\" Fret=\"1\" RemarkOrChordName=\"\"/>",
                2, 0, "C5", 960));
            expectedXml.AppendLine("</Song>");

            Assert.AreEqual(expectedXml.ToString(), xmlScoreWriter.ToXmlNotes());
        }

        [Test]
        public void GenerateSyncElementFor3GuitarScoreNote()
        {
            var notes = new SortedList<GuitarScoreNote, GuitarScoreNote>();

            {
                var n = new GuitarScoreNote("A4", 1, 0, 240, 0);
                notes.Add(n, n);
            }
            {
                var n = new GuitarScoreNote("B4", 1, 240, 240, 250);
                notes.Add(n, n);
            }
            {
                var n = new GuitarScoreNote("C5", 2, 0, 960, 500);
                notes.Add(n, n);
            }

            var artist = "Artist X";
            var title = "Song Y";

            var xmlScoreWriter = new XmlScoreWriter(artist, title, PlayingMode.EletricGuitarScore, notes);

            var expectedXml = new StringBuilder();

            //Prepera the expected XML
            expectedXml.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            expectedXml.AppendLine(string.Format("<SongSync Artist=\"{0}\" Title=\"{1}\" PlayingMode=\"{2}\">",
                artist, title, PlayingMode.EletricGuitarScore));
            expectedXml.AppendLine(string.Format("\t<ScoreNote Beat=\"{0}\" Tick=\"{1}\" SyncSongPin=\"0:0:0\"/>",
                1, 0, "A4", 240));
            expectedXml.AppendLine(string.Format("\t<ScoreNote Beat=\"{0}\" Tick=\"{1}\" SyncSongPin=\"0:0:250\"/>",
                1, 240, "B4", 240));
            expectedXml.AppendLine(string.Format("\t<ScoreNote Beat=\"{0}\" Tick=\"{1}\" SyncSongPin=\"0:0:500\"/>",
                2, 0, "C5", 960));
            expectedXml.AppendLine("</SongSync>");

            Assert.AreEqual(expectedXml.ToString(), xmlScoreWriter.ToXmlSync());
        }

    }
}
