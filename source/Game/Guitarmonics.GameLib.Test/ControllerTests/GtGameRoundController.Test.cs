using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib;
using Guitarmonics.AudioLib.Analysis;
using NUnit.Framework;
using Microsoft.Xna.Framework.Input;
using Guitarmonics.AudioLib.Player;
using System.Timers;
using Guitarmonics.GameLib.Model;
using Guitarmonics.GameLib.View;
using Guitarmonics.GameLib.Controller;

namespace Guitarmonics.GameLib.ControllerTest
{
    [TestFixture]
    public class GtGameRoundControllerTest
    {
        #region Construction

        [Test]
        public void Constructor_OnePlayer()
        {
            var factory = new GtFactory();
            factory.AddMapping<IAudioListener, DoubleAudioListenerDoNothing>();

            var controller = new GtGameRoundController(
                factory,
                new GtGameController(new GtFileLoaderDouble6()));

            Assert.IsNotNull(controller);
        }

        #endregion

        #region Main commands 

        [Test]
        public void EscapeFinishGameRound()
        {
            var player1 = new GtUserActionsListener();

            var factory = new GtFactory();
            factory.AddMapping<IAudioListener, DoubleAudioListenerDoNothing>();

            var controller = new GtGameRoundController(
                factory,
                new GtGameController(new GtFileLoaderDouble6()));

            controller.Update(player1);

            controller.GameRoundState = EnumGameRoundState.Playing;

            KeyboardState keyboardState = new KeyboardState(Keys.Escape); ;
            GamePadState gamePadState = new GamePadState();
            player1.Update(keyboardState, gamePadState);

            controller.Update(player1);

            Assert.AreEqual(EnumGameRoundState.Finished, controller.GameRoundState);
        }

        [TestCase(100, 95)]
        [TestCase(30, 30)]
        public void LeftArrowDecreaseVelocityByPointOne(decimal initialVelocityValue, decimal finalVelocityValue)
        {
            var player1 = new GtUserActionsListener();

            var factory = new GtFactory();
            factory.AddMapping<IAudioListener, DoubleAudioListenerDoNothing>();

            var controller = new GtGameRoundController(
                factory,
                new GtGameController(new GtFileLoaderDouble6()));

            KeyboardState keyboardState = new KeyboardState(Keys.Left); ;
            GamePadState gamePadState = new GamePadState();

            player1.Update(keyboardState, gamePadState);

            controller.VelocityValue = initialVelocityValue;
            controller.Update(player1);

            Assert.AreEqual(finalVelocityValue, controller.VelocityValue);
        }

        [TestCase(30, 35)]
        [TestCase(100, 100)]
        public void RightArrowDecreaseVelocityByPointOne(decimal initialVelocityValue, decimal finalVelocityValue)
        {
            var player1 = new GtUserActionsListener();

            var factory = new GtFactory();
            factory.AddMapping<IAudioListener, DoubleAudioListenerDoNothing>();

            var controller = new GtGameRoundController(
                factory,
                new GtGameController(new GtFileLoaderDouble6()));

            KeyboardState keyboardState = new KeyboardState(Keys.Right); ;
            GamePadState gamePadState = new GamePadState();

            player1.Update(keyboardState, gamePadState);

            controller.VelocityValue = initialVelocityValue;
            controller.Update(player1);

            Assert.AreEqual(finalVelocityValue, controller.VelocityValue);
        }


        #endregion

        #region Setup Song

        [Test]
        public void SetupSong_OnePlayer()
        {
            var factory = new GtFactory();
            factory.AddMapping<IAudioListener, DoubleAudioListenerDoNothing>();

            var controller = new GtGameRoundController(
                factory,
                new GtGameController(new GtFileLoaderDouble6()));

            var guitar1 = new GtSceneGuitar(new GtTickDataTable(4));

            controller.SetupSong(1, new Double_SongPlayerWithTimer(new BeatTick(5, 0)), "Artist", "Album", "Song", 0.0f, guitar1);

            Assert.AreEqual(1, controller.SceneGuitars.Count());
            Assert.AreSame(guitar1, controller.SceneGuitars[0]);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidGameRoundControlerParameters))]
        public void SetupSong_CantBeNull()
        {
            var factory = new GtFactory();
            factory.AddMapping<IAudioListener, DoubleAudioListenerDoNothing>();

            var controller = new GtGameRoundController(
                factory,
                new GtGameController(new GtFileLoaderDouble6()));

            controller.SetupSong(1, new Double_SongPlayerWithTimer(new BeatTick(5, 0)), "Artist", "Album", "Song", 0.0f, null);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidGameRoundControlerParameters))]
        public void SetupSong_ZeroPlayers()
        {
            var factory = new GtFactory();
            factory.AddMapping<IAudioListener, DoubleAudioListenerDoNothing>();

            var controller = new GtGameRoundController(
                factory,
                new GtGameController(new GtFileLoaderDouble6()));

            controller.SetupSong(0, new Double_SongPlayerWithTimer(new BeatTick(5, 0)), "Artist", "Album", "Song", 0.0f);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidGameRoundControlerParameters))]
        public void SetupSong_TrePlayers()
        {
            var factory = new GtFactory();
            factory.AddMapping<IAudioListener, DoubleAudioListenerDoNothing>();

            var controller = new GtGameRoundController(
                factory,
                new GtGameController(new GtFileLoaderDouble6()));

            var guitar1 = new GtSceneGuitar(new GtTickDataTable(4));
            var guitar2 = new GtSceneGuitar(new GtTickDataTable(4));
            var guitar3 = new GtSceneGuitar(new GtTickDataTable(4));
            controller.SetupSong(3, new Double_SongPlayerWithTimer(new BeatTick(5, 0)), "Artist", "Album", "Song", 0.0f, guitar1, guitar2, guitar3);
        }


        [Test]
        public void SetupSong_TwoPlayers()
        {
            var factory = new GtFactory();
            factory.AddMapping<IAudioListener, DoubleAudioListenerDoNothing>();

            var controller = new GtGameRoundController(
                factory,
                new GtGameController(new GtFileLoaderDouble6()));

            var guitar1 = new GtSceneGuitar(new GtTickDataTable(4));
            var guitar2 = new GtSceneGuitar(new GtTickDataTable(4));

            controller.SetupSong(2, new Double_SongPlayerWithTimer(new BeatTick(5, 0)), "Artist", "Album", "Song", 0.0f, guitar1, guitar2);

            Assert.AreEqual(2, controller.SceneGuitars.Count());
            Assert.AreSame(guitar1, controller.SceneGuitars[0]);
            Assert.AreSame(guitar2, controller.SceneGuitars[1]);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidGameRoundControlerParameters))]
        public void SetupSong_InvalidNumberOfPlayers()
        {
            var factory = new GtFactory();
            factory.AddMapping<IAudioListener, DoubleAudioListenerDoNothing>();

            var controller = new GtGameRoundController(
                factory,
                new GtGameController(new GtFileLoaderDouble6()));

            var guitar1 = new GtSceneGuitar(new GtTickDataTable(4));
            var guitar2 = new GtSceneGuitar(new GtTickDataTable(4));

            controller.SetupSong(1, new Double_SongPlayerWithTimer(new BeatTick(5, 0)), "Artist", "Album", "Song", 0.0f, guitar1, guitar2);
        }

        #endregion

        [Test]
        public void PlayMusic()
        {
            var factory = new GtFactory();
            factory.AddMapping<IAudioListener, DoubleAudioListenerDoNothing>();

            var controller = new GtGameRoundController(
                factory,
                new GtGameController(new GtFileLoaderDouble6()));

            var guitar1 = new GtSceneGuitar(new GtTickDataTable(4));

            controller.SetupSong(1, new Double_SongPlayerWithTimer(new BeatTick(5, 0)), "Artist", "Album", "Song", 0.0f, guitar1);

            controller.PlaySong();

            Assert.AreEqual(EnumGameRoundState.Playing, controller.GameRoundState);
        }
    }

    /// <summary>
    /// Used for test
    /// </summary>
    //public class GtUserActionsListenerForTest : GtUserActionsListener
    //{
    //    public void SetEscapePressed(bool value)
    //    {
    //        this.EscapePressed = value;
    //    }
    //}

    public class DoubleAudioListenerDoNothing : IAudioListener
    {
        public DoubleAudioListenerDoNothing(int pSampleFrequence)
        {
        }

        #region IAudioListener Members

        public int SampleFrequence
        {
            get { return 40; }
        }

        public System.Threading.Thread WorkerThread
        {
            get { throw new NotImplementedException(); }
        }

        public bool Stopped
        {
            get { return false; }
        }

        public bool AudioDeviceInitialized
        {
            get { return true; }
        }

        public int RecordChannel
        {
            get { return 0; }
        }

        public float[] FftData
        {
            get { return new float[4096]; }
        }

        public void Start()
        {
        }

        public void Stop()
        {
        }

        public void Dispose()
        {
        }

        #endregion
    }


    public class Double_SongPlayerWithTimer : ISongPlayer
    {
        public Double_SongPlayerWithTimer(BeatTick pDuration)
        {
            this.fExecutionLog = new List<string>();

            this.fCurrentPosition = new BeatTick(1, 0);

            this.fTimer = new Timer(10 * TICK_AS_MILISECONDS);
            this.fTimer.Elapsed += new ElapsedEventHandler(fTimer_Elapsed);

            this.fSongPlayerStatus = SongPlayerStatus.Stopped;

            this.fDuration = pDuration;
        }

        private const int TICK_AS_MILISECONDS = 5;
        private List<string> fExecutionLog;
        private Timer fTimer;
        private BeatTick fCurrentPosition;
        private BeatTick fDuration;
        private SongPlayerStatus fSongPlayerStatus;

        void fTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            fCurrentPosition.AddTicks(10);

            if (fCurrentPosition >= fDuration)
                this.Stop();
        }

        public List<string> ExecutionLog
        {
            get
            {
                return fExecutionLog;
            }
        }

        public TickNotifyEvent TickNotifyEvent { get; set; }

        #region ISongPlayer Members

        public long CurrentPosition
        {
            get { return this.fCurrentPosition.AsTicks(); }
        }

        public double CurrentPositionAsSeconds
        {
            get { return this.fCurrentPosition.AsTicks() * TICK_AS_MILISECONDS * 1000; }
        }

        public double DurationAsSeconds
        {
            get { return this.fDuration.AsTicks() * TICK_AS_MILISECONDS * 1000; }
        }

        public long Length
        {
            get { return this.fDuration.AsTicks(); }
        }

        public SongPlayerStatus Status
        {
            get { return this.fSongPlayerStatus; }
        }

        public double TickDurationAsSeconds
        {
            get { return TICK_AS_MILISECONDS * 1000; }
        }

        public GtTimeSignature TimeSignature
        {
            get { return GtTimeSignature.Time4x4; }
        }

        public void LoadStream()
        {
            fExecutionLog.Add("LoadStream()");
        }

        public void Pause()
        {
            this.fSongPlayerStatus = SongPlayerStatus.Paused;
            this.fTimer.Stop();

            fExecutionLog.Add("Pause()");
        }

        public void Play()
        {
            Play(100);
        }

        public void Play(int velocity)
        {
            this.fSongPlayerStatus = SongPlayerStatus.Playing;
            this.fTimer.Start();

            fExecutionLog.Add("Play()");
        }


        public void Stop()
        {
            this.fSongPlayerStatus = SongPlayerStatus.Stopped;
            this.fTimer.Stop();

            fExecutionLog.Add("Stop()");
        }


        public double TempoBPM
        {
            get { throw new NotImplementedException(); }
        }

        public void SetupSong(string pFileName, double pTempoBPM, GtTimeSignature pTimeSignature)
        {
            throw new NotImplementedException();
        }

        #endregion

        #region ISongPlayer Members

        long ISongPlayer.CurrentPosition
        {
            get { throw new NotImplementedException(); }
        }

        double ISongPlayer.CurrentPositionAsSeconds
        {
            get { throw new NotImplementedException(); }
        }

        double ISongPlayer.DurationAsSeconds
        {
            get { throw new NotImplementedException(); }
        }

        long ISongPlayer.Length
        {
            get { throw new NotImplementedException(); }
        }

        void ISongPlayer.LoadStream()
        {
            throw new NotImplementedException();
        }

        void ISongPlayer.Pause()
        {
        }

        void ISongPlayer.Play()
        {
        }

        SongPlayerStatus ISongPlayer.Status
        {
            get { return SongPlayerStatus.NotInitialized; }
        }

        void ISongPlayer.Stop()
        {
        }

        GtTimeSignature ISongPlayer.TimeSignature
        {
            get { throw new NotImplementedException(); }
        }

        TickNotifyEvent ISongPlayer.TickNotifyEvent
        {
            get
            {
                throw new NotImplementedException();
            }
            set
            {
                throw new NotImplementedException();
            }
        }

        void ISongPlayer.SetupSong(string pFileName, GtTimeSignature pTimeSignature)
        {
            throw new NotImplementedException();
        }

        #endregion

        public void Dispose()
        {
        }



        public void ChangeVelocity(int newVelocity)
        {
        }


        public void Play(int velocity, float pitch)
        {
        }
    }
}
