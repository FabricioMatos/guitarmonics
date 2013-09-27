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
    public class MidiImporterTest
    {
        [Test]
        public void MidiSequenceUsage()
        {
            var midiEvents = new MidiEventCollection();
            midiEvents.Add(new NoteOn(0, 1, "C#4", 100));
            midiEvents.Add(new NoteOn(120, 1, "C#4", 0));


            var midiImporter = new MidiImporter(midiEvents, 120);
            Assert.AreEqual(1, midiImporter.ScoreNotes.Count);

            var firstScoreNote = midiImporter.ScoreNotes.ElementAt(0).Value;

            //Time is 1:0 (beat:tick)
            Assert.AreEqual(1, firstScoreNote.Beat);
            Assert.AreEqual(0, firstScoreNote.Tick);

            //Note is C#4
            Assert.AreEqual(NoteValue.Db, firstScoreNote.Note.Value);
            Assert.AreEqual(4, firstScoreNote.Note.Number);
        }


        [Test]
        public void OneNote()
        {
            var midiEvents = new MidiEventCollection();
            midiEvents.Add(new NoteOn(480, 1, "C#4", 100)); //480 = 4 x 120 (4 beats)
            midiEvents.Add(new NoteOn(120, 1, "C#4", 0));

            var midiImporter = new MidiImporter(midiEvents, 120);
            Assert.AreEqual(1, midiImporter.ScoreNotes.Count);

            var firstScoreNote = midiImporter.ScoreNotes.ElementAt(0).Value;

            //Time is 5:0 (beat:tick)
            Assert.AreEqual(5, firstScoreNote.Beat);
            Assert.AreEqual(0, firstScoreNote.Tick);

            //Note is C#4
            Assert.AreEqual(NoteValue.Db, firstScoreNote.Note.Value);
            Assert.AreEqual(4, firstScoreNote.Note.Number);
        }

        [Test]
        public void TwoNotes()
        {
            var midiEvents = new MidiEventCollection();

            midiEvents.Add(new NoteOn(0, 1, "C#4", 100));
            midiEvents.Add(new NoteOn(120, 1, "C#4", 0));
            midiEvents.Add(new NoteOn(0, 1, "G#3", 100));
            midiEvents.Add(new NoteOn(120, 1, "G#3", 0));

            var midiImporter = new MidiImporter(midiEvents, 120);
            Assert.AreEqual(2, midiImporter.ScoreNotes.Count);

            {
                var scoreNote = midiImporter.ScoreNotes.ElementAt(0).Value;

                Assert.AreEqual(1, scoreNote.Beat);
                Assert.AreEqual(0, scoreNote.Tick);
                Assert.AreEqual(NoteValue.Db, scoreNote.Note.Value);
                Assert.AreEqual(4, scoreNote.Note.Number);
            }

            {
                var scoreNote = midiImporter.ScoreNotes.ElementAt(1).Value;

                Assert.AreEqual(2, scoreNote.Beat);
                Assert.AreEqual(0, scoreNote.Tick);
                Assert.AreEqual(NoteValue.Ab, scoreNote.Note.Value);
                Assert.AreEqual(3, scoreNote.Note.Number);
            }
        }

        [Test]
        public void NoteDuration_OneNote_OneBeat()
        {
            var midiEvents = new MidiEventCollection();
            midiEvents.Add(new NoteOn(0, 1, "C#4", 100));
            midiEvents.Add(new NoteOn(120, 1, "C#4", 0));

            var midiImporter = new MidiImporter(midiEvents, 120);
            Assert.AreEqual(1, midiImporter.ScoreNotes.Count);

            {
                var scoreNote = midiImporter.ScoreNotes.ElementAt(0).Value;

                Assert.AreEqual(1, scoreNote.Beat);
                Assert.AreEqual(0, scoreNote.Tick);
                Assert.AreEqual(NoteValue.Db, scoreNote.Note.Value);
                Assert.AreEqual(4, scoreNote.Note.Number);

                //Duration in Beats
                Assert.AreEqual(480, scoreNote.DurationInTicks);
            }
        }

        [Test]
        public void NoteDuration_OneNote_TwoBeats()
        {
            var midiEvents = new MidiEventCollection();
            midiEvents.Add(new NoteOn(120, 1, "C#4", 100));
            midiEvents.Add(new NoteOn(240, 1, "C#4", 0)); //set the duration (2 beats): velocity = 0
                    
            var midiImporter = new MidiImporter(midiEvents, 120);
            Assert.AreEqual(1, midiImporter.ScoreNotes.Count);

            {
                var scoreNote = midiImporter.ScoreNotes.ElementAt(0).Value;

                Assert.AreEqual(2, scoreNote.Beat);
                Assert.AreEqual(0, scoreNote.Tick);
                Assert.AreEqual(NoteValue.Db, scoreNote.Note.Value);
                Assert.AreEqual(4, scoreNote.Note.Number);

                //Duration in Beats
                Assert.AreEqual(960, scoreNote.DurationInTicks);
            }
        }

        [Test]
        public void NoteDuration_OneNote_OneAndHalf()
        {
            var midiEvents = new MidiEventCollection();
            midiEvents.Add(new NoteOn(120, 1, "C#4", 100));
            midiEvents.Add(new NoteOn(180, 1, "C#4", 0)); //set the duration (2 beats): velocity = 0

            var midiImporter = new MidiImporter(midiEvents, 120);
            Assert.AreEqual(1, midiImporter.ScoreNotes.Count);

            {
                var scoreNote = midiImporter.ScoreNotes.ElementAt(0).Value;

                Assert.AreEqual(2, scoreNote.Beat);
                Assert.AreEqual(0, scoreNote.Tick);
                Assert.AreEqual(NoteValue.Db, scoreNote.Note.Value);
                Assert.AreEqual(4, scoreNote.Note.Number);

                //Duration in Beats
                Assert.AreEqual(480 + 240, scoreNote.DurationInTicks);
            }

        }

        [Test]
        public void NoteDuration_OneNote_TwoChords()
        {
            var midiEvents = new MidiEventCollection();

            midiEvents.Add(new NoteOn(480, 1, "C4", 100));
            midiEvents.Add(new NoteOn(0, 1, "G4", 100));
            midiEvents.Add(new NoteOn(0, 1, "C5", 100));

            midiEvents.Add(new NoteOn(30, 1, "C4", 0));
            midiEvents.Add(new NoteOn(0, 1, "G4", 0));
            midiEvents.Add(new NoteOn(0, 1, "C5", 0));

            midiEvents.Add(new NoteOn(30, 1, "G3", 100));
            midiEvents.Add(new NoteOn(0, 1, "D4", 100));
            midiEvents.Add(new NoteOn(0, 1, "G4", 100));

            midiEvents.Add(new NoteOn(180, 1, "G3", 0));
            midiEvents.Add(new NoteOn(0, 1, "D4", 0));
            midiEvents.Add(new NoteOn(0, 1, "G4", 0));
            
            var midiImporter = new MidiImporter(midiEvents, 120);
            Assert.AreEqual(6, midiImporter.ScoreNotes.Count);

            //Get the first chord (and test the position/duration)
            var firstChord = midiImporter.ScoreNotes.Where(
                p => (p.Value.Beat == 5)
                && (p.Value.Tick == 0)
                && (p.Value.DurationInTicks == 120)).ToList();

            Assert.AreEqual(3, firstChord.Count);

            //Get the second chord (and test the position/duration)
            var secondChord = midiImporter.ScoreNotes.Where(
                p => (p.Value.Beat == 5)
                && (p.Value.Tick == 240)
                && (p.Value.DurationInTicks == 480 + 240)).ToList();

            Assert.AreEqual(3, secondChord.Count);
        }

        [Test]
        [ExpectedException(ExpectedException=typeof(NoteOnNotFound))]
        public void NoteDuration_NoteOnNotFound()
        {
            var midiEvents = new MidiEventCollection();

            midiEvents.Add(new NoteOn(0, 1, "C4", 100));
            midiEvents.Add(new NoteOn(120, 1, "C4", 0));

            midiEvents.Add(new NoteOn(120, 1, "C4", 0));

            var midiImporter = new MidiImporter(midiEvents, 120);

        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidMidiEventsSequence))]
        public void NoteDuration_NotFinalizedNotes()
        {
            var midiEvents = new MidiEventCollection();

            midiEvents.Add(new NoteOn(0, 1, "C4", 100));
            midiEvents.Add(new NoteOn(120, 1, "C4", 0));

            midiEvents.Add(new NoteOn(0, 1, "C4", 100)); //not finalized

            var midiImporter = new MidiImporter(midiEvents, 120);

        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidMidiEventsSequence))]
        public void NoteDuration_NotFinalized()
        {
            var midiEvents = new MidiEventCollection();

            midiEvents.Add(new NoteOn(0, 1, "C4", 100));

            var midiImporter = new MidiImporter(midiEvents, 120);
        }


        [Test]
        public void NoteDuration_AdmitTheSameNote()
        {
            var midiEvents = new MidiEventCollection();

            midiEvents.Add(new NoteOn(0, 1, "C4", 100));
            midiEvents.Add(new NoteOn(0, 1, "C4", 100));

            midiEvents.Add(new NoteOn(120, 1, "C4", 0));
            midiEvents.Add(new NoteOn(120, 1, "C4", 0));

            var midiImporter = new GuitarMidiImporter(midiEvents, 120);
            Assert.AreEqual(2, midiImporter.ScoreNotes.Count);

            var firstChord = midiImporter.ScoreNotes.Where(p => (p.Value.Beat == 1)
                && (p.Value.Tick == 0)).ToList();

            Assert.AreEqual(2, firstChord.Count);
        }
    }
}
