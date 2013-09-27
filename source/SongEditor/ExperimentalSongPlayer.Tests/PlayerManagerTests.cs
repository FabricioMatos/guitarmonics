using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Windows.Forms;

namespace ExperimentalSongPlayer.Tests
{
    [TestFixture]
    public class PlayerManagerTests : PlayerManager
    {
        [SetUp]
        public void SetUp()
        {
            //arrange
            Timer = new Timer();
        }
        [Test]
        public void StartEnablesTimer()
        {
            //act
            Start();

            //assert
            Assert.IsTrue(Timer.Enabled);
            Assert.AreEqual(0, CurrentMeasure);

        }

        [Test]
        public void StopDisablesTimer()
        {
            //arrange
            Timer.Enabled = true;

            //act
            Stop();

            //assert
            Assert.IsFalse(Timer.Enabled);
        }
    }
}
