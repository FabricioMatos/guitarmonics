using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Common;
using System.Xml;

namespace Guitarmonics.AudioLib.MusicConfigFiles
{
    public class XmlScoreNotesReader : XmlScoreReaderBase
    {
        public XmlScoreNotesReader(string pFileName)
            : base(pFileName)
        {
        }

        protected string fNoteId;
        protected int? fDuration;
        protected int? fString;
        protected int? fFret;
        protected string fRemarkOrChordName;

        protected List<GuitarScoreNote> fScoreNotes = new List<GuitarScoreNote>();
        public List<GuitarScoreNote> ScoreNotes
        {
            get { return fScoreNotes; }
        }

        protected override void ClearProperties()
        {
            base.ClearProperties();

            this.fNoteId = "";
            this.fDuration = null;
            this.fString = null;
            this.fFret = null;
            this.fRemarkOrChordName = "";
        }

        protected override void ParseXmlProperties(XmlReader pXmlReader)
        {
            base.ParseXmlProperties(pXmlReader);

            if (pXmlReader.Name == "NoteId")
                this.fNoteId = pXmlReader.Value;

            if (pXmlReader.Name == "Duration")
                this.fDuration = int.Parse(pXmlReader.Value);

            if (pXmlReader.Name == "String")
                this.fString = int.Parse(pXmlReader.Value);

            if (pXmlReader.Name == "Fret")
                this.fFret = int.Parse(pXmlReader.Value);

            if (pXmlReader.Name == "RemarkOrChordName")
                this.fRemarkOrChordName = pXmlReader.Value;

        }

        protected override void AddNoteToCollection()
        {
            var scoreNote = new GuitarScoreNote(fNoteId, (int)fBeat, (int)fTick, fDuration, null);

            scoreNote.RemarkOrChordName = this.fRemarkOrChordName;

            scoreNote.NotePositions.Clear();
            scoreNote.NotePositions.Add(this.fFret.Value, new NotePosition(this.fString.Value, this.fFret.Value));
            scoreNote.DefaultNotePositionIndex = 0;

            this.fScoreNotes.Add(scoreNote);
        }

    }
}
