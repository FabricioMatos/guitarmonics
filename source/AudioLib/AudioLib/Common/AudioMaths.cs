using System;
using System.Collections.Generic;
using System.Text;

namespace Guitarmonics.AudioLib.Common
{
    public static class AudioMaths
    {
        public static double BeatTempoAsSeconds(double pTempoBPM)
        {
            //1 bpm = 60 bps
            return 60.0 / pTempoBPM;
        }

        public static double TickTempoAsSeconds(double pTempoBPM)
        {
            //There are 480 ticks in one beat.
            return BeatTempoAsSeconds(pTempoBPM) / 480.0;
        }

        public static float CalculateNoteFrequence(float pReferenceFrequence, int pDistanceInHalfSteps)
        {
            //fn = f0 * a^n, where:
            //fn = the frequency of the note n half steps away.
            //f0 =  the frequency of one fixed note which must be defined. A common choice is setting the A above middle C (A4) at f0 =  440 Hz.
            //n = the number of half steps away from the fixed note you are. If you are at a higher note, n is positive. If you are on a lower note, n is negative.
            //a = (2)^1/12 = the twelth root of 2 = the number which when multiplied by itself 12 times equals 2 = 1.059463094359...
            //http://www.phy.mtu.edu/~suits/NoteFreqCalcs.html

            double a = Math.Pow(2.0, 1.0 / 12.0);
            return (float)(pReferenceFrequence * Math.Pow(a, pDistanceInHalfSteps));
        }

    }
}
