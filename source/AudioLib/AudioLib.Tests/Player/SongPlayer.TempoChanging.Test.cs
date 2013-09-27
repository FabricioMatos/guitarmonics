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
    public class SongPlayer_TempoChanging_Test : SongPlayerTestBase
	{
        [TearDown]
        public void TearDown()
        {
            Bass.FreeMe();
            BassFx.FreeMe();
        }

        [TestCase(100)]
        [TestCase(75)]
        [TestCase(50)]
        [TestCase(150)]
        public void PlayReallyWorks(int velocity)
        {
            using (var songPlayer = new SongPlayer(MP3MattRedman, GtTimeSignature.Time4x4))
            {
                songPlayer.Play(velocity);
                Assert.AreEqual(SongPlayerStatus.Playing, songPlayer.Status);


                var waitTimeInSeconds = 2; //wait 2 seconds

                Thread.Sleep(waitTimeInSeconds * 1000);
                var position = songPlayer.CurrentPositionAsSeconds;
                Console.WriteLine(position);


                //check if the elapsed time is proportional to the velocity.
                Assert.Less((float)waitTimeInSeconds/(100.0f/velocity) - position, 0.2f); //0.2 seconds of tolerance
            }
        }
    }
}
