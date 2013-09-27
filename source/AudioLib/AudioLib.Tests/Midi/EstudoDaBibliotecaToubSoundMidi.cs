using System;
using System.Collections.Generic;
using System.Text;
using Un4seen.Bass;
using NUnit.Framework;
using System.IO;
using Toub.Sound.Midi;
using System.Diagnostics;
using Guitarmonics.AudioLib.Tests;

namespace Guitarmonics.Midi.Tests
{
    [TestFixture]
    public class EstudoDaBibliotecaToubSoundMidi
    {
        private static string MIDI_for_whom_the_bell_tolls = TestConfig.AudioPath + "metallica-for_whom_the_bell_tolls.mid";

        [Test]
        public void LoadFile_Check10Tracks()
        {
            var sequence = MidiSequence.Import(MIDI_for_whom_the_bell_tolls);
            Assert.AreEqual(10, sequence.NumberOfTracks);
        }

        [Test]
        public void Guitar1Track()
        {
            var sequence = MidiSequence.Import(MIDI_for_whom_the_bell_tolls);
                        
            Assert.IsTrue(sequence.GetTracks()[2].Events[0] is SequenceTrackName);

            var trackName = (SequenceTrackName)sequence.GetTracks()[2].Events[0];

            Assert.AreEqual("Guitar 1", trackName.Text);
        }


        [Test]
        public void ParametersTrack()
        {
            var sequence = MidiSequence.Import(MIDI_for_whom_the_bell_tolls);

            // 0 - a single multi-channel track
            // 1 - one or more simultaneous tracks
            // 2 - one or more sequentially independent single-track patterns
            Assert.AreEqual(1, sequence.Format);

            //Beats per minute (tempo)
            Assert.AreEqual(120, sequence.Division);
        }

        [Test]
        public void TotalTime()
        {
            var sequence = MidiSequence.Import(MIDI_for_whom_the_bell_tolls);

            //MediaPlayer: 4m54s = 294s
            //totalTime/120 = number of music beats
            var musicTime = (4 * 60) + 54;

            long maxTotalTime = 0;
            foreach (var track in sequence.GetTracks())
            {
                long totalTime = 0;
                foreach (MidiEvent midiEvent in track.Events)
                {
                    totalTime += midiEvent.DeltaTime;
                }

                if (totalTime > maxTotalTime)
                    maxTotalTime = totalTime;

                //(60.0/120.0) = time of one beat (120bpm)
                var trackTime = (totalTime / sequence.Division) * (60.0 / sequence.Division);
                Assert.LessOrEqual(trackTime, musicTime);

                //Trace.TraceInformation(string.Format("Track {0}: {1} seconds",
                //    ((SequenceTrackName)track.Events[0]).Text,
                //    (totalTime / sequence.Division) * (60.0 / sequence.Division)));
            }

            var maxTrackTime = (maxTotalTime / sequence.Division) * (60.0 / sequence.Division);
            Assert.AreEqual(musicTime, maxTrackTime);
            
        }


        [Test]
        public void CheckTheFirstChord()
        {
            var sequence = MidiSequence.Import(MIDI_for_whom_the_bell_tolls);
            var guitar1Track = sequence.GetTracks()[2];

            var listOfChords = new List<List<NoteOn>>();
            List<NoteOn> chordNotes = null;
            bool isPause = false;
            foreach (MidiEvent midiEvent in guitar1Track.Events)
            {
                if (midiEvent is NoteOn)
                {
                    var note = (NoteOn)midiEvent;
                    if (note.DeltaTime > 0)
                    {
                        if ((chordNotes != null) && (chordNotes.Count > 0))
                            listOfChords.Add(chordNotes);

                        chordNotes = new List<NoteOn>();

                        isPause = (note.Velocity == 0); //used for the next notes of the acord (same time)
                    }

                    if ((note.Velocity > 0) && (!isPause))
                    {
                        chordNotes.Add(note);
                    }
                }
            }

            //start on 1680 from the beggining
            {
                var chord = listOfChords[0];
                
                Assert.AreEqual(3, chord.Count);

                Assert.AreEqual(1680, chord[0].DeltaTime);
                Assert.AreEqual(100, chord[0].Velocity);

                Assert.AreEqual("C#4", MidiEvent.GetNoteName(chord[0].Note));
                Assert.AreEqual("F#4", MidiEvent.GetNoteName(chord[1].Note));
                Assert.AreEqual("F#3", MidiEvent.GetNoteName(chord[2].Note));
            }

            //start on 40 after the last event
            {
                var chord = listOfChords[1];

                Assert.AreEqual(3, chord.Count);

                Assert.AreEqual(40, chord[0].DeltaTime);
                Assert.AreEqual(100, chord[0].Velocity);

                Assert.AreEqual("C#4", MidiEvent.GetNoteName(chord[0].Note));
                Assert.AreEqual("F#4", MidiEvent.GetNoteName(chord[1].Note));
                Assert.AreEqual("F#3", MidiEvent.GetNoteName(chord[2].Note));
            }

            //and so on...

            /*
                    track.Events.Add(new NoteOn(1680, 1, "C#4", 100));
                    track.Events.Add(new NoteOn(0, 1, "F#4", 100));
                    track.Events.Add(new NoteOn(0, 1, "F#3", 100));
                    track.Events.Add(new NoteOn(40, 1, "F#3", 0));
                    track.Events.Add(new NoteOn(0, 1, "F#4", 0));
                    track.Events.Add(new NoteOn(0, 1, "C#4", 0));
                    track.Events.Add(new NoteOn(0, 1, "C#4", 100));
                    track.Events.Add(new NoteOn(0, 1, "F#4", 100));
                    track.Events.Add(new NoteOn(0, 1, "F#3", 100));
                    track.Events.Add(new NoteOn(40, 1, "F#3", 0));
                    track.Events.Add(new NoteOn(0, 1, "F#4", 0));
                    track.Events.Add(new NoteOn(0, 1, "C#4", 0));
                    track.Events.Add(new NoteOn(40, 1, "C#4", 100));
                    track.Events.Add(new NoteOn(0, 1, "F#4", 100));
                    track.Events.Add(new NoteOn(0, 1, "F#3", 100));
            */
        }

        /*

                private MidiTrack CreateTrack1()
                {
                    MidiTrack track = new MidiTrack();
                    track.Events.Add(new SequenceTrackName(0, "untitled"));
                    track.Events.Add(new TimeSignature(0, 4, 2, 24, 8));
                    track.Events.Add(new KeySignature(0, Key.Sharp1, Tonality.Major));
                    track.Events.Add(new Tempo(0, 500000));
                    track.Events.Add(new TimeSignature(59520, 2, 2, 24, 8));
                    track.Events.Add(new TimeSignature(240, 4, 2, 24, 8));
                    track.Events.Add(new TimeSignature(960, 2, 2, 24, 8));
                    track.Events.Add(new EndOfTrack(0));
                    return track;
                }
         */
    }
}
