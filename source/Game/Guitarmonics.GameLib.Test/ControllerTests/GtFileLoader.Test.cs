using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib.Tests;
using Guitarmonics.WebServiceClient;
using NUnit.Framework;
using Guitarmonics.GameLib.Controller;
using Guitarmonics.GameLib.Model;
using Guitarmonics.AudioLib.MusicConfigFiles;
using Guitarmonics.AudioLib.Common;
using Rhino.Mocks;

namespace Guitarmonics.GameLib.Controller.Test.ControllerTests
{
    [TestFixture]
    public class GtFileLoaderTest
    {
        [TestCase(10)]
        [TestCase(480)]
        [TestCase(490)]
        [TestCase(960)]
        public void CalculateNumberOfBeats_1Note(int pDuration)
        {
            IList<GuitarScoreNote> scoreNotes = new List<GuitarScoreNote>();

            scoreNotes.Add(new GuitarScoreNote("G3", 1, 0, pDuration, 0));

            var fileLoader = new GtFileLoader();

            int beats = (int)pDuration / 480;
            if ((pDuration % 480) > 0)
                beats++;

            Assert.AreEqual(beats, fileLoader.CalculateNumberOfBeats(scoreNotes));
        }

        [Test]
        public void CalculateNumberOfBeats_0Notes()
        {
            IList<GuitarScoreNote> scoreNotes = new List<GuitarScoreNote>();

            var fileLoader = new GtFileLoader();

            Assert.AreEqual(1, fileLoader.CalculateNumberOfBeats(scoreNotes));
        }

        [Test]
        public void CalculateNumberOfBeats_3Notes()
        {
            IList<GuitarScoreNote> scoreNotes = new List<GuitarScoreNote>();

            scoreNotes.Add(new GuitarScoreNote("G3", 1, 0, 120, null));
            scoreNotes.Add(new GuitarScoreNote("G3", 2, 0, 4 * 480, null));
            scoreNotes.Add(new GuitarScoreNote("G3", 3, 0, 120, null));

            var fileLoader = new GtFileLoader();

            Assert.AreEqual(5, fileLoader.CalculateNumberOfBeats(scoreNotes));
        }

        [Test]
        public void FillTickDataTable_1Note()
        {
            var note = new GuitarScoreNote("G3", 1, 0, 480, null);
            var tickDataTable = new GtTickDataTable(1);
            var fileLoader = new GtFileLoader();

            fileLoader.FillTickDataTable(tickDataTable, note);

            for (var pos = new BeatTick(1, 0); pos < new BeatTick(2, 0); pos = pos.AddTicks(10))
            {
                var tickData = tickDataTable[pos.Beat, pos.Tick];

                Assert.AreEqual(pos == new BeatTick(1, 0), tickData.IsStartTick);
                Assert.AreEqual(pos == new BeatTick(1, 470), tickData.IsEndTick);

                Assert.IsNull(tickData.String1);
                Assert.IsNull(tickData.String2);
                Assert.IsNull(tickData.String3);
                Assert.IsNull(tickData.String4);
                Assert.IsNull(tickData.String5);
                Assert.IsNull(tickData.MomentInMiliseconds);
                Assert.AreEqual(3, tickData.String6);

            }
        }

        [Test]
        public void FillTickDataTable_1NoteWithMomentInMiliseconds()
        {
            var note = new GuitarScoreNote("G3", 1, 0, 480, 1000);
            var tickDataTable = new GtTickDataTable(1);
            var fileLoader = new GtFileLoader();

            fileLoader.FillTickDataTable(tickDataTable, note);

            for (var pos = new BeatTick(1, 0); pos < new BeatTick(2, 0); pos = pos.AddTicks(10))
            {
                var tickData = tickDataTable[pos.Beat, pos.Tick];

                Assert.AreEqual(pos == new BeatTick(1, 0), tickData.IsStartTick);
                Assert.AreEqual(pos == new BeatTick(1, 470), tickData.IsEndTick);

                Assert.IsNull(tickData.String1);
                Assert.IsNull(tickData.String2);
                Assert.IsNull(tickData.String3);
                Assert.IsNull(tickData.String4);
                Assert.IsNull(tickData.String5);
                Assert.AreEqual(3, tickData.String6);

                if (pos == new BeatTick(1, 0))
                    Assert.AreEqual(1000, tickData.MomentInMiliseconds);
                else
                    Assert.IsNull(tickData.MomentInMiliseconds);

            }
        }

        [Test]
        public void ConvertScoreMomentsInTickTable()
        {
            IList<GuitarScoreNote> scoreNotes = new List<GuitarScoreNote>();

            scoreNotes.Add(new GuitarScoreNote("G3", 1, 0, 480, null) { RemarkOrChordName = "G" });

            var fileLoader = new GtFileLoader();
            var tickDataTable = fileLoader.ConvertScoreNotesInTickTable(scoreNotes);

            Assert.AreEqual(1 + GtFileLoader.NUMBER_ADITIONAL_BEATS, tickDataTable.NumberOfBeats);
            Assert.AreEqual((1 + GtFileLoader.NUMBER_ADITIONAL_BEATS) * 48, tickDataTable.fItems.Length);

            for (int i = 0; i < 48; i++)
            {
                GtTickData tickData = tickDataTable[1, i * 10];

                Assert.IsNull(tickData.String1);
                Assert.IsNull(tickData.String2);
                Assert.IsNull(tickData.String3);
                Assert.IsNull(tickData.String4);
                Assert.IsNull(tickData.String5);
                Assert.AreEqual(3, tickData.String6);

                if (i == 0)
                    Assert.AreEqual("G", tickData.RemarkOrChordName);
                else
                    Assert.AreEqual("", tickData.RemarkOrChordName);
            }
        }

        [Test]
        public void ReadNotesFromXmlFile()
        {
            var fileLoader = new GtFileLoader();

            var songDescription = new SongDescription()
            {
                Song = "Song Name",
                Artist = "Artist Name",
                ConfigFileName = TestConfig.AudioPath + "metallica-for_whom_the_bell_tolls(linked).song.xml",
                SyncFileName = TestConfig.AudioPath + "metallica-for_whom_the_bell_tolls(linked).song.xml",
                AudioFileName = TestConfig.AudioPath + "?.mp3",
                TimeSignature = Guitarmonics.AudioLib.Player.GtTimeSignature.Time4x4,
            };

            //IList<GuitarScoreNote> scores = fileLoader.ReadXmlScores(songDescription);
            var tickDataTable = fileLoader.LoadTickDataTable(ref songDescription);

            Assert.IsNotNull(tickDataTable);
        }

        [Test]
        public void DownloadSongAllFilesExist()
        {
            var fileLoader = new GtFileLoader();

            var songDescription = new SongDescription()
            {
                Song = "Song Name",
                Artist = "Artist Name",
                ConfigFileName = TestConfig.AudioPath + "metallica-for_whom_the_bell_tolls(linked).song.xml",
                SyncFileName = TestConfig.AudioPath + "metallica-for_whom_the_bell_tolls(linked).song.xml",
                AudioFileName = TestConfig.AudioPath + "?.mp3",
                TimeSignature = Guitarmonics.AudioLib.Player.GtTimeSignature.Time4x4,
            };


            fileLoader.DownloadSong(songDescription);
        }

        
        [Test]
        public void DownloadSongConfigFileDontExist()
        {
            var mockRepository = new MockRepository();

            var fileLoader = mockRepository.PartialMock<GtFileLoader>();

            var songDescription = new SongDescription()
            {
                Song = "Song Name",
                Artist = "Artist Name",
                ConfigFileName = TestConfig.AudioPath + "INVALID",
                SyncFileName = TestConfig.AudioPath + "metallica-for_whom_the_bell_tolls(linked).song.xml",
                AudioFileName = TestConfig.AudioPath + "?.mp3",
                TimeSignature = Guitarmonics.AudioLib.Player.GtTimeSignature.Time4x4,
            };

            fileLoader.Expect(x => x.DownloadContent(songDescription));

            mockRepository.ReplayAll();

            fileLoader.DownloadSong(songDescription);

            mockRepository.VerifyAll();
        }

        [Test]
        public void DownloadSongSyncFileDontExist()
        {
            var mockRepository = new MockRepository();

            var fileLoader = mockRepository.PartialMock<GtFileLoader>();

            var songDescription = new SongDescription()
            {
                Song = "Song Name",
                Artist = "Artist Name",
                ConfigFileName = TestConfig.AudioPath + "metallica-for_whom_the_bell_tolls(linked).song.xml",
                SyncFileName = TestConfig.AudioPath + "INVALID",
                AudioFileName = TestConfig.AudioPath + "?.mp3",
                TimeSignature = Guitarmonics.AudioLib.Player.GtTimeSignature.Time4x4,
            };

            fileLoader.Expect(x => x.DownloadSynchronization(songDescription));

            mockRepository.ReplayAll();

            fileLoader.DownloadSong(songDescription);

            mockRepository.VerifyAll();
        }
        
        [Ignore]
        [Test]
        public void TestDownloadContent()
        {
            var songDescription = new SongDescription()
            {
                Song = "Seek & Destroy",
                Artist = "Metallica",
                OidHardTablature = Guid.Empty,
                ConfigFileName = TestConfig.DataFolderPath + "Metallica.KillEmAll.Seek&Destroy\\hard.xml",
                SyncFileName = TestConfig.DataFolderPath + "Metallica.KillEmAll.Seek&Destroy\\Sync.xml",
                AudioFileName = TestConfig.DataFolderPath + "Metallica.KillEmAll.Seek&Destroy\\Metallica.KillEmAll.Seek&Destroy.mp3",
                TimeSignature = Guitarmonics.AudioLib.Player.GtTimeSignature.Time4x4,
            };

            File.Delete(songDescription.ConfigFileName);

            var fileLoader = new GtFileLoader();

            fileLoader.DownloadContent(songDescription);

            Assert.IsTrue(File.Exists(songDescription.ConfigFileName));
        }

        [Ignore]
        [Test]
        public void TestDownloadSynchronization()
        {
            var songDescription = new SongDescription()
            {
                Song = "Seek & Destroy",
                Artist = "Metallica",
                OidHardTablature = Guid.Empty,
                ConfigFileName = TestConfig.DataFolderPath + "Metallica.KillEmAll.Seek&Destroy\\hard.xml",
                SyncFileName = TestConfig.DataFolderPath + "Metallica.KillEmAll.Seek&Destroy\\Sync.xml",
                AudioFileName = TestConfig.DataFolderPath + "Metallica.KillEmAll.Seek&Destroy\\Metallica.KillEmAll.Seek&Destroy.mp3",
                TimeSignature = Guitarmonics.AudioLib.Player.GtTimeSignature.Time4x4,
            };

            File.Delete(songDescription.SyncFileName);

            var fileLoader = new GtFileLoader();

            fileLoader.DownloadSynchronization(songDescription);

            Assert.IsTrue(File.Exists(songDescription.SyncFileName));
        }

        [Ignore]
        [Test]
        public void TestDownloadMp3()
        {
            var songDescription = new SongDescription()
            {
                Song = "Seek & Destroy",
                Artist = "Metallica",
                OidHardTablature = Guid.Empty,
                ConfigFileName = TestConfig.DataFolderPath + "Metallica.KillEmAll.Seek&Destroy\\hard.xml",
                SyncFileName = TestConfig.DataFolderPath + "Metallica.KillEmAll.Seek&Destroy\\Sync.xml",
                AudioFileName = TestConfig.DataFolderPath + "Metallica.KillEmAll.Seek&Destroy\\Metallica.KillEmAll.Seek&Destroy.mp3",
                TimeSignature = Guitarmonics.AudioLib.Player.GtTimeSignature.Time4x4,
            };

            File.Delete(songDescription.AudioFileName);

            var fileLoader = new GtFileLoader();

            fileLoader.DownloadMp3(songDescription);

            Assert.IsTrue(File.Exists(songDescription.AudioFileName));
        }

        //verificar versao e forcar download (com sobreescrita) se existir um versao mais atual

        //Implememntar fileLoader.DownloadContent()
        //   - tratar quando retornar mensagem de erro, como p.e. "TablatureNotFound"
        //   - Falta implementar o lado do webservice.

        //Implememntar fileLoader.DownloadSynchronization()

        //Se existir arquivo de audio no servidor, e nao existir local, baixar

    }
}
