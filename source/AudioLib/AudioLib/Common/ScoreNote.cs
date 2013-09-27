using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Analysis;

namespace Guitarmonics.AudioLib.Common
{
    public interface IBeatTickMoment
    {
        int Beat { get; }
        int Tick { get; }
        long? MomentInMiliseconds { get; set; }
    }

    public static class MixinForIBeatTickMoment
    {
        public static long MomentInTicks(this IBeatTickMoment pBeatTickMoment)
        {
            return (pBeatTickMoment.Beat*480) + pBeatTickMoment.Tick;
        }
    }

    public class ScoreNote : IComparable, IBeatTickMoment
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pNoteId">Note identification. For example "A#4"</param>
        /// <param name="pBeat">Note position (beat) in the music</param>
        /// <param name="pTick">Note position (tick) in the music</param>
        /// <param name="pDurationInTicks">Note duration in ticks. For example, 240 indicate half beat</param>
        /// <param name="pMoment">Note position (miliseconds) in the music</param>
        public ScoreNote(string pNoteId, int pBeat, int pTick, int? pDurationInTicks, long? pMomentInMiliseconds)
        {
            fNoteId = pNoteId;
            fNote = new MusicalNote(pNoteId);
            fBeat = pBeat;
            fTick = (pTick / 10) * 10; //round
            fDurationInTicks = (pDurationInTicks / 10) * 10; //round
            fMomentInMiliseconds = pMomentInMiliseconds;
            RemarkOrChordName = "";
        }

        #region Fields

        private string fNoteId;
        private MusicalNote fNote;
        private int fBeat;
        private int fTick;
        private int? fDurationInTicks;
        private long? fMomentInMiliseconds;

        #endregion

        #region Properties

        /// <summary>
        /// Note identification. For example "A#4""
        /// </summary>
        public string NoteId
        {
            get { return fNoteId; }
        }

        /// <summary>
        /// Musical note equivalent to this.NoteId
        /// </summary>
        public MusicalNote Note
        {
            get { return fNote; }
        }

        /// <summary>
        /// Note position (beat) in the music
        /// </summary>
        public int Beat
        {
            get { return fBeat; }
        }

        /// <summary>
        /// Note position (tick) in the music
        /// </summary>
        public int Tick
        {
            get { return fTick; }
        }

        /// <summary>
        /// Note duration in ticks. For example, 240 indicate half beat
        /// </summary>
        public int? DurationInTicks
        {
            get { return fDurationInTicks; }
            set 
            { 
                //round the value (make it divisible for 10)
                fDurationInTicks = (value / 10) * 10; 
            }
        }

        /// <summary>
        /// Note position (miliseconds) in the music
        /// </summary>
        public long? MomentInMiliseconds
        {
            get { return fMomentInMiliseconds; }
            set { fMomentInMiliseconds = value; }
        }

        /// <summary>
        /// Textual remark of some note (example: "15b17" (bend), "F#m7" (chord name), etc.) 
        /// </summary>
        public string RemarkOrChordName { get; set; }

        #endregion

        #region Static
        /// <summary>
        /// Number of Ticks of one Beat.
        /// </summary>
        public static int OneBeat = 480;

        /// <summary>
        /// Return the number of ticks relative to a number (fractional) of beats.
        /// For example: 0.5 beats = 240 ticks
        /// </summary>
        /// <param name="pBeats">Number of beats</param>
        /// <returns></returns>
        public static int BeatsToTicks(float pBeats)
        {
            return (int)(ScoreNote.OneBeat * pBeats);
        }

        #endregion

        #region Operators

        public static bool operator ==(ScoreNote a, ScoreNote b)
        {
            if (System.Object.ReferenceEquals(a, null) && (System.Object.ReferenceEquals(b, null)))
                return true;

            if (System.Object.ReferenceEquals(a, null) || (System.Object.ReferenceEquals(b, null)))
                return false;

            return (a.Beat == b.Beat) &&
                (a.Tick == b.Tick) &&
                (a.Note == b.Note);
        }

        public static bool operator !=(ScoreNote a, ScoreNote b)
        {
            return !(a == b);
        }

        public static bool operator <(ScoreNote a, ScoreNote b)
        {
            if ((a == null) || (b == null))
                return false;

            if (a.Beat != b.Beat)
                return (a.Beat < b.Beat);

            if (a.Tick != b.Tick)
                return (a.Tick < b.Tick);

            if (a.Note != b.Note)
                return (a.Note < b.Note);

            return false; //they are equal
        }

        public static bool operator <=(ScoreNote a, ScoreNote b)
        {
            return (a < b) || (a == b);
        }

        public static bool operator >(ScoreNote a, ScoreNote b)
        {
            return !(a <= b);
        }

        public static bool operator >=(ScoreNote a, ScoreNote b)
        {
            return !(a < b);
        }

        #endregion

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            if (this == (ScoreNote)obj)
                return 0;

            if (this < (ScoreNote)obj)
                return -1;

            return 1;
        }

        #endregion
    }
}
