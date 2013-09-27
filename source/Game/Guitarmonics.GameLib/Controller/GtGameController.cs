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
                    this.AudioEffects.PlaySongTheme();

                    break;

                case EnumGameScreen.Main:
                    //Main => Menu
                    if ((pUser1ActionsListener.IsKeyDownNow(Keys.Enter)) ||
                        (pUser1ActionsListener.CurrentGamePadState.IsButtonDown(Buttons.Start)))
                    {
                        this.CurrentScreen = EnumGameScreen.Menu;
                        this.MenuScreenSelectedItem = EnumMenuScreenItems.QuickPlay;

                        this.AudioEffects.StopSongTheme();
                        this.AudioEffects.PlayDoubleHit();
                    }

                    break;

                case EnumGameScreen.Menu:
                    //Menu => ChooseSong
                    if ((pUser1ActionsListener.IsKeyDownNow(Keys.Enter)) ||
                        (pUser1ActionsListener.CurrentGamePadState.IsButtonDown(Buttons.A)))
                    {
                        switch (this.MenuScreenSelectedItem)
                        {
                            case EnumMenuScreenItems.QuickPlay:
                                try
                                {
                                    this.AudioEffects.PlayDoubleHit();

                                    LoadSongList();

                                    this.CurrentScreen = EnumGameScreen.ChooseSong;
                                }
                                catch (Exception e)
                                {
                                    //TODO: Show the error message (connection timeout, for example)

                                    throw e;
                                }

                                break;

                            case EnumMenuScreenItems.Tune:
                                this.CurrentScreen = EnumGameScreen.Tune;
                                break;

                            case EnumMenuScreenItems.Quit:
                                this.CurrentScreen = EnumGameScreen.Quit;
                                break;
                        }

                        this.AudioEffects.PlayDoubleHit();

                    }

                    //change menu option - down
                    if ((pUser1ActionsListener.IsKeyDownNow(Keys.Down)) ||
                        (pUser1ActionsListener.CurrentGamePadState.IsButtonDown(Buttons.DPadDown)))
                    {
                        switch (this.MenuScreenSelectedItem)
                        {
                            case EnumMenuScreenItems.QuickPlay:
                                this.MenuScreenSelectedItem = EnumMenuScreenItems.Tune;
                                this.AudioEffects.PlaySingleHit();
                                break;
                            case EnumMenuScreenItems.Tune:
                                this.MenuScreenSelectedItem = EnumMenuScreenItems.Quit;
                                this.AudioEffects.PlaySingleHit();
                                break;
                            case EnumMenuScreenItems.Quit:
                                //nothing - this is the last one.
                                break;
                        }
                    }

                    //change menu option - up
                    if ((pUser1ActionsListener.IsKeyDownNow(Keys.Up)) ||
                        (pUser1ActionsListener.CurrentGamePadState.IsButtonDown(Buttons.DPadUp)))
                    {
                        switch (this.MenuScreenSelectedItem)
                        {
                            case EnumMenuScreenItems.QuickPlay:
                                //nothing - this is the first one.
                                break;
                            case EnumMenuScreenItems.Tune:
                                this.MenuScreenSelectedItem = EnumMenuScreenItems.QuickPlay;
                                this.AudioEffects.PlaySingleHit();
                                break;
                            case EnumMenuScreenItems.Quit:
                                this.MenuScreenSelectedItem = EnumMenuScreenItems.Tune;
                                this.AudioEffects.PlaySingleHit();
                                break;
                        }
                    }

                    break;

                case EnumGameScreen.Tune:
                    //Tune => Menu (back)
                    if ((pUser1ActionsListener.IsKeyDownNow(Keys.Escape)) ||
                        (pUser1ActionsListener.CurrentGamePadState.IsButtonDown(Buttons.B)))
                    {
                        this.CurrentScreen = EnumGameScreen.Menu;
                        this.MenuScreenSelectedItem = EnumMenuScreenItems.Tune;
                        this.AudioEffects.PlayDoubleHit();
                        break;
                    }

                    break;

                case EnumGameScreen.ChooseSong:

                    //ChooseSong => Main (back)
                    if ((pUser1ActionsListener.IsKeyDownNow(Keys.Escape)) ||
                        (pUser1ActionsListener.CurrentGamePadState.IsButtonDown(Buttons.B)))
                    {
                        this.CurrentScreen = EnumGameScreen.Menu;
                        this.MenuScreenSelectedItem = EnumMenuScreenItems.QuickPlay;
                        this.AudioEffects.PlayDoubleHit();
                        break;
                    }

                    //Change selected song - Next
                    if ((pUser1ActionsListener.IsKeyDownNow(Keys.Down)) ||
                        (pUser1ActionsListener.CurrentGamePadState.IsButtonDown(Buttons.DPadDown)))
                    {
                        if (this.SelectedSongIndex < (this.SongList.Count - 1))
                        {
                            this.SelectedSongIndex++;
                            this.AudioEffects.PlaySingleHit();
                        }
                        break;
                    }

                    //Change selected song - Prior
                    if ((pUser1ActionsListener.IsKeyDownNow(Keys.Up)) ||
                        (pUser1ActionsListener.CurrentGamePadState.IsButtonDown(Buttons.DPadUp)))
                    {
                        if (this.SelectedSongIndex > 0)
                        {
                            this.SelectedSongIndex--;
                            this.AudioEffects.PlaySingleHit();
                        }
                        break;
                    }


                    //ChooseSong => PlayingSong
                    if ((pUser1ActionsListener.IsKeyDownNow(Keys.Enter)) ||
                        (pUser1ActionsListener.CurrentGamePadState.IsButtonDown(Buttons.A)))
                    {
                        if (this.SelectedSong != null)
                        {
                            this.AudioEffects.PlayDoubleHit();

                            this.CurrentScreen = EnumGameScreen.PlayingSong;

                            this.fFileLoader.DownloadSong(this.SelectedSong);

                            this.SongPlayer.SetupSong(
                                this.SelectedSong.AudioFileName,
                                this.SelectedSong.TimeSignature);

                            this.SongPlayer.LoadStream();

                            this.GameRoundController.InitializeProperties();

                            var song = this.SelectedSong;
                            var tickDataTable = this.fFileLoader.LoadTickDataTable(ref song);

                            this.GameRoundController.SetupSong(1,
                                this.SongPlayer,
                                song.Artist,
                                song.Album,
                                song.Song,
                                song.Pitch,
                                //new GtSceneGuitar(tickDataTable, pNumberOfVisibleBeats: 5));
                                new GtSceneGuitar(tickDataTable, pNumberOfVisibleBeats: 7));

                            this.GameRoundController.PlaySong();
                        }
                        break;
                    }


                    break;

                case EnumGameScreen.PlayingSong:
                    if (pUser1ActionsListener.IsKeyDownNow(Keys.Space))
                    {
                        switch (this.GameRoundController.GameRoundState)
                        {
                            case EnumGameRoundState.Playing:
                                this.GameRoundController.GameRoundState = EnumGameRoundState.Paused;
                                this.AudioEffects.PlaySingleHit();
                                break;

                            case EnumGameRoundState.Paused:
                                this.GameRoundController.GameRoundState = EnumGameRoundState.Playing;
                                this.AudioEffects.PlaySingleHit();
                                break;

                            case EnumGameRoundState.Finished:
                                this.GameRoundController.GameRoundState = EnumGameRoundState.Playing;
                                break;

                        }
                    }

                    if (pUser1ActionsListener.IsKeyDownNow(Keys.Escape))
                    {
                        switch (this.GameRoundController.GameRoundState)
                        {
                            case EnumGameRoundState.Playing:
                                this.GameRoundController.GameRoundState = EnumGameRoundState.Aborting;
                                this.AudioEffects.PlaySingleHit();
                                break;

                            //case EnumGameRoundState.Paused:
                            //    this.GameRoundController.GameRoundState = EnumGameRoundState.Playing;
                            //    break;

                            case EnumGameRoundState.Finished:

                                this.CurrentScreen = EnumGameScreen.ChooseSong;

                                //reset the SongPlayer in order to play another song
                                if (this.SongPlayer != null)
                                {
                                    this.SongPlayer.Dispose();
                                    this.SongPlayer = new SongPlayer();
                                }

                                this.AudioEffects.PlayDoubleHit();
                                break;
                        }
                    }


                    if (pUser1ActionsListener.IsKeyDownNow(Keys.Enter))
                    {
                        switch (this.GameRoundController.GameRoundState)
                        {
                            case EnumGameRoundState.Finished:

                                this.CurrentScreen = EnumGameScreen.ChooseSong;

                                //reset the SongPlayer in order to play another song
                                if (this.SongPlayer != null)
                                {
                                    this.SongPlayer.Dispose();
                                    this.SongPlayer = new SongPlayer();
                                }

                                this.AudioEffects.PlayDoubleHit();
                                break;
                        }
                    }


                    this.GameRoundController.Update(pUser1ActionsListener);

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
