using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Analysis;

namespace Guitarmonics.AudioLib.Common
{
    public class NotePosition
    {
        public NotePosition(int pString, int pFret)
        {
            this.String = pString;
            this.Fret = pFret;
        }

        public int String { get; set; }

        public int Fret { get; set; }
    }

    public class GuitarScoreNote : ScoreNote, IComparable
    {
        /// <summary>
        /// Constructor
        /// </summary>
        /// <param name="pNoteId">Note identification. For example "A#4"</param>
        /// <param name="pBeat">Note position (beat) in the music</param>
        /// <param name="pTick">Note position (tick) in the music</param>
        /// <param name="pDurationInTicks">Note duration in beats. For example, 1.5f indicate 1 beat and 240 ticks (half beat)</param>
        public GuitarScoreNote(string pNoteId, int pBeat, int pTick, int? pDurationInTicks, long? pMomentInMiliseconds) :
            base(pNoteId, pBeat, pTick, pDurationInTicks, pMomentInMiliseconds)
        {
            fNotePositions = new SortedList<int, NotePosition>();
            fDefaultNotePositionIndex = 0;

            //Default tunning
            fFirstString = new MusicalNote("E5");
            fSecondString = new MusicalNote("B4");
            fThirdString = new MusicalNote("G4");
            fFourthString = new MusicalNote("D4");
            fFifthString = new MusicalNote("A3");
            fSixthString = new MusicalNote("E3");

            if (this.Note < fSixthString)
            {
                throw new GuitarScoreNoteOutOfRange(string.Format("Note {0} is too low to be played by this guitar.", pNoteId));
            }

            if (this.Note > fFirstString.Octave(2))
            {
                throw new GuitarScoreNoteOutOfRange(string.Format("Note {0} is too high to be played by this guitar.", pNoteId));
            }

            AddNotePosicion(1);
            AddNotePosicion(2);
            AddNotePosicion(3);
            AddNotePosicion(4);
            AddNotePosicion(5);
            AddNotePosicion(6);
        }

        #region Fields

        /// <summary>
        /// Note equivalent to the first string (assume a specific tunning)
        /// </summary>
        private MusicalNote fFirstString;
        /// <summary>
        /// Note equivalent to the second string (assume a specific tunning)
        /// </summary>
        private MusicalNote fSecondString;
        /// <summary>
        /// Note equivalent to the third string (assume a specific tunning)
        /// </summary>
        private MusicalNote fThirdString;
        /// <summary>
        /// Note equivalent to the fourth string (assume a specific tunning)
        /// </summary>
        private MusicalNote fFourthString;
        /// <summary>
        /// Note equivalent to the fifth string (assume a specific tunning)
        /// </summary>
        private MusicalNote fFifthString;
        /// <summary>
        /// Note equivalent to the sixth string (assume a specific tunning)
        /// </summary>
        private MusicalNote fSixthString;

        /// <summary>
        /// Note equivalent to the sixth string (assume a specific tunning)
        /// </summary>
        private int fDefaultNotePositionIndex;

        private SortedList<int, NotePosition> fNotePositions;

        #endregion

        #region Properties

        /// <summary>
        /// Identify the selected note position in this.NotePositions to be returned by this.DefaultNotePosition.
        /// </summary>
        public int DefaultNotePositionIndex
        {
            get
            {
                return fDefaultNotePositionIndex;
            }
            set
            {
                fDefaultNotePositionIndex = value;
            }
        }

        public SortedList<int, NotePosition> NotePositions
        {
            get
            {
                return fNotePositions;
            }
        }

        public NotePosition DefaultNotePosition
        {
            get
            {
                if (fNotePositions.Count < fDefaultNotePositionIndex + 1)
                    throw new InvalidGuitarPosition("GuitarScoreNote.fNotePositions don't have elements at position" + fDefaultNotePositionIndex.ToString());

                return fNotePositions.ElementAt(fDefaultNotePositionIndex).Value;
            }
        }

        #endregion

        #region Methods

        public override string ToString()
        {
            return string.Format("GuitarScoreNote: {0} [{1}, {2}] {3}ms, {4}", this.NoteId, this.Beat, this.Tick, this.MomentInMiliseconds, this.RemarkOrChordName);
        }

        private void AddNotePosicion(int pString)
        {
            var notePosition = CalculateNotePosition(pString);

            if (notePosition != null)
            {
                //remark: fNotePositions is a sorted list
                fNotePositions.Add(notePosition.Fret, notePosition);
            }
        }

        public NotePosition CalculateNotePosition(int pString)
        {
            if ((pString < 1) || (pString > 6))
            {
                throw new InvalidGuitarString(string.Format("The current GuitarScoreNote don't have string {0}.", pString));
            }


            NotePosition result = null;

            switch (pString)
            {
                case 6:
                    result = DoCalculateNotePosition(pString, this.fSixthString);
                    break;
                case 5:
                    result = DoCalculateNotePosition(pString, this.fFifthString);
                    break;
                case 4:
                    result = DoCalculateNotePosition(pString, this.fFourthString);
                    break;
                case 3:
                    result = DoCalculateNotePosition(pString, this.fThirdString);
                    break;
                case 2:
                    result = DoCalculateNotePosition(pString, this.fSecondString);
                    break;
                case 1:
                    result = DoCalculateNotePosition(pString, this.fFirstString);
                    break;
                default:
                    break;
            }

            //can be null
            return result;
        }

        private NotePosition DoCalculateNotePosition(int pString, MusicalNote pStringNote)
        {
            if ((this.Note >= pStringNote) && (this.Note <= pStringNote.Octave(2)))
            {
                if (this.Note == pStringNote)
                    return new NotePosition(pString, 0);

                int octavesAux;
                var interval = pStringNote.Value.Interval(this.Note.Value, out octavesAux);

                var octaves = (this.Note.Number - pStringNote.Number) - octavesAux;

                var fret = (int)interval + 1 + (12 * octaves);
                return new NotePosition(pString, fret);
            }
            return null;
        }

        #endregion

        #region Operators

        public static bool operator ==(GuitarScoreNote a, GuitarScoreNote b)
        {
            if (System.Object.ReferenceEquals(a, null) && (System.Object.ReferenceEquals(b, null)))
                return true;

            if (System.Object.ReferenceEquals(a, null) || (System.Object.ReferenceEquals(b, null)))
                return false;

            return (a.Beat == b.Beat) &&
                (a.Tick == b.Tick) &&
                (a.Note == b.Note) &&
                (a.DefaultNotePosition.String == b.DefaultNotePosition.String) &&
                (a.DefaultNotePosition.Fret == b.DefaultNotePosition.Fret);

        }

        public static bool operator !=(GuitarScoreNote a, GuitarScoreNote b)
        {
            return !(a == b);
        }

        public static bool operator <(GuitarScoreNote a, GuitarScoreNote b)
        {
            if ((a == null) || (b == null))
                return false;

            if (a.Beat != b.Beat)
                return (a.Beat < b.Beat);

            if (a.Tick != b.Tick)
                return (a.Tick < b.Tick);

            if (a.Note != b.Note)
                return (a.Note < b.Note);

            if (a.DefaultNotePosition.String != b.DefaultNotePosition.String)
                return (a.DefaultNotePosition.String > b.DefaultNotePosition.String); //6th string come first

            if (a.DefaultNotePosition.Fret != b.DefaultNotePosition.Fret)
                return (a.DefaultNotePosition.Fret > b.DefaultNotePosition.Fret);

            return false; //they are equal
        }

        public static bool operator <=(GuitarScoreNote a, GuitarScoreNote b)
        {
            return (a < b) || (a == b);
        }

        public static bool operator >(GuitarScoreNote a, GuitarScoreNote b)
        {
            return !(a <= b);
        }

        public static bool operator >=(GuitarScoreNote a, GuitarScoreNote b)
        {
            return !(a < b);
        }

        #endregion

        #region IComparable Members

        int IComparable.CompareTo(object obj)
        {
            if (this == (GuitarScoreNote)obj)
                return 0;

            if (this < (GuitarScoreNote)obj)
                return -1;

            return 1;
        }

        #endregion

    }
}
