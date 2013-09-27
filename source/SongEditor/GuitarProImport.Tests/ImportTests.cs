using System;
using System.Text;
using System.Collections.Generic;
using System.Linq;
using NUnit.Framework;
using Guitarmonics.SongData;

namespace Guitarmonics.Importer.Tests
{
    /// <summary>
    /// Summary description for UnitTest1
    /// </summary>
    [TestFixture]
    public class ImportTests
    {
        private const string BASE_PATH = @"D:\_GuitarMonics\svn-guitarmonics\trunk\Fontes\SongEditor\GuitarProImport.Tests\gpFiles";

        Song song;

        [SetUp]
        public void SetUp()
        {
            song = new Song();
        }

        [Test]
        [TestCase(Song.STR_V4_06, "")]
        [TestCase(Song.STR_V1, "This file was reconized as a file for GuitarPRO v1 and couldn't be open.")]
        [TestCase(Song.STR_V1_01, "This file was reconized as a file for GuitarPRO v1.01 and couldn't be open.")]
        [TestCase(Song.STR_V1_02, "This file was reconized as a file for GuitarPRO v1.02 and couldn't be open.")]
        [TestCase(Song.STR_V1_03, "This file was reconized as a file for GuitarPRO v1.03 and couldn't be open.")]
        [TestCase(Song.STR_V1_04, "This file was reconized as a file for GuitarPRO v1.04 and couldn't be open.")]
        [TestCase(Song.STR_V2_20, "This file was reconized as a file for GuitarPRO v2.20 and couldn't be open.")]
        [TestCase(Song.STR_V2_21, "This file was reconized as a file for GuitarPRO v2.21 and couldn't be open.")]
        [TestCase(Song.STR_V3_00, "This file was reconized as a file for GuitarPRO v3.00 and couldn't be open.")]
        [TestCase(Song.STR_V4_00, "This file was reconized as a file for GuitarPRO v4.00 and couldn't be open.")]
        [TestCase(Song.STR_L4_06, "This file was reconized as a file for GuitarPRO L4.06 and couldn't be open.")]
        [TestCase("some other thing", "This file was not reconized as GuitarPRO.")]
        public void MessagesReturnedWhenCheckingFileVersion(string fileVersionRead, string expectedMessage)
        {
            Assert.AreEqual(expectedMessage, new Importer().CheckVersion(fileVersionRead));
        }




        [TestCase("Led Zeppelin - Stairway To Heaven.gp", "This file was reconized as a file for GuitarPRO v3.00 and couldn't be open.")]
        [TestCase("Metallica - ...And Justice For All.gp4", "This file was reconized as a file for GuitarPRO v4.00 and couldn't be open.")]
        [TestCase("luiz-gonzaga--asa-branca--guitarpro.gp4", "")]
        public void RealFilesTests(string sFileTried, string expectedMessage)
        {
            Assert.AreEqual(expectedMessage, new Importer().Load(BASE_PATH + @"\" + sFileTried, song));
        }

        [TestCase(0, "00000000")]
        [TestCase(1, "00000001")]
        [TestCase(2, "00000010")]
        [TestCase(4, "00000100")]
        [TestCase(8, "00001000")]
        [TestCase(127, "01111111")]
        [TestCase(128, "10000000")]
        [TestCase(254, "11111110")]
        [TestCase(255, "11111111")]
        public void FlagMapTests(byte b, string flag)
        {
            Assert.AreEqual(flag, GP4Reader.FlagMap(b));
        }

    }
}