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
    /// Read Midi Events (MidiEventCollection) and return a ScoreNotes (collection of ScoreNote)
    /// </summary>
    public class MidiImporter : MidiImporterBase<ScoreNote>
    {
        public MidiImporter(MidiEventCollection pMidiEvents, int pBpm) :
            base(pMidiEvents, pBpm, 0)
        {
        }

        protected override ScoreNote NewScoreNote(string pNoteId, int pBeat, int pTick)
        {
            return new ScoreNote(pNoteId, pBeat, pTick, null, null);
        }
    }

}
