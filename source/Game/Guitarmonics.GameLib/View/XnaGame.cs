using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Audio;
using Microsoft.Xna.Framework.Content;
using Microsoft.Xna.Framework.GamerServices;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using Microsoft.Xna.Framework.Media;
using Microsoft.Xna.Framework.Net;
using Microsoft.Xna.Framework.Storage;
using Guitarmonics.AudioLib.Player;
using System.Threading;
using Guitarmonics.GameLib.Controller;

namespace Guitarmonics.GameLib.View
{
    public class XnaGame : Microsoft.Xna.Framework.Game
    {
        public int Width = 960;
        public int Height = 600;

        private DateTime GameStartTime;
        private DateTime LastUpdateTime;

        //720p
        //public int Width = 1280;
        //public int Height = 720;


        //private EnumGameScreen fCurrentState;
        private GtScreenBase fCurrentScreen = null;
        public GtGameController GameController { get; protected set; }
        private GtUserActionsListener fUser1ActionListener;
        private EnumGameScreen fCurrentGameScreen = EnumGameScreen.Undefined;

        public GraphicsDeviceManager GraphicsDeviceManager { get; private set; }

        public SpriteFont FontFretNumbers { protected set; get; }
        public SpriteFont FontSongDescription { protected set; get; }
        public SpriteFont FontSmallTexts { protected set; get; }
        public SpriteFont FontPlayingSongScreenScore { protected set; get; }
        public SpriteFont FontPlayingSongScreenSongTitle { protected set; get; }
        public SpriteFont FontPlayingSongScreenSongSubtitle { protected set; get; }

        public Texture2D GuitarNeckTexture { protected set; get; }
        public Texture2D StringTexture { protected set; get; }
        public Texture2D BallTexture { protected set; get; }
        public Texture2D SquareTexture { protected set; get; }
        public Texture2D StarTexture { protected set; get; }
        public Texture2D NoteSusteinTexture { protected set; get; }
        public Texture2D NoteSusteinEndTexture { protected set; get; }
        public Texture2D NoteSusteinPlayingEffect { protected set; get; }
        public Texture2D NoteSusteinPlayingLight { protected set; get; }
        public Texture2D FretTexture { protected set; get; }
        public Texture2D MatchBallTexture { protected set; get; }
        public Texture2D FireTexture { protected set; get; }
        public Texture2D EqualizerBackgroundTexture { protected set; get; }
        public Texture2D EqualizerOnePointTexture { protected set; get; }
        public Texture2D ChordPictureBackground { protected set; get; }
        public Texture2D BackgroundPlayingSong { protected set; get; }
        public Texture2D BackgroundChooseSong { protected set; get; }
        public Texture2D BackgroundMenu_QuickPlay { protected set; get; }
        public Texture2D BackgroundMenu_Tune { protected set; get; }
        public Texture2D BackgroundMenu_Quit { protected set; get; }
        public Texture2D BackgroundMainScreen { protected set; get; }
        public Texture2D BackgroundTuneScreen { protected set; get; }

        public AudioEngine AudioEngine { protected set; get; }
        public WaveBank WaveBank { protected set; get; }
        public SoundBank SoundBank { protected set; get; }

        public EnumGameScreen CurrentGameScreen
        {
            get { return fCurrentGameScreen; }
        }


        /// <summary>
        /// Current game screen instance
        /// </summary>
        public GtScreenBase CurrentScreen
        {
            get { return fCurrentScreen; }
        }

        public XnaGame(GtGameController pGameController)
        {
            this.Window.Title = "Guitarmonics";

            this.IsFixedTimeStep = true;

            this.GameController = pGameController;

            this.fUser1ActionListener = new GtUserActionsListener();

            GraphicsDeviceManager = new GraphicsDeviceManager(this);

            GraphicsDeviceManager.PreferredBackBufferWidth = this.Width;
            GraphicsDeviceManager.PreferredBackBufferHeight = this.Height;

            GraphicsDeviceManager.IsFullScreen = true;

            if (!GraphicsDeviceManager.IsFullScreen)
                GraphicsDeviceManager.ApplyChanges();

            Content.RootDirectory = "Content";
        }

        /// <summary>
        /// Allows the game to perform any initialization it needs to before starting to run.
        /// This is where it can query for any required services and load any non-graphic
        /// related content.  Calling base.Initialize will enumerate through any components
        /// and initialize them as well.
        /// </summary>
        protected override void Initialize()
        {
            // TODO: Add your initialization logic here
            base.Initialize();

            this.GameStartTime = DateTime.Now;
            this.LastUpdateTime = DateTime.Now;

        }

        #region Load Contents

        /// <summary>
        /// LoadContent will be called once per game and is the place to load
        /// all of your content.
        /// </summary>
        protected override void LoadContent()
        {
            base.LoadContent();

            LoadFonts();

            LoadTexture2D();

            LoadAudioEffects();
        }

        private void LoadAudioEffects()
        {
            this.AudioEngine = new AudioEngine(@"Content\Audio\GuitarmonicsAudioClips.xgs");
            this.WaveBank = new WaveBank(this.AudioEngine, @"Content\Audio\SoundEffects.xwb");
            this.SoundBank = new SoundBank(this.AudioEngine, @"Content\Audio\SoundBank.xsb");

            //Could be more elegant (without cast), but I was in a rush.
            if (this.GameController.AudioEffects is AudioEffects)
            {
                ((AudioEffects)this.GameController.AudioEffects).SoundBank = this.SoundBank;
            }
        }

        private void LoadTexture2D()
        {
            this.GuitarNeckTexture = Content.Load<Texture2D>(@"Images\PlayingSongScreen\GuitarNeck");
            this.StringTexture = Content.Load<Texture2D>(@"Images\PlayingSongScreen\String");
            this.BallTexture = Content.Load<Texture2D>(@"Images\PlayingSongScreen\ball");
            this.SquareTexture = Content.Load<Texture2D>(@"Images\PlayingSongScreen\square");
            this.StarTexture = Content.Load<Texture2D>(@"Images\PlayingSongScreen\star");
            this.NoteSusteinTexture = Content.Load<Texture2D>(@"Images\PlayingSongScreen\NoteSustein");
            this.NoteSusteinEndTexture = Content.Load<Texture2D>(@"Images\PlayingSongScreen\NoteSusteinEnd");
            this.NoteSusteinPlayingEffect = Content.Load<Texture2D>(@"Images\PlayingSongScreen\NoteSusteinPlayingEffect");
            this.NoteSusteinPlayingLight = Content.Load<Texture2D>(@"Images\PlayingSongScreen\NoteSusteinPlayingLight");
            this.FretTexture = Content.Load<Texture2D>(@"Images\PlayingSongScreen\Fret");
            this.MatchBallTexture = Content.Load<Texture2D>(@"Images\PlayingSongScreen\MatchBall");
            this.FireTexture = Content.Load<Texture2D>(@"Images\PlayingSongScreen\Fire");
            this.ChordPictureBackground = Content.Load<Texture2D>(@"Images\PlayingSongScreen\ChordPictureBackground");
            this.BackgroundPlayingSong = Content.Load<Texture2D>(@"Images\PlayingSongScreen\BackgroundPlayingSong");

            this.EqualizerBackgroundTexture = Content.Load<Texture2D>(@"Images\Equalizer\background");
            this.EqualizerOnePointTexture = Content.Load<Texture2D>(@"Images\Equalizer\onepoint");

            this.BackgroundChooseSong = Content.Load<Texture2D>(@"Images\ChooseSongScreen\BackgroundChooseSong");

            this.BackgroundMenu_QuickPlay = Content.Load<Texture2D>(@"Images\MenuScreen\BackgroundMenu-QuickPlay");
            this.BackgroundMenu_Tune = Content.Load<Texture2D>(@"Images\MenuScreen\BackgroundMenu-Tune");
            this.BackgroundMenu_Quit = Content.Load<Texture2D>(@"Images\MenuScreen\BackgroundMenu-Quit");

            this.BackgroundMainScreen = Content.Load<Texture2D>(@"Images\MainScreen\BackgroundMainScreen");

            this.BackgroundTuneScreen = Content.Load<Texture2D>(@"Images\TuneScreen\BackgroundTune");
        }

        private void LoadFonts()
        {
            this.FontFretNumbers = Content.Load<SpriteFont>("FontFretNumbers");
            this.FontSongDescription = Content.Load<SpriteFont>("FontSongDescription");
            this.FontSmallTexts = Content.Load<SpriteFont>("FontSmallTexts");
            this.FontPlayingSongScreenScore = Content.Load<SpriteFont>("FontPlayingSongScreenScore");
            this.FontPlayingSongScreenSongTitle = Content.Load<SpriteFont>("FontPlayingSongScreenSongTitle");
            this.FontPlayingSongScreenSongSubtitle = Content.Load<SpriteFont>("FontPlayingSongScreenSongSubtitle");
        }

        #endregion

        /// <summary>
        /// UnloadContent will be called once per game and is the place to unload
        /// all content.
        /// </summary>
        protected override void UnloadContent()
        {
            // TODO: Unload any non ContentManager content here
        }

        /// <summary>
        /// Allows the game to run logic such as updating the world,
        /// checking for collisions, gathering input, and playing audio.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Update(GameTime pGameTime)
        {
            this.fUser1ActionListener.Update(Keyboard.GetState(), GamePad.GetState(PlayerIndex.One));

            //Alt+Enter => Alternate the full screen mode
            //TODO: This should be placed in GameController
            if ((Keyboard.GetState().IsKeyDown(Keys.LeftAlt)) && (this.fUser1ActionListener.IsKeyDownNow(Keys.Enter)))
            {
                GraphicsDeviceManager.IsFullScreen = !GraphicsDeviceManager.IsFullScreen;
                GraphicsDeviceManager.ApplyChanges();

                //Importante update again because if not the "Enter press" will be used again (changing the screen, for example)
                this.fUser1ActionListener.Update(Keyboard.GetState(), GamePad.GetState(PlayerIndex.One));
            }

            this.GameController.Update(this.fUser1ActionListener);

            this.HandleScreenChanges(this.GameController.CurrentScreen);


            TimeSpan totalTime = DateTime.Now.Subtract(this.GameStartTime);
            TimeSpan elapsedTime = DateTime.Now.Subtract(this.LastUpdateTime);

            this.LastUpdateTime = DateTime.Now;

            if (fCurrentScreen != null)
                fCurrentScreen.Update(totalTime, elapsedTime);
            //fCurrentScreen.Update(pGameTime.TotalGameTime, pGameTime.ElapsedGameTime);

            this.AudioEngine.Update();

            base.Update(pGameTime);

        }

        private void HandleScreenChanges(EnumGameScreen pNewGameScreen)
        {
            if (pNewGameScreen == this.fCurrentGameScreen)
                return;

            switch (pNewGameScreen)
            {
                case EnumGameScreen.Undefined:
                    break;

                case EnumGameScreen.Main:
                    fCurrentScreen = new GtMainScreen(this);
                    break;

                case EnumGameScreen.Menu:
                    fCurrentScreen = new GtMenuScreen(this);
                    break;

                case EnumGameScreen.Tune:
                    fCurrentScreen = new GtTuneScreen(this, this.GameController.TuneController);
                    break;

                case EnumGameScreen.ChooseSong:
                    fCurrentScreen = new GtChooseSongScreen(this);
                    break;

                case EnumGameScreen.PlayingSong:
                    fCurrentScreen = new GtPlayingSongScreen(this, this.GameController.GameRoundController);
                    break;

                case EnumGameScreen.Quit:
                    this.Exit();
                    break;

                default:
                    throw new Exception("Unexpected pNewGameScreen value.");
            }

            this.fCurrentGameScreen = pNewGameScreen;
        }


        /// <summary>
        /// This is called when the game should draw itself.
        /// </summary>
        /// <param name="gameTime">Provides a snapshot of timing values.</param>
        protected override void Draw(GameTime gameTime)
        {
            GraphicsDevice.Clear(Color.Black);

            if (fCurrentScreen != null)
                fCurrentScreen.Render();

            base.Draw(gameTime);
        }
    }
}
