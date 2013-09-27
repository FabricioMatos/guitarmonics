using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Common;

namespace Guitarmonics.AudioLib.Analysis
{
    public interface ISpectrumAnalyzer
    {
        List<IMusicalNote> GetMusicalNotes(float[] pFft);
        void DeleteUnusefulNotes(ref List<IMusicalNote> pNotes);
    }

    public class SpectrumAnalyzer : ISpectrumAnalyzer
    {
        public virtual List<IMusicalNote> GetMusicalNotes(float[] pFft)
        {
            var notes = new List<IMusicalNote>();

            for (int i = 1; i < pFft.Length - 1; i++)
            {
                //if (pFft[i] < 0.0005f)
                if (pFft[i] < 0.025f)
                        continue;

                //calculate the frequence relative to position "i" in FFT
                var frequence = (i * 44100.0f) / pFft.Length;

                //Low cut in 96,89941Hz
                if (frequence <= 96.89941f)
                    continue;

                //check if pFft[i] is a local maximum
                if ((pFft[i] > pFft[i - 1]) && (pFft[i] > pFft[i + 1]))
                {
                    var note = new MusicalNote(frequence, pFft[i]);

                    notes.Add(note);
                }
            }

            return notes;
        }

        public void SortMusicalNotesByVolume(ref List<IMusicalNote> pNotes)
        {
            pNotes.Sort(CompareDinosByLength);
        }

        private static int CompareDinosByLength(IMusicalNote pNote1, IMusicalNote pNote2)
        {
            if ((pNote1 == null) || (pNote2 == null))
                return 0;

            if (pNote1.Volume == pNote2.Volume)
                return 0;

            if (pNote1.Volume > pNote2.Volume)
                return -1;

            return 1;
        }

        public void DeleteUnusefulNotes(ref List<IMusicalNote> pNotes)
        {
            float maxVolume = 0;

            foreach (var note in pNotes)
            {
                if (note.Volume > maxVolume)
                    maxVolume = note.Volume;
            }

            for (int i = pNotes.Count - 1; i >= 0; i--)
            {
                if (pNotes[i].Number < 3)
                    pNotes.RemoveAt(i);
                else if (pNotes[i].Volume < (maxVolume / 10.0f))
                    pNotes.RemoveAt(i);
            }

            while (pNotes.Count() > 6)
            {
                pNotes.RemoveAt(pNotes.Count() - 1);
            }
        }
    }

    public class DoubleSpectrumAnalyzerAlwaysE : SpectrumAnalyzer
    {
        public override List<IMusicalNote> GetMusicalNotes(float[] pFft)
        {
            var notes = new List<IMusicalNote>();

            notes.Add(new MusicalNote("E3"));
            notes.Add(new MusicalNote("B3"));
            notes.Add(new MusicalNote("E4"));
            notes.Add(new MusicalNote("Ab4"));
            notes.Add(new MusicalNote("B4"));
            notes.Add(new MusicalNote("E5"));

            return notes;
        }
    }
}
