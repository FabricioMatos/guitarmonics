using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Toub.Sound.Midi;
using Guitarmonics.AudioLib.Midi;
using Guitarmonics.AudioLib.Tests;
using Guitarmonics.AudioLib.Analysis;
using Guitarmonics.AudioLib.Common;

namespace Guitarmonics.Midi.Tests
{
    [TestFixture]
    public class GuitarMidiImporterTest
    {
        //private static string MIDI_for_whom_the_bell_tolls = TestConfig.AudioPath + "metallica-for_whom_the_bell_tolls.mid";

        private void TestFirstNotePosition(NoteValue pNoteValue, string pNoteId, int pString, int pPosition)
        {
            var midiEvents = new MidiEventCollection();
            midiEvents.Add(new NoteOn(0, 1, pNoteId, 100));
            midiEvents.Add(new NoteOn(120, 1, pNoteId, 0));

            var midiImporter = new GuitarMidiImporter(midiEvents, 120);

            var firstScoreNote = midiImporter.ScoreNotes.ElementAt(0).Value;

            Assert.AreEqual(pNoteValue, firstScoreNote.Note.Value, firstScoreNote.NoteId);
            Assert.AreEqual(pString, firstScoreNote.NotePositions.ElementAt(0).Value.String, firstScoreNote.NoteId);
            Assert.AreEqual(pPosition, firstScoreNote.NotePositions.ElementAt(0).Value.Fret, firstScoreNote.NoteId);
        }

        [Test]
        public void FirstPosition()
        {
            //6th string
            TestFirstNotePosition(NoteValue.E, "E3", 6, 0);
            TestFirstNotePosition(NoteValue.F, "F3", 6, 1);
            TestFirstNotePosition(NoteValue.Gb, "F#3", 6, 2);
            TestFirstNotePosition(NoteValue.G, "G3", 6, 3);
            TestFirstNotePosition(NoteValue.Ab, "G#3", 6, 4);

            //5th string
            TestFirstNotePosition(NoteValue.A, "A3", 5, 0);
            TestFirstNotePosition(NoteValue.Bb, "A#3", 5, 1);
            TestFirstNotePosition(NoteValue.B, "B3", 5, 2);
            TestFirstNotePosition(NoteValue.C, "C4", 5, 3);
            TestFirstNotePosition(NoteValue.Db, "C#4", 5, 4);

            //4th string
            TestFirstNotePosition(NoteValue.D, "D4", 4, 0);
            TestFirstNotePosition(NoteValue.Eb, "D#4", 4, 1);
            TestFirstNotePosition(NoteValue.E, "E4", 4, 2);
            TestFirstNotePosition(NoteValue.F, "F4", 4, 3);
            TestFirstNotePosition(NoteValue.Gb, "F#4", 4, 4);

            //3rd string
            TestFirstNotePosition(NoteValue.G, "G4", 3, 0);
            TestFirstNotePosition(NoteValue.Ab, "Ab4", 3, 1);
            TestFirstNotePosition(NoteValue.A, "A4", 3, 2);
            TestFirstNotePosition(NoteValue.Bb, "Bb4", 3, 3);
            //TestFirstNotePosition(NoteValue.B, "B4", 3, 4); //B4 default is on 2nd string

            //2nd string
            TestFirstNotePosition(NoteValue.B, "B4", 2, 0);
            TestFirstNotePosition(NoteValue.C, "C5", 2, 1);
            TestFirstNotePosition(NoteValue.Db, "Db5", 2, 2);
            TestFirstNotePosition(NoteValue.D, "D5", 2, 3);
            TestFirstNotePosition(NoteValue.Eb, "Eb5", 2, 4);

            //1st string
            TestFirstNotePosition(NoteValue.E, "E5", 1, 0);
            TestFirstNotePosition(NoteValue.F, "F5", 1, 1);
            TestFirstNotePosition(NoteValue.Gb, "Gb5", 1, 2);
            TestFirstNotePosition(NoteValue.G, "G5", 1, 3);
            TestFirstNotePosition(NoteValue.Ab, "Ab5", 1, 4);

            //1st string ...
            TestFirstNotePosition(NoteValue.A, "A5", 1, 5);
            TestFirstNotePosition(NoteValue.Bb, "A#5", 1, 6);
            TestFirstNotePosition(NoteValue.B, "B5", 1, 7);
            TestFirstNotePosition(NoteValue.C, "C6", 1, 8);
            TestFirstNotePosition(NoteValue.Db, "C#6", 1, 9);
            TestFirstNotePosition(NoteValue.D, "D6", 1, 10);
            TestFirstNotePosition(NoteValue.Eb, "D#6", 1, 11);
            TestFirstNotePosition(NoteValue.E, "E6", 1, 12);
        }

        [Test]
        public void Skip3Beats()
        {
            var midiEvents = new MidiEventCollection();
            midiEvents.Add(new NoteOn(0, 1, "C4", 100));
            midiEvents.Add(new NoteOn(120, 1, "C4", 0));

            var midiImporter = new GuitarMidiImporter(midiEvents, 120, 3);

            var firstScoreNote = midiImporter.ScoreNotes.ElementAt(0).Value;

            Assert.AreEqual(1 + 3, firstScoreNote.Beat);
            Assert.AreEqual(0, firstScoreNote.Tick);
        }


        /// <summary>
        /// BUG - returning (6th string) F3 instead of F4. Now corrected!
        /// </summary>
        [Test]
        public void TesteF4()
        {
            var scoreNote = new GuitarScoreNote("F4", 1, 0, 480, 0);

            {
                var notePosition = scoreNote.CalculateNotePosition(6);

                Assert.AreEqual(6, notePosition.String);
                Assert.AreEqual(13, notePosition.Fret);
            }

            {
                var notePosition = scoreNote.CalculateNotePosition(5);

                Assert.AreEqual(5, notePosition.String);
                Assert.AreEqual(8, notePosition.Fret);
            }

            {
                var notePosition = scoreNote.CalculateNotePosition(4);

                Assert.AreEqual(4, notePosition.String);
                Assert.AreEqual(3, notePosition.Fret);
            }
        }

        [Test]
        public void TesteE5()
        {
            var scoreNote = new GuitarScoreNote("E5", 1, 0, 480, 0);

            {
                var notePosition = scoreNote.CalculateNotePosition(6);

                Assert.AreEqual(6, notePosition.String);
                Assert.AreEqual(24, notePosition.Fret);
            }

            {
                var notePosition = scoreNote.CalculateNotePosition(5);

                Assert.AreEqual(5, notePosition.String);
                Assert.AreEqual(19, notePosition.Fret);
            }

            {
                var notePosition = scoreNote.CalculateNotePosition(4);

                Assert.AreEqual(4, notePosition.String);
                Assert.AreEqual(14, notePosition.Fret);
            }

            {
                var notePosition = scoreNote.CalculateNotePosition(3);

                Assert.AreEqual(3, notePosition.String);
                Assert.AreEqual(9, notePosition.Fret);
            }

            {
                var notePosition = scoreNote.CalculateNotePosition(2);

                Assert.AreEqual(2, notePosition.String);
                Assert.AreEqual(5, notePosition.Fret);
            }

            {
                var notePosition = scoreNote.CalculateNotePosition(1);

                Assert.AreEqual(1, notePosition.String);
                Assert.AreEqual(0, notePosition.Fret);
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(GuitarScoreNoteOutOfRange))]
        public void InvalidGuitarNote_TooLow()
        {
            var scoreNote = new GuitarScoreNote("Eb3", 1, 0, 480, 0);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(GuitarScoreNoteOutOfRange))]
        public void InvalidGuitarNote_TooHigh()
        {
            //TODO: Verify if E7 is more accute guitar note.
            var scoreNote = new GuitarScoreNote("F7", 1, 0, 480, 0);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidGuitarString))]
        public void InvalidString_0()
        {
            var scoreNote = new GuitarScoreNote("E3", 1, 0, 480, 0);

            var notePosition = scoreNote.CalculateNotePosition(0);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidGuitarString))]
        public void InvalidString_7()
        {
            var scoreNote = new GuitarScoreNote("E6", 1, 0, 480, 0);

            var notePosition = scoreNote.CalculateNotePosition(7);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidMidiEventsSequence))]
        public void NoteDuration_NewInitializationWithoutFinalization()
        {
            var midiEvents = new MidiEventCollection();

            midiEvents.Add(new NoteOn(0, 1, "C4", 100));
            midiEvents.Add(new NoteOn(0, 1, "C4", 100));

            var midiImporter = new GuitarMidiImporter(midiEvents, 120);
        }


        //garantir que D4 existe na sexta corda (terceira opcao), por exemplo, embora sua primeira opcao seja a quarta corda solta e a segunda a quinta corda.

        //lista de posicoes possiveis
        //Ao importar o MIDI, teria de analisar acordes. Pensar, por exemplo em um C#
    }
}
