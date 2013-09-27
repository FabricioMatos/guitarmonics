using System;
using System.Collections.Generic;
using System.Text;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.AudioLib.Analysis;

namespace Guitarmonics.AudioLib.Common
{
    public class OctaveOutOfRange : Exception
    {
        public OctaveOutOfRange(string pMessage)
            : base(pMessage)
        {
        }
    }

    /// <summary>
    /// Used to map frequences to musical notes, and vice-versa.
    /// We can construct the note giving the note frequence and letting
    /// the class calculate the musical note (and the cents distant of the 
    /// correct frequence)
    /// </summary>
    public class MusicalNote : IMusicalNote
    {
        /// <summary>
        /// construtor (by note string id - C#4, for example)
        /// </summary>
        /// <param name="pPlayedFrequence">note frequence</param>
        public MusicalNote(string pNoteId)
        {
            if (!NoteToFrequenceTable.Instance.Table.ContainsKey(pNoteId))
            {
                throw new InvalidMusicalNoteId(string.Format("The note {0} is not valid.", pNoteId));
            }
            var playedFrequence = NoteToFrequenceTable.Instance.Table[pNoteId];

            SetupNoteBasedOnPlayedFrequence(playedFrequence, 0);
        }

        /// <summary>
        /// construtor (by note string id - C#4, for example)
        /// </summary>
        public MusicalNote(string pNoteId, float pVolume)
        {
            if (!NoteToFrequenceTable.Instance.Table.ContainsKey(pNoteId))
            {
                throw new InvalidMusicalNoteId(string.Format("The note {0} is not valid.", pNoteId));
            }
            var playedFrequence = NoteToFrequenceTable.Instance.Table[pNoteId];

            SetupNoteBasedOnPlayedFrequence(playedFrequence, pVolume);
        }


        /// <summary>
        /// construtor (by note frequence)
        /// </summary>
        /// <param name="pPlayedFrequence">note frequence</param>
        public MusicalNote(float pPlayedFrequence)
        {
            SetupNoteBasedOnPlayedFrequence(pPlayedFrequence, 0);
        }

        /// <summary>
        /// construtor (by note frequence and volume)
        /// </summary>
        /// <param name="pPlayedFrequence"></param>
        /// <param name="pVolume"></param>
        public MusicalNote(float pPlayedFrequence, float pVolume)
        {
            SetupNoteBasedOnPlayedFrequence(pPlayedFrequence, pVolume);
        }

        #region Fields
        private NoteValue fValue;
        private int fNumber;
        private float fPlayedFrequence;
        private float fFrequence;
        private int fCents;
        #endregion

        #region Properties
        /// <summary>
        /// Enum indicating the note itself (C, D, F#/Gb, etc..)
        /// </summary>
        public NoteValue Value
        {
            get
            {
                return fValue;
            }
        }

        /// <summary>
        /// The number of the note, like the numbers in: A4, G3, F5, etc..
        /// </summary>
        public int Number
        {
            get
            {
                return fNumber;
            }
        }

        /// <summary>
        /// The note frequence as it was played
        /// </summary>
        public float PlayedFrequence
        {
            get
            {
                return fPlayedFrequence;
            }
        }

        /// <summary>
        /// The correct frequence of the detected note
        /// </summary>
        public float Frequence
        {
            get
            {
                return fFrequence;
            }
        }
        /// <summary>
        /// The diference (in cents) between PlayedFrequence and Frequence.
        /// Near to zero means more tunned.
        /// </summary>
        public int Cents
        {
            get
            {
                return fCents;
            }
        }

        /// <summary>
        /// Valume of the note (frequence level in FFT analysis).
        /// Usually less then 1.
        /// </summary>
        public float Volume { get; private set; }

        #endregion

        #region Methods

        /// <summary>
        /// Initialize the internal fields calculating the corret note based on the given frequence (pPlayedFrequence)
        /// Note the use of the NoteToFrequenceTable in order to calculate the frequence. This table adopted 
        /// some A4 for reference (usually, A4 = 440Hz)
        /// </summary>
        /// <param name="pPlayedFrequence"></param>
        private void SetupNoteBasedOnPlayedFrequence(float pPlayedFrequence, float pVolume)
        {
            this.fPlayedFrequence = pPlayedFrequence;
            this.Volume = pVolume;

            var noteName = "";
            var distance = float.MaxValue;
            var correctFrequence = 0f;
            foreach (var item in NoteToFrequenceTable.Instance.Table)
            {
                if (Math.Abs(pPlayedFrequence - item.Value) < distance)
                {
                    distance = pPlayedFrequence - item.Value;
                    noteName = item.Key;
                    correctFrequence = item.Value;
                }
            }

            this.fNumber = 0;
            this.fFrequence = correctFrequence;
            this.fCents = (int)Math.Truncate((100 * distance) / (AudioMaths.CalculateNoteFrequence(correctFrequence, 1) - correctFrequence));

            if (noteName.Length == 2)
            {
                switch (noteName[0])
                {
                    case 'C':
                        this.fValue = NoteValue.C;
                        break;
                    case 'D':
                        this.fValue = NoteValue.D;
                        break;
                    case 'E':
                        this.fValue = NoteValue.E;
                        break;
                    case 'F':
                        this.fValue = NoteValue.F;
                        break;
                    case 'G':
                        this.fValue = NoteValue.G;
                        break;
                    case 'A':
                        this.fValue = NoteValue.A;
                        break;
                    case 'B':
                        this.fValue = NoteValue.B;
                        break;
                    default:
                        break;
                }

                this.fNumber = int.Parse(noteName.Substring(1));
            }
            else
            {
                switch (noteName.Substring(0, 2))
                {
                    case "C#":
                        this.fValue = NoteValue.Db;
                        break;
                    case "Db":
                        this.fValue = NoteValue.Db;
                        break;
                    case "D#":
                        this.fValue = NoteValue.Eb;
                        break;
                    case "Eb":
                        this.fValue = NoteValue.Eb;
                        break;
                    case "F#":
                        this.fValue = NoteValue.Gb;
                        break;
                    case "Gb":
                        this.fValue = NoteValue.Gb;
                        break;
                    case "G#":
                        this.fValue = NoteValue.Ab;
                        break;
                    case "Ab":
                        this.fValue = NoteValue.Ab;
                        break;
                    case "A#":
                        this.fValue = NoteValue.Bb;
                        break;
                    case "Bb":
                        this.fValue = NoteValue.Bb;
                        break;
                    default:
                        break;
                }

                this.fNumber = int.Parse(noteName.Substring(2, 1));
            }

        }


        /// <summary>
        /// Return a string description of the note: C, Bb, A, etc..
        /// We're talking about notes, not chords (Am7, etc..)
        /// In terms of frequence (in a equal-tempered scale), C# = Db, D# = Eb, and so on.
        /// So here we choose always show the bemol form.
        /// </summary>
        /// <returns></returns>
        public override string ToString()
        {
            string note = "";

            switch (this.Value)
            {
                case NoteValue.C:
                    note = "C";
                    break;
                case NoteValue.Db:
                    note = "Db";
                    break;
                case NoteValue.D:
                    note = "D";
                    break;
                case NoteValue.Eb:
                    note = "Eb";
                    break;
                case NoteValue.E:
                    note = "E";
                    break;
                case NoteValue.F:
                    note = "F";
                    break;
                case NoteValue.Gb:
                    note = "Gb";
                    break;
                case NoteValue.G:
                    note = "G";
                    break;
                case NoteValue.Ab:
                    note = "Ab";
                    break;
                case NoteValue.A:
                    note = "A";
                    break;
                case NoteValue.Bb:
                    note = "Bb";
                    break;
                case NoteValue.B:
                    note = "B";
                    break;
            }

            return note + this.Number.ToString();
        }

        /// <summary>
        /// Produce a new MusicalNote distanced by pNumberOfOctave octaves.
        /// </summary>
        /// <param name="pNumberOfOctave">Number of octaves. Can be negative.</param>
        /// <returns></returns>
        public MusicalNote Octave(int pNumberOfOctave)
        {
            var number = this.Number + pNumberOfOctave;

            var noteId = this.Value.ToString() + number.ToString();

            if ((number > 9) || (number < 0))
                throw new OctaveOutOfRange(string.Format(
                    "MusicalNote Octave operation produced a value out of range (NoteId = {0}).", 
                    noteId));

            return new MusicalNote(noteId);
        }

        #endregion

        #region Operators

        public static bool operator ==(MusicalNote a, MusicalNote b)
        {
            if (System.Object.ReferenceEquals(a, null) && (System.Object.ReferenceEquals(b, null)))
                return true;

            if (System.Object.ReferenceEquals(a, null) || (System.Object.ReferenceEquals(b, null)))
                return false;

            return (a.Frequence == b.Frequence);
        }

        public static bool operator !=(MusicalNote a, MusicalNote b)
        {
            return !(a == b);
        }

        public static bool operator <(MusicalNote a, MusicalNote b)
        {
            if ((a == null) || (b == null))
                return false;

            return (a.Frequence < b.Frequence);
        }

        public static bool operator <=(MusicalNote a, MusicalNote b)
        {
            return (a < b) || (a == b);
        }

        public static bool operator >(MusicalNote a, MusicalNote b)
        {
            return !(a <= b);
        }

        public static bool operator >=(MusicalNote a, MusicalNote b)
        {
            return !(a < b);
        }

        #endregion

    }
        
    public enum NoteValue
    {
        C = 0,
        Db = 1,
        D = 2,
        Eb = 3,
        E = 4,
        F = 5,
        Gb = 6,
        G = 7,
        Ab = 8,
        A = 9,
        Bb = 10,
        B = 11
    }

    public static class NoteValueExtension
    {
        public static string AsSharpedString(this NoteValue value)
        {
            string result = "";
            switch (value)
            {
                case NoteValue.C:
                    result =  "C";
                    break;
                case NoteValue.Db:
                    result =  "C#";
                    break;
                case NoteValue.D:
                    result =  "D";
                    break;
                case NoteValue.Eb:
                    result =  "D#";
                    break;
                case NoteValue.E:
                    result =  "E";
                    break;
                case NoteValue.F:
                    result =  "F";
                    break;
                case NoteValue.Gb:
                    result =  "F#";
                    break;
                case NoteValue.G:
                    result =  "G";
                    break;
                case NoteValue.Ab:
                    result =  "G#";
                    break;
                case NoteValue.A:
                    result =  "A";
                    break;
                case NoteValue.Bb:
                    result =  "A#";
                    break;
                case NoteValue.B:
                    result =  "B";
                    break;
            }

            return result;
        }
    }
}
