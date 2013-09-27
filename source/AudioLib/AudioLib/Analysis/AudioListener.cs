using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Guitarmonics.AudioLib.Common;
using Un4seen.Bass;

namespace Guitarmonics.AudioLib.Analysis
{
    public class ErrorInvalidSampleFrequence : Exception
    {
        public ErrorInvalidSampleFrequence(string pMessage) :
            base(pMessage)
        {

        }
    }

    public interface IAudioListener
    {
        int SampleFrequence { get; }
        Thread WorkerThread { get; }
        bool Stopped { get; }
        bool AudioDeviceInitialized { get; }
        int RecordChannel { get; }
        float[] FftData { get; }
        void Start();
        void Stop();
        void Dispose();
    }

    public class AudioListener : IDisposable, IAudioListener
    {
        public AudioListener(int pSampleFrequence)
        {
            //less then 4Hz or greater then 100Hz is not valid!
            if ((pSampleFrequence < 4) || (pSampleFrequence > 100))
            {
                throw new ErrorInvalidSampleFrequence(
                    string.Format("The sample frequence {0} is not valid for AudioListener.",
                    pSampleFrequence));
            }

            this.fFft = new float[4096];

            this.Stopped = true;
            this.AudioDeviceInitialized = false;
            this.SampleFrequence = pSampleFrequence;
            this.WorkerThread = new Thread(WorkerThreadMethod);

        }

        private object FftLock = new object();

        public int SampleFrequence { get; private set; }
        public Thread WorkerThread { get; private set; }
        public bool Stopped { get; private set; }
        public bool AudioDeviceInitialized { get; private set; }
        public int RecordChannel { get; private set; }
        private float[] fFft;
        public float[] FftData
        {
            get
            {
                var fftClone = new float[this.fFft.Length];

                //lock fFft and copy it to the clone
                lock (this.FftLock)
                {
                    for (int i = 0; i < this.fFft.Length; i++)
                    {
                        fftClone[i] = this.fFft[i];
                    }
                }

                return fftClone;
            }
        }


        private void WorkerThreadMethod()
        {
            bool isStopped;
            var startTime = DateTime.Now;

            //lock (this)
            {
                isStopped = this.Stopped;

                if (!isStopped)
                    this.StartListening(); //esta travando aqui.
            }

            while (!isStopped)
            {
                DateTime endTime = DateTime.Now;
                TimeSpan timeDifference = endTime - startTime;

                if (timeDifference.Milliseconds >= (1000 / this.SampleFrequence))
                {
                    startTime = endTime;

                    lock (this.FftLock)
                    {
                        ProcessAudioInput();
                    }
                }
                else
                {
                    Thread.Sleep(1);
                }

                //lock (this)
                {
                    isStopped = this.Stopped;
                }
            }

            this.StopListening();
        }

        protected virtual void ProcessAudioInput()
        {
            BassWrapper.Instance.ChannelGetData(this.RecordChannel, fFft, (int)BASSData.BASS_DATA_FFT8192);

            //TODO: do something with fft

            //See SpectrumAnalyzer_OLD.GetNormalizedFFT()...
        }

        protected virtual void StartListening()
        {
            //lock (this)
            {
                this.RecordChannel = BassWrapper.Instance.RecordStart();
            }
        }

        protected virtual void StopListening()
        {
            //lock (this)
            {
                BassWrapper.Instance.RecordFree();
            }
        }

        private void InitializeAudioDevice()
        {
            if (this.AudioDeviceInitialized)
                return;

            if (!BassWrapper.Instance.BassInit())
            {
                throw new Exception("AudioListener.InitializeAudioDevice() faild.");
            }
            this.AudioDeviceInitialized = true;
        }

        private void FinalizeAudioDevice()
        {
            if (!this.AudioDeviceInitialized)
                return;

            this.AudioDeviceInitialized = false;
            BassWrapper.Instance.BassFree();
        }


        public virtual void Start()
        {
            //lock (this)
            {
                if (!this.Stopped)
                    return;

            }

            this.Stopped = false;

            this.InitializeAudioDevice();

            this.WorkerThread.Start();
        }

        public virtual void Stop()
        {
            //lock (this)
            {
                this.Stopped = true;
            }

            int count = 0;

            //whait for the thread end
            while (this.WorkerThread.IsAlive)
            {
                Thread.Sleep(10);
                count++;

                if (count > 100) //1 sec (100 x 10ms)
                {
                    this.WorkerThread.Abort();

                    throw new Exception("The AudioListener.Stop() couldn't end his main thread.");
                }
            }

            this.FinalizeAudioDevice();
        }

        #region IDisposable Members

        public void Dispose()
        {
            this.Stop();
        }

        #endregion
    }
}
