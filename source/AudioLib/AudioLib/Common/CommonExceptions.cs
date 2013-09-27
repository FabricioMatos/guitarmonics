using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guitarmonics.AudioLib.Common
{
    public class GuitarScoreNoteOutOfRange: Exception
    {
        public GuitarScoreNoteOutOfRange(string pMessage)
            : base(pMessage)
        {
        }
    }

    public class InvalidGuitarPosition : Exception
    {
        public InvalidGuitarPosition(string pMessage)
            : base(pMessage)
        {
        }
    }

    public class InvalidGuitarString : InvalidGuitarPosition
    {
        public InvalidGuitarString(string pMessage)
            : base(pMessage)
        {
        }
    }

}
