using System;
using System.Collections.Generic;
using System.Text;
using System.IO;
using Guitarmonics.AudioLib.Common;
using Un4seen.Bass;
using System.Threading;
using Guitarmonics.AudioLib.Common;
using System.Diagnostics;
using Un4seen.Bass.AddOn.Fx;

namespace Guitarmonics.AudioLib.Player
{

    public delegate void TickNotifyEvent(SongPlayer pSongPlayer, long pBeat, long pTick);

    public class SongPlayer : IDisposable, ISongPlayer
    {
        private const int MIN_TEMPO_BPM = 40;
        private const int MAX_TEMPO_BPM = 180;
        private float Velocity;
        private float Pitch;

        public SongPlayer(string pFileName, GtTimeSignature pTimeSignature)
        {
            this.SetupSong(pFileName, pTimeSignature);
        }

        public SongPlayer()
        {
        }

        public void SetupSong(string pFileName, GtTimeSignature pTimeSignature)
        {
            if ((pFileName != string.Empty) && (!File.Exists(pFileName)))
            {
                throw new FileNotFound(string.Format("SongPlayer could't find the file [{0}].", pFileName));
            }

            //Constructor parameters
            fFileName = pFileName;
            fTimeSignature = pTimeSignature;

            //Default values
            fStatus = SongPlayerStatus.Stopped;
            fLastBeatNotifyPosition = 0;

            //Callback handlers
            fOnFinishedHandle = new SYNCPROC(OnFinished);
            fOnBeatNotifyHandle = new SYNCPROC(OnBeatNotify);
            fOnTickNotifyHandle = new SYNCPROC(OnTickNotify);
            fOnBeatNotifyId = 0;
        }

        #region Fields

        /// <summary>
        /// Int returned by BASS_StreamCreateFile in order to identify the stream created to play some file.
        /// </summary>
        //private int fStreamHandler;

        /// <summary>
        /// Int returned by BASS_FX_TempoCreate in order to create an FX channel to process tempo changes.
        /// </summary>
        private int fStreamFxHandler;

        /// <summary>
        /// Audio file name
        /// </summary>
        private string fFileName;

        /// <summary>
        /// field of Status property.
        /// </summary>
        private SongPlayerStatus fStatus;

        /// <summary>
        /// Indicate if the audio device (Bass lib) is initialized.
        /// </summary>
        private bool fAudioDeviceInitialized
        {
            get
            {
                return BassWrapper.Instance.Initiallized;
            }
        }

        /// <summary>
        /// SYNCPROC used to point to the callback method used to notify the end of the music.
        /// </summary>
        private SYNCPROC fOnFinishedHandle;

        /// <summary>
        /// SYNCPROC used to point to the callback method installed for eacho beat.
        /// </summary>
        private SYNCPROC fOnBeatNotifyHandle;

        /// <summary>
        /// SYNCPROC used to point to the callback method installed for eacho 40 ticks.
        /// </summary>
        private SYNCPROC fOnTickNotifyHandle;

        /// <summary>
        /// Id of the installed beat notify callback used to uninstall it.
        /// </summary>
        private int fOnBeatNotifyId;

        /// <summary>
        /// Tempo in Beats per minutes (BPM) passed by the constructor.
        /// </summary>
        //public double TempoBPM { get;  private set; }

        /// <summary>
        /// Last position programmed to the OnBeatNotfiy be fired.
        /// </summary>
        private long fLastBeatNotifyPosition;

        /// <summary>
        /// Last beat notified
        /// </summary>
        private long fLastBeatNotified;

        /// <summary>
        /// Last tick notified
        /// </summary>
        private long fLastTickNotified;

        /// <summary>
        /// The song's time signature (4x4, for example).
        /// </summary>
        private GtTimeSignature fTimeSignature;

        /// <summary>
        /// Event used to notify the application about the OnTick40Notify.
        /// </summary>
        public TickNotifyEvent TickNotifyEvent { get; set; }

        private bool PlayingNothing
        {
            get { return fFileName == string.Empty; }
        }

        #endregion

        #region Properties

        /// <summary>
        /// Int returned by BASS_StreamCreateFile in order to identify the stream created to play some file.
        /// </summary>
        public int StreamHandler
        {
            get
            {
                lock (this)
                {
                    //return fStreamHandler;
                    return fStreamFxHandler;
                }
            }
        }


        /// <summary>
        /// Status of the player: Stopped, Playing or Paused
        /// </summary>
        public SongPlayerStatus Status
        {
            get
            {
                lock (this)
                {
                    return fStatus;
                }
            }
        }

        /// <summary>
        /// Byte position of the current position of the playing music.
        /// </summary>
        public long CurrentPosition
        {
            get
            {
                lock (this)
                {
                    if (fStatus == SongPlayerStatus.Stopped)
                    {
                        return 0;
                    }

                    ValidateSongPlayingOrPaused();

                    //return Bass.BASS_ChannelGetPosition(fStreamHandler, BASSMode.BASS_POS_BYTES);
                    return Bass.BASS_ChannelGetPosition(fStreamFxHandler, BASSMode.BASS_POS_BYTES);
                }
            }
        }

        /// <summary>
        /// Current position of the playing music in seconds.
        /// </summary>
        public double CurrentPositionAsSeconds
        {
            get
            {
                lock (this)
                {
                    //return Bass.BASS_ChannelBytes2Seconds(fStreamHandler, this.CurrentPosition);
                    return Bass.BASS_ChannelBytes2Seconds(fStreamFxHandler, this.CurrentPosition);
                }
            }
        }

        /// <summary>
        /// Song's length in bytes. Note that the content may be compacted.
        /// </summary>
        public long Length
        {
            get
            {
                lock (this)
                {
                    LoadStream();

                    //return Bass.BASS_ChannelGetLength(fStreamHandler);
                    return Bass.BASS_ChannelGetLength(fStreamFxHandler);
                }
            }
        }

        /// <summary>
        /// Song's duration in seconds
        /// </summary>
        public double DurationAsSeconds
        {
            get
            {
                lock (this)
                {
                    //return Bass.BASS_ChannelBytes2Seconds(fStreamHandler, this.Length);
                    return Bass.BASS_ChannelBytes2Seconds(fStreamFxHandler, this.Length);
                }
            }
        }

        //public double TickDurationAsSeconds
        //{
        //    get
        //    {
        //        lock (this)
        //        {
        //            return AudioMaths.TickTempoAsSeconds(this.TempoBPM);
        //        }
        //    }
        //}

        /// <summary>
        /// The song's time signature (4x4, for example).
        /// </summary>
        public GtTimeSignature TimeSignature
        {
            get
            {
                lock (this)
                {
                    return fTimeSignature;
                }
            }
        }

        #endregion

        #region Public Methods

        public void Play()
        {
            Play(100, 0.0f);
        }


        public void Play(int velocity)
        {
            Play(velocity, 0.0f);
        }

        public void Play(int velocity, float pitch)
        {
            Velocity = velocity;
            Pitch = pitch;

            LoadStream();

            lock (this)
            {
                if (this.Status == SongPlayerStatus.Stopped)
                {
                    fLastBeatNotified = 1;
                    fLastTickNotified = 0;

                    InstallCallbackMethods();
                }

                bool restart = (this.Status == SongPlayerStatus.Stopped);

                InstallBeatNotifyEvent();

                //Update the SongPlayer.Status
                this.fStatus = SongPlayerStatus.Playing;

                if (TickNotifyEvent != null)
                {
                    //Trace.TraceInformation(string.Format("OnBeatNotify({0})", counterTest++));
                    TickNotifyEvent(this, fLastBeatNotified, fLastTickNotified);
                }

                if (fStreamFxHandler != 0)
                {
                    bool status = false;

                    Bass.BASS_ChannelSetAttribute(fStreamFxHandler, BASSAttribute.BASS_ATTRIB_TEMPO, (Velocity - 100));

                    if (Pitch != 0)
                    {
                        if (!Bass.BASS_ChannelSetAttribute(fStreamFxHandler, BASSAttribute.BASS_ATTRIB_TEMPO_PITCH, Pitch))
                            throw new AudioProcessingError(string.Format("SongPlayer.Play() couldn´t change the pitch for the file [{0}].", fFileName));

                    }

                    //Start playing with FX
                    status = Bass.BASS_ChannelPlay(fStreamFxHandler, restart);

                    if (!status)
                    {
                        throw new AudioProcessingError(string.Format("SongPlayer.Play() couldn´t play the file [{0}].", fFileName));
                    }
                }
            }
        }

        public void Pause()
        {
            lock (this)
            {
                if (fStatus != SongPlayerStatus.Playing)
                {
                    throw new SongPlayerInconsistence(
                        string.Format("Unexpected SongPlayer status {0}.", fStatus.ToString()));
                }

                //if (fStreamHandler != 0)
                if (fStreamFxHandler != 0)
                {
                    //bool status = Bass.BASS_ChannelPause(fStreamHandler);
                    bool status = Bass.BASS_ChannelPause(fStreamFxHandler);

                    if (!status)
                    {
                        throw new AudioProcessingError(
                            string.Format("SongPlayer.Pause() couldn´t pause the file [{0}].", fFileName));
                    }
                }

                //Update the SongPlayer.Status
                this.fStatus = SongPlayerStatus.Paused;
            }
        }

        public void Stop()
        {
            lock (this)
            {
                ValidateSongPlayingOrPaused();

                //if (fStreamHandler != 0)
                if (fStreamFxHandler != 0)
                {
                    //bool status = Bass.BASS_ChannelStop(fStreamHandler);
                    bool status = Bass.BASS_ChannelStop(fStreamFxHandler);

                    if (!status)
                    {
                        throw new AudioProcessingError(
                            string.Format("SongPlayer.Stop() couldn´t stop the file [{0}].", fFileName));
                    }
                }

                //Update the SongPlayer.Status
                this.fStatus = SongPlayerStatus.Stopped;
            }
        }

        public void LoadStream()
        {
            if (fFileName == string.Empty)
                return;

            if (!this.fAudioDeviceInitialized)
            {
                InitializeAudioDevice();
            }

            if (fStreamFxHandler == 0)
            //if (fStreamHandler == 0)
            {
                //Create the stream of the song file in order to play it
                var fStreamHandler = Bass.BASS_StreamCreateFile(fFileName, 0L, 0L, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT
                    | BASSFlag.BASS_MUSIC_NOSAMPLE | BASSFlag.BASS_MUSIC_DECODE);

                if (fStreamHandler == 0)
                    throw new AudioProcessingError(string.Format("SongPlayer couldn't create the stream of the file [{0}].", fFileName));

                //Create the strem over the song to process effects (velocity)
                fStreamFxHandler = BassFx.BASS_FX_TempoCreate(fStreamHandler, BASSFlag.BASS_FX_FREESOURCE | BASSFlag.BASS_SAMPLE_FLOAT);

                if (fStreamFxHandler == 0)
                    throw new AudioProcessingError(string.Format("SongPlayer couldn't create the stream for FX effects for the file [{0}].", fFileName));

                Bass.BASS_ChannelSetAttribute(fStreamFxHandler, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_PREVENT_CLICK, 1);
                Bass.BASS_ChannelSetAttribute(fStreamFxHandler, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_SEQUENCE_MS, 82);
                Bass.BASS_ChannelSetAttribute(fStreamFxHandler, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_SEEKWINDOW_MS, 14);
                Bass.BASS_ChannelSetAttribute(fStreamFxHandler, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_OVERLAP_MS, 12);
            }

        }

        #endregion

        #region Private Methdos

        private void InitializeAudioDevice()
        {
            BassWrapper.Instance.BassInit();

            if (!this.fAudioDeviceInitialized)
            {
                throw new AudioProcessingError("SongPlayer.InitializeAudioDevice() faild.");
            }
        }

        private void FinalizeAudioDevice()
        {
            BassWrapper.Instance.BassFree();
        }

        private void ValidateSongPlayingOrPaused()
        {
            if ((fStatus != SongPlayerStatus.Playing) && (fStatus != SongPlayerStatus.Paused))
            {
                    throw new SongPlayerInconsistence(string.Format("Unexpected SongPlayer status {0}.", fStatus.ToString()));
            }

            //if ((!this.PlayingNothing) && (fStreamHandler == 0))
            if ((!this.PlayingNothing) && (fStreamFxHandler == 0))
            {
                throw new SongPlayerInconsistence(string.Format("Invalid stream handler for file [{0}].", fFileName));
            }
        }

        private void InstallCallbackMethods()
        {
            //Set a callback in order to be notifed when the song is finished.
            Bass.BASS_ChannelSetSync(
                //fStreamHandler,
                fStreamFxHandler,
                BASSSync.BASS_SYNC_END | BASSSync.BASS_SYNC_MIXTIME,
                0,
                fOnFinishedHandle,
                IntPtr.Zero);
        }

        private void InstallBeatNotifyEvent()
        {
            return;

            //if (fOnBeatNotifyId != 0)
            //{
            //    Bass.BASS_ChannelRemoveSync(fStreamHandler, fOnBeatNotifyId);
            //    fOnBeatNotifyId = 0;
            //}

            //this.InstallAllTickNotifyEvents();

            ////Increase the "last position" to notify the next beat (we're notified once for each beat).
            //fLastBeatNotifyPosition += Bass.BASS_ChannelSeconds2Bytes(
            //    fStreamHandler,
            //    AudioMaths.BeatTempoAsSeconds(this.TempoBPM));


            ////Trace.TraceInformation(string.Format("Installing BeatNotify on beat {0} and time {1}.", fLastBeatNotified + 1, fLastBeatNotifyPosition));

            ////set the callback
            //this.fOnBeatNotifyId = Bass.BASS_ChannelSetSync(
            //    fStreamHandler,
            //    BASSSync.BASS_SYNC_POS | BASSSync.BASS_SYNC_MIXTIME,
            //    fLastBeatNotifyPosition,
            //    fOnBeatNotifyHandle,
            //    IntPtr.Zero);
        }

        private void InstallAllTickNotifyEvents()
        {
            return;

            //Trace.TraceInformation("Begin: InstallAllTickNotifyEvents");

            //for (int i = 40; i < 480; i += 40)
            //{
            //    var tickPosition = fLastBeatNotifyPosition
            //        + Bass.BASS_ChannelSeconds2Bytes(
            //            fStreamHandler,
            //            i * AudioMaths.TickTempoAsSeconds(this.TempoBPM)
            //            );

            //    //Trace.TraceInformation(string.Format("Installing TickNotify on beat:tick {0}:{1} and time {2}.",
            //    //    fLastBeatNotified, i, tickPosition));

            //    //set the callback
            //    Bass.BASS_ChannelSetSync(
            //        fStreamHandler,
            //        BASSSync.BASS_SYNC_POS | BASSSync.BASS_SYNC_MIXTIME,
            //        tickPosition,
            //        fOnTickNotifyHandle,
            //        IntPtr.Zero);

            //}

            //Trace.TraceInformation("End: InstallAllTickNotifyEvents");
        }


        /// <summary>
        /// Evento disparado quando uma música termina.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="channel"></param>
        /// <param name="data"></param>
        /// <param name="user"></param>
        private void OnFinished(int handle, int channel, int data, IntPtr user)
        {
            lock (this)
            {
                this.fStatus = SongPlayerStatus.Stopped;
            }
        }

        /// <summary>
        /// Event called for each beat.
        /// </summary>
        /// <param name="handle"></param>
        /// <param name="channel"></param>
        /// <param name="data"></param>
        /// <param name="user"></param>
        private void OnBeatNotify(int handle, int channel, int data, IntPtr user)
        {
            lock (this)
            {
                //Trace.TraceInformation(string.Format("OnBeatNotify({0})", counterTest++));

                fLastBeatNotified++;
                fLastTickNotified = 0;

                //Install the event for the next tick40
                InstallBeatNotifyEvent();

                if (TickNotifyEvent != null)
                    TickNotifyEvent(this, fLastBeatNotified, fLastTickNotified);
            }
        }

        private void OnTickNotify(int handle, int channel, int data, IntPtr user)
        {
            lock (this)
            {
                //Trace.TraceInformation(string.Format("OnTickNotify({0})", counterTest++));

                if (fLastTickNotified < 470)
                {
                    fLastTickNotified += 40;
                }

                if (TickNotifyEvent != null)
                    TickNotifyEvent(this, fLastBeatNotified, fLastTickNotified);
            }

        }

        #endregion

        #region IDisposable Members

        public void Dispose()
        {
            if ((fStatus == SongPlayerStatus.Playing) || (fStatus == SongPlayerStatus.Paused))
            {
                this.Stop();
            }

            if (this.fAudioDeviceInitialized)
            {
                FinalizeAudioDevice();
            }
        }

        #endregion



        public void ChangeVelocity(int newVelocity)
        {
            Velocity = newVelocity;

            Bass.BASS_ChannelSetAttribute(fStreamFxHandler, BASSAttribute.BASS_ATTRIB_TEMPO, (Velocity - 100));
        }

    }
}
