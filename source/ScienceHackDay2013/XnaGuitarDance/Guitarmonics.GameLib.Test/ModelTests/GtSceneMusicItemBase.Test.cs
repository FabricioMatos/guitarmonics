using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Guitarmonics.GameLib.View;
using Guitarmonics.GameLib.Model;

namespace Guitarmonics.GameLib.ViewTest
{
    [TestFixture]
    public class GtSceneMusicItemBaseTest
    {
        [Test]
        public void ConstructorAndInitializations()
        {
            IGtSceneMusicItem item = new GtSceneMusicItemBase(new BeatTick(1, 0), new BeatTick(1, 470));

            Assert.AreEqual(1, item.StartPosition.Beat);
            Assert.AreEqual(0, item.StartPosition.Tick);

            Assert.AreEqual(1, item.EndPosition.Beat);
            Assert.AreEqual(470, item.EndPosition.Tick);

            Assert.AreEqual(-1, item.CurrentPosition.Beat);
            Assert.AreEqual(-1, item.CurrentPosition.Tick);

            Assert.IsFalse(item.IsGone);
        }

        [Test]
        public void IsGone_NotChanged()
        {
            IGtSceneMusicItem item = new GtSceneMusicItemBase(new BeatTick(1, 0), new BeatTick(1, 470));
            
            item.UpdatePosition(new BeatTick(1, 100));
            
            Assert.IsFalse(item.IsGone);
        }

        [Test]
        public void IsGone_NotChanged_AtBound()
        {
            IGtSceneMusicItem item = new GtSceneMusicItemBase(new BeatTick(1, 0), new BeatTick(1, 470));

            item.UpdatePosition(new BeatTick(1, 470));

            Assert.IsFalse(item.IsGone);
        }

        [Test]
        public void IsGone_Changed()
        {
            IGtSceneMusicItem item = new GtSceneMusicItemBase(new BeatTick(1, 0), new BeatTick(1, 470));

            item.UpdatePosition(new BeatTick(2, 0));

            Assert.IsTrue(item.IsGone);
        }
    }
}
