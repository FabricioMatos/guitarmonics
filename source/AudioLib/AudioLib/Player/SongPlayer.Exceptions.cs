using System;
using System.Collections.Generic;
using System.Text;

namespace Guitarmonics.AudioLib.Player
{
    public class SongPlayerException : Exception
    {
        public SongPlayerException(string pMessage)
            : base(pMessage)
        {
        }
    }

    public class FileNotFound : SongPlayerException
    {
        public FileNotFound(string pMessage)
            : base(pMessage)
        {
        }
    }

    public class SongPlayerInconsistence : SongPlayerException
    {
        public SongPlayerInconsistence(string pMessage)
            : base(pMessage)
        {
        }
    }

    public class AudioProcessingError : SongPlayerException
    {
        public AudioProcessingError(string pMessage)
            : base(pMessage)
        {
        }
    }

    public class InvalidParameter : SongPlayerException
    {
        public InvalidParameter(string pMessage)
            : base(pMessage)
        {
        }
    }
}
