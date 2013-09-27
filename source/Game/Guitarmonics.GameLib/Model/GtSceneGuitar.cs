using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.AudioLib.Analysis;

namespace Guitarmonics.GameLib.Model
{
    public class GtSceneGuitarItemInvalidParameter : Exception
    {
        public GtSceneGuitarItemInvalidParameter(string pMessage)
            : base(pMessage)
        {
        }
    }

    /// <summary>
    /// A GtSceneGuitar is one "guitar" with his respective song notes
    /// </summary>
    public class GtSceneGuitar
    {
        public const int PLAYER_TOLERANCE_IN_TICKS = 20;

        #region Constructor

        public GtSceneGuitar(GtTickDataTable pTickDataTable, int pNumberOfVisibleBeats)
        {
            Initialize(pTickDataTable, pNumberOfVisibleBeats);
        }

        public GtSceneGuitar(GtTickDataTable pTickDataTable)
        {
            Initialize(pTickDataTable, 4 /*pNumberOfVisibleBeats*/);
        }

        #endregion

        #region Private Fields

        private GtTickDataTable fTickDataTable;
        private List<GtSceneGuitarNote> fNotes = new List<GtSceneGuitarNote>();
        private BeatTick fCurrentPosition;
        private long fCurrentPositionInMiliseconds;

        #endregion

        #region Public Properties

        public int NumberOfVisibleBeats { get; protected set; }

        public int Points { get; set; }
        public int MaxPoints { get; set; }

        public GtTickDataTable TickDataTable
        {
            get { return fTickDataTable; }
        }

        public List<GtSceneGuitarNote> Notes
        {
            get { return fNotes; }
        }

        /// <summary>
        /// List of notes that should be triggered (played) at this time (current position).
        /// </summary>
        public List<GtSceneGuitarNote> CurrentStartingNotes
        {
            get
            {
                return fNotes.Where(p => Math.Abs(p.DistanceFromCurrentPosition) <= PLAYER_TOLERANCE_IN_TICKS).ToList();
            }
        }

        /// <summary>
        /// List of notes that should be triggered (played) at this time (current position).
        /// </summary>
        public List<GtSceneGuitarNote> CurrentExpectedPlayingNotes
        {
            get
            {
                var currentPosition = fCurrentPosition;

                //Prepar for the "currentPosition.SubTicks(PLAYER_TOLERANCE_IN_TICKS)" because a BeatTick less then (1:0) is invalid.
                if ((currentPosition.AsTicks() - BeatTick.FirstBeat.AsTicks()) < PLAYER_TOLERANCE_IN_TICKS)
                    currentPosition = currentPosition.AddTicks(PLAYER_TOLERANCE_IN_TICKS - (currentPosition.AsTicks() - BeatTick.FirstBeat.AsTicks()));

                return (from c in this.fNotes
                        where (c.StartPosition <= (currentPosition.AddTicks(PLAYER_TOLERANCE_IN_TICKS)) &&
                              (c.EndPosition >= (currentPosition.SubTicks(PLAYER_TOLERANCE_IN_TICKS))))
                        select c).ToList();
            }
        }

        public BeatTick CurrentPosition
        {
            get { return fCurrentPosition; }
        }

        #endregion

        #region Public Methods

        /// <summary>
        /// Define the current song position (beat/tick).
        /// Used to sync the screen update with the song played.
        /// </summary>
        /// <param name="pCurrentPosition"></param>
        public void ForceCurrentPosition(BeatTick pCurrentPosition)
        {
            this.fCurrentPosition = pCurrentPosition;

            //return; //performance test

            foreach (var note in this.fNotes)
            {
                note.Update(pCurrentPosition);
            }
        }

        /// <summary>
        /// Define the current song position (beat/tick) based on current position in miliseconds.
        /// Used to sync the screen update with the song played.
        /// </summary>
        /// <param name="pCurrentPositionInMiliseconds"></param>
        public void ForceCurrentPositionInMiliseconds(long pCurrentPositionInMiliseconds)
        {
            this.fCurrentPositionInMiliseconds = pCurrentPositionInMiliseconds;

            if (fCurrentPositionInMiliseconds == 0)
            {
                this.ForceCurrentPosition(new BeatTick(1, 0));
                return;
            }


            var position = this.fCurrentPosition;

            //find the fTickDataTable position more close to the pCurrentPositionInMiliseconds
            while ((position.Beat < fTickDataTable.NumberOfBeats) &&
                ((this.fTickDataTable[position.Beat, position.Tick].MomentInMiliseconds == null) ||
                (this.fTickDataTable[position.Beat, position.Tick].MomentInMiliseconds < this.fCurrentPositionInMiliseconds)))
            {
                position = position.AddTicks(10);
            }

            if (position.Beat == fTickDataTable.NumberOfBeats)
            {
                //TODO: Fim da musica
            }

            this.ForceCurrentPosition(position);
        }



        public IList<GtSceneGuitarNote> GetVisibleNotes()
        {
            var visibleNotes = from c in this.fNotes
                               where (c.StartPosition < (this.CurrentPosition.AddTicks(this.NumberOfVisibleBeats * 480))) &&
                                     (c.EndPosition >= this.CurrentPosition)
                               select c;

            return visibleNotes.ToList();
        }

        #endregion

        #region Private Methods

        private void Initialize(GtTickDataTable pTickDataTable, int pNumberOfVisibleBeats)
        {
            if (pTickDataTable == null)
                throw new GtSceneGuitarItemInvalidParameter("pTickDataTable parameter is required for GtSceneGuitarItem construction.");

            if (pTickDataTable.fItems.Length == 0)
                throw new GtSceneGuitarItemInvalidParameter("pTickDataTable parameter must have items to be passed to GtSceneGuitarItem constructor.");

            NumberOfVisibleBeats = pNumberOfVisibleBeats;

            fTickDataTable = pTickDataTable;

            fCurrentPosition = new BeatTick(1, 0);
            fCurrentPositionInMiliseconds = 0;

            InitializeSceneItems();
        }

        /// <summary>
        /// Process the data stored in fTckDataTable, creating the items in fNotes.
        /// </summary>
        private void InitializeSceneItems()
        {
            var auxiliarTable = new GtSceneGuitarNote[6];

            var endPosition = new BeatTick(fTickDataTable.NumberOfBeats, 470);

            for (var pos = new BeatTick(1, 0); pos <= endPosition; pos = pos.AddTicks(10))
            {
                var tickData = fTickDataTable[pos.Beat, pos.Tick];

                SetSceneGuitarNote(ref auxiliarTable, 1, tickData.String1, pos);
                SetSceneGuitarNote(ref auxiliarTable, 2, tickData.String2, pos);
                SetSceneGuitarNote(ref auxiliarTable, 3, tickData.String3, pos);
                SetSceneGuitarNote(ref auxiliarTable, 4, tickData.String4, pos);
                SetSceneGuitarNote(ref auxiliarTable, 5, tickData.String5, pos);
                SetSceneGuitarNote(ref auxiliarTable, 6, tickData.String6, pos);

                if (tickData.IsEndTick)
                {
                    AddNewNoteWhenExists(ref auxiliarTable, 1);
                    AddNewNoteWhenExists(ref auxiliarTable, 2);
                    AddNewNoteWhenExists(ref auxiliarTable, 3);
                    AddNewNoteWhenExists(ref auxiliarTable, 4);
                    AddNewNoteWhenExists(ref auxiliarTable, 5);
                    AddNewNoteWhenExists(ref auxiliarTable, 6);
                }
            }
        }

        /// <summary>
        /// Create a new GtSceneGuitarNote when needed, and update the EndPosition.
        /// </summary>
        /// <param name="pAuxiliarTable">Used to keep the references for the recently created GtSceneGuitarNote not ended until now</param>
        /// <param name="pString">String number (1..6)</param>
        /// <param name="pFret">Fret number</param>
        /// <param name="pCurrentPosition">Current position - the note still playing in this position</param>
        private void SetSceneGuitarNote(ref GtSceneGuitarNote[] pAuxiliarTable, int pString, int? pFret, BeatTick pCurrentPosition)
        {
            var position = pCurrentPosition;

            if (pFret != null)
            {
                if (pAuxiliarTable[pString - 1] == null)
                {
                    //if (!tickData.IsStartTick)
                    //    throw ...

                    pAuxiliarTable[pString - 1] = new GtSceneGuitarNote(position, position, pString, (int)pFret);
                }

                pAuxiliarTable[pString - 1].EndPosition = position;
            }
        }

        /// <summary>
        /// Called for the last tick of some note.
        /// If exists a note in some pAuxiliarTable position, add it to fNotes property and reset the pAuxiliarTable position.
        /// </summary>
        /// <param name="pAuxiliarTable">Used to keep the references for the recently created GtSceneGuitarNote</param>
        /// <param name="pString"></param>
        private void AddNewNoteWhenExists(ref GtSceneGuitarNote[] pAuxiliarTable, int pString)
        {
            if (pAuxiliarTable[pString - 1] != null)
            {
                fNotes.Add(pAuxiliarTable[pString - 1]);
            }
            pAuxiliarTable[pString - 1] = null;
        }

        #endregion

    }

    public class GtSceneGuitarNote : GtSceneMusicItemBase

    {
        public GtSceneGuitarNote(BeatTick pStartPosition, BeatTick pEndPosition, int pString, int pFret)
            : base(pStartPosition, pEndPosition)
        {
            fString = pString;
            fFret = pFret;
        }

        private int fString;
        private int fFret;

        public int String
        {
            get
            {
                return fString;
            }
        }
        public int Fret
        {
            get
            {
                return fFret;
            }
        }

        public bool StartHited { get; set; }
        public bool Playing { get; set; }

        public NoteValue NoteValue
        {
            get
            {
                var noteValue = NoteValue.C;
                switch (this.String)
                {
                    case 1:
                        noteValue = NoteValue.E;
                        noteValue = CalculateTheActualNote(noteValue, this.Fret);

                        break;
                    case 2:
                        noteValue = NoteValue.B;
                        noteValue = CalculateTheActualNote(noteValue, this.Fret);

                        break;
                    case 3:
                        noteValue = NoteValue.G;
                        noteValue = CalculateTheActualNote(noteValue, this.Fret);

                        break;
                    case 4:
                        noteValue = NoteValue.D;
                        noteValue = CalculateTheActualNote(noteValue, this.Fret);

                        break;
                    case 5:
                        noteValue = NoteValue.A;
                        noteValue = CalculateTheActualNote(noteValue, this.Fret);

                        break;
                    case 6:
                        noteValue = NoteValue.E;
                        noteValue = CalculateTheActualNote(noteValue, this.Fret);

                        break;
                }

                return noteValue;
            }
        }

        private NoteValue CalculateTheActualNote(NoteValue noteValue, int pFret)
        {
            int fret = pFret;
            while (fret > 0)
            {
                noteValue = noteValue.Shift(Interval.MinorSecond);
                fret--;
            }

            return noteValue;
        }

        public int NoteNumber
        {
            get
            {
                int noteNumber = 0;

                switch (this.String)
                {
                    case 1:
                        noteNumber = CalculateNoteNumber(5, 8); //E5
                        break;
                    case 2:
                        noteNumber = CalculateNoteNumber(4, 1); //B4
                        break;
                    case 3:
                        noteNumber = CalculateNoteNumber(4, 5); //G4
                        break;
                    case 4:
                        noteNumber = CalculateNoteNumber(4, 10); //D4
                        break;
                    case 5:
                        noteNumber = CalculateNoteNumber(3, 3); //A3
                        break;
                    case 6:
                        noteNumber = CalculateNoteNumber(3, 8); //E3
                        break;
                }

                return noteNumber;
            }
        }

        private int CalculateNoteNumber(int pNoteNumber, int pFretOfC)
        {
            int fret = this.Fret;
            while (fret > 12)
            {
                fret -= 12;
                pNoteNumber++;
            }

            if (fret >= pFretOfC)
                pNoteNumber++;

            return pNoteNumber;
        }
    }
}
