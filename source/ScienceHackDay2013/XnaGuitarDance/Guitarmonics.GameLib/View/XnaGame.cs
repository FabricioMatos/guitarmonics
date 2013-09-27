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
        public int Width = 1152;
        public int Height = 720;

        //public int Width = 960;
        //public int Height = 600;


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

        public Texture2D Body { protected set; get; }

        public Texture2D HeadFront { protected set; get; }
        public Texture2D HeadLeft { protected set; get; }
        public Texture2D HeadRight { protected set; get; }
        public Texture2D HeadUp { protected set; get; }
        public Texture2D HeadDown { protected set; get; }

        public Texture2D LeftArmDown1 { protected set; get; }
        public Texture2D LeftArmUp1 { protected set; get; }
        public Texture2D LeftArmDown2 { protected set; get; }
        public Texture2D LeftArmUp2 { protected set; get; }
        public Texture2D LeftArmDown3 { protected set; get; }
        public Texture2D LeftArmUp3 { protected set; get; }

        public Texture2D RightArmDown1 { protected set; get; }
        public Texture2D RightArmUp1 { protected set; get; }
        public Texture2D RightArmDown2 { protected set; get; }
        public Texture2D RightArmUp2 { protected set; get; }
        public Texture2D RightArmDown3 { protected set; get; }
        public Texture2D RightArmUp3 { protected set; get; }

        public Texture2D LeftLegDown1 { protected set; get; }
        public Texture2D LeftLegUp1 { protected set; get; }
        public Texture2D LeftLegDown2 { protected set; get; }
        public Texture2D LeftLegUp2 { protected set; get; }

        public Texture2D RightLegDown1 { protected set; get; }
        public Texture2D RightLegUp1 { protected set; get; }
        public Texture2D RightLegDown2 { protected set; get; }
        public Texture2D RightLegUp2 { protected set; get; }


        public Texture2D antebraco { protected set; get; }
        public Texture2D antebraco_esq { protected set; get; }
        public Texture2D cabeca { protected set; get; }
        public Texture2D cabeca_3_quartos { protected set; get; }
        public Texture2D cabeca_alto { protected set; get; }
        public Texture2D cabeca_baixo { protected set; get; }
        public Texture2D cabeca_esquerda_alto { protected set; get; }
        public Texture2D cabeca_direita_alto { protected set; get; }
        public Texture2D cabeça_perfil { protected set; get; }
        public Texture2D cabecadelado_baixo { protected set; get; }
        public Texture2D canela_dir { protected set; get; }
        public Texture2D canela_esq { protected set; get; }
        public Texture2D coxa_dir { protected set; get; }
        public Texture2D coxa_esq { protected set; get; }
        public Texture2D mao_direita { protected set; get; }
        public Texture2D mao_esq { protected set; get; }
        public Texture2D ombro_dir { protected set; get; }
        public Texture2D ombro_esq { protected set; get; }
        public Texture2D pe_dir { protected set; get; }
        public Texture2D pe_esq { protected set; get; }
        public Texture2D tronco { protected set; get; }


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
            this.Window.Title = "Crazzie Non Sense";

            this.IsFixedTimeStep = true;

            this.GameController = pGameController;

            this.fUser1ActionListener = new GtUserActionsListener();

            GraphicsDeviceManager = new GraphicsDeviceManager(this);

            GraphicsDeviceManager.PreferredBackBufferWidth = this.Width;
            GraphicsDeviceManager.PreferredBackBufferHeight = this.Height;

            //GraphicsDeviceManager.IsFullScreen = true;
            GraphicsDeviceManager.IsFullScreen = false;

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
            #region Antigos
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

            #endregion

            this.Body = Content.Load<Texture2D>(@"Images\HappyGuy\Body");

            this.HeadFront = Content.Load<Texture2D>(@"Images\HappyGuy\HeadFront");
            this.HeadLeft = Content.Load<Texture2D>(@"Images\HappyGuy\HeadLeft");
            this.HeadRight = Content.Load<Texture2D>(@"Images\HappyGuy\HeadRight");
            this.HeadUp = Content.Load<Texture2D>(@"Images\HappyGuy\HeadUp");
            this.HeadDown = Content.Load<Texture2D>(@"Images\HappyGuy\HeadDown");

            this.LeftArmDown1 = Content.Load<Texture2D>(@"Images\HappyGuy\LeftArmDown1");
            this.LeftArmUp1 = Content.Load<Texture2D>(@"Images\HappyGuy\LeftArmUp1");
            this.LeftArmDown2 = Content.Load<Texture2D>(@"Images\HappyGuy\LeftArmDown2");
            this.LeftArmUp2 = Content.Load<Texture2D>(@"Images\HappyGuy\LeftArmUp2");
            this.LeftArmDown3 = Content.Load<Texture2D>(@"Images\HappyGuy\LeftArmDown3");
            this.LeftArmUp3 = Content.Load<Texture2D>(@"Images\HappyGuy\LeftArmUp3");

            this.RightArmDown1 = Content.Load<Texture2D>(@"Images\HappyGuy\RightArmDown1");
            this.RightArmUp1 = Content.Load<Texture2D>(@"Images\HappyGuy\RightArmUp1");
            this.RightArmDown2 = Content.Load<Texture2D>(@"Images\HappyGuy\RightArmDown2");
            this.RightArmUp2 = Content.Load<Texture2D>(@"Images\HappyGuy\RightArmUp2");
            this.RightArmDown3 = Content.Load<Texture2D>(@"Images\HappyGuy\RightArmDown3");
            this.RightArmUp3 = Content.Load<Texture2D>(@"Images\HappyGuy\RightArmUp3");

            this.LeftLegDown1 = Content.Load<Texture2D>(@"Images\HappyGuy\LeftLegDown1");
            this.LeftLegUp1 = Content.Load<Texture2D>(@"Images\HappyGuy\LeftLegUp1");
            this.LeftLegDown2 = Content.Load<Texture2D>(@"Images\HappyGuy\LeftLegDown2");
            this.LeftLegUp2 = Content.Load<Texture2D>(@"Images\HappyGuy\LeftLegUp2");

            this.RightLegDown1 = Content.Load<Texture2D>(@"Images\HappyGuy\RightLegDown1");
            this.RightLegUp1 = Content.Load<Texture2D>(@"Images\HappyGuy\RightLegUp1");
            this.RightLegDown2 = Content.Load<Texture2D>(@"Images\HappyGuy\RightLegDown2");
            this.RightLegUp2 = Content.Load<Texture2D>(@"Images\HappyGuy\RightLegUp2");


            this.antebraco = Content.Load<Texture2D>(@"Images\HappyGuyParts\antebraco");
            this.antebraco_esq = Content.Load<Texture2D>(@"Images\HappyGuyParts\antebraco_esq");
            this.cabeca = Content.Load<Texture2D>(@"Images\HappyGuyParts\cabeca");
            this.cabeca_3_quartos = Content.Load<Texture2D>(@"Images\HappyGuyParts\cabeca_3_quartos");
            this.cabeca_alto = Content.Load<Texture2D>(@"Images\HappyGuyParts\cabeca_alto");
            this.cabeca_baixo = Content.Load<Texture2D>(@"Images\HappyGuyParts\cabeca_baixo");
            this.cabeca_esquerda_alto = Content.Load<Texture2D>(@"Images\HappyGuyParts\cabeca_esquerda_alto");
            this.cabeca_direita_alto = Content.Load<Texture2D>(@"Images\HappyGuyParts\cabeca_direita_alto");
            this.cabeça_perfil = Content.Load<Texture2D>(@"Images\HappyGuyParts\cabeça_perfil");
            this.cabecadelado_baixo = Content.Load<Texture2D>(@"Images\HappyGuyParts\cabecadelado_baixo");
            this.canela_dir = Content.Load<Texture2D>(@"Images\HappyGuyParts\canela_dir");
            this.canela_esq = Content.Load<Texture2D>(@"Images\HappyGuyParts\canela_esq");
            this.coxa_dir = Content.Load<Texture2D>(@"Images\HappyGuyParts\coxa_dir");
            this.coxa_esq = Content.Load<Texture2D>(@"Images\HappyGuyParts\coxa_esq");
            this.mao_direita = Content.Load<Texture2D>(@"Images\HappyGuyParts\mao_direita");
            this.mao_esq = Content.Load<Texture2D>(@"Images\HappyGuyParts\mao_esq");
            this.ombro_dir = Content.Load<Texture2D>(@"Images\HappyGuyParts\ombro_dir");
            this.ombro_esq = Content.Load<Texture2D>(@"Images\HappyGuyParts\ombro_esq");
            this.pe_dir = Content.Load<Texture2D>(@"Images\HappyGuyParts\pe_dir");
            this.pe_esq = Content.Load<Texture2D>(@"Images\HappyGuyParts\pe_esq");
            this.tronco = Content.Load<Texture2D>(@"Images\HappyGuyParts\tronco");

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
