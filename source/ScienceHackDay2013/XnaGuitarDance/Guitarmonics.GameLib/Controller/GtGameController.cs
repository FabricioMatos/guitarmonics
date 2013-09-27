using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib;
using Microsoft.Xna.Framework.Input;
using Guitarmonics.GameLib.Model;
using Guitarmonics.AudioLib.Player;
using Guitarmonics.AudioLib.Analysis;

namespace Guitarmonics.GameLib.Controller
{
    public enum EnumGameScreen
    {
        Undefined,
        Main,
        Menu,
        ChooseSong,
        Tune,
        PlayingSong,
        SongPaused,
        Quit
    }

    public enum EnumMenuScreenItems
    {
        Undefined,
        QuickPlay,
        Tune,
        Quit
    }

    public class GtGameController : IDisposable
    {
        public GtGameController(GtFactory pFactory, GtFileLoader pFileLoader, ISongPlayer pSongPlayer, IAudioEffects pAudioEffects)
        {
            if (pSongPlayer == null)
                throw new InvalidParameter("pSongPlayer can't be null");

            if (pFactory == null)
                throw new InvalidParameter("pFactory can't be null");

            if (pAudioEffects == null)
                throw new InvalidParameter("pAudioEffects can't be null");

            Initialize(pFactory, pFileLoader, pSongPlayer, pAudioEffects);
        }

        public GtGameController(GtFileLoader pFileLoader)
        {
            var factory = GtFactory.Instance;

            Initialize(factory, pFileLoader, new SongPlayer(), new AudioEffects());
        }

        private void Initialize(GtFactory pFactory, GtFileLoader pFileLoader, ISongPlayer pSongPlayer, IAudioEffects pAudioEffects)
        {
            this.Factory = pFactory;

            this.SongPlayer = pSongPlayer;

            this.fFileLoader = pFileLoader;

            this.AudioEffects = pAudioEffects;

            this.CurrentScreen = EnumGameScreen.Undefined;

            this.AudioListener = pFactory.Instantiate<IAudioListener>(SAMPLE_FREQUENCE);

            //must be after AudioListener instantiation
            this.GameRoundController = pFactory.Instantiate<IGtGameRoundController>(pFactory, this);

            this.TuneController = pFactory.Instantiate<IGtTuneController>();

            this.AudioListener.Start();
        }

        #region Fields and Properties

        public const int VISIBLE_SONG_WINDOW_SIZE = 7; //an odd number is better!
        public const int SAMPLE_FREQUENCE = 12; //12Hz

        private GtFileLoader fFileLoader;
        protected int SelectedSongIndex { get; set; }

        public IAudioListener AudioListener { get; protected set; }
        public IAudioEffects AudioEffects { get; protected set; }
        public GtFactory Factory { get; protected set; }
        public ISongPlayer SongPlayer { get; protected set; }
        public EnumGameScreen CurrentScreen { get; protected set; }
        public EnumMenuScreenItems MenuScreenSelectedItem { get; protected set; }
        public IList<SongDescription> SongList { get; protected set; }
        public SongDescription SelectedSong
        {
            get
            {
                if (this.SelectedSongIndex >= this.SongList.Count)
                    return null;

                return this.SongList[this.SelectedSongIndex];
            }
        }

        public IList<SongDescription> VisibleSongs
        {
            get
            {
                int start;
                var result = new List<SongDescription>();

                int halfWindow = (VISIBLE_SONG_WINDOW_SIZE - 1) / 2;

                if ((this.SelectedSongIndex - halfWindow) >= 0)
                {
                    start = this.SelectedSongIndex - halfWindow;
                }
                else
                {
                    start = 0;
                }


                if ((this.SelectedSongIndex + halfWindow) >= this.SongList.Count)
                {
                    start = this.SongList.Count - VISIBLE_SONG_WINDOW_SIZE;
                }

                if (start < 0)
                    start = 0;

                for (int i = start; i < start + VISIBLE_SONG_WINDOW_SIZE; i++)
                {
                    if (i < this.SongList.Count)
                        result.Add(this.SongList[i]);
                }

                return result;
            }
        }

        public IGtGameRoundController GameRoundController { get; protected set; }
        public IGtTuneController TuneController { get; protected set; }

        #endregion

        public void Update(GtUserActionsListener pUser1ActionsListener)
        {
            switch (this.CurrentScreen)
            {
                case EnumGameScreen.Undefined:
                    this.CurrentScreen = EnumGameScreen.Main;
                    //this.AudioEffects.PlaySongTheme();

                    break;

                case EnumGameScreen.Main:
                    if ((pUser1ActionsListener.IsKeyDownNow(Keys.Enter)) ||
                        (pUser1ActionsListener.IsKeyDownNow(Keys.Escape)) ||
                        (pUser1ActionsListener.CurrentGamePadState.IsButtonDown(Buttons.Start)))
                    {
                        this.CurrentScreen = EnumGameScreen.Quit;

                        //this.AudioEffects.StopSongTheme();
                        this.AudioEffects.PlayDoubleHit();
                    }

                    break;
            }
        }

        public void LoadSongList()
        {
            this.SongList = this.fFileLoader.ListAllSongs();
            this.SelectedSongIndex = 0;
        }

        public void Dispose()
        {
            AudioListener.Stop();
            AudioListener.Dispose();
        }
    }
}
