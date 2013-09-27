using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Guitarmonics.AudioLib.MusicConfigFiles;
using Guitarmonics.AudioLib.Common;

namespace Guitarmonics.AudioLib.Tests.MusicConfigFiles
{
    [TestFixture]
    public class XmlScoreReaderTest
    {
        private const string SongFile_TesteOk = TestConfig.ConstAudioPath + "TesteOk.song.xml";
        private const string SongSyncFile_TesteOk = TestConfig.ConstAudioPath + "TesteOk.sync.xml";

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidFileName))]
        public void XmlScoreReader_InvalidFileName_REMOVER()
        {
            var xmlScoreReader = new XmlScoreReader("invalid file.xml");
        }

        [Test]
        public void XmlScoreReader_InvalidFileName()
        {
            Assert.Throws<InvalidFileName>(() => new XmlScoreReader("invalid file.xml", SongFile_TesteOk));
            Assert.Throws<InvalidFileName>(() => new XmlScoreReader(SongFile_TesteOk, "invalid file.xml"));

        }

        [Test]
        public void ImportXmlUsingXmlScoreReader()
        {
            var xmlScoreReader = new XmlScoreReader(SongFile_TesteOk, SongSyncFile_TesteOk);

            Assert.IsNotNull(xmlScoreReader);
            Assert.AreEqual("TesteOk", xmlScoreReader.Artist);
            Assert.AreEqual("Song 01", xmlScoreReader.Title);
            Assert.AreEqual(-0.5f, xmlScoreReader.Pitch);           
            Assert.AreEqual(12, xmlScoreReader.ScoreNotes.Count);

            this.AssertScoreNote(xmlScoreReader.ScoreNotes[0], "F#3", 160, 6, 2, 1000, "F#");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[1], "C#4", 160, 5, 4, 1000, "");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[2], "F#4", 160, 4, 4, 1000, "");

            this.AssertScoreNote(xmlScoreReader.ScoreNotes[3], "F#3", 160, 6, 2, 1333, "F#");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[4], "C#4", 160, 5, 4, 1333, "");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[5], "F#4", 160, 4, 4, 1333, "");

            this.AssertScoreNote(xmlScoreReader.ScoreNotes[6], "E3", 2880, 6, 0, 3000, "E");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[7], "B3", 2880, 5, 2, 3000, "");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[8], "E5", 2880, 2, 5, 3000, "");

            this.AssertScoreNote(xmlScoreReader.ScoreNotes[9], "F#3", 160, 6, 2, 9000, "F#");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[10], "C#4", 160, 5, 4, 9000, "");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[11], "F#4", 160, 4, 4, 9000, "");
        }

        [Test]
        public void AutoCompleteMomentInMilisecondsWithAllNull()
        {
            var notes = new List<GuitarScoreNote>();

            {
                var note = new GuitarScoreNote("B3", 1, 0, 240, null);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("F#3", 2, 0, 240, null);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("G#3", 3, 0, 240, null);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("E3", 4, 0, 240, null);
                notes.Add(note);
            }


            var xmlScoreReader = new MockXmlScoreReader(notes);

            Assert.IsNull(xmlScoreReader.ScoreNotes[0].MomentInMiliseconds);
            Assert.IsNull(xmlScoreReader.ScoreNotes[1].MomentInMiliseconds);
            Assert.IsNull(xmlScoreReader.ScoreNotes[2].MomentInMiliseconds);
            Assert.IsNull(xmlScoreReader.ScoreNotes[3].MomentInMiliseconds);

        }


        [Test]
        public void AutoCompleteMomentInMilisecondsWith2Pins()
        {
            var notes = new List<GuitarScoreNote>();

            {
                var note = new GuitarScoreNote("B3", 1, 0, 240, 1000);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("F#3", 2, 0, 240, null);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("G#3", 3, 0, 240, null);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("E3", 4, 0, 240, 4000);
                notes.Add(note);
            }


            var xmlScoreReader = new MockXmlScoreReader(notes);

            Assert.AreEqual(1000, xmlScoreReader.ScoreNotes[0].MomentInMiliseconds);
            Assert.AreEqual(2000, xmlScoreReader.ScoreNotes[1].MomentInMiliseconds); //auto completed
            Assert.AreEqual(3000, xmlScoreReader.ScoreNotes[2].MomentInMiliseconds); //auto completed
            Assert.AreEqual(4000, xmlScoreReader.ScoreNotes[3].MomentInMiliseconds);

        }

        [Test]
        public void AutoCompleteMomentInMilisecondsWith2PinsInTheMiddle()
        {
            var notes = new List<GuitarScoreNote>();

            {
                var note = new GuitarScoreNote("B3", 1, 0, 240, null);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("F#3", 2, 0, 240, 2000);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("G#3", 3, 0, 240, null);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("E3", 4, 0, 240, 4000);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("E3", 5, 0, 240, null);
                notes.Add(note);
            }


            var xmlScoreReader = new MockXmlScoreReader(notes);

            Assert.AreEqual(1000, xmlScoreReader.ScoreNotes[0].MomentInMiliseconds); //auto completed
            Assert.AreEqual(2000, xmlScoreReader.ScoreNotes[1].MomentInMiliseconds);
            Assert.AreEqual(3000, xmlScoreReader.ScoreNotes[2].MomentInMiliseconds); //auto completed
            Assert.AreEqual(4000, xmlScoreReader.ScoreNotes[3].MomentInMiliseconds);
            Assert.AreEqual(5000, xmlScoreReader.ScoreNotes[4].MomentInMiliseconds); //auto completed
        }


        [Test]
        public void AutoCompleteMomentInMilisecondsWithVariableTime()
        {
            var notes = new List<GuitarScoreNote>();

            {
                var note = new GuitarScoreNote("B3", 1, 0, 240, null);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("F#3", 2, 0, 240, 2000);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("G#3", 3, 0, 240, null);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("E3", 4, 0, 240, 4000);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("E3", 5, 0, 240, null);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("E3", 6, 0, 240, 8000);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("E3", 7, 0, 240, null);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("E3", 8, 0, 240, null);
                notes.Add(note);
            }


            var xmlScoreReader = new MockXmlScoreReader(notes);

            Assert.AreEqual(1000, xmlScoreReader.ScoreNotes[0].MomentInMiliseconds); //auto completed
            Assert.AreEqual(2000, xmlScoreReader.ScoreNotes[1].MomentInMiliseconds);
            Assert.AreEqual(3000, xmlScoreReader.ScoreNotes[2].MomentInMiliseconds); //auto completed
            Assert.AreEqual(4000, xmlScoreReader.ScoreNotes[3].MomentInMiliseconds);
            Assert.AreEqual(6000, xmlScoreReader.ScoreNotes[4].MomentInMiliseconds); //auto completed
            Assert.AreEqual(8000, xmlScoreReader.ScoreNotes[5].MomentInMiliseconds);
            Assert.AreEqual(10000, xmlScoreReader.ScoreNotes[6].MomentInMiliseconds); //auto completed
            Assert.AreEqual(12000, xmlScoreReader.ScoreNotes[7].MomentInMiliseconds); //auto completed
        }

        [Test]
        public void AutoCompleteMomentInMilisecondsWithNotesAtTheSameBeatTick()
        {
            var notes = new List<GuitarScoreNote>();

            {
                var note = new GuitarScoreNote("B3", 1, 0, 240, null);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("F#3", 2, 0, 240, 2000);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("C#4", 2, 0, 240, 2000);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("F#4", 2, 0, 240, 2000);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("G#3", 3, 0, 240, null);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("D#4", 3, 0, 240, null);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("E3", 4, 0, 240, 4000);
                notes.Add(note);
            }

            {
                var note = new GuitarScoreNote("E3", 5, 0, 240, null);
                notes.Add(note);
            }


            var xmlScoreReader = new MockXmlScoreReader(notes);

            Assert.AreEqual(1000, xmlScoreReader.ScoreNotes[0].MomentInMiliseconds); //auto completed
            Assert.AreEqual(2000, xmlScoreReader.ScoreNotes[1].MomentInMiliseconds);
            Assert.AreEqual(2000, xmlScoreReader.ScoreNotes[2].MomentInMiliseconds);
            Assert.AreEqual(2000, xmlScoreReader.ScoreNotes[3].MomentInMiliseconds);
            Assert.AreEqual(3000, xmlScoreReader.ScoreNotes[4].MomentInMiliseconds); //auto completed
            Assert.AreEqual(3000, xmlScoreReader.ScoreNotes[5].MomentInMiliseconds); //auto completed
            Assert.AreEqual(4000, xmlScoreReader.ScoreNotes[6].MomentInMiliseconds);
            Assert.AreEqual(5000, xmlScoreReader.ScoreNotes[7].MomentInMiliseconds); //auto completed
        }


        public class MockXmlScoreReader : XmlScoreReader
        {
            public MockXmlScoreReader(List<GuitarScoreNote> pScoreNotes)
                : base("")
            {
                fScoreNotes = pScoreNotes;

                AutoCompleteEmptyMoments(
                    pScoreNotes.Cast<IBeatTickMoment>().Select(x => x as IBeatTickMoment).ToList());

            }

            private List<GuitarScoreNote> fScoreNotes = new List<GuitarScoreNote>();
            public List<GuitarScoreNote> ScoreNotes
            {
                get { return fScoreNotes; }
            }

            protected override void OpenXmlNotesFile(string pFileName)
            {
                //do nothing
            }

            protected override void LoadScoreNotes()
            {
                //do nothing
            }

        }



        #region Methods for Assert

        //private void AssertAllScoreNotesStartAtTheSameTime(ScoreMoment pScoreMoment)
        //{
        //    Assert.Greater(pScoreMoment.ScoreNotes.Count, 0);

        //    var firstScoreNote = pScoreMoment.ScoreNotes[0];

        //    Assert.AreEqual(firstScoreNote.Beat, pScoreMoment.Beat);
        //    Assert.AreEqual(firstScoreNote.Tick, pScoreMoment.Tick);

        //    for (int i = 1; i < pScoreMoment.ScoreNotes.Count; i++)
        //    {
        //        Assert.AreEqual(pScoreMoment.Beat, pScoreMoment.ScoreNotes[i].Beat);
        //        Assert.AreEqual(pScoreMoment.Tick, pScoreMoment.ScoreNotes[i].Tick);
        //    }

        //}

        private void AssertScoreNote(ScoreNote pScoreNote, string pNoteId, int pDuration, int pString, int pFret, long pMomentInMiliseconds, string pRemarkOrChordName)
        {
            Assert.AreEqual(pNoteId, pScoreNote.NoteId);
            Assert.AreEqual(pDuration, pScoreNote.DurationInTicks);
            Assert.AreEqual(pRemarkOrChordName, pScoreNote.RemarkOrChordName);

            if (pScoreNote is GuitarScoreNote)
            {
                Assert.AreEqual(pString, ((GuitarScoreNote)pScoreNote).DefaultNotePosition.String);
                Assert.AreEqual(pFret, ((GuitarScoreNote)pScoreNote).DefaultNotePosition.Fret);
                Assert.AreEqual(pMomentInMiliseconds, ((GuitarScoreNote)pScoreNote).MomentInMiliseconds);
            }
        }

        #endregion
    }
}
