using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.MusicXml;
using NUnit.Framework;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.AudioLib.MusicConfigFiles;
using Guitarmonics.AudioLib.Midi;
using System.IO;
using System.Xml;
using Guitarmonics.AudioLib.Tests;


namespace Guitarmonics.AudioLib.Tests.MusicXml
{

    [TestFixture]
    public class GuitarMusicXmlImporterTest
    {
        private static string MusicXmlTest = TestConfig.AudioPath + "MusicXmlTest.xml";
        private const string MusicXml_when_you_where_young = TestConfig.ConstAudioPath + "The Killers - When You Were Young.xml";
        private const string SongFile_when_you_where_young = TestConfig.ConstAudioPath + "The Killers - When You Were Young.song.xml";

        [Test]
        public void OpenMusicXmlFile()
        {
            var importer = new GuitarMusicXmlImporter();

            XmlDocument xmlDoc = importer.OpenMusicXmlFile(MusicXmlTest);

            Assert.IsNotNull(xmlDoc);
        }



        [Test]
        public void ImporterReturnsTrackListCorrectly()
        {
            var importer = new GuitarMusicXmlImporter();

            XmlDocument xmlDoc = importer.OpenMusicXmlFile(MusicXmlTest);

            IList<TrackInfo> tracks = importer.ListTracks(xmlDoc);

            Assert.IsNotNull(tracks);
            Assert.AreEqual(2, tracks.Count);

            Assert.AreEqual("P1", tracks[0].Id);
            Assert.AreEqual("My Track 1", tracks[0].Name);

            Assert.AreEqual("P2", tracks[1].Id);
            Assert.AreEqual("My Track 2", tracks[1].Name);

        }

        [Test]
        public void InvalidScorePartNode_PartNameMissing()
        {
            var importer = new GuitarMusicXmlImporter();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<score-partwise><part-list><score-part id = \"P1\"></score-part></part-list></score-partwise>");

            var exception = Assert.Throws<InvalidXmlMusicFile>(() => importer.ListTracks(xmlDoc));
            Assert.AreEqual("'part-name' was not found in 'score-part' node of a MusicXml file.", exception.Message);
        }

        [Test]
        public void InvalidScorePartNode_PartIdMissing()
        {
            var importer = new GuitarMusicXmlImporter();

            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.LoadXml("<score-partwise><part-list><score-part><part-name></part-name></score-part></part-list></score-partwise>");

            var exception = Assert.Throws<InvalidXmlMusicFile>(() => importer.ListTracks(xmlDoc));
            Assert.AreEqual("'id' attribute was not found in 'score-part' node of a MusicXml file.", exception.Message);
        }


        //[Test]
        //public void ListAllMeasuresOfOneTrack()
        //{
        //    var importer = new GuitarMusicXmlImporter();

        //    XmlDocument xmlDoc = importer.OpenMusicXmlFile(MusicXmlTest);

        //    var track = new TrackInfo()
        //    {
        //        Id = "P1",
        //        Name = ""
        //    };

        //    XmlNodeList measures = importer.ListAllMeasuresOfOneTrack(xmlDoc, track);

        //    Assert.AreEqual(2, measures.Count);
        //    Assert.AreEqual("1", measures[0].Attributes["number"].Value);
        //    Assert.AreEqual("2", measures[1].Attributes["number"].Value);

        //}

        //[Test]
        //public void ListAllNotesOfOneMeasure()
        //{
        //    var importer = new GuitarMusicXmlImporter();

        //    XmlDocument xmlDoc = importer.OpenMusicXmlFile(MusicXmlTest);

        //    var track = new TrackInfo()
        //    {
        //        Id = "P1",
        //        Name = ""
        //    };

        //    XmlNodeList measures = importer.ListAllMeasuresOfOneTrack(xmlDoc, track);

        //    XmlNodeList nodes = importer.ListAllNotesOfOneMeasure(measures[0]);

        //    Assert.AreEqual(5, nodes.Count);

        //    {
        //        var step = nodes[0].SelectSingleNode("descendant::step");
        //        Assert.IsNotNull(step);
        //        Assert.AreEqual("C", step.InnerText);
        //    }

        //    {
        //        var step = nodes[1].SelectSingleNode("descendant::step");
        //        Assert.IsNotNull(step);
        //        Assert.AreEqual("C", step.InnerText);
        //    }

        //    {
        //        var step = nodes[2].SelectSingleNode("descendant::step");
        //        Assert.IsNotNull(step);
        //        Assert.AreEqual("D", step.InnerText);
        //    }

        //    {
        //        var step = nodes[3].SelectSingleNode("descendant::step");
        //        Assert.IsNull(step);
        //        var rest = nodes[3].SelectSingleNode("descendant::rest");
        //        Assert.IsNotNull(rest);
        //    }

        //    {
        //        var step = nodes[4].SelectSingleNode("descendant::step");
        //        Assert.IsNotNull(step);
        //        Assert.AreEqual("E", step.InnerText);
        //    }
        //}

        
        [Test]
        public void ListAllNotesOfOneTrack()
        {
            var importer = new GuitarMusicXmlImporter();

            XmlDocument xmlDoc = importer.OpenMusicXmlFile(MusicXmlTest);

            var track = new TrackInfo()
            {
                Id = "P1",
                Name = ""
            };

            XmlNodeList nodes = importer.ListAllNotesOfOneTrack(xmlDoc, track);

            Assert.AreEqual(9, nodes.Count);

            {
                Assert.AreEqual("C9", nodes[0].InnerText);
            }

            {
                var step = nodes[1].SelectSingleNode("descendant::step");
                Assert.IsNotNull(step);
                Assert.AreEqual("C", step.InnerText);
            }

            {
                var step = nodes[2].SelectSingleNode("descendant::step");
                Assert.IsNotNull(step);
                Assert.AreEqual("C", step.InnerText);
            }

            {
                var step = nodes[3].SelectSingleNode("descendant::step");
                Assert.IsNotNull(step);
                Assert.AreEqual("D", step.InnerText);
            }

            {
                var step = nodes[4].SelectSingleNode("descendant::step");
                Assert.IsNull(step);
                var rest = nodes[4].SelectSingleNode("descendant::rest");
                Assert.IsNotNull(rest);
            }

            {
                Assert.AreEqual("E9", nodes[5].InnerText);
            }

            {
                var step = nodes[6].SelectSingleNode("descendant::step");
                Assert.IsNotNull(step);
                Assert.AreEqual("E", step.InnerText);
            }

            {
                var step = nodes[7].SelectSingleNode("descendant::step");
                Assert.IsNotNull(step);
                Assert.AreEqual("C", step.InnerText);
            }

            {
                var step = nodes[8].SelectSingleNode("descendant::step");
                Assert.IsNotNull(step);
                Assert.AreEqual("G", step.InnerText);
            }
        }

        [Test]
        public void ConvertNotesInGuitarScoreNote()
        {
            var importer = new GuitarMusicXmlImporter();

            XmlDocument xmlDoc = importer.OpenMusicXmlFile(MusicXmlTest);

            var track = new TrackInfo()
            {
                Id = "P1",
                Name = ""
            };

            XmlNodeList notes = importer.ListAllNotesOfOneTrack(xmlDoc, track);


            var scoreNotes = new SortedList<GuitarScoreNote, GuitarScoreNote>();
            importer.ConvertNotesInGuitarScoreNote(scoreNotes, notes);

            Assert.AreEqual(6, scoreNotes.Count);

            {
                var scoreNote = scoreNotes.ElementAt(0).Value;
                Assert.AreEqual("C#4", scoreNote.NoteId);
                Assert.AreEqual(1, scoreNote.Beat);
                Assert.AreEqual(0, scoreNote.Tick);
                Assert.AreEqual(480, scoreNote.DurationInTicks);
                Assert.AreEqual(0, scoreNote.MomentInMiliseconds);
                Assert.AreEqual(5, scoreNote.DefaultNotePosition.String);
                Assert.AreEqual(4, scoreNote.DefaultNotePosition.Fret);
                Assert.AreEqual("C9", scoreNote.RemarkOrChordName);
            }

            {
                var scoreNote = scoreNotes.ElementAt(1).Value;
                Assert.AreEqual("C#4", scoreNote.NoteId);
                Assert.AreEqual(2, scoreNote.Beat);
                Assert.AreEqual(0, scoreNote.Tick);
                Assert.AreEqual(240, scoreNote.DurationInTicks);
                Assert.AreEqual(0, scoreNote.MomentInMiliseconds);
                Assert.AreEqual(5, scoreNote.DefaultNotePosition.String);
                Assert.AreEqual(4, scoreNote.DefaultNotePosition.Fret);
                Assert.AreEqual("", scoreNote.RemarkOrChordName);
            }

            {
                var scoreNote = scoreNotes.ElementAt(2).Value;
                Assert.AreEqual("D4", scoreNote.NoteId);
                Assert.AreEqual(2, scoreNote.Beat);
                Assert.AreEqual(240, scoreNote.Tick);
                Assert.AreEqual(240, scoreNote.DurationInTicks);
                Assert.AreEqual(0, scoreNote.MomentInMiliseconds);
                Assert.AreEqual(5, scoreNote.DefaultNotePosition.String);
                Assert.AreEqual(5, scoreNote.DefaultNotePosition.Fret);
                Assert.AreEqual("", scoreNote.RemarkOrChordName);
            }

            {
                var scoreNote = scoreNotes.ElementAt(3).Value;
                Assert.AreEqual("E4", scoreNote.NoteId);
                Assert.AreEqual(4, scoreNote.Beat);
                Assert.AreEqual(0, scoreNote.Tick);
                Assert.AreEqual(480, scoreNote.DurationInTicks);
                Assert.AreEqual(0, scoreNote.MomentInMiliseconds);
                Assert.AreEqual(5, scoreNote.DefaultNotePosition.String);
                Assert.AreEqual(7, scoreNote.DefaultNotePosition.Fret);
                Assert.AreEqual("E9", scoreNote.RemarkOrChordName);
            }

            {
                var scoreNote = scoreNotes.ElementAt(4).Value;
                Assert.AreEqual("C#4", scoreNote.NoteId);
                Assert.AreEqual(5, scoreNote.Beat);
                Assert.AreEqual(0, scoreNote.Tick);
                Assert.AreEqual(1920, scoreNote.DurationInTicks);
                Assert.AreEqual(0, scoreNote.MomentInMiliseconds);
                Assert.AreEqual(5, scoreNote.DefaultNotePosition.String);
                Assert.AreEqual(4, scoreNote.DefaultNotePosition.Fret);
                Assert.AreEqual("", scoreNote.RemarkOrChordName);
            }

            {
                var scoreNote = scoreNotes.ElementAt(5).Value;
                Assert.AreEqual("G#4", scoreNote.NoteId);
                Assert.AreEqual(5, scoreNote.Beat);
                Assert.AreEqual(0, scoreNote.Tick);
                Assert.AreEqual(1920, scoreNote.DurationInTicks);
                Assert.AreEqual(0, scoreNote.MomentInMiliseconds);
                Assert.AreEqual(4, scoreNote.DefaultNotePosition.String);
                Assert.AreEqual(6, scoreNote.DefaultNotePosition.Fret);
                Assert.AreEqual("", scoreNote.RemarkOrChordName);
            }
        }




        [TestCase("whole", DottedType.NonDotted, 1920)]
        [TestCase("whole", DottedType.Dotted, 2880)]
        [TestCase("half", DottedType.NonDotted, 960)]
        [TestCase("half", DottedType.Dotted, 1440)]
        [TestCase("quarter", DottedType.NonDotted, 480)]
        [TestCase("quarter", DottedType.Dotted, 720)]
        [TestCase("eighth", DottedType.NonDotted, 240)]
        [TestCase("eighth", DottedType.Dotted, 360)]
        [TestCase("16th", DottedType.NonDotted, 120)]
        [TestCase("16th", DottedType.Dotted, 180)]
        [TestCase("32th", DottedType.NonDotted, 60)]
        [TestCase("32th", DottedType.Dotted, 90)]
        public void ConvertNoteTypeToTicks(string pNoteType, DottedType pDottedType, int pTicks)
        {
            var importer = new GuitarMusicXmlImporter();
            Assert.AreEqual(pTicks, importer.ConvertNoteTypeToTicks(pNoteType, pDottedType));
        }

        [Test]
        public void Import()
        {
            var importer = new GuitarMusicXmlImporter();

            XmlDocument xmlDoc = importer.OpenMusicXmlFile(MusicXmlTest);

            var track = new TrackInfo()
                            {
                                Id = "P1",
                                Name = ""
                            };

            SortedList<GuitarScoreNote, GuitarScoreNote> scoreNotes = importer.Import(xmlDoc, track);

            Assert.AreEqual(6, scoreNotes.Count);

            //Test only the last note (all generated notes was tested in ConvertNotesInGuitarScoreNote test case)
            {
                var scoreNote = scoreNotes.ElementAt(5).Value;
                Assert.AreEqual("G#4", scoreNote.NoteId);
                Assert.AreEqual(5, scoreNote.Beat);
                Assert.AreEqual(0, scoreNote.Tick);
                Assert.AreEqual(1920, scoreNote.DurationInTicks);
                Assert.AreEqual(0, scoreNote.MomentInMiliseconds);
                Assert.AreEqual(4, scoreNote.DefaultNotePosition.String);
                Assert.AreEqual(6, scoreNote.DefaultNotePosition.Fret);
            }

        }


        [Test]
        public void GenerateSongFile_when_you_where_young()
        {
            var importer = new GuitarMusicXmlImporter();

            XmlDocument xmlDoc = importer.OpenMusicXmlFile(MusicXml_when_you_where_young);

            var track = new TrackInfo()
                            {
                                Id = "P1",
                                Name = ""
                            };

            SortedList<GuitarScoreNote, GuitarScoreNote> scoreNotes = importer.Import(xmlDoc, track);


            var artist = "The Killers";
            var title = "When You Where Young";
            var xmlScoreWriter = new XmlScoreWriter(artist, title, PlayingMode.EletricGuitarScore, scoreNotes);

            if (File.Exists(SongFile_when_you_where_young))
                File.Delete(SongFile_when_you_where_young);

            xmlScoreWriter.SaveXmlNotesToFile(SongFile_when_you_where_young);

            Assert.IsTrue(File.Exists(SongFile_when_you_where_young));
        }

        [Test]
        public void TrackInfoToString()
        {
            var track = new TrackInfo()
            {
                Id = "P1",
                Name = "Track 1 Name"
            };

            Assert.AreEqual(track.Name, track.ToString());
        }
    }
}
