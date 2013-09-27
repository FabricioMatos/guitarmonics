using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Common;
using System.IO;
using System.Collections;
using System.Xml;

namespace Guitarmonics.AudioLib.MusicConfigFiles
{
    //TODO: Parece que nao eh usado mais. Eliminar!!!
    //public class XmlScoreMomentReader : XmlScoreNotesReader
    //{
    //    public XmlScoreMomentReader(string pFileName)
    //        : base(pFileName)
    //    {
    //    }

    //    public XmlScoreMomentReader(string pXmlNotesFileName, string pXmlSyncFileName)
    //        : base(pXmlNotesFileName)
    //    {
    //    }

    //    protected long? fMomentInMiliseconds; //para parar de dar erro (antes XmlScoreMomentReader especializava XmlScoreReader


    //    private List<ScoreMoment> fScoreMoments = new List<ScoreMoment>();

    //    public List<ScoreMoment> ScoreMoments
    //    {
    //        get { return fScoreMoments; }
    //    }

    //    protected override void AddNoteToCollection()
    //    {
    //        ScoreMoment scoreMoment = null;

    //        if ((fScoreMoments.Count > 0) &&
    //            (fScoreMoments.Last().ScoreNotes.First().Beat == fBeat) &&
    //            (fScoreMoments.Last().ScoreNotes.First().Tick == fTick))
    //        {
    //            scoreMoment = fScoreMoments.Last();
    //        }

    //        if (scoreMoment == null)
    //        {
    //            scoreMoment = new ScoreMoment();
    //            this.ScoreMoments.Add(scoreMoment);
    //        }

    //        var scoreNote = new GuitarScoreNote(fNoteId, (int)fBeat, (int)fTick, fDuration, fMomentInMiliseconds);
            
    //        scoreNote.RemarkOrChordName = this.fRemarkOrChordName;
    //        scoreNote.NotePositions.Clear();
    //        scoreNote.NotePositions.Add(this.fFret.Value, new NotePosition(this.fString.Value, this.fFret.Value));
    //        scoreNote.DefaultNotePositionIndex = 0;

    //        scoreMoment.ScoreNotes.Add(scoreNote);
    //    }
    //}

    /// <summary>
    /// Put together all ScoreNotes that starts at the same time (same beat:tick)
    /// </summary>
    //public class ScoreMoment : IBeatTickMoment
    //{
    //    public ScoreMoment()
    //    {
    //        fScoreNotes = new List<ScoreNote>();
    //    }

    //    private List<ScoreNote> fScoreNotes;

    //    public List<ScoreNote> ScoreNotes
    //    {
    //        get { return fScoreNotes; }
    //    }

    //    public int Beat
    //    {
    //        get
    //        {
    //            if (fScoreNotes.Count == 0)
    //                return -1;

    //            return fScoreNotes[0].Beat;
    //        }
    //    }

    //    public int Tick
    //    {
    //        get
    //        {
    //            if (fScoreNotes.Count == 0)
    //                return -1;

    //            return fScoreNotes[0].Tick;
    //        }
    //    }

    //    public long? MomentInMiliseconds
    //    {
    //        get { throw new NotImplementedException(); }
    //        set { throw new NotImplementedException(); }
    //    }
    //}
}
