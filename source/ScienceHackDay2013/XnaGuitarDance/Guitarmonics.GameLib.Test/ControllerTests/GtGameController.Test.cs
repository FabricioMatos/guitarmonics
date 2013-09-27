using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib.Tests;
using Guitarmonics.GameLib;
using Guitarmonics.AudioLib.Analysis;
using NUnit.Framework;
using Microsoft.Xna.Framework.Input;
using Guitarmonics.GameLib.Controller;
using Guitarmonics.GameLib.Model;
using Guitarmonics.AudioLib.Player;
using Guitarmonics.GameLib.ViewTest;

namespace Guitarmonics.GameLib.ControllerTest
{
    [TestFixture]
    public class GtGameControllerAudioListenerIntegration : GtGameControllerTestBase
    {
        [Test]
        public void FftProperty()
        {
            using (var gameController = new GtGameController(
                Factory,
                new GtFileLoader(),
                new SongPlayerDoNothing(),
                new AudioEffectsDoNothing()))
            {
                Assert.IsNotNull(gameController.AudioListener.FftData);
            }
        }

        [Test]
        public void AudioEffectsProperty()
        {
            var audioEffects = new AudioEffectsDoNothing();


            using (var gameController = new GtGameController(
                Factory,
                new GtFileLoader(),
                new SongPlayerDoNothing(),
                audioEffects))
            {
                Assert.AreSame(audioEffects, gameController.AudioEffects);
            }
        }

        [Test]
        public void AudioEffectsParameterCantBeNull()
        {
            var e = Assert.Throws<InvalidParameter>(() =>
                new GtGameController(
                    Factory,
                    new GtFileLoader(),
                    new SongPlayerDoNothing(),
                    null));

            Assert.AreEqual("pAudioEffects can't be null", e.Message);
        }

    }

    [TestFixture]
    public class GtGameControllerScreenNavigationTests : GtGameControllerTestBase
    {
        [SetUp]
        public void SetUp()
        {
            Factory.AddMapping<IAudioListener, AudioListenerDoubleDoNothing>();
        }

        [TearDown]
        public void TearDown()
        {
            Factory.ClearAllMappings();
        }

        [Test]
        public void ConstructorSintaxe()
        {
            using (var gameController = new GtGameControllerDouble())
            {
                Assert.AreEqual(EnumGameScreen.Undefined, gameController.CurrentScreen);
                Assert.IsNotNull(gameController.AudioListener);
                Assert.IsNotNull(gameController.GameRoundController);
                Assert.IsNotNull(gameController.TuneController);
            }
        }


        #region Screen Navigation

        [Test]
        public void MainScreen()
        {
            using (var gameController = new GtGameControllerDouble())
            {
                gameController.Update(new GtUserActionsListener());

                Assert.AreEqual(EnumGameScreen.Main, gameController.CurrentScreen);
            }
        }


        [TestCase(EnumGameScreen.Main)]
        [TestCase(EnumGameScreen.Menu)]
        [TestCase(EnumGameScreen.ChooseSong)]
        [TestCase(EnumGameScreen.PlayingSong)]
        public void NoKeyOrButton_NoScreenChange(EnumGameScreen pGameScreen)
        {
            KeyboardState keyboardState = new KeyboardState();
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);


            using (var gameController = new GtGameControllerDouble(pGameScreen))
            {
                gameController.Update(user1ActionListener);

                Assert.AreEqual(pGameScreen, gameController.CurrentScreen);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //Main [Enter key] => Menu
        [TestCase(EnumGameScreen.Main, EnumMenuScreenItems.Undefined, Keys.Enter, EnumGameScreen.Menu, EnumMenuScreenItems.QuickPlay)]
        //Menu.QuickPlay [Enter key] => Choose Song
        [TestCase(EnumGameScreen.Menu, EnumMenuScreenItems.QuickPlay, Keys.Enter, EnumGameScreen.ChooseSong, EnumMenuScreenItems.QuickPlay)]
        //Choose Song [Esc key] => Menu.QuickPlay
        [TestCase(EnumGameScreen.ChooseSong, EnumMenuScreenItems.QuickPlay, Keys.Escape, EnumGameScreen.Menu, EnumMenuScreenItems.QuickPlay)]
        //Menu.Tune [Enter key] => Tune
        [TestCase(EnumGameScreen.Menu, EnumMenuScreenItems.Tune, Keys.Enter, EnumGameScreen.Tune, EnumMenuScreenItems.Tune)]
        //Tune [Esc key] => Menu.Tune
        [TestCase(EnumGameScreen.Tune, EnumMenuScreenItems.Tune, Keys.Escape, EnumGameScreen.Menu, EnumMenuScreenItems.Tune)]
        //Menu.Quit [A button] => Quit
        [TestCase(EnumGameScreen.Menu, EnumMenuScreenItems.Quit, Keys.Enter, EnumGameScreen.Quit, EnumMenuScreenItems.Quit)]
        public void ScreenChangesUsingKeyboard(EnumGameScreen pCurrentScreen, EnumMenuScreenItems pMenuItem,
            Keys pKey, EnumGameScreen pExpectedScreen, EnumMenuScreenItems pExpectedMenuItem)
        {
            KeyboardState keyboardState = new KeyboardState(pKey);
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            //this update will be called by the XNA Game class.
            using (var gameController = new GtGameControllerDouble(pCurrentScreen, pMenuItem))
            {
                gameController.Update(user1ActionListener);

                Assert.AreEqual(pExpectedScreen, gameController.CurrentScreen);
                Assert.AreEqual(pExpectedMenuItem, gameController.MenuScreenSelectedItem);
            }
        }

        /// <summary>
        /// 
        /// </summary>
        //Main [start button] => Menu
        [TestCase(EnumGameScreen.Main, EnumMenuScreenItems.Undefined, Buttons.Start, EnumGameScreen.Menu, EnumMenuScreenItems.QuickPlay)]
        //Menu.QuickPlay [A button] => Chose Song
        [TestCase(EnumGameScreen.Menu, EnumMenuScreenItems.QuickPlay, Buttons.A, EnumGameScreen.ChooseSong, EnumMenuScreenItems.QuickPlay)]
        //Choose song [B button] => Menu.QuickPlay
        [TestCase(EnumGameScreen.ChooseSong, EnumMenuScreenItems.QuickPlay, Buttons.B, EnumGameScreen.Menu, EnumMenuScreenItems.QuickPlay)]
        //Menu.Tune [A button] => Tune
        [TestCase(EnumGameScreen.Menu, EnumMenuScreenItems.Tune, Buttons.A, EnumGameScreen.Tune, EnumMenuScreenItems.Tune)]
        //Tune [B button] => Menu.Tune
        [TestCase(EnumGameScreen.Tune, EnumMenuScreenItems.Tune, Buttons.B, EnumGameScreen.Menu, EnumMenuScreenItems.Tune)]
        //Menu.Quit [A button] => Quit
        [TestCase(EnumGameScreen.Menu, EnumMenuScreenItems.Quit, Buttons.A, EnumGameScreen.Quit, EnumMenuScreenItems.Quit)]
        public void ScreenChangesUsingGamePad(EnumGameScreen pCurrentScreen, EnumMenuScreenItems pMenuItem,
            Buttons pButton, EnumGameScreen pExpectedScreen, EnumMenuScreenItems pExpectedMenuItem)
        {
            KeyboardState keyboardState = new KeyboardState();
            GamePadState gamePadState = new GamePadState(
                new GamePadThumbSticks(),
                new GamePadTriggers(),
                new GamePadButtons(pButton),
                new GamePadDPad());

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            //this update will be called by the XNA Game class.
            using (var gameController = new GtGameControllerDouble(pCurrentScreen, pMenuItem))
            {
                gameController.Update(user1ActionListener);

                Assert.AreEqual(pExpectedScreen, gameController.CurrentScreen);
                Assert.AreEqual(pExpectedMenuItem, gameController.MenuScreenSelectedItem);
            }
        }


        /// <summary>
        /// When we press Enter and keep it pressed, each screen was considering
        /// that the enter was pressed. We want in this case that only the first
        /// screen do it.
        /// </summary>
        [Test]
        public void EnterKeyIsResetedAfterUsed()
        {
            KeyboardState keyboardState = new KeyboardState(Keys.Enter);
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();

            //this update will be called by the XNA Game class.
            var gameController = new GtGameControllerDouble(new GtFileLoaderDouble6(), EnumGameScreen.Main);

            user1ActionListener.Update(keyboardState, gamePadState);
            gameController.Update(user1ActionListener);

            Assert.AreEqual(EnumGameScreen.Menu, gameController.CurrentScreen);

            user1ActionListener.Update(keyboardState, gamePadState);
            gameController.Update(user1ActionListener); //here the Enter must be discarted

            Assert.AreEqual(EnumGameScreen.Menu, gameController.CurrentScreen);
        }


        #endregion

        #region Select Song (ChooseSongScreen)

        [Test]
        public void CheckTheSongListInSongScreen()
        {
            using (var gameController = new GtGameControllerDouble(new GtFileLoaderDouble6(), EnumGameScreen.ChooseSong))
            {
                gameController.LoadSongList();

                Assert.AreEqual(6, gameController.SongList.Count);

                Assert.AreEqual("For whom the bell tolls", gameController.SongList[0].Song);
                Assert.AreEqual("Metallica", gameController.SongList[0].Artist);

                Assert.AreEqual("I use to love her", gameController.SongList[1].Song);
                Assert.AreEqual("Guns N'Roses", gameController.SongList[1].Artist);

                Assert.AreEqual("Money for nothing", gameController.SongList[2].Song);
                Assert.AreEqual("Dire Straits", gameController.SongList[2].Artist);

                Assert.AreEqual("Wish you were here", gameController.SongList[3].Song);
                Assert.AreEqual("Pink Floyd", gameController.SongList[3].Artist);

                Assert.AreEqual("Fade to Black", gameController.SongList[4].Song);
                Assert.AreEqual("Metallica", gameController.SongList[4].Artist);

                Assert.AreEqual("Seek & Destroy", gameController.SongList[5].Song);
                Assert.AreEqual("Metallica", gameController.SongList[5].Artist);
            }
        }

        [Test]
        public void SongDescriptionProperties()
        {
            using (var gameController = new GtGameControllerDouble(new GtFileLoaderDouble6(), EnumGameScreen.ChooseSong))
            {
                gameController.LoadSongList();

                Assert.AreEqual("For whom the bell tolls", gameController.SongList[0].Song);
                Assert.AreEqual("Metallica", gameController.SongList[0].Artist);
                Assert.AreEqual(TestConfig.AudioPath + "metallica-for_whom_the_bell_tolls(linked).song.xml",
                                gameController.SongList[0].ConfigFileName);
                Assert.AreEqual(TestConfig.AudioPath + "metallica-for_whom_the_bell_tolls.mp3",
                                gameController.SongList[0].AudioFileName);
                //Assert.AreEqual(118.4, gameController.SongList[0].TempoBPM);
                Assert.AreEqual(GtTimeSignature.Time4x4, gameController.SongList[0].TimeSignature);
            }
        }

        [Test]
        public void InitiallyTheFirstSongIsSelected()
        {
            using (var gameController = new GtGameControllerDouble(new GtFileLoaderDouble6(), EnumGameScreen.ChooseSong))
            {
                gameController.LoadSongList();

                Assert.AreSame(gameController.SongList[0], gameController.SelectedSong);
            }
        }

        [Test]
        public void SelectTheThirdSong_Keyboard()
        {
            KeyboardState keyboardState = new KeyboardState(Keys.Down);
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            //this update will be called by the XNA Game class.
            using (var gameController = new GtGameControllerDouble(new GtFileLoaderDouble6(), EnumGameScreen.ChooseSong))
            {
                gameController.LoadSongList();

                Assert.AreSame(gameController.SongList[0], gameController.SelectedSong);

                gameController.Update(user1ActionListener);
                gameController.Update(user1ActionListener);

                Assert.AreSame(gameController.SongList[2], gameController.SelectedSong);
            }
        }

        [Test]
        public void SelectTheThirdSong_Keyboard_WithBack()
        {
            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(new KeyboardState(Keys.Down), new GamePadState());

            //this update will be called by the XNA Game class.
            using (var gameController = new GtGameControllerDouble(new GtFileLoaderDouble6(), EnumGameScreen.ChooseSong))
            {
                gameController.LoadSongList();

                Assert.AreSame(gameController.SongList[0], gameController.SelectedSong);

                gameController.Update(user1ActionListener);
                gameController.Update(user1ActionListener);
                gameController.Update(user1ActionListener);

                user1ActionListener.Update(new KeyboardState(Keys.Up), new GamePadState());
                gameController.Update(user1ActionListener);

                Assert.AreSame(gameController.SongList[2], gameController.SelectedSong);
            }
        }


        [Test]
        public void VisibleSongsWindow_FirstSelected()
        {
            using (var gameController = new GtGameControllerDouble(new GtFileLoaderDouble100(), EnumGameScreen.ChooseSong))
            {
                gameController.LoadSongList();

                Assert.AreSame(gameController.SongList[0], gameController.SelectedSong);

                Assert.AreEqual(GtGameController.VISIBLE_SONG_WINDOW_SIZE, gameController.VisibleSongs.Count);

                for (int i = 0; i < GtGameController.VISIBLE_SONG_WINDOW_SIZE; i++)
                {
                    Assert.AreSame(gameController.SongList[i], gameController.VisibleSongs[i]);
                }
            }
        }

        [Test]
        public void VisibleSongsWindow_HalfSelected()
        {
            KeyboardState keyboardState = new KeyboardState(Keys.Down);
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            using (var gameController = new GtGameControllerDouble(new GtFileLoaderDouble100(), EnumGameScreen.ChooseSong))
            {
                gameController.LoadSongList();

                for (int i = 0; i < (GtGameController.VISIBLE_SONG_WINDOW_SIZE / 2); i++)
                {
                    gameController.Update(user1ActionListener);
                }

                Assert.AreSame(gameController.SongList[GtGameController.VISIBLE_SONG_WINDOW_SIZE / 2],
                               gameController.SelectedSong);

                Assert.AreEqual(GtGameController.VISIBLE_SONG_WINDOW_SIZE, gameController.VisibleSongs.Count);

                for (int i = 0; i < GtGameController.VISIBLE_SONG_WINDOW_SIZE; i++)
                {
                    Assert.AreSame(gameController.SongList[i], gameController.VisibleSongs[i]);
                }
            }
        }

        [Test]
        public void VisibleSongsWindow_HalfSelectedPlusOne()
        {
            KeyboardState keyboardState = new KeyboardState(Keys.Down);
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            using (var gameController = new GtGameControllerDouble(new GtFileLoaderDouble100(), EnumGameScreen.ChooseSong))
            {
                gameController.LoadSongList();

                for (int i = 0; i < (GtGameController.VISIBLE_SONG_WINDOW_SIZE / 2); i++)
                {
                    gameController.Update(user1ActionListener);
                }
                gameController.Update(user1ActionListener);

                Assert.AreSame(gameController.SongList[(GtGameController.VISIBLE_SONG_WINDOW_SIZE / 2) + 1],
                               gameController.SelectedSong);

                Assert.AreEqual(GtGameController.VISIBLE_SONG_WINDOW_SIZE, gameController.VisibleSongs.Count);

                for (int i = 0; i < GtGameController.VISIBLE_SONG_WINDOW_SIZE; i++)
                {
                    Assert.AreSame(gameController.SongList[i + 1], gameController.VisibleSongs[i]);
                }
            }
        }

        [Test]
        public void VisibleSongsWindow_LastSelected()
        {
            KeyboardState keyboardState = new KeyboardState(Keys.Down);
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            using (var gameController = new GtGameControllerDouble(new GtFileLoaderDouble100(), EnumGameScreen.ChooseSong))
            {

                gameController.LoadSongList();

                for (int i = 0; i < gameController.SongList.Count - 1; i++)
                {
                    gameController.Update(user1ActionListener);
                }

                Assert.AreSame(gameController.SongList[gameController.SongList.Count - 1],
                               gameController.SelectedSong);

                Assert.AreEqual(GtGameController.VISIBLE_SONG_WINDOW_SIZE, gameController.VisibleSongs.Count);

                for (int i = 0; i < GtGameController.VISIBLE_SONG_WINDOW_SIZE; i++)
                {
                    Assert.AreSame(
                        gameController.SongList[
                            gameController.SongList.Count - GtGameController.VISIBLE_SONG_WINDOW_SIZE + i],
                        gameController.VisibleSongs[i]);
                }
            }
        }

        [Test]
        public void SelectTheThirdSong_GamePad()
        {
            KeyboardState keyboardState = new KeyboardState();
            GamePadState gamePadState = new GamePadState(
                new GamePadThumbSticks(),
                new GamePadTriggers(),
                new GamePadButtons(),
                new GamePadDPad(ButtonState.Released, ButtonState.Pressed, ButtonState.Released, ButtonState.Released));

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            //this update will be called by the XNA Game class.
            var gameController = new GtGameControllerDouble(new GtFileLoaderDouble6(), EnumGameScreen.ChooseSong);

            gameController.LoadSongList();

            Assert.AreSame(gameController.SongList[0], gameController.SelectedSong);

            gameController.Update(user1ActionListener);
            gameController.Update(user1ActionListener);

            Assert.AreSame(gameController.SongList[2], gameController.SelectedSong);
        }

        [Test]
        public void GoToPlayingSongScreenForTheSelectedSong_Keyboard()
        {
            var gameController = new GtGameControllerDouble(new GtFileLoaderDouble6(), EnumGameScreen.ChooseSong);

            gameController.LoadSongList();

            KeyboardState keyboardState = new KeyboardState(Keys.Enter);
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            gameController.Update(user1ActionListener);

            Assert.AreEqual(EnumGameScreen.PlayingSong, gameController.CurrentScreen);
        }

        [Test]
        public void GoToPlayingSongScreen_NoSelectedSong()
        {
            var gameController = new GtGameControllerDouble(new GtFileLoaderDouble0(), EnumGameScreen.ChooseSong);
            gameController.LoadSongList();

            KeyboardState keyboardState = new KeyboardState(Keys.Enter);
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            gameController.Update(user1ActionListener);

            Assert.AreEqual(EnumGameScreen.ChooseSong, gameController.CurrentScreen);
        }

        [Test]
        public void GoToPlayingSongScreenForTheSelectedSong_GamePad()
        {
            var gameController = new GtGameControllerDouble(new GtFileLoaderDouble6(), EnumGameScreen.ChooseSong);

            gameController.LoadSongList();

            KeyboardState keyboardState = new KeyboardState();
            GamePadState gamePadState = new GamePadState(
                new GamePadThumbSticks(),
                new GamePadTriggers(),
                new GamePadButtons(Buttons.A),
                new GamePadDPad());

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            gameController.Update(user1ActionListener);

            Assert.AreEqual(EnumGameScreen.PlayingSong, gameController.CurrentScreen);
            Assert.AreEqual(EnumGameRoundState.Playing, gameController.GameRoundController.GameRoundState);


        }

        #endregion

        #region Menu Selection

        [Test]
        public void MenuSelection_QuickPlayIsTheDefault()
        {
            KeyboardState keyboardState = new KeyboardState(Keys.Enter);
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            //this update will be called by the XNA Game class.
            using (var gameController = new GtGameControllerDouble(EnumGameScreen.Main))
            {
                gameController.Update(user1ActionListener);

                Assert.AreEqual(EnumGameScreen.Menu, gameController.CurrentScreen);
                Assert.AreEqual(EnumMenuScreenItems.QuickPlay, gameController.MenuScreenSelectedItem);
            }
        }


        [TestCase(Keys.Down, EnumMenuScreenItems.QuickPlay, EnumMenuScreenItems.Tune)]
        [TestCase(Keys.Down, EnumMenuScreenItems.Tune, EnumMenuScreenItems.Quit)]
        [TestCase(Keys.Down, EnumMenuScreenItems.Quit, EnumMenuScreenItems.Quit)]
        [TestCase(Keys.Up, EnumMenuScreenItems.Quit, EnumMenuScreenItems.Tune)]
        [TestCase(Keys.Up, EnumMenuScreenItems.Tune, EnumMenuScreenItems.QuickPlay)]
        [TestCase(Keys.Up, EnumMenuScreenItems.QuickPlay, EnumMenuScreenItems.QuickPlay)]
        public void MenuSelection_SecondItemIsTune(Keys pKey, EnumMenuScreenItems pCurrentItem, EnumMenuScreenItems pExpectedItem)
        {
            var keyboardState = new KeyboardState(pKey);
            var gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            //this update will be called by the XNA Game class.
            using (var gameController = new GtGameControllerDouble(EnumGameScreen.Menu, pCurrentItem))
            {
                gameController.Update(user1ActionListener);

                Assert.AreEqual(EnumGameScreen.Menu, gameController.CurrentScreen);
                Assert.AreEqual(pExpectedItem, gameController.MenuScreenSelectedItem);
            }
        }


        #endregion

        #region Not Implemented

        [Test]
        public void PausePlayingSong_Keyboard()
        {
            var gameController = new GtGameControllerDouble(new GtFileLoaderDouble6(), EnumGameScreen.PlayingSong);
            gameController.GameRoundController.ForceGameRoundState(EnumGameRoundState.Playing);

            KeyboardState keyboardState = new KeyboardState(Keys.Space);
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            gameController.Update(user1ActionListener);

            Assert.AreEqual(EnumGameScreen.PlayingSong, gameController.CurrentScreen);
            Assert.AreEqual(EnumGameRoundState.Paused, gameController.GameRoundController.GameRoundState);
        }

        [Test]
        public void ResumePausedSong_Keyboard()
        {
            var gameController = new GtGameControllerDouble(new GtFileLoaderDouble6(), EnumGameScreen.PlayingSong);
            gameController.GameRoundController.ForceGameRoundState(EnumGameRoundState.Paused);

            KeyboardState keyboardState = new KeyboardState(Keys.Space);
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            gameController.Update(user1ActionListener);

            Assert.AreEqual(EnumGameScreen.PlayingSong, gameController.CurrentScreen);
            Assert.AreEqual(EnumGameRoundState.Playing, gameController.GameRoundController.GameRoundState);
        }

        [Test]
        public void FinishedToReplay_Keyboard()
        {
            var gameController = new GtGameControllerDouble(new GtFileLoaderDouble6(), EnumGameScreen.PlayingSong);
            gameController.GameRoundController.ForceGameRoundState(EnumGameRoundState.Finished);

            KeyboardState keyboardState = new KeyboardState(Keys.Space);
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            gameController.Update(user1ActionListener);

            Assert.AreEqual(EnumGameScreen.PlayingSong, gameController.CurrentScreen);
            Assert.AreEqual(EnumGameRoundState.Playing, gameController.GameRoundController.GameRoundState);
        }


        [Test]
        public void FinishedToMenu_Keyboard()
        {
            var gameController = new GtGameControllerDouble(new GtFileLoaderDouble6(), EnumGameScreen.PlayingSong);
            gameController.GameRoundController.ForceGameRoundState(EnumGameRoundState.Finished);

            KeyboardState keyboardState = new KeyboardState(Keys.Escape);
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            gameController.Update(user1ActionListener);

            Assert.AreEqual(EnumGameScreen.ChooseSong, gameController.CurrentScreen);
        }

        [Test]
        public void PlayingToEscaping_Keyboard()
        {
            var gameController = new GtGameControllerDouble(new GtFileLoaderDouble6(), EnumGameScreen.PlayingSong);
            gameController.GameRoundController.ForceGameRoundState(EnumGameRoundState.Playing);

            KeyboardState keyboardState = new KeyboardState(Keys.Escape);
            GamePadState gamePadState = new GamePadState();

            var user1ActionListener = new GtUserActionsListener();
            user1ActionListener.Update(keyboardState, gamePadState);

            gameController.Update(user1ActionListener);

            Assert.AreEqual(EnumGameScreen.PlayingSong, gameController.CurrentScreen);
            Assert.AreEqual(EnumGameRoundState.Finished, gameController.GameRoundController.GameRoundState);
        }

        //[Test]
        public void FromPausedToChooseSongScreen()
        {
            throw new NotImplementedException();
        }

        /// <summary>
        /// when some music finish and we need to show the player score
        /// </summary>
        //[Test]
        public void SongEndScreen()
        {
            throw new NotImplementedException();
        }

        #endregion

        [Ignore]
        [Test]
        public void ContinuarAqui()
        {
            Assert.Fail("todo: Tune screen not implemented");
        }

        /* TEST LIST
         * 
         * - Implementar as novas telas e os fluxos de navegacao entre elas
         * - No momento da selecao do grau de dificuldade de uma musica, setar o ConfigFileName correto
         * - Implementar o mecanismo de cache (copia do xml para a pasta DataFolder\Songs)
         * - Implementar o afinador
         */
    }

    public class GtGameControllerTestBase
    {
        public static GtFactory Factory = new GtFactory();
        //public static Rhino.Mocks.MockRepository = new Rhino.Mocks.MockRepository();

    }

    #region Double classes (for test)

    public class GtGameControllerDouble : GtGameController
    {
        public GtGameControllerDouble()
            : base(GtGameControllerScreenNavigationTests.Factory, new GtFileLoader(), new SongPlayerDoNothing(), new AudioEffectsDoNothing())
        {
        }

        public GtGameControllerDouble(EnumGameScreen pInitialGameScreen)
            : base(GtGameControllerScreenNavigationTests.Factory, new GtFileLoader(), new SongPlayerDoNothing(), new AudioEffectsDoNothing())
        {
            this.CurrentScreen = pInitialGameScreen;

            if (pInitialGameScreen == EnumGameScreen.Menu)
            {
                this.MenuScreenSelectedItem = EnumMenuScreenItems.QuickPlay;
            }
        }


        public GtGameControllerDouble(EnumGameScreen pInitialGameScreen, EnumMenuScreenItems pMenuScreenSelectItem)
            : base(GtGameControllerScreenNavigationTests.Factory, new GtFileLoader(), new SongPlayerDoNothing(), new AudioEffectsDoNothing())
        {
            this.CurrentScreen = pInitialGameScreen;

            if (pInitialGameScreen == EnumGameScreen.Menu)
            {
                this.MenuScreenSelectedItem = pMenuScreenSelectItem;
            }
        }


        public GtGameControllerDouble(GtFileLoader pFileLoader, EnumGameScreen pInitialGameScreen)
            : base(GtGameControllerScreenNavigationTests.Factory, pFileLoader, new SongPlayerDoNothing(), new AudioEffectsDoNothing())
        {
            this.CurrentScreen = pInitialGameScreen;

            if (pInitialGameScreen == EnumGameScreen.Menu)
            {
                this.MenuScreenSelectedItem = EnumMenuScreenItems.QuickPlay;
            }
        }
    }

    public class GtFileLoaderDouble6 : GtFileLoader
    {
        public GtFileLoaderDouble6()
        {
            fAllSongs = Generate6DoubleSongs();
        }

        private IList<SongDescription> Generate6DoubleSongs()
        {
            var list = new List<SongDescription>();

            list.Add(new SongDescription()
                {
                    Song = "For whom the bell tolls",
                    Artist = "Metallica",
                    ConfigFileName = TestConfig.AudioPath + "metallica-for_whom_the_bell_tolls(linked).song.xml",
                    SyncFileName = TestConfig.AudioPath + "metallica-for_whom_the_bell_tolls(linked).song.xml",
                    AudioFileName = TestConfig.AudioPath + "metallica-for_whom_the_bell_tolls.mp3",
                    TimeSignature = GtTimeSignature.Time4x4,
                });
            list.Add(new SongDescription()
                {
                    Song = "I use to love her",
                    Artist = "Guns N'Roses",
                    ConfigFileName = TestConfig.AudioPath + "XXX.song.xml",
                    SyncFileName = TestConfig.AudioPath + "XXX.song.xml",
                    AudioFileName = TestConfig.AudioPath + "XXX.mp3",
                    TimeSignature = GtTimeSignature.Time4x4,
                });
            list.Add(new SongDescription()
                {
                    Song = "Money for nothing",
                    Artist = "Dire Straits",
                    ConfigFileName = TestConfig.AudioPath + "XXX.song.xml",
                    SyncFileName = TestConfig.AudioPath + "XXX.song.xml",
                    AudioFileName = TestConfig.AudioPath + "XXX.mp3",
                    TimeSignature = GtTimeSignature.Time4x4,
                });
            list.Add(new SongDescription()
                {
                    Song = "Wish you were here",
                    Artist = "Pink Floyd",
                    ConfigFileName = TestConfig.AudioPath + "XXX.song.xml",
                    SyncFileName = TestConfig.AudioPath + "XXX.song.xml",
                    AudioFileName = TestConfig.AudioPath + "XXX.mp3",
                    TimeSignature = GtTimeSignature.Time4x4,
                });
            list.Add(new SongDescription()
                {
                    Song = "Fade to Black",
                    Artist = "Metallica",
                    ConfigFileName = TestConfig.AudioPath + "XXX.song.xml",
                    SyncFileName = TestConfig.AudioPath + "XXX.song.xml",
                    AudioFileName = TestConfig.AudioPath + "XXX.mp3",
                    TimeSignature = GtTimeSignature.Time4x4,
                });
            list.Add(new SongDescription()
                {
                    Song = "Seek & Destroy",
                    Artist = "Metallica",
                    ConfigFileName = TestConfig.AudioPath + "XXX.song.xml",
                    SyncFileName = TestConfig.AudioPath + "XXX.song.xml",
                    AudioFileName = TestConfig.AudioPath + "XXX.mp3",
                    TimeSignature = GtTimeSignature.Time4x4,
                });

            return list;
        }

        private IList<SongDescription> fAllSongs;

        public override IList<SongDescription> ListAllSongs()
        {
            return fAllSongs;
        }

        public override void DownloadSong(SongDescription pSelectedSong)
        {
        }
    }

    public class GtFileLoaderDouble6_FakeTickDataTable : GtFileLoaderDouble6
    {
        public override GtTickDataTable LoadTickDataTable(ref SongDescription pSongDescription)
        {
            return new Double_GtTickDataTable_3200Notes(); //fake it!
        }
    }

    public class GtFileLoaderDoubleChromaticScale_FakeTickDataTable : GtFileLoader
    {
        public GtFileLoaderDoubleChromaticScale_FakeTickDataTable()
        {
            fAllSongs = GenerateDoubleSongs();
        }

        private IList<SongDescription> GenerateDoubleSongs()
        {
            var list = new List<SongDescription>();

            list.Add(new SongDescription()
            {
                Song = "Simple Chromaic Scale",
                Artist = "Fabricio Matos",
                //ConfigFileName = TestConfig.AudioPath + "metallica-for_whom_the_bell_tolls.song.xml",
                ConfigFileName = string.Empty,
                AudioFileName = string.Empty,
                TimeSignature = GtTimeSignature.Time4x4,
            });

            return list;
        }

        private IList<SongDescription> fAllSongs;

        public override IList<SongDescription> ListAllSongs()
        {
            return fAllSongs;
        }

        public override GtTickDataTable LoadTickDataTable(ref SongDescription pSongDescription)
        {
            return new Double_GtTickDataTable_ChromaticScale(); //fake it!
        }
    }

    public class GtFileLoaderDouble100 : GtFileLoader
    {
        public GtFileLoaderDouble100()
        {
            fAllSongs = Generate100DoubleSongs();
        }

        private IList<SongDescription> Generate100DoubleSongs()
        {
            var list = new List<SongDescription>();

            for (int i = 0; i < 100; i++)
            {
                list.Add(new SongDescription()
                {
                    Song = "Song Double For Test #" + i.ToString(),
                    Artist = "Artist Name",
                });
            }

            return list;
        }

        private IList<SongDescription> fAllSongs;

        public override IList<SongDescription> ListAllSongs()
        {
            return fAllSongs;
        }

    }

    public class GtFileLoaderDouble0 : GtFileLoader
    {
        public GtFileLoaderDouble0()
        {
            fAllSongs = Generate6DoubleSongs();
        }

        private IList<SongDescription> Generate6DoubleSongs()
        {
            return new List<SongDescription>();
        }

        private IList<SongDescription> fAllSongs;

        public override IList<SongDescription> ListAllSongs()
        {
            return fAllSongs;
        }

    }

    public class SongPlayerDoNothing : ISongPlayer
    {
        public SongPlayerDoNothing()
        {
        }

        #region ISongPlayer Members

        public long CurrentPosition
        {
            get { return 0; }
        }

        public double CurrentPositionAsSeconds
        {
            get { return 0; }
        }

        public double DurationAsSeconds
        {
            get { return 0; }
        }

        public long Length
        {
            get { return 0; }
        }

        public void LoadStream()
        {

        }

        public void Pause()
        {

        }

        public void Play()
        {
        }


        public void Play(int velocity)
        {
        }

        public SongPlayerStatus Status
        {
            get { return SongPlayerStatus.NotInitialized; }
        }

        public void Stop()
        {

        }

        public double TickDurationAsSeconds
        {
            get { return 0; }
        }

        public GtTimeSignature TimeSignature
        {
            get { return GtTimeSignature.Time4x4; }
        }

        public TickNotifyEvent TickNotifyEvent
        {
            get
            {
                return null;
            }
            set
            {

            }
        }

        public void SetupSong(string pFileName, GtTimeSignature pTimeSignature)
        {

        }

        public void ChangeVelocity(int newVelocity)
        {
        }

        public void Dispose()
        {
        }

        #endregion


        public void Play(int velocity, float pitch)
        {
        }
    }

    public class AudioListenerDoubleDoNothing : IAudioListener
    {

        public AudioListenerDoubleDoNothing(int pFrequence)
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
            get { throw new NotImplementedException(); }
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

    public class AudioEffectsDoNothing : IAudioEffects
    {
        public AudioEffectsDoNothing()
        {
        }

        public void PlaySingleHit()
        {
        }

        public void PlayDoubleHit()
        {
        }

        public void PlaySongTheme()
        {
        }

        public void StopSongTheme()
        {
        }
    }


    #endregion

}
