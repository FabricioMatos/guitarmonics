using System;
using System.Collections.Generic;
using System.Text;
using Guitarmonics.AudioLib.Common;

namespace Guitarmonics.AudioLib.Analysis
{
    public enum Interval
    {
        MinorSecond,    //(1 half step)
        MajorSecond,    //(2 half steps)
        MinorThird,     //(3 half steps)
        MajorThird,     //(4 half steps)
        PerfectFourth,  //(5 half steps)
        DiminishedFifth,//(6 half steps) //or AugmentedForth
        PerfectFifth,   //(7 half steps)
        AugmentedFifth, //(8 half steps)
        MajorSixth,     //(9 half steps)
        MinorSeventh,   //(10 half steps) //or AugmentedSixth
        MajorSeventh,   //(11 half steps)
        PerfectOctave   //(12 half steps)
    }

    public static class NoteValueExtension
    {
        public static NoteValue Shift(this NoteValue pNote, Interval pInterval)
        {
            int noteAsInt = (int)pNote;
            int intervalAsInt = (int)pInterval;

            return (NoteValue)((noteAsInt + intervalAsInt + 1) % 12);
        }

        public static Interval Interval(this NoteValue pNote1, NoteValue pNote2, out int pOctaves)
        {
            int difference = (int)pNote2 - (int)pNote1 - 1;
            pOctaves = 0;

            while (difference < 0)
            {
                difference += 12;
                pOctaves++;
            }

            return (Interval)(difference % 12);
        }

        public static Interval Interval(this NoteValue pNote1, NoteValue pNote2)
        {
            int octaves;
            return Interval(pNote1, pNote2, out octaves);
        }
    
    }

    /// <summary>
    /// Represent any set of musical notes, used for solos, triads, guitar chords, etc.. 
    /// </summary>
    public class NoteSet
    {
        public NoteSet(List<IMusicalNote> pNotes)
        {
            if (pNotes == null)
            {
                throw new InvalidChord("The parameter pNotesList can't be null");
            }

            fNotes = pNotes;
        }

        protected List<IMusicalNote> fNotes;

        public List<IMusicalNote> Notes
        {
            get
            {
                return fNotes;
            }
        }

        protected bool FindNote(NoteValue pNote)
        {
            foreach (var note in this.fNotes)
            {
                if (note.Value == pNote)
                    return true;
            }

            return false;
        }
    }

    /// <summary>
    /// 
    /// </summary>
    public class ChordBase : NoteSet
    {
        public ChordBase(List<IMusicalNote> pNotesList) : base(pNotesList)
        {
            //if (pNotesList.Count < 3)
            //{
            //    throw new InvalidChord("The chord must have at least 3 notes");
            //}

            FilterUnusefulNotes();
            DefineSuitableChord();
        }

        #region Fields

        protected NoteValue? fTonicNote;
        protected Interval? fThirdInterval;
        protected Interval? fFourthInterval;
        protected Interval? fFifthInterval;
        protected Interval? fSixthInterval;
        protected Interval? fSeventhInterval;
        protected Interval? fNinthInterval;
        protected bool fImprecise = false;

        #endregion

        #region Properties

        public NoteValue? TonicNote
        {
            get { return fTonicNote; }
        }

        public Interval? ThirdInterval
        {
            get { return fThirdInterval; }
        }

        public Interval? FourthInterval
        {
            get { return fFourthInterval; }
        }

        public Interval? FifthInterval
        {
            get { return fFifthInterval; }
        }

        public Interval? SixthInterval
        {
            get { return fSixthInterval; }
        }

        public Interval? SeventhInterval
        {
            get { return fSeventhInterval; }
        }

        public Interval? NinthInterval
        {
            get { return fNinthInterval; }
        }

        #endregion

        protected virtual void FilterUnusefulNotes()
        {
            //do nothing
        }

        /// <summary>
        /// Our general strategy is define the chord based on the most grave note,
        /// considering all other notes as accidents.
        /// In this scenario, D/F#, for example, will be something like F#m5+
        /// </summary>
        protected virtual void DefineSuitableChord()
        {
            fTonicNote = fNotes[0].Value;

            LookForTheThird();

            LookForTheFourth();

            LookForTheFifth();

            LookForTheSixth();

            LookForTheSeventh();

            LookForTheNinth();
        }

        #region Identify each interval

        private void LookForTheThird()
        {
            if (this.FindNote(((NoteValue)fTonicNote).Shift(Interval.MajorThird)))
            {
                fThirdInterval = Interval.MajorThird;
            }
            else if (this.FindNote(((NoteValue)fTonicNote).Shift(Interval.MinorThird)))
            {
                fThirdInterval = Interval.MinorThird;
            }
            else
            {
                fThirdInterval = null;
            }
        }

        private void LookForTheFourth()
        {
            if (this.FindNote(((NoteValue)fTonicNote).Shift(Interval.PerfectFourth)))
            {
                fFourthInterval = Interval.PerfectFourth;
            }
            else
            {
                fFourthInterval = null;
            }
        }

        private void LookForTheFifth()
        {
            if (this.FindNote(((NoteValue)fTonicNote).Shift(Interval.PerfectFifth)))
            {
                fFifthInterval = Interval.PerfectFifth;
            }
            else if (this.FindNote(((NoteValue)fTonicNote).Shift(Interval.DiminishedFifth)))
            {
                fFifthInterval = Interval.DiminishedFifth;
            }
            else if (this.FindNote(((NoteValue)fTonicNote).Shift(Interval.AugmentedFifth)))
            {
                fFifthInterval = Interval.AugmentedFifth;
            }
            else
            {
                fFifthInterval = null;
            }
        }

        private void LookForTheSixth()
        {
            if (this.FindNote(((NoteValue)fTonicNote).Shift(Interval.MajorSixth)))
            {
                fSixthInterval = Interval.MajorSixth;
            }
            else
            {
                fSixthInterval = null;
            }
        }

        private void LookForTheSeventh()
        {
            if (this.FindNote(((NoteValue)fTonicNote).Shift(Interval.MajorSeventh)))
            {
                fSeventhInterval = Interval.MajorSeventh;
            }
            else if (this.FindNote(((NoteValue)fTonicNote).Shift(Interval.MinorSeventh)))
            {
                fSeventhInterval = Interval.MinorSeventh;
            }
            else
            {
                fSeventhInterval = null;
            }
        }

        private void LookForTheNinth()
        {
            if (this.FindNote(((NoteValue)fTonicNote).Shift(Interval.MajorSecond)))
            {
                fNinthInterval = Interval.MajorSecond;
            }
            else if (this.FindNote(((NoteValue)fTonicNote).Shift(Interval.MinorSecond)))
            {
                fNinthInterval = Interval.MinorSecond;
            }
            else
            {
                fNinthInterval = null;
            }
        }

        #endregion


        public override string ToString()
        {
            if (fTonicNote == null)
                return "(null)";

            string sChord = fTonicNote.ToString();
            string slash = "";


            if (fThirdInterval == Interval.MinorThird)
            {
                sChord += "m";
            }

            if (fFourthInterval == Interval.PerfectFourth)
            {
                sChord += slash + "4";
                slash = "/";
            }

            if (fSixthInterval == Interval.MajorSixth)
            {
                sChord += slash + "6";
                slash = "/";
            }

            if (fSeventhInterval == Interval.MinorSeventh)
            {
                sChord += slash + "7";
                slash = "/";
            }
            else if (fSeventhInterval == Interval.MajorSeventh)
            {
                sChord += slash + "7+";
                slash = "/";
            }

            if (fNinthInterval == Interval.MinorSecond)
            {
                sChord += slash + "9b";
                slash = "/";
            }
            else if (fNinthInterval == Interval.MajorSecond)
            {
                sChord += slash + "9";
                slash = "/";
            }

            if (fFifthInterval == Interval.AugmentedFifth)
            {
                sChord += "(#5)";
            }
            else if (fFifthInterval == Interval.DiminishedFifth)
            {
                sChord += "(b5)";
            }

            return sChord;
        }

        public string Description()
        {
            var result = "";

            if (this.fImprecise)
            {
                result += "(imprecise) ";
            }

            foreach (var note in fNotes)
            {
                result += note.ToString() + "-";
            }
            return result;
        }
    }
}
