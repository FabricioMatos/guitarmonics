using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Input;
using Guitarmonics.AudioLib.Player;
using Un4seen.Bass;

namespace Guitarmonics.GameLib.Controller
{
    /// <summary>
    /// Used to process the values of some user inputs (Keyboard or GamePad).
    /// Note that one instance of GtUserActionsListener will be used for each user (if in multiplayer mode)
    /// </summary>
    public class GtUserActionsListener
    {
        public GtUserActionsListener()
        {
        }

        private KeyboardState fCurrentKeyboardState;
        private GamePadState fCurrentGamePadState;

        private KeyboardState fPreviousKeyboardState;
        private GamePadState fPreviousGamePadState;

        public KeyboardState CurrentKeyboardState
        {
            get { return fCurrentKeyboardState; }
        }

        public GamePadState CurrentGamePadState
        {
            get { return fCurrentGamePadState; }
        }

        public virtual void Update(KeyboardState pCurrentKeyboardState, GamePadState pCurrentGamePadState)
        {
            this.fPreviousKeyboardState = this.fCurrentKeyboardState;
            this.fPreviousGamePadState = this.fCurrentGamePadState;

            this.fCurrentKeyboardState = pCurrentKeyboardState;
            this.fCurrentGamePadState = pCurrentGamePadState;
        }

        public bool IsKeyDownNow(Keys pKey)
        {
            return (this.fCurrentKeyboardState.IsKeyDown(pKey))
                && (!this.fPreviousKeyboardState.IsKeyDown(pKey));
        }

        public bool IsButtonDownNow(Buttons pButton)
        {
            return (this.fCurrentGamePadState.IsButtonDown(pButton))
                && (!this.fPreviousGamePadState.IsButtonDown(pButton));
        }
    }

    //public class GtUserActionsListener
    //{
    //    private XnaGame fGame;
    //    private KeyboardState LastkeyboardState;
    //    private GamePadState LastGamePadState;
    //    private SongPlayer SongPlayer;

    //    public long CurrentBeat { get; set; }
    //    public long CurrentTick { get; set; }

    //    public GtUserActionsListener(XnaGame pGame)
    //    {
    //        fGame = pGame;
    //        LastkeyboardState = Keyboard.GetState();
    //        LastGamePadState = GamePad.GetState(PlayerIndex.One);
    //    }

    //    public static string AudioPath = @"D:\_Qualidata\_SVN\pastas-particulares\fabricio\Game Guitarra\Prototipo01\XnaGame.Player.Tests\Audio\";
    //    protected static string PingHi = AudioPath + "Ping Hi.wav";
    //    protected static string Mp3_ForWhomTheBellTolls = AudioPath + "metallica-for_whom_the_bell_tolls.mp3";
    //    protected static string MP3MattRedman = AudioPath + "Matt Redman.Facedown.Track 05.MP3";

    //    public void Update(TimeSpan pTotalTime, TimeSpan pElapsedTime)
    //    {

    //        KeyboardState currentKeyboardState = Keyboard.GetState();

    //        // Esc => Exit Game
    //        if ((GamePad.GetState(PlayerIndex.One).Buttons.Back == ButtonState.Pressed) || (currentKeyboardState.IsKeyDown(Keys.Escape)))
    //        {
    //            if (this.SongPlayer != null)
    //            {
    //                if (this.SongPlayer.Status == SongPlayerStatus.Playing)
    //                    this.SongPlayer.Stop();
    //            }

    //            fGame.Exit();
    //        }

    //        //Space => Play/Pause
    //        if ((LastkeyboardState.IsKeyDown(Keys.Space)) && (currentKeyboardState.IsKeyUp(Keys.Space)))
    //        {
    //            if (this.SongPlayer != null)
    //            {
    //                if (this.SongPlayer.Status == SongPlayerStatus.Playing)
    //                    this.SongPlayer.Pause();
    //                else
    //                    this.SongPlayer.Play();
    //            }
    //            else
    //            {

    //                this.SongPlayer = new SongPlayer(MP3MattRedman, 140, TimeSignature.Time4x4);
    //                //this.SongPlayer = new SongPlayer(Mp3_ForWhomTheBellTolls, 120, TimeSignature.Time4x4);
    //                this.SongPlayer.TickNotifyEvent += new TickNotifyEvent(HandleTickNotifyEvent);
    //                this.SongPlayer.TickNotifyEvent += new TickNotifyEvent(HandleTickNotifyEvent_WithClick);

    //                this.SongPlayer.Play();
    //            }
    //        }

    //        this.LastkeyboardState = Keyboard.GetState();
    //        this.LastGamePadState = GamePad.GetState(PlayerIndex.One);

    //    }

    //    private void HandleTickNotifyEvent(SongPlayer pSongPlayer, long pBeat, long pTick)
    //    {
    //        this.CurrentBeat = pBeat;
    //        this.CurrentTick = pTick;
    //    }

    //    private void HandleTickNotifyEvent_WithClick(SongPlayer pSongPlayer, long pBeat, long pTick)
    //    {
    //        if (pTick == 0)
    //        {
    //            int sampleHandle = Bass.BASS_SampleLoad(PingHi, 0, 0, 10, BASSFlag.BASS_SAMPLE_OVER_POS);
    //            int sampleChannel = Bass.BASS_SampleGetChannel(sampleHandle, false);
    //            Bass.BASS_ChannelPlay(sampleChannel, false);
    //        }
    //    }

    //}
}
