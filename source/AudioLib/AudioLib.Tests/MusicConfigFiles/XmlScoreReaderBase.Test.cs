using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Guitarmonics.AudioLib.MusicConfigFiles;
using Guitarmonics.AudioLib.Common;

namespace Guitarmonics.AudioLib.Tests.MusicConfigFiles
{
    [TestFixture]
    public class XmlScoreReaderBaseTest
    {      
        private const string SongFile_TesteOk = TestConfig.ConstAudioPath + "TesteOk.song.xml";

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidFileName))]
        public void InvalidFileName()
        {
            var xmlScoreReader = new XmlScoreReaderBase("invalid file.xml");
        }

        [Test]
        public void ValidFileName()
        {
            var xmlScoreReader = new XmlScoreReaderBase(SongFile_TesteOk);
            Assert.IsNotNull(xmlScoreReader);
        }


    }
}
