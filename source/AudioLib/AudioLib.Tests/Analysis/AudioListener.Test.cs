using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using System.Threading;
using Un4seen.Bass;

namespace Guitarmonics.AudioLib.Analysis.Tests
{
    [TestFixture]
    public class AudioListenerTest
    {
        [TearDown]
        public void TearDown()
        {
            Bass.BASS_RecordFree();
            Bass.BASS_Free();
        }

        [Test]
        public void AudioListenerConstructor()
        {
            var audioListener = new AudioListener(40); //40Hz sample frequence

            Assert.IsNotNull(audioListener);
            Assert.AreEqual(40, audioListener.SampleFrequence);
            Assert.IsFalse(audioListener.AudioDeviceInitialized);

            audioListener.Dispose();
        }


        [TestCase(-1)]
        [TestCase(0)]
        [TestCase(3)]
        [TestCase(101)]
        [ExpectedException(typeof(ErrorInvalidSampleFrequence))]
        public void AudioListenerConstructorInvalidSamples(int pSampleFrequence)
        {
            //less then 4Hz or greater then 100Hz is not valid!
            new AudioListener(pSampleFrequence);
        }

        [TestCase(4)]
        [TestCase(50)]
        [TestCase(100)]
        public void AudioListenerConstructorValidSamples(int pSampleFrequence)
        {
            //less then 4Hz or greater then 100Hz is not valid!
            var audioListener = new AudioListener(pSampleFrequence);

            Assert.IsNotNull(audioListener);
        }

        [Test]
        public void AudioListenerStartAndStop()
        {
            var audioListener = new AudioListenerMockWithCounter(40); //40Hz sample frequence

            audioListener.Start();

            Assert.IsTrue(audioListener.AudioDeviceInitialized);
            Assert.IsFalse(audioListener.Stopped);

            Thread.Sleep(100);

            Assert.IsTrue(audioListener.WorkerThread.IsAlive);

            audioListener.Stop();

            Assert.IsFalse(audioListener.WorkerThread.IsAlive);
            Assert.IsTrue(audioListener.Stopped);
        }

        [Test]
        public void AudioListenerSampleFrequenceWorks()
        {
            var audioListener = new AudioListenerMockWithCounter(40); //40Hz sample frequence

            audioListener.Start();

            Thread.Sleep(1000); //sleep 1 second (waiting for 25 samples: 25x 40 = 1000)

            audioListener.Stop();

            Thread.Sleep(10);

            Assert.IsTrue((audioListener.NumberOfSamples >= 38) || (audioListener.NumberOfSamples <= 40),
                "Returned " + audioListener.NumberOfSamples.ToString());
        }


        [Test]
        public void AudioListenerFftReturnClones()
        {
            var audioListener = new AudioListener(40); //40Hz sample frequence

            float[] fft1 = audioListener.FftData;
            float[] fft2 = audioListener.FftData;

            Assert.AreNotSame(fft1, fft2);
        }

        [Ignore]
        [Test]
        public void AudioListenerFftIsNotZero()
        {
            var audioListener = new AudioListenerMockWithCounter(40); //40Hz sample frequence

            audioListener.Start();

            Thread.Sleep(100);

            float[] fft1 = audioListener.FftData;

            Thread.Sleep(100);

            float[] fft2 = audioListener.FftData;

            audioListener.Stop();

            bool allEqual = true;

            for (int i = 0; i < fft1.Length; i++)
            {
                if (fft1[i] != fft2[i])
                {
                    allEqual = false;
                    break;
                }
            }

            Assert.IsFalse(allEqual);
        }

        [TestCase(80)] //sample frequence in Hz
        [TestCase(40)] //sample frequence in Hz
        [TestCase(30)] //sample frequence in Hz
        public void TestTheSampleFrequence(int pSampleFrequence)
        {
            float period = 1000.0f / (float)pSampleFrequence;
            int twentyPeriods = (int)(100.0f * period);

            var audioListenerMock = new AudioListenerMockWithCounter(pSampleFrequence);

            audioListenerMock.Start();

            Thread.Sleep(twentyPeriods);

            audioListenerMock.Stop();

            //We gave +/- 5% of tolerance (should be equal 100)
            Assert.IsTrue(95 <= audioListenerMock.NumberOfSamples, audioListenerMock.NumberOfSamples.ToString());
            Assert.IsTrue(audioListenerMock.NumberOfSamples <= 105, audioListenerMock.NumberOfSamples.ToString());
        }


        #region Mocks

        public class AudioListenerMockWithCounter : AudioListener
        {
            public AudioListenerMockWithCounter(int pSampleFrequence)
                : base(pSampleFrequence)
            {
                this.NumberOfSamples = 0;
            }

            public int NumberOfSamples { get; private set; }

            protected override void ProcessAudioInput()
            {
                base.ProcessAudioInput();
                this.NumberOfSamples++;
            }
        }

        #endregion

    }
}
