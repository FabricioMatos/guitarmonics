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
using Guitarmonics.AudioLib.Common;

namespace Guitarmonics.AudioLib.Analysis.Tests
{
    [TestFixture]
    public class GuitarChordTest
    {
        #region NoteValueShiftExtension

        [Test]
        public void NoteValueShiftExtension_PerfectFifth()
        {
            var note = NoteValue.C;

            var noteShifted = note.Shift(Interval.PerfectFifth);

            Assert.AreEqual(NoteValue.G, noteShifted);
        }

        [Test]
        public void NoteValueShiftExtension_MinorSeventh()
        {
            var note = NoteValue.C;

            var noteShifted = note.Shift(Interval.MinorSeventh);

            Assert.AreEqual(NoteValue.Bb, noteShifted);
        }

        [Test]
        public void NoteValueShiftExtension_PerfectOctave()
        {
            var note = NoteValue.E;

            var noteShifted = note.Shift(Interval.PerfectOctave);

            Assert.AreEqual(NoteValue.E, noteShifted);
        }


        [Test]
        public void NoteValueIntervalExtension_PerfectFourth()
        {
            var note1 = NoteValue.C;
            var note2 = NoteValue.F;

            var interval = note1.Interval(note2);

            Assert.AreEqual(Interval.PerfectFourth, interval);
        }

        [Test]
        public void NoteValueIntervalExtension_MinorSecond()
        {
            var note1 = NoteValue.C;
            var note2 = NoteValue.Db;

            var interval = note1.Interval(note2);

            Assert.AreEqual(Interval.MinorSecond, interval);
        }

        [Test]
        public void NoteValueIntervalExtension_PerfectOctave()
        {
            var note1 = NoteValue.C;
            var note2 = NoteValue.C;

            var interval = note1.Interval(note2);

            Assert.AreEqual(Interval.PerfectOctave, interval);
        }


        [Test]
        public void NoteValueIntervalExtension_MinorThird_Inverse()
        {
            var note1 = NoteValue.A;
            var note2 = NoteValue.C;

            var interval = note1.Interval(note2);

            Assert.AreEqual(Interval.MinorThird, interval);
        }

        [Test]
        public void NoteValueIntervalExtension_MajorSeventh_Inverse()
        {
            var note1 = NoteValue.A;
            var note2 = NoteValue.Ab;

            var interval = note1.Interval(note2);

            Assert.AreEqual(Interval.MajorSeventh, interval);
        }

        #endregion

        #region Chord names

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidChord))]
        public void ContructorParameterCantBeNull()
        {
            var chord = new GuitarChord(null);
        }

        [Test]
        public void NoMininumOfNotes()
        {
            var noteList = new List<IMusicalNote>();
            noteList.Add(new MusicalNoteMock(NoteValue.G, 3, "G3"));

            var chord = new GuitarChord(noteList);
            Assert.AreEqual("G", chord.ToString());
        }

        [Test]
        public void Trivial_C()
        {
            var noteList = new List<IMusicalNote>();
            noteList.Add(new MusicalNoteMock(NoteValue.C, 4, "C4"));
            noteList.Add(new MusicalNoteMock(NoteValue.E, 4, "E4"));
            noteList.Add(new MusicalNoteMock(NoteValue.G, 4, "G4"));

            var chord = new GuitarChord(noteList);

            Assert.AreEqual(NoteValue.C, chord.TonicNote);
            Assert.AreEqual(Interval.PerfectFifth, chord.FifthInterval);
            Assert.AreEqual(Interval.MajorThird, chord.ThirdInterval);
            Assert.IsNull(chord.SeventhInterval);
            Assert.IsNull(chord.FourthInterval);
            Assert.IsNull(chord.SixthInterval);
            Assert.IsNull(chord.NinthInterval);
            Assert.AreEqual("C", chord.ToString());
        }

        [Test]
        public void Trivial_Caug5()
        {
            var noteList = new List<IMusicalNote>();
            noteList.Add(new MusicalNoteMock(NoteValue.C, 4, "C4"));
            noteList.Add(new MusicalNoteMock(NoteValue.E, 4, "E4"));
            noteList.Add(new MusicalNoteMock(NoteValue.Ab, 4, "Ab4"));

            var chord = new GuitarChord(noteList);

            Assert.AreEqual(NoteValue.C, chord.TonicNote);
            Assert.AreEqual(Interval.AugmentedFifth, chord.FifthInterval);
            Assert.AreEqual(Interval.MajorThird, chord.ThirdInterval);
            Assert.IsNull(chord.SeventhInterval);
            Assert.IsNull(chord.FourthInterval);
            Assert.IsNull(chord.SixthInterval);
            Assert.IsNull(chord.NinthInterval);
            Assert.AreEqual("C(#5)", chord.ToString());
        }

        [Test]
        public void Trivial_Cb5()
        {
            var noteList = new List<IMusicalNote>();
            noteList.Add(new MusicalNoteMock(NoteValue.C, 4, "C4"));
            noteList.Add(new MusicalNoteMock(NoteValue.E, 4, "E4"));
            noteList.Add(new MusicalNoteMock(NoteValue.Gb, 4, "Gb4"));

            var chord = new GuitarChord(noteList);

            Assert.AreEqual(NoteValue.C, chord.TonicNote);
            Assert.AreEqual(Interval.DiminishedFifth, chord.FifthInterval);
            Assert.AreEqual(Interval.MajorThird, chord.ThirdInterval);
            Assert.IsNull(chord.SeventhInterval);
            Assert.IsNull(chord.FourthInterval);
            Assert.IsNull(chord.SixthInterval);
            Assert.IsNull(chord.NinthInterval);
            Assert.AreEqual("C(b5)", chord.ToString());
        }


        [Test]
        public void Guitar_D()
        {
            var noteList = new List<IMusicalNote>();
            noteList.Add(new MusicalNoteMock(NoteValue.D, 4, "D4"));
            noteList.Add(new MusicalNoteMock(NoteValue.A, 4, "A4"));
            noteList.Add(new MusicalNoteMock(NoteValue.D, 5, "D5"));
            noteList.Add(new MusicalNoteMock(NoteValue.Gb, 5, "Gb5"));

            var chord = new GuitarChord(noteList);

            Assert.AreEqual(NoteValue.D, chord.TonicNote);
            Assert.AreEqual(Interval.PerfectFifth, chord.FifthInterval);
            Assert.AreEqual(Interval.MajorThird, chord.ThirdInterval);
            Assert.IsNull(chord.SeventhInterval);
            Assert.IsNull(chord.FourthInterval);
            Assert.IsNull(chord.SixthInterval);
            Assert.IsNull(chord.NinthInterval);
            Assert.AreEqual("D", chord.ToString());
        }

        [Test]
        public void Guitar_Dm()
        {
            var noteList = new List<IMusicalNote>();
            noteList.Add(new MusicalNoteMock(NoteValue.D, 4, "D4"));
            noteList.Add(new MusicalNoteMock(NoteValue.A, 4, "A4"));
            noteList.Add(new MusicalNoteMock(NoteValue.D, 5, "D5"));
            noteList.Add(new MusicalNoteMock(NoteValue.F, 5, "F5"));

            var chord = new GuitarChord(noteList);

            Assert.AreEqual(NoteValue.D, chord.TonicNote);
            Assert.AreEqual(Interval.PerfectFifth, chord.FifthInterval);
            Assert.AreEqual(Interval.MinorThird, chord.ThirdInterval);
            Assert.IsNull(chord.SeventhInterval);
            Assert.IsNull(chord.FourthInterval);
            Assert.IsNull(chord.SixthInterval);
            Assert.IsNull(chord.NinthInterval);
            Assert.AreEqual("Dm", chord.ToString());
        }

        [Test]
        public void Guitar_D7()
        {
            var noteList = new List<IMusicalNote>();
            noteList.Add(new MusicalNoteMock(NoteValue.D, 4, "D4"));
            noteList.Add(new MusicalNoteMock(NoteValue.A, 4, "A4"));
            noteList.Add(new MusicalNoteMock(NoteValue.C, 5, "C5"));
            noteList.Add(new MusicalNoteMock(NoteValue.Gb, 5, "Gb5"));

            var chord = new GuitarChord(noteList);

            Assert.AreEqual(NoteValue.D, chord.TonicNote);
            Assert.AreEqual(Interval.PerfectFifth, chord.FifthInterval);
            Assert.AreEqual(Interval.MajorThird, chord.ThirdInterval);
            Assert.AreEqual(Interval.MinorSeventh, chord.SeventhInterval);
            Assert.IsNull(chord.FourthInterval);
            Assert.IsNull(chord.SixthInterval);
            Assert.IsNull(chord.NinthInterval);
            Assert.AreEqual("D7", chord.ToString());
        }

        [Test]
        public void Guitar_Dmaj7()
        {
            var noteList = new List<IMusicalNote>();
            noteList.Add(new MusicalNoteMock(NoteValue.D, 4, "D4"));
            noteList.Add(new MusicalNoteMock(NoteValue.A, 4, "A4"));
            noteList.Add(new MusicalNoteMock(NoteValue.Db, 5, "Db5"));
            noteList.Add(new MusicalNoteMock(NoteValue.Gb, 5, "Gb5"));

            var chord = new GuitarChord(noteList);

            Assert.AreEqual(NoteValue.D, chord.TonicNote);
            Assert.AreEqual(Interval.PerfectFifth, chord.FifthInterval);
            Assert.AreEqual(Interval.MajorThird, chord.ThirdInterval);
            Assert.AreEqual(Interval.MajorSeventh, chord.SeventhInterval);
            Assert.IsNull(chord.FourthInterval);
            Assert.IsNull(chord.SixthInterval);
            Assert.IsNull(chord.NinthInterval);
            Assert.AreEqual("D7+", chord.ToString());
        }

        [Test]
        public void Guitar_Dm7()
        {
            var noteList = new List<IMusicalNote>();
            noteList.Add(new MusicalNoteMock(NoteValue.D, 4, "D4"));
            noteList.Add(new MusicalNoteMock(NoteValue.A, 4, "A4"));
            noteList.Add(new MusicalNoteMock(NoteValue.C, 5, "C5"));
            noteList.Add(new MusicalNoteMock(NoteValue.F, 5, "F5"));

            var chord = new GuitarChord(noteList);

            Assert.AreEqual(NoteValue.D, chord.TonicNote);
            Assert.AreEqual(Interval.PerfectFifth, chord.FifthInterval);
            Assert.AreEqual(Interval.MinorThird, chord.ThirdInterval);
            Assert.AreEqual(Interval.MinorSeventh, chord.SeventhInterval);
            Assert.IsNull(chord.FourthInterval);
            Assert.IsNull(chord.SixthInterval);
            Assert.IsNull(chord.NinthInterval);
            Assert.AreEqual("Dm7", chord.ToString());
        }


        [Test]
        public void Guitar_Em7()
        {
            var noteList = new List<IMusicalNote>();
            noteList.Add(new MusicalNoteMock(NoteValue.E, 3, "E4"));
            noteList.Add(new MusicalNoteMock(NoteValue.B, 3, "B3"));
            noteList.Add(new MusicalNoteMock(NoteValue.E, 4, "E4"));
            noteList.Add(new MusicalNoteMock(NoteValue.G, 4, "G4"));
            noteList.Add(new MusicalNoteMock(NoteValue.D, 5, "D5"));
            noteList.Add(new MusicalNoteMock(NoteValue.E, 5, "E5"));

            var chord = new GuitarChord(noteList);

            Assert.AreEqual(NoteValue.E, chord.TonicNote);
            Assert.AreEqual(Interval.PerfectFifth, chord.FifthInterval);
            Assert.AreEqual(Interval.MinorThird, chord.ThirdInterval);
            Assert.AreEqual(Interval.MinorSeventh, chord.SeventhInterval);
            Assert.IsNull(chord.FourthInterval);
            Assert.IsNull(chord.SixthInterval);
            Assert.IsNull(chord.NinthInterval);
            Assert.AreEqual("Em7", chord.ToString());
        }


        [Test]
        public void Guitar_Gbm7()
        {
            var noteList = new List<IMusicalNote>();
            noteList.Add(new MusicalNoteMock(NoteValue.Gb, 3, "Gb3"));
            noteList.Add(new MusicalNoteMock(NoteValue.Db, 4, "Db4"));
            noteList.Add(new MusicalNoteMock(NoteValue.E, 4, "E4"));
            noteList.Add(new MusicalNoteMock(NoteValue.A, 4, "A4"));
            noteList.Add(new MusicalNoteMock(NoteValue.Db, 5, "Db5"));
            noteList.Add(new MusicalNoteMock(NoteValue.Gb, 5, "Gb5"));

            var chord = new GuitarChord(noteList);

            Assert.AreEqual(NoteValue.Gb, chord.TonicNote);
            Assert.AreEqual(Interval.PerfectFifth, chord.FifthInterval);
            Assert.AreEqual(Interval.MinorThird, chord.ThirdInterval);
            Assert.AreEqual(Interval.MinorSeventh, chord.SeventhInterval);
            Assert.IsNull(chord.FourthInterval);
            Assert.IsNull(chord.SixthInterval);
            Assert.IsNull(chord.NinthInterval);
            Assert.AreEqual("Gbm7", chord.ToString());
        }

        //TODO: Include 4,6,7,9(#5/b5). Example: B7/4/9

        [Test]
        public void Guitar_B479()
        {
            var noteList = new List<IMusicalNote>();
            noteList.Add(new MusicalNoteMock(NoteValue.B, 3, "B3"));
            noteList.Add(new MusicalNoteMock(NoteValue.E, 4, "E4"));
            noteList.Add(new MusicalNoteMock(NoteValue.A, 4, "A4"));
            noteList.Add(new MusicalNoteMock(NoteValue.Db, 5, "Db5"));
            noteList.Add(new MusicalNoteMock(NoteValue.Gb, 5, "Gb5"));

            var chord = new GuitarChord(noteList);

            Assert.AreEqual(NoteValue.B, chord.TonicNote);
            Assert.AreEqual(Interval.PerfectFifth, chord.FifthInterval);
            Assert.IsNull(chord.ThirdInterval);
            Assert.AreEqual(Interval.PerfectFourth, chord.FourthInterval);
            Assert.AreEqual(Interval.MinorSeventh, chord.SeventhInterval);
            Assert.AreEqual(Interval.MajorSecond, chord.NinthInterval);
            Assert.AreEqual("B4/7/9", chord.ToString());
        }


        //Guitar_A_Major (all inverstions imagined - generate using my old utility)

        //See http://www.logue.net/chords/ 
    }

        #endregion

    public class MusicalNoteMock : IMusicalNote
    {
        public MusicalNoteMock(NoteValue pNoteValue, int pNumber, string pNoteName)
        {
            fValue = pNoteValue;
            fNumber = pNumber;
            fNoteName = pNoteName;
            this.Volume = 0;
        }

        private NoteValue fValue;
        private int fNumber;
        private string fNoteName;

        #region IMusicalNote Members

        public int Cents
        {
            get { return 0; }
        }

        public float Frequence
        {
            get { return NoteToFrequenceTable.Instance.Table[fNoteName]; }
        }

        public int Number
        {
            get { return fNumber; }
        }

        public float PlayedFrequence
        {
            get { return NoteToFrequenceTable.Instance.Table[fNoteName]; }
        }

        public NoteValue Value
        {
            get { return fValue; }
        }

        public float Volume { get; private set; }

        #endregion
    }
}
