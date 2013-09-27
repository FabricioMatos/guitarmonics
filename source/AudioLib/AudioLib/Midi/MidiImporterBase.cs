using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Toub.Sound.Midi;
using Guitarmonics.AudioLib.Common;
using System.Runtime.Serialization;

namespace Guitarmonics.AudioLib.Midi
{
    /// <summary>
    /// Base class of the Midi importers that convert MidiEventCollection to ScoreNotes (collection of ScoreNote)
    /// </summary>
    public abstract class MidiImporterBase<T>
        where T : ScoreNote
    {
        public MidiImporterBase(MidiEventCollection pMidiEvents, int pBpm, int pSkipedBeats)
        {
            fScoreNotes = new SortedList<T, T>();

            fCurrentTime = 0;

            fBpm = pBpm;
            fMidiEvents = pMidiEvents;
            fSkipedBeats = pSkipedBeats;

            ImportNotes();
        }

        #region Fields

        private MidiEventCollection fMidiEvents;
        protected SortedList<T, T> fScoreNotes;
        private long fCurrentTime;
        private int fBpm;
        private int fSkipedBeats;

        #endregion

        #region Properties

        public SortedList<T, T> ScoreNotes
        {
            get
            {
                return fScoreNotes;
            }
        }

        #endregion

        #region Methods

        protected abstract T NewScoreNote(string pNoteId, int pBeat, int pTick);

        private void ImportNotes()
        {
            foreach (var midiEvent in fMidiEvents)
            {
                if (midiEvent is NoteOn)
                {
                    var noteOnEvent = (NoteOn)midiEvent;

                    this.AddNewNote(MidiEvent.GetNoteName(noteOnEvent.Note), noteOnEvent.DeltaTime, noteOnEvent.Velocity);

                }
                else if (midiEvent is NoteOff)
                {
                    var noteOffEvent = (NoteOff)midiEvent;

                    this.AddNewNote(MidiEvent.GetNoteName(noteOffEvent.Note), noteOffEvent.DeltaTime, 0);

                    //this.AddNewNote(MidiEvent.GetNoteName(noteOffEvent.Note), noteOffEvent.DeltaTime, noteOffEvent.Velocity);

                    //if (noteOffEvent.Velocity > 0)
                    //    this.AddNewNote(MidiEvent.GetNoteName(noteOffEvent.Note), 0, 0);

                }
            }
            var numberOfNotFinalizedNotes = fScoreNotes.Where(p => p.Value.DurationInTicks == null).Count();
            if (numberOfNotFinalizedNotes > 0)
            {
                throw new InvalidMidiEventsSequence(string.Format("Invalid MIDI event collection. "
                    + "We found {0} not finalized notes.",
                    numberOfNotFinalizedNotes));
            }

        }

        private void AddNewNote(string pNoteId, long pDeltaTime, byte pVelocity)
        {
            fCurrentTime += pDeltaTime;

            int beat = (int)(fCurrentTime / fBpm) + 1 + fSkipedBeats;

            long tickTime = fCurrentTime % fBpm;
            float tickPercent = ((float)tickTime / (float)fBpm);
            int tick = (int)(tickPercent * 480);

            if (pVelocity > 0)
            {
                var scoreNote = this.NewScoreNote(pNoteId, beat, tick);

                this.DoAddNewNote((T)scoreNote);

            }
            else
            {
                try
                {
                    var noteReference = new MusicalNote(pNoteId);

                    //TODO: refatorar - utilizar FirstOrDefalut e gerar excecao se for nulo (ao inves de um try...catch)
                    ScoreNote note = fScoreNotes.Where(p => (p.Value.DurationInTicks == null)
                        && (p.Value.Note.ToString() == noteReference.ToString())).First().Value;

                    int begin = (note.Beat * ScoreNote.OneBeat) + note.Tick;
                    int end = (beat * ScoreNote.OneBeat) + tick;

                    note.DurationInTicks = end - begin;

                    if (note.DurationInTicks < 0)
                    {
                        throw new InvalidMidiEventsSequence(string.Format("Note {0} on {1}:{2} (beat:tick) with negative duration!",
                            note.NoteId, note.Beat, note.Tick));
                    }
                }
                catch
                {
                    throw new NoteOnNotFound(string.Format("Invalid event. "
                        + "We found a 0 velocity event but could't find one note {0} with duration null to set.",
                        pNoteId));
                }
            }
        }

        protected virtual void DoAddNewNote(T scoreNote)
        {
            fScoreNotes.Add(scoreNote, scoreNote);
        }
        #endregion
    }
}
