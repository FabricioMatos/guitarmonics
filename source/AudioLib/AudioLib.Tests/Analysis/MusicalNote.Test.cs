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
    public class MusicalNoteTest
    {
        [Test]
        public void MusicalNote_PreciseNotes()
        {
            {
                var note = new MusicalNote(440.0f);

                Assert.AreEqual("A4", note.ToString());
                Assert.AreEqual(NoteValue.A, note.Value);
                Assert.AreEqual(4, note.Number);
                Assert.AreEqual(440.0f, note.Frequence);
                Assert.AreEqual(0f, note.Cents);
            }

            {
                var note = new MusicalNote(523.25f);

                Assert.AreEqual("C5", note.ToString());
                Assert.AreEqual(NoteValue.C, note.Value);
                Assert.AreEqual(5, note.Number);
                Assert.AreEqual(523.25116f, note.Frequence);
                Assert.AreEqual(0f, note.Cents);
            }

            {
                var note = new MusicalNote(4698.64f);

                Assert.AreEqual("D8", note.ToString());
                Assert.AreEqual(NoteValue.D, note.Value);
                Assert.AreEqual(8, note.Number);
                Assert.AreEqual(4698.63623f, note.Frequence);
                Assert.AreEqual(0f, note.Cents);
            }

            {
                var note = new MusicalNote(466.1637573f);

                Assert.AreEqual("Bb4", note.ToString());
                Assert.AreEqual(NoteValue.Bb, note.Value);
                Assert.AreEqual(4, note.Number);
                Assert.AreEqual(466.1637573f, note.Frequence);
                Assert.AreEqual(0f, note.Cents);
            }
        }

        [Test]
        public void MusicalNote_NotePreciseNotes()
        {
            float diferenceBetween_A_Bb = 26.1637573f;
            //Positive Cents
            {
                var note = new MusicalNote(440.0f + diferenceBetween_A_Bb);

                Assert.AreEqual("Bb4", note.ToString());
                Assert.AreEqual(NoteValue.Bb, note.Value);
                Assert.AreEqual(4, note.Number);
                Assert.AreEqual(440.0f + diferenceBetween_A_Bb, note.Frequence);
                Assert.AreEqual(0f, note.Cents);
            }

            //Positive Cents
            {
                var difference = 20;
                var note = new MusicalNote(440.0f + diferenceBetween_A_Bb / difference);

                Assert.AreEqual("A4", note.ToString());
                Assert.AreEqual(NoteValue.A, note.Value);
                Assert.AreEqual(4, note.Number);
                Assert.AreEqual(440.0f, note.Frequence);
                Assert.AreEqual(100 / difference, note.Cents);
            }

            //Negative Cents
            {
                var difference = -20;
                var note = new MusicalNote(440.0f + diferenceBetween_A_Bb / difference);

                Assert.AreEqual("A4", note.ToString());
                Assert.AreEqual(NoteValue.A, note.Value);
                Assert.AreEqual(4, note.Number);
                Assert.AreEqual(440.0f, note.Frequence);
                Assert.AreEqual(100 / difference, note.Cents);
            }

            //Oitave 2*freq(A4) => A5
            {
                var note = new MusicalNote(440.0f * 2);

                Assert.AreEqual("A5", note.ToString());
                Assert.AreEqual(NoteValue.A, note.Value);
                Assert.AreEqual(5, note.Number);
                Assert.AreEqual(440.0f * 2, note.Frequence);
                Assert.AreEqual(0, note.Cents);
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidMusicalNoteId))]
        public void MusicalNote_NoteIdInvalid_SizeInferior()
        {
            var note = new MusicalNote("");
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidMusicalNoteId))]
        public void MusicalNote_NoteIdInvalid_SizeSuperior()
        {
            var note = new MusicalNote("C#10");
        }


        [Test]
        public void MusicalNote_A4_ById()
        {
            var note = new MusicalNote("A4");

            Assert.AreEqual("A4", note.ToString());
            Assert.AreEqual(NoteValue.A, note.Value);
            Assert.AreEqual(4, note.Number);
            Assert.AreEqual(440.0f, note.Frequence);
            Assert.AreEqual(0f, note.Cents);
        }


        [Test]
        public void MusicalNote_ContructFromStringId()
        {
            {
                var note = new MusicalNote("A4");

                Assert.AreEqual("A4", note.ToString());
                Assert.AreEqual(NoteValue.A, note.Value);
                Assert.AreEqual(4, note.Number);
                Assert.AreEqual(440.0f, note.Frequence);
                Assert.AreEqual(0f, note.Cents);
            }

            {
                var note = new MusicalNote("C5");

                Assert.AreEqual("C5", note.ToString());
                Assert.AreEqual(NoteValue.C, note.Value);
                Assert.AreEqual(5, note.Number);
                Assert.AreEqual(523.25116f, note.Frequence);
                Assert.AreEqual(0f, note.Cents);
            }

            {
                var note = new MusicalNote("D8");

                Assert.AreEqual("D8", note.ToString());
                Assert.AreEqual(NoteValue.D, note.Value);
                Assert.AreEqual(8, note.Number);
                Assert.AreEqual(4698.63623f, note.Frequence);
                Assert.AreEqual(0f, note.Cents);
            }

            {
                var note = new MusicalNote("Bb4");

                Assert.AreEqual("Bb4", note.ToString());
                Assert.AreEqual(NoteValue.Bb, note.Value);
                Assert.AreEqual(4, note.Number);
                Assert.AreEqual(466.1637573f, note.Frequence);
                Assert.AreEqual(0f, note.Cents);
            }

            {
                var note = new MusicalNote("A#4");

                Assert.AreEqual("Bb4", note.ToString());
                Assert.AreEqual(NoteValue.Bb, note.Value);
                Assert.AreEqual(4, note.Number);
                Assert.AreEqual(466.1637573f, note.Frequence);
                Assert.AreEqual(0f, note.Cents);
            }
        }

        [Test]
        public void MusicalNote_Octave_PositiveOk()
        {
            var octaves = 2;
            var note = new MusicalNote("A7");
            var note2 = note.Octave(octaves);

            Assert.AreEqual(note.Value, note2.Value);
            Assert.AreEqual(note.Number + octaves, note2.Number);
        }

        [Test]
        public void MusicalNote_Octave_NegativeOk()
        {
            var octaves = -3;
            var note = new MusicalNote("A3");
            var note2 = note.Octave(octaves);

            Assert.AreEqual(note.Value, note2.Value);
            Assert.AreEqual(note.Number + octaves, note2.Number);
        }


        [Test]
        [ExpectedException(ExpectedException=typeof(OctaveOutOfRange))]
        public void MusicalNote_Octave_LowerBound()
        {
            var octaves = -1;
            var note = new MusicalNote("A0");
            var note2 = note.Octave(octaves);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(OctaveOutOfRange))]
        public void MusicalNote_Octave_UpperBound()
        {
            var octaves = 1;
            var note = new MusicalNote("A9");
            var note2 = note.Octave(octaves);
        }

        #region MuscialNote Operators

        [Test]
        public void Operator_Equal_Ok()
        {
            MusicalNote a = new MusicalNote("A4");
            MusicalNote b = new MusicalNote("A4");

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        [Test]
        public void Operator_Equal_BothNull()
        {
            MusicalNote a = null;
            MusicalNote b = null;

            Assert.IsTrue(a == b);
            Assert.IsTrue(b == a);
        }

        [Test]
        public void Operator_Equal_JustOneNull()
        {
            MusicalNote a = new MusicalNote("A4");
            MusicalNote b = null;

            Assert.IsFalse(a == b);
            Assert.IsFalse(b == a);
        }

        [Test]
        public void Operator_Equal_Different()
        {
            MusicalNote a = new MusicalNote("A4");
            MusicalNote b = new MusicalNote("A#4");

            Assert.IsTrue(a != b);
            Assert.IsTrue(b != a);
        }

        [Test]
        public void Operator_Less_Ok()
        {
            MusicalNote a = new MusicalNote("A4");
            MusicalNote b = new MusicalNote("A#4");

            Assert.IsTrue(a < b);
            Assert.IsFalse(b < a);
        }

        [Test]
        public void Operator_Less_OneNull()
        {
            MusicalNote a = new MusicalNote("A4");
            MusicalNote b = null;

            Assert.IsFalse(a < b);
            Assert.IsFalse(b < a);
        }

        [Test]
        public void Operator_LessOrEqual_Less()
        {
            MusicalNote a = new MusicalNote("A4");
            MusicalNote b = new MusicalNote("A#4");

            Assert.IsTrue(a <= b);
            Assert.IsFalse(b <= a);
        }

        [Test]
        public void Operator_LessOrEqual_Equal()
        {
            MusicalNote a = new MusicalNote("A4");
            MusicalNote b = new MusicalNote("A4");

            Assert.IsTrue(a <= b);
            Assert.IsTrue(b <= a);
        }

        [Test]
        public void Operator_LessOrEqual_OneNull()
        {
            MusicalNote a = new MusicalNote("A4");
            MusicalNote b = null;

            Assert.IsFalse(a <= b);
            Assert.IsFalse(b <= a);
        }

        [Test]
        public void Operator_Greater_Ok()
        {
            MusicalNote a = new MusicalNote("A4");
            MusicalNote b = new MusicalNote("A#4");

            Assert.IsTrue(b > a);
            Assert.IsFalse(a > b);
        }

        [Test]
        public void Operator_GreaterOrEqual_Greater()
        {
            MusicalNote a = new MusicalNote("A4");
            MusicalNote b = new MusicalNote("A#4");

            Assert.IsTrue(b >= a);
            Assert.IsFalse(a >= b);
        }

        [Test]
        public void Operator_GreaterOrEqual_Equal()
        {
            MusicalNote a = new MusicalNote("A4");
            MusicalNote b = new MusicalNote("A4");

            Assert.IsTrue(b >= a);
            Assert.IsTrue(a >= b);
        }

        #endregion

    }
}
