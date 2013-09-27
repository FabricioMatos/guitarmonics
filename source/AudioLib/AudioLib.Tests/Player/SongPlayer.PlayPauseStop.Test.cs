using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading;
using System.Diagnostics;
using Un4seen.Bass;
using Guitarmonics.AudioLib.Tests;

namespace Guitarmonics.AudioLib.Player.Tests
{
    public class SongPlayerTestBase
    {
        protected static string BaseBlues1_Clean = TestConfig.AudioPath + "BaseBlues1-Clean.mp3";
        protected static string MP3MattRedman = TestConfig.AudioPath + "Matt Redman.Facedown.Track 05.MP3";
        protected static string SmallSample = TestConfig.AudioPath + "SmallSample.wav";
        protected static string Matt_4Beats = TestConfig.AudioPath + "Matt-4Beats.wav";
        protected static string PingHi = TestConfig.AudioPath + "Ping Hi.wav";
    }

    [TestFixture]
    public class SongPlayer_PlayPauseStop_Test : SongPlayerTestBase
    {
        [TearDown]
        public void TearDown()
        {
            Bass.FreeMe();
        }

        [Test]
        public void Instantiation()
        {
            var songPlayer = new SongPlayer(MP3MattRedman, GtTimeSignature.Time4x4);

            Assert.AreEqual(SongPlayerStatus.Stopped, songPlayer.Status);
            Assert.AreEqual(0, songPlayer.CurrentPosition);
        }

        [Test]
        public void AlternativeInstantiation()
        {
            var songPlayer = new SongPlayer();

            Assert.AreEqual(SongPlayerStatus.NotInitialized, songPlayer.Status);

            songPlayer.SetupSong(MP3MattRedman, GtTimeSignature.Time4x4);

            Assert.AreEqual(SongPlayerStatus.Stopped, songPlayer.Status);
            Assert.AreEqual(0, songPlayer.CurrentPosition);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(FileNotFound))]
        public void FileNotFoundError()
        {
            var songPlayer = new SongPlayer("c:\file that not exists.mp3", GtTimeSignature.Time4x4);
        }

        #region Play and Stop

        [Test]
        public void Play()
        {
            var songPlayer = new SongPlayer(MP3MattRedman, GtTimeSignature.Time4x4);
            try
            {
                songPlayer.Play();
                Assert.AreEqual(SongPlayerStatus.Playing, songPlayer.Status);
            }
            finally
            {
                songPlayer.Stop();
                songPlayer.Dispose();
            }
        }

        [Test]
        public void PlayReallyWorks()
        {
            var songPlayer = new SongPlayer(MP3MattRedman, GtTimeSignature.Time4x4);
            try
            {
                songPlayer.Play();
                Assert.AreEqual(SongPlayerStatus.Playing, songPlayer.Status);

                var position = songPlayer.CurrentPosition;

                int i = 0;
                while (i++ < 5)
                {
                    Thread.Sleep(100);

                    //Trace.TraceWarning(string.Format("PlayReallyWorks: CurrentPositionAsSeconds = {0}.", songPlayer.CurrentPositionAsSeconds));
                    Assert.Greater(songPlayer.CurrentPosition, position);

                    position = songPlayer.CurrentPosition;
                }
            }
            finally
            {
                songPlayer.Dispose();
            }
        }


        [Test]
        public void PlayAndStop()
        {
            var songPlayer = new SongPlayer(MP3MattRedman, GtTimeSignature.Time4x4);
            try
            {
                songPlayer.Play();
                Assert.AreEqual(SongPlayerStatus.Playing, songPlayer.Status);

                Thread.Sleep(100);

                songPlayer.Stop();
                Assert.AreEqual(SongPlayerStatus.Stopped, songPlayer.Status);
                Assert.AreEqual(0, songPlayer.CurrentPosition);
            }
            finally
            {
                songPlayer.Dispose();
            }
        }

        [Test]
        public void PlayAndStopAndPlay()
        {
            var songPlayer = new SongPlayer(MP3MattRedman, GtTimeSignature.Time4x4);
            try
            {
                songPlayer.Play();

                Thread.Sleep(100);
                var currentPosition = songPlayer.CurrentPosition;

                songPlayer.Stop();

                //start again from the beggining
                songPlayer.Play();

                Assert.AreEqual(SongPlayerStatus.Playing, songPlayer.Status);
                Assert.Less(songPlayer.CurrentPosition, currentPosition);
            }
            finally
            {
                songPlayer.Dispose();
            }
        }

        [Test]
        public void PlayAndPauseAndPlay()
        {
            var songPlayer = new SongPlayer(MP3MattRedman, GtTimeSignature.Time4x4);
            try
            {
                songPlayer.Play();
                Thread.Sleep(100);

                songPlayer.Pause();
                var currentPosition = songPlayer.CurrentPosition;

                Thread.Sleep(100);

                Assert.AreEqual(SongPlayerStatus.Paused, songPlayer.Status);

                //Is really paused
                Assert.AreEqual(currentPosition, songPlayer.CurrentPosition);

                //start again from the paused point
                songPlayer.Play();
                Thread.Sleep(100);

                Assert.AreEqual(SongPlayerStatus.Playing, songPlayer.Status);
                Assert.Greater(songPlayer.CurrentPosition, currentPosition);
            }
            finally
            {
                songPlayer.Dispose();
            }
        }

        #endregion

        [Test]
        public void MusicDuration()
        {
            var songPlayer = new SongPlayer(MP3MattRedman, GtTimeSignature.Time4x4);
            try
            {
                songPlayer.LoadStream();
                var duration = songPlayer.DurationAsSeconds;

                //Test the song duration with 1 second of tolerance (duration is not integer)
                Assert.LessOrEqual(0, Math.Abs(duration - 309)); //309 seg = 5min 9seg (this song according MediaPlayer)
                Assert.Greater(1, Math.Abs(duration - 309));
            }
            finally
            {
                songPlayer.Dispose();
            }
        
        
        
        
        
        
        
        
        
        }

        #region Test TempoBPM constructor parameter


        //[Test]
        //public void ValidTempo_InferiorBound()
        //{
        //    var songPlayer = new SongPlayer(SmallSample, GtTimeSignature.Time4x4);
        //    songPlayer.Dispose();
        //}

        //[Test]
        //public void ValidTempo_SuperiorBound()
        //{
        //    var songPlayer = new SongPlayer(SmallSample, GtTimeSignature.Time4x4);
        //    songPlayer.Dispose();
        //}

        //[Test]
        //[ExpectedException(ExpectedException = typeof(InvalidParameter))]
        //public void InvalidTempo_UnderMin()
        //{
        //    var songPlayer = new SongPlayer(SmallSample, GtTimeSignature.Time4x4);
        //    songPlayer.Dispose();
        //}

        //[Test]
        //[ExpectedException(ExpectedException = typeof(InvalidParameter))]
        //public void InvalidTempo_AboveMax()
        //{
        //    var songPlayer = new SongPlayer(SmallSample, GtTimeSignature.Time4x4);
        //    songPlayer.Dispose();
        //}

        #endregion

        #region Playing Empty File Name

        [Test]
        public void PlayEmptyFileName()
        {
            using (ISongPlayer songPlayer = new SongPlayer(string.Empty, GtTimeSignature.Time4x4))
            {

                songPlayer.Play();
                Thread.Sleep(100);

                Assert.AreEqual(-1, songPlayer.CurrentPosition);

                songPlayer.Pause();
                Thread.Sleep(100);

                Assert.AreEqual(SongPlayerStatus.Paused, songPlayer.Status);

                //start again from the paused point
                songPlayer.Play();
                Thread.Sleep(100);

                Assert.AreEqual(SongPlayerStatus.Playing, songPlayer.Status);
                Assert.AreEqual(-1, songPlayer.CurrentPosition);
            }
        }

        #endregion

    }
}
