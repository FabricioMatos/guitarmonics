using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Common;
using NUnit.Framework;

namespace Guitarmonics.AudioLib.Tests.Common
{
    [TestFixture]
    public class BassWrapperTests
    {
        [TearDown]
        public void TearDown()
        {
            BassWrapper.Instance.BassFree();
        }

        [Test]
        public void TestBassInit()
        {
            IBassWrapper bassWrapper = new BassWrapper();

            Assert.IsFalse(bassWrapper.Initiallized);

            bool result = bassWrapper.BassInit();

            Assert.IsTrue(result);
            Assert.IsTrue(bassWrapper.Initiallized);

            result = bassWrapper.BassInit(); //the second call dont raise any exception

            Assert.IsTrue(result);
            Assert.IsTrue(bassWrapper.Initiallized);

        }

        [Test]
        public void BassWrapperDefaultInstance()
        {
            var bassWrapper1 = BassWrapper.Instance;
            var bassWrapper2 = BassWrapper.Instance;

            Assert.AreSame(bassWrapper1, bassWrapper2);
        }
    }
}
