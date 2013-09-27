using System;
using System.Collections.Generic;
using System.Text;
using NUnit.Framework;
using System.Threading;
using System.Diagnostics;
using Un4seen.Bass;
using Un4seen.Bass.AddOn.Fx;

namespace Guitarmonics.AudioLib.Player.Tests
{
    [TestFixture]
    public class SongPlayer_BeatAndTickEvents_Test : SongPlayerTestBase
    {
        [TearDown]
        public void TearDown()
        {
            Bass.FreeMe();
            BassFx.FreeMe();
        }


        #region Events using Callback Sync

        [Test]
        public void StatusIsStopoedWhenFinished()
        {
            var songPlayer = new SongPlayer(SmallSample, GtTimeSignature.Time4x4);
            try
            {
                songPlayer.Play();

                Assert.AreEqual(SongPlayerStatus.Playing, songPlayer.Status);

                //wait until the status change to Stopped
                int i = 0;
                while (songPlayer.Status != SongPlayerStatus.Stopped)
                {
                    Thread.Sleep(200);

                    i++;
                    if (i > 50) //after 10seg the test fail
                        Assert.Fail("The song didn't stoped at the end.");
                }

                Assert.AreEqual(SongPlayerStatus.Stopped, songPlayer.Status);
            }
            finally
            {
                songPlayer.Dispose();
            }

        }

        internal class BeatTick
        {
            public long Beat { get; set; }
            public long Tick { get; set; }
        }

        #region Handler for TickNotifyEvent

        private int fNumberOfBeats;
        private List<BeatTick> fTickNotificationList = new List<BeatTick>();

        private void HandleTickNotifyEventForTest(SongPlayer pSongPlayer, long pBeat, long pTick)
        {
            fTickNotificationList.Add(new BeatTick()
            {
                Beat = pBeat,
                Tick = pTick
            });

            if (pTick == 0)
            {
                int sampleHandle = Bass.BASS_SampleLoad(PingHi, 0, 0, 10, BASSFlag.BASS_SAMPLE_OVER_POS);
                int sampleChannel = Bass.BASS_SampleGetChannel(sampleHandle, false);
                Bass.BASS_ChannelPlay(sampleChannel, false);

                fNumberOfBeats++;
            }
        }

        #endregion

        //[Test]
        //public void LogOfTicksAndBeats()
        //{
        //    var songPlayer = new SongPlayer(Matt_4Beats, GtTimeSignature.Time4x4);
        //    try
        //    {
        //        this.fTickNotificationList.Clear();

        //        //This event will populate this.fTickNotificationList with all tick notification.
        //        songPlayer.TickNotifyEvent += new TickNotifyEvent(HandleTickNotifyEventForTest);

        //        songPlayer.Play();

        //        //wait for the song's end
        //        while (songPlayer.Status != SongPlayerStatus.Stopped)
        //        {
        //            Thread.Sleep(1);
        //        }

        //        //Prepare the expected list of tick notifications
        //        var beatsTicksExpected = PrepareExpectedTicksList();

        //        //trace...
        //        //for (int i = 0; i < beatsTicksExpected.Count; i++)
        //        //{
        //        //    string description = string.Format("Excpected {0}:{1} and found {2}:{3}",
        //        //        beatsTicksExpected[i].Beat, beatsTicksExpected[i].Tick,
        //        //        fTickNotificationList[i].Beat, fTickNotificationList[i].Tick);

        //        //    Trace.TraceInformation(description);
        //        //}

        //        Assert.AreEqual(beatsTicksExpected.Count, fTickNotificationList.Count);

        //        for (int i = 0; i < beatsTicksExpected.Count; i++)
        //        {
        //            string description = string.Format("Excpected {0}:{1} but was found {2}:{3}",
        //                beatsTicksExpected[i].Beat, beatsTicksExpected[i].Tick,
        //                fTickNotificationList[i].Beat, fTickNotificationList[i].Tick);

        //            Assert.AreEqual(beatsTicksExpected[i].Beat, fTickNotificationList[i].Beat, description);
        //            Assert.AreEqual(beatsTicksExpected[i].Tick, fTickNotificationList[i].Tick, description);
        //        }
        //    }
        //    finally
        //    {
        //        songPlayer.Dispose();
        //    }
        //}

        //private List<BeatTick> PrepareExpectedTicksList()
        //{
        //    var beatsTicksExpected = new List<BeatTick>();

        //    for (int beat = 1; beat <= 4; beat++)
        //    {
        //        for (int tick = 0; tick <= 470; tick += 40)
        //        {
        //            beatsTicksExpected.Add(new BeatTick()
        //            {
        //                Beat = beat,
        //                Tick = tick
        //            });
        //        }
        //    }

        //    //Remove the 1:0 (the event isn't fired for the position 0. So the first one is the 1:10)
        //    //beatsTicksExpected.RemoveAt(0);

        //    //Add the 5:0 (the last tick notification)
        //    beatsTicksExpected.Add(new BeatTick()
        //    {
        //        Beat = 5,
        //        Tick = 0
        //    });

        //    return beatsTicksExpected;
        //}

        //This test is too slow (full music).
        //[Test]
        public void NumberOfBeatsPlayed_CompleteSong()
        {
            var songPlayer = new SongPlayer(MP3MattRedman, GtTimeSignature.Time4x4);

            try
            {
                this.fNumberOfBeats = 0;

                //This event will increase the numberOfBeats for each new beat, and play the click sample
                songPlayer.TickNotifyEvent += new TickNotifyEvent(HandleTickNotifyEventForTest);

                songPlayer.Play();

                //wait for the song's end
                while (songPlayer.Status != SongPlayerStatus.Stopped)
                {
                    Thread.Sleep(1);
                }

                Assert.AreEqual(721, this.fNumberOfBeats);
            }
            finally
            {
                songPlayer.Dispose();
            }

        }

        #endregion

        //TODO: Garantir que OnProgressUpdated não é disparado quando a musica está em pause.

    }
}
