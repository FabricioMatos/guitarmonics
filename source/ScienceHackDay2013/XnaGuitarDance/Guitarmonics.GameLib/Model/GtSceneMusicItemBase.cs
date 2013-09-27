using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guitarmonics.GameLib.Model
{
    public class GtSceneMusicItemBase : IGtSceneMusicItem
    {
        public GtSceneMusicItemBase(BeatTick pStartPosition, BeatTick pEndPosition)
        {
            this.fDistanceFromCurrentPosition = long.MaxValue;

            this.fStartPosition = pStartPosition;
            this.fEndPosition = pEndPosition;

            this.fCurrentPosition = BeatTick.NullValue;

            this.fIsGone = false;
        }

        #region Private Fields

        private long fDistanceFromCurrentPosition;
        private BeatTick fCurrentPosition;
        private BeatTick fStartPosition;
        private BeatTick fEndPosition;

        bool fIsGone;

        #endregion

        #region IGtSceneMusicItem Members

        public BeatTick CurrentPosition
        {
            get { return fCurrentPosition; }
        }

        public BeatTick StartPosition
        {
            get { return fStartPosition; }
        }

        public BeatTick EndPosition
        {
            get { return fEndPosition; }
            set { fEndPosition = value; }
        }


        public bool IsGone
        {
            get { return fIsGone; }
        }

        /// <summary>
        /// Distance in "ticks" of the start position from the current position
        /// </summary>
        public long DistanceFromCurrentPosition
        {
            get { return fDistanceFromCurrentPosition; }
        }

        public void Update(BeatTick pCurrentPosition)
        {
            fCurrentPosition = pCurrentPosition;

            this.fIsGone = (this.CurrentPosition > fEndPosition);

            this.fDistanceFromCurrentPosition = this.StartPosition.AsTicks() - this.CurrentPosition.AsTicks();
        }

        public void UpdatePosition(BeatTick pCurrentPosition)
        {
            fCurrentPosition = pCurrentPosition;

            this.fIsGone = (this.CurrentPosition > fEndPosition);
        }

        #endregion
    }
}
