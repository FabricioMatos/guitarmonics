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
    public class XmlScoreSyncReaderTest
    {
        private const string SyncSongFile_TesteOk = TestConfig.ConstAudioPath + "TesteOk.sync.xml";

        [Test]
        public void ValidFileName()
        {
            var xmlScoreReader = new XmlScoreSyncReader(SyncSongFile_TesteOk);
            Assert.IsNotNull(xmlScoreReader);
        }


        [Test]
        public void ImportXmlUsingXmlScoreReader()
        {
            var xmlScoreReader = new XmlScoreSyncReader(SyncSongFile_TesteOk);
            Assert.AreEqual(12, xmlScoreReader.SyncElements.Count);

            Assert.AreEqual(1000, xmlScoreReader.SyncElements[0].MomentInMiliseconds);
            Assert.AreEqual(1000, xmlScoreReader.SyncElements[1].MomentInMiliseconds);
            Assert.AreEqual(1000, xmlScoreReader.SyncElements[2].MomentInMiliseconds);

            Assert.AreEqual(1333, xmlScoreReader.SyncElements[3].MomentInMiliseconds);
            Assert.AreEqual(1333, xmlScoreReader.SyncElements[4].MomentInMiliseconds);
            Assert.AreEqual(1333, xmlScoreReader.SyncElements[5].MomentInMiliseconds);

            Assert.AreEqual(3000, xmlScoreReader.SyncElements[6].MomentInMiliseconds);
            Assert.AreEqual(3000, xmlScoreReader.SyncElements[7].MomentInMiliseconds);
            Assert.AreEqual(3000, xmlScoreReader.SyncElements[8].MomentInMiliseconds);

            Assert.AreEqual(9000, xmlScoreReader.SyncElements[9].MomentInMiliseconds);
            Assert.AreEqual(9000, xmlScoreReader.SyncElements[10].MomentInMiliseconds);
            Assert.AreEqual(9000, xmlScoreReader.SyncElements[11].MomentInMiliseconds);
        }
    }
}
