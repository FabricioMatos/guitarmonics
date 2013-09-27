using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guitarmonics.GameLib.Model
{
    /// <summary>
    /// Represent the pair Beat:Tick used to point a music position (like in MIDI sequences)
    /// </summary>
    public struct BeatTick
    {
        public BeatTick(long pBeat, long pTick)
        {
            //Skip the validation in case of null BeatTick: new BeatTick(-1, -1)
            if ((pBeat != -1) || (pTick != -1))
            {
                if (pBeat <= 0)
                    throw new InvalidBeatValue(string.Format("The beat:tick {0}:{1} is invalid. The first beat value is 1.", pBeat, pTick));

                if (pTick < 0)
                    throw new InvalidTickValue(string.Format("The beat:tick {0}:{1} is invalid. The first tick value is 0.", pBeat, pTick));

                if (pTick > 479)
                    throw new InvalidTickValue(string.Format("The beat:tick {0}:{1} is invalid. The last valid tick is 479.", pBeat, pTick));
            }

            fBeat = pBeat;
            fTick = pTick;
        }

        private long fBeat;
        private long fTick;

        public long Beat
        {
            get { return fBeat; }
        }

        public long Tick
        {
            get { return fTick; }
        }

        public static BeatTick NullValue = new BeatTick(-1, -1);
        public static BeatTick FirstBeat = new BeatTick(1, 0);

        #region Operators

        public static bool operator ==(BeatTick a, BeatTick b)
        {
            return (a.Beat == b.Beat) &&
                (a.Tick == b.Tick);

        }

        public static bool operator !=(BeatTick a, BeatTick b)
        {
            return !(a == b);
        }

        public static bool operator <(BeatTick a, BeatTick b)
        {
            if (a.Beat != b.Beat)
                return (a.Beat < b.Beat);

            if (a.Tick != b.Tick)
                return (a.Tick < b.Tick);

            return false; //they are equal
        }

        public static bool operator <=(BeatTick a, BeatTick b)
        {
            return (a < b) || (a == b);
        }

        public static bool operator >(BeatTick a, BeatTick b)
        {
            return !(a <= b);
        }

        public static bool operator >=(BeatTick a, BeatTick b)
        {
            return !(a < b);
        }

        //just to avoid a warning
        public override int GetHashCode()
        {
            return (int)((fBeat * 480) + fTick);
        }

        //just to avoid a warning
        public override bool Equals(Object obj)
        {
            if (!(obj is BeatTick))
                return false;

            return (this == (BeatTick)obj);
        }
        #endregion

        #region Methods

        public BeatTick AddTicks(long pTicks)
        {
            long totalTicks = this.Beat * 480 + this.Tick + pTicks;

            long beat = totalTicks / 480;
            long tick = totalTicks % 480;

            return new BeatTick(beat, tick);
        }

        public BeatTick SubTicks(long pTicks)
        {
            return this.AddTicks(-pTicks);
        }

        public long AsTicks()
        {
            return ((this.Beat - 1) * 480) + this.Tick;
        }

        public override string ToString()
        {
            return string.Format("BeatTick({0}, {1})", this.fBeat, this.fTick);
        }

        #endregion

    }
}
