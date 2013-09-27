using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Guitarmonics.GameLib.Model;

namespace Guitarmonics.GameLib.ModelTest
{
    [TestFixture]
    public class BeatTick_Operators
    {
        [Test]
        public void Equal()
        {
            BeatTick a = new BeatTick(1, 100);
            BeatTick b = new BeatTick(1, 100);

            Assert.IsTrue(a == b);
            Assert.IsTrue(a <= b);
            Assert.IsTrue(a >= b);
        }

        [Test]
        public void Diferent_1()
        {
            BeatTick a = new BeatTick(1, 100);
            BeatTick b = new BeatTick(1, 110);

            Assert.IsTrue(a != b);
        }

        [Test]
        public void Diferent_2()
        {
            BeatTick a = new BeatTick(1, 100);
            BeatTick b = new BeatTick(2, 100);

            Assert.IsTrue(a != b);
        }

        [Test]
        public void Less_1()
        {
            BeatTick a = new BeatTick(1, 100);
            BeatTick b = new BeatTick(2, 100);

            Assert.IsTrue(a < b);
            Assert.IsTrue(b > a);
        }

        [Test]
        public void Less_2()
        {
            BeatTick a = new BeatTick(1, 100);
            BeatTick b = new BeatTick(1, 110);

            Assert.IsTrue(a < b);
            Assert.IsTrue(b > a);
        }

        [Test]
        public void NullBeatTick()
        {
            BeatTick a = new BeatTick(-1, -1);
            Assert.AreEqual(BeatTick.NullValue, a);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidBeatValue))]
        public void InvalidBeat_1()
        {
            BeatTick a = new BeatTick(0, 0);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidBeatValue))]
        public void InvalidBeat_2()
        {
            BeatTick a = new BeatTick(-1, 0);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidTickValue))]
        public void InvalidTick_1()
        {
            BeatTick a = new BeatTick(1, -1);
        }

        [Test]
        public void ValidTick_TheBound()
        {
            BeatTick a = new BeatTick(1, 479);
            Assert.AreEqual(479, a.Tick);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidTickValue))]
        public void InvalidTick_2()
        {
            BeatTick a = new BeatTick(1, 480);
        }

        [Test]
        public void Add_0_Ticks()
        {
            var a = new BeatTick(1, 0);
            var b = a.AddTicks(0);

            Assert.AreEqual(a, b);
        }

        [Test]
        public void Add_1_Tick()
        {
            var a = new BeatTick(1, 0);
            var b = a.AddTicks(1);

            Assert.AreEqual(a.Beat, b.Beat);
            Assert.AreEqual(a.Tick+1, b.Tick);
        }

        [Test]
        public void Add_Many_Ticks()
        {
            var a = new BeatTick(1, 120);
            var b = a.AddTicks(480 + 480 + 10);

            Assert.AreEqual(a.Beat + 2, b.Beat);
            Assert.AreEqual(a.Tick + 10, b.Tick);
        }

        [Test]
        public void Add_1_Tick_ChangingTheBeat()
        {
            var a = new BeatTick(1, 479);
            var b = a.AddTicks(1);

            Assert.AreEqual(2, b.Beat);
            Assert.AreEqual(0, b.Tick);
        }

        [TestCase(1)]
        [TestCase(2)]
        [TestCase(120)]
        [TestCase(1000)]
        public void SubTicks(int pTicks)
        {
            var a = new BeatTick(10, 479);

            Assert.AreEqual(a.AddTicks(-pTicks), a.SubTicks(pTicks));
        }
    
    }

}
