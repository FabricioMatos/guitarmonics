using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guitarmonics.AudioLib.MusicConfigFiles
{
    public class InvalidFileName : Exception
    {
        public InvalidFileName(string pMessage)
            : base(pMessage)
        {
        }
    }

    public class InvalidSongFile : Exception
    {
        public InvalidSongFile(string pMessage)
            : base(pMessage)
        {
        }
    }
}
