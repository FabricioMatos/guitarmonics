using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib.Model;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;
using Microsoft.Xna.Framework;

namespace Guitarmonics.GameLib.Controller.Test.ModelTests
{
    [TestFixture]
    public class HSLColorTest
    {
        [Test]
        public void ColorsEquivalence()
        {
            Color color = new HSLColor(0, 240, 120);
            Assert.AreEqual(Color.Red, color);
        }

        [Test]
        public void SolidBlue()
        {
            Color color = new HSLColor(160, 240, 120);
            Assert.AreEqual(Color.Blue, color);
        }

        [Test]
        public void SolidYellow()
        {
            Color color = new HSLColor(40, 240, 120);
            Assert.AreEqual(Color.Yellow, color);
        }

        /// <summary>
        /// A round problem was causing an error
        /// </summary>
        [Test]
        public void TestBugFix()
        {
            Color color2 = new HSLColor(120, 240, 120); //cyan: RGB (0,255,255)
            Color color1 = new HSLColor(140, 240, 120); //RGB (0,128,255)

            Assert.AreNotEqual(color1, color2);
        }
    }
}
