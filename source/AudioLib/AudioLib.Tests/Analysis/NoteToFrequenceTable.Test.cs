using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading;
using System.Diagnostics;
using Un4seen.Bass;
using Guitarmonics.AudioLib.Player;
using System.Collections;
using Guitarmonics.AudioLib.Analysis;

namespace Guitarmonics.AudioLib.Analysis.Tests
{
    [TestFixture]
    public class NoteToFrequenceTableTest
    {
        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidNoteName))]
        public void NoteFrequence_EmptyStringThrows()
        {
            var frequence = NoteToFrequenceTable.Instance[""];
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidNoteName))]
        public void NoteFrequence_NullStringThrows()
        {
            var frequence = NoteToFrequenceTable.Instance[null];
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidNoteName))]
        public void NoteFrequence_OnlyOneCharacterThrows()
        {
            var frequence = NoteToFrequenceTable.Instance["C"];
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidNoteName))]
        public void NoteFrequence_MoreThenTreeCharactersThrows()
        {
            var frequence = NoteToFrequenceTable.Instance["C#10"]; //we accept only up to "9"
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidNoteName))]
        public void NoteFrequence_InvalidSymbolsThrows()
        {
            var frequence = NoteToFrequenceTable.Instance["X#"];
        }

        [Test]
        public void CalculateNoteFrequence()
        {
            var spectrumAnalysis = new SpectrumAnalyzer();

            Assert.AreEqual(440.0.ToString("####0.00"), NoteToFrequenceTable.Instance["A4"].ToString("####0.00"));
            Assert.AreEqual(523.25.ToString("####0.00"), NoteToFrequenceTable.Instance["C5"].ToString("####0.00"));
            Assert.AreEqual(2959.96.ToString("####0.00"), NoteToFrequenceTable.Instance["F#7"].ToString("####0.00"));
            Assert.AreEqual(2959.96.ToString("####0.00"), NoteToFrequenceTable.Instance["Gb7"].ToString("####0.00"));
            Assert.AreEqual(116.54.ToString("####0.00"), NoteToFrequenceTable.Instance["Bb2"].ToString("####0.00"));
            Assert.AreEqual(4698.64.ToString("####0.00"), NoteToFrequenceTable.Instance["D8"].ToString("####0.00"));

            Assert.AreEqual(NoteToFrequenceTable.Instance["Db7"], NoteToFrequenceTable.Instance["C#7"]);
            Assert.AreEqual(NoteToFrequenceTable.Instance["Eb7"], NoteToFrequenceTable.Instance["D#7"]);
            Assert.AreEqual(NoteToFrequenceTable.Instance["Gb7"], NoteToFrequenceTable.Instance["F#7"]);
            Assert.AreEqual(NoteToFrequenceTable.Instance["Ab7"], NoteToFrequenceTable.Instance["G#7"]);
            Assert.AreEqual(NoteToFrequenceTable.Instance["Bb7"], NoteToFrequenceTable.Instance["A#7"]);
        }
    }
}
