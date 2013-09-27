using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using Guitarmonics.AudioLib.Common;

namespace Guitarmonics.AudioLib.Common.Tests
{
    [TestFixture]
    public class AudioMathsTest
    {
        #region BeatTempoAsSeconds()
       
        [Test]
        public void BeatTempoAsSeconds_60()
        {
            double sec = AudioMaths.BeatTempoAsSeconds(60);
            Assert.AreEqual(1, sec);
        }

        [Test]
        public void BeatTempoAsSeconds_120()
        {
            double sec = AudioMaths.BeatTempoAsSeconds(120);
            Assert.AreEqual(0.5, sec);
        }

        [Test]
        public void BeatTempoAsSeconds_117()
        {
            double sec = AudioMaths.BeatTempoAsSeconds(117);
            Assert.AreEqual(60.0/117.0, sec);
        }

        #endregion

        #region TickTempoAsSeconds()

        [Test]
        public void TickTempoAsSeconds_120()
        {
            int bpm = 120;
            double sec = AudioMaths.TickTempoAsSeconds(bpm);
            Assert.AreEqual((60.0 / bpm) / 480.0, sec);
        }

        [Test]
        public void TickTempoAsSeconds_117()
        {
            int bpm = 117;
            double sec = AudioMaths.TickTempoAsSeconds(bpm);
            Assert.AreEqual((60.0 / bpm) / 480.0, sec);
        }

        #endregion

    }
}
