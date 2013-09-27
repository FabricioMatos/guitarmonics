using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace Guitarmonics.GameLib.Model
{
    public interface IGtSceneMusicItem
    {
        BeatTick CurrentPosition { get; }

        BeatTick StartPosition { get; }

        BeatTick EndPosition { get; }

        bool IsGone { get; }

        void UpdatePosition(BeatTick pCurrentPosition);
    }
}
