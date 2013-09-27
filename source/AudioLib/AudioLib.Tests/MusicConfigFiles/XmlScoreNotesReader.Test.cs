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
    public class XmlScoreNotesReaderTest
    {
        private const string SongFile_TesteOk = TestConfig.ConstAudioPath + "TesteOk.song.xml";

        [Test]
        public void ValidFileName()
        {
            var xmlScoreReader = new XmlScoreNotesReader(SongFile_TesteOk);
            Assert.IsNotNull(xmlScoreReader);
        }



        [Test]
        public void ImportXmlUsingXmlScoreReader()
        {
            var xmlScoreReader = new XmlScoreNotesReader(SongFile_TesteOk);
            Assert.AreEqual(12, xmlScoreReader.ScoreNotes.Count);

            //this.AssertScoreNote(xmlScoreReader.ScoreNotes[0], "F#3", 160, 6, 2, 1000, "F#");
            //this.AssertScoreNote(xmlScoreReader.ScoreNotes[1], "C#4", 160, 5, 4, 1000, "");
            //this.AssertScoreNote(xmlScoreReader.ScoreNotes[2], "F#4", 160, 4, 4, 1000, "");

            //this.AssertScoreNote(xmlScoreReader.ScoreNotes[3], "F#3", 160, 6, 2, 1333, "F#");
            //this.AssertScoreNote(xmlScoreReader.ScoreNotes[4], "C#4", 160, 5, 4, 1333, "");
            //this.AssertScoreNote(xmlScoreReader.ScoreNotes[5], "F#4", 160, 4, 4, 1333, "");

            //this.AssertScoreNote(xmlScoreReader.ScoreNotes[6], "E3", 2880, 6, 0, 3000, "E");
            //this.AssertScoreNote(xmlScoreReader.ScoreNotes[7], "B3", 2880, 5, 2, 3000, "");
            //this.AssertScoreNote(xmlScoreReader.ScoreNotes[8], "E5", 2880, 2, 5, 3000, "");

            //this.AssertScoreNote(xmlScoreReader.ScoreNotes[9], "F#3", 160, 6, 2, 9000, "F#");
            //this.AssertScoreNote(xmlScoreReader.ScoreNotes[10], "C#4", 160, 5, 4, 9000, "");
            //this.AssertScoreNote(xmlScoreReader.ScoreNotes[11], "F#4", 160, 4, 4, 9000, "");


            this.AssertScoreNote(xmlScoreReader.ScoreNotes[0], "F#3", 160, 6, 2, "F#");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[1], "C#4", 160, 5, 4, "");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[2], "F#4", 160, 4, 4, "");

            this.AssertScoreNote(xmlScoreReader.ScoreNotes[3], "F#3", 160, 6, 2, "F#");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[4], "C#4", 160, 5, 4, "");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[5], "F#4", 160, 4, 4, "");

            this.AssertScoreNote(xmlScoreReader.ScoreNotes[6], "E3", 2880, 6, 0, "E");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[7], "B3", 2880, 5, 2, "");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[8], "E5", 2880, 2, 5, "");

            this.AssertScoreNote(xmlScoreReader.ScoreNotes[9], "F#3", 160, 6, 2, "F#");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[10], "C#4", 160, 5, 4, "");
            this.AssertScoreNote(xmlScoreReader.ScoreNotes[11], "F#4", 160, 4, 4, "");
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

        private void AssertScoreNote(ScoreNote pScoreNote, string pNoteId, int pDuration, int pString, int pFret, string pRemarkOrChordName)
        {
            Assert.AreEqual(pNoteId, pScoreNote.NoteId);
            Assert.AreEqual(pDuration, pScoreNote.DurationInTicks);
            Assert.AreEqual(pRemarkOrChordName, pScoreNote.RemarkOrChordName);

            if (pScoreNote is GuitarScoreNote)
            {
                Assert.AreEqual(pString, ((GuitarScoreNote)pScoreNote).DefaultNotePosition.String);
                Assert.AreEqual(pFret, ((GuitarScoreNote)pScoreNote).DefaultNotePosition.Fret);
            }
        }

        #endregion

    }
}
