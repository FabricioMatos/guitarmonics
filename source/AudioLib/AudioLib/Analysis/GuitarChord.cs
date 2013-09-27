using System;
using System.Collections.Generic;
using System.Text;
using Guitarmonics.AudioLib.Common;

namespace Guitarmonics.AudioLib.Analysis
{
    /// <summary>
    /// Use the knowledge of the guitar limitations in order to facilitate the chord identification.
    /// </summary>
    public class GuitarChord : ChordBase
    {
        public GuitarChord(List<IMusicalNote> pNotesList)
            : base(pNotesList)
        {

        }

        protected override void FilterUnusefulNotes()
        {
            //Too many harmonics indicate not well defined chord.
            if (fNotes.Count > 30)
            {
                fImprecise = true;
            }
            else
            {
                //remove notes (harmonics) out of the guitar extension
                for (int i = fNotes.Count - 1; i >= 0; i--)
                {
                    if ((fNotes[i].Number > 5) || (fNotes[i].Number < 3))
                    {
                        fNotes.RemoveAt(i);
                    }
                }

                //keep only the 6 more grave notes (we have 6 strings)
                for (int i = fNotes.Count - 1; i >= 6; i--)
                {
                    fNotes.RemoveAt(i);
                }
            }
        }
    }
}
