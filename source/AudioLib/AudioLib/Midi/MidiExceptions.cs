using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guitarmonics.AudioLib.Midi
{
    public class InvalidMidiEventsSequence : Exception
    {
        public InvalidMidiEventsSequence(string pMessage)
            : base(pMessage)
        {
        }
    }

    public class NoteOnNotFound : InvalidMidiEventsSequence
    {
        public NoteOnNotFound(string pMessage)
            : base(pMessage)
        {
        }
    }

}
