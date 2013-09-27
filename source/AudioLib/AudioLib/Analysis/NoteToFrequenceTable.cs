using System;
using System.Collections.Generic;
using System.Text;
using Guitarmonics.AudioLib.Common;

namespace Guitarmonics.AudioLib.Analysis
{
    public class InvalidNoteName : Exception
    {
        public InvalidNoteName(string pMessage)
            : base(pMessage)
        {
        }
    }

    public class NoteToFrequenceTable
    {
        public static NoteToFrequenceTable Instance = new NoteToFrequenceTable();

        public NoteToFrequenceTable()
        {
            InitializeNoteFrequenceTable();
        }

        #region Private Filds

        private float fA4Frequence;
        private Dictionary<string, float> fTable = new Dictionary<string, float>(120);

        #endregion

        #region Public Properties

        public Dictionary<string, float> Table
        {
            get
            {
                return fTable;
            }
        }

        #endregion

        #region Public Methods

        public bool ContainsKey(string pNoteName)
        {
            return fTable.ContainsKey(pNoteName);
        }

        public float this[string pNoteName]
        {
            get 
            {
                if (pNoteName == null)
                {
                    throw new InvalidNoteName("pNoteName parameter can't be null.");
                }

                //var regex = new Regex("^[CDEFGAB][#b][123456789]$");
                //if (!regex.Match(pNote).Success)
                //{
                //    throw new InvalidNote(string.Format("Can't calculate the frequence of a invalid musical note: {0}.", pNote));
                //}

                if (!NoteToFrequenceTable.Instance.ContainsKey(pNoteName))
                {
                    throw new InvalidNoteName(string.Format("Note \"{0}\" not found.", pNoteName));
                }
 
                return fTable[pNoteName]; 
            }
        }

        #endregion

        #region Private Methods

        private void InitializeNoteFrequenceTable()
        {
            fA4Frequence = 440.0f; //A4 = 440Hz

            //A0 has distance of -60 => C0 has distance of -57 (-60 + 3)
            int distanceFromA4 = -57;

            //Add from C0 to B9
            for (int i = 0; i <= 9; i++)
            {
                fTable.Add("C" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4++));
                fTable.Add("C#" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4));
                fTable.Add("Db" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4++));
                fTable.Add("D" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4++));
                fTable.Add("D#" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4));
                fTable.Add("Eb" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4++));
                fTable.Add("E" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4++));
                fTable.Add("F" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4++));
                fTable.Add("F#" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4));
                fTable.Add("Gb" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4++));
                fTable.Add("G" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4++));
                fTable.Add("G#" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4));
                fTable.Add("Ab" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4++));
                fTable.Add("A" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4++));
                fTable.Add("A#" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4));
                fTable.Add("Bb" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4++));
                fTable.Add("B" + i.ToString(),
                    AudioMaths.CalculateNoteFrequence(fA4Frequence, distanceFromA4++));
            }
        }
        
        #endregion

    }
}
