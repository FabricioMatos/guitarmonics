using System.IO;
using System;
using System.Collections.Generic;

namespace Guitarmonics.SongData
{
    public class Beat
    {
        public byte? Status { get; set; }
        public byte? Duration { get; set; }
        public int? NTuplet { get; set; }
        public string Text { get; set; }

        public IList<NoteClass> Notes { get; set; }

        public Beat()
        {
            Notes = new List<NoteClass>();
        }
    }
}
