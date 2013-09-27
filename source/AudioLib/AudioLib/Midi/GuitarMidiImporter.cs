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
    /// Read Midi Events (MidiEventCollection) and return a ScoreNotes (collection of GuitarScoreNote)
    /// </summary>
    public class GuitarMidiImporter : MidiImporterBase<GuitarScoreNote>
    {
        public GuitarMidiImporter(MidiEventCollection pMidiEvents, int pBpm) :
            base(pMidiEvents, pBpm, 0)
        {
        }

        public GuitarMidiImporter(MidiEventCollection pMidiEvents, int pBpm, int pSkipedBeats) :
            base(pMidiEvents, pBpm, pSkipedBeats)
        {
        }

        protected override GuitarScoreNote NewScoreNote(string pNoteId, int pBeat, int pTick)
        {
            var guitarScoreNote = new GuitarScoreNote(pNoteId, pBeat, pTick, null, null);

            DefineDefaultNotePosition(ref guitarScoreNote);

            return guitarScoreNote;
        }

        //Fo chords, try to use a position in the next string
        private void DefineDefaultNotePosition(ref GuitarScoreNote pGuitarScoreNote)
        {
            if (this.ScoreNotes.Count == 0)
                return;

            //get the last scoreNote.
            var lastScoreNote = this.ScoreNotes.Last().Value;

            if ((lastScoreNote.Beat == pGuitarScoreNote.Beat) && (lastScoreNote.Tick == pGuitarScoreNote.Tick))
            {
                var lastStringUsed = lastScoreNote.DefaultNotePosition.String;

                //look for a NotePosition using the next string
                for (int i = 1; i < pGuitarScoreNote.NotePositions.Count; i++)
                {
                    if (pGuitarScoreNote.NotePositions.ElementAt(i).Value.String == (lastStringUsed - 1))
                    {
                        pGuitarScoreNote.DefaultNotePositionIndex = i;
                        return;
                    }
                }
            }
        }

        protected override void DoAddNewNote(GuitarScoreNote scoreNote)
        {
            //If this note/position exists, choose a different NotePosition
            while (fScoreNotes.ContainsKey(scoreNote) && (scoreNote.DefaultNotePositionIndex < scoreNote.NotePositions.Count - 1))
            {
                scoreNote.DefaultNotePositionIndex++;
            }

            //try
            {
                if (!fScoreNotes.ContainsKey(scoreNote))
                    fScoreNotes.Add(scoreNote, scoreNote);
            }
            //catch
            {
                //throw new InvalidMidiEventsSequence(string.Format("It's not possible to add again this GuitarScoreNote: {0}/{1}:{2}.",
                //    scoreNote.NoteId, scoreNote.DefaultNotePosition.String, scoreNote.DefaultNotePosition.Fret));
            }
        }

    }

}
