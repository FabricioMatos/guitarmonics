using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guitarmonics.AudioLib.Analysis
{
    public class InvalidMusicalNoteId : Exception
    {
        public InvalidMusicalNoteId(string pMessage)
            : base(pMessage)
        {
        }
    }

    public class InvalidChord : Exception
    {
        public InvalidChord(string pMessage)
            : base(pMessage)
        {
        }
    }

}
