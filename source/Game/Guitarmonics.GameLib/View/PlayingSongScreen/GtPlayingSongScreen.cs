using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Analysis;
using Guitarmonics.AudioLib.Common;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Guitarmonics.GameLib.Controller;
using Microsoft.Xna.Framework.Input;
using Guitarmonics.GameLib.View;
using Guitarmonics.AudioLib.Player;
using Guitarmonics.GameLib.Model;

namespace Guitarmonics.GameLib.View
{
    //TODO: GtPlayingSongScreen isn't well covered by unit tests

    public class GtPlayingSongScreen : GtScreenBase
    {
        private IGtGameRoundController fGameRoundController;
        private string GameTitleTest = "";
        private GtEqualizer fEqualizer;
        private GtNotesLegend fGtNotesLegend;

        //private int FrameRate = 0;
        //private int FrameCount = 0;
        private TimeSpan ElapsedTime = TimeSpan.FromSeconds(0);

        public GtPlayingSongScreen(XnaGame pGame, IGtGameRoundController pGameRoundController)
            : base(pGame)
        {
            if (pGameRoundController == null)
                throw new Exception("pGameController can't be null");

            this.fGameRoundController = pGameRoundController;

            this.fEqualizer = new GtEqualizer(pGame, 60, 300);

            this.fGtNotesLegend = new GtNotesLegend(pGame);
        }

        public override void Update(TimeSpan pTotalTime, TimeSpan pElapsedTime)
        {
            //test: calculate frame rate
            ElapsedTime += pElapsedTime;
            if (ElapsedTime > TimeSpan.FromSeconds(1))
            {
                //FrameRate = FrameCount;
                //FrameCount = 0;

                ElapsedTime = TimeSpan.FromSeconds(0);
            }

            base.Update(pTotalTime, pElapsedTime);

            this.fGameRoundController.UpdateProgress(pTotalTime);

            this.fEqualizer.Update(this.fGame.GameController.AudioListener.FftData);

            this.fGame.Window.Title = GameTitleTest;
        }

        public override void Render()
        {
            base.Render();

            this.SpriteBatch.Begin();

            this.SpriteBatch.Draw(
                this.fGame.BackgroundPlayingSong,
                new Rectangle(0, 0, fGame.Window.ClientBounds.Width, fGame.Window.ClientBounds.Height),
                Color.White);


            RenderGuitars(this.SpriteBatch);

            SpriteBatch.DrawString(this.fGame.FontPlayingSongScreenSongTitle,
                this.fGameRoundController.Song, new Vector2(15, 25), Color.White);

            SpriteBatch.DrawString(this.fGame.FontPlayingSongScreenSongSubtitle,
                this.fGameRoundController.Artist, new Vector2(15, 75), Color.White);

            SpriteBatch.DrawString(this.fGame.FontPlayingSongScreenSongSubtitle,
                this.fGameRoundController.Album, new Vector2(15, 100), Color.White);

            this.fEqualizer.Render(this.SpriteBatch);

            this.fGtNotesLegend.Render(this.SpriteBatch, this.ScreenWidth - 240, 220);

            //draw the FrameRate
            //FrameCount++;
            //SpriteBatch.DrawString(this.fGame.FontSmallTexts,
            //    "Frame Rate: " + this.FrameRate.ToString(), new Vector2(600, 20), Color.Gray);


            //SCORE
            SpriteBatch.DrawString(this.fGame.FontPlayingSongScreenScore,
                string.Format("{0:000000}", this.fGameRoundController.SceneGuitars[0].Points),
                new Vector2(this.ScreenWidth - 250, 60), Color.White);


            //% RIGHT
            var percentRight = 0;
            if (this.fGameRoundController.SceneGuitars[0].Points > 0)
                percentRight = (100 * this.fGameRoundController.SceneGuitars[0].Points) / this.fGameRoundController.SceneGuitars[0].MaxPoints;

            SpriteBatch.DrawString(this.fGame.FontPlayingSongScreenScore,
                string.Format("{0}%", percentRight),
                new Vector2(this.ScreenWidth - 200, 120), Color.White);


            //draw current position (beat:tick)
            if (this.fGameRoundController.SceneGuitars[0].CurrentPosition.Beat > 1)
            {
                var currentPosition = this.fGameRoundController.SceneGuitars[0].CurrentPosition.SubTicks(480);
                SpriteBatch.DrawString(this.fGame.FontPlayingSongScreenSongTitle,
                                       string.Format("{0:000} : {1:000}",
                                                     currentPosition.Beat,
                                                     currentPosition.Tick),
                                       new Vector2(85, 200), Color.White);
            }


            if (this.fGameRoundController.GameRoundState == EnumGameRoundState.Paused)
            {
                SpriteBatch.DrawString(this.fGame.FontPlayingSongScreenSongSubtitle,
                                       "[Paused]",  
                                       new Vector2(100, 250), Color.White);
            }

            if (this.fGameRoundController.GameRoundState == EnumGameRoundState.Finished)
            {
                SpriteBatch.DrawString(this.fGame.FontPlayingSongScreenSongSubtitle,
                                       "[Finished]",
                                       new Vector2(100, 250), Color.White);
            }


            SpriteBatch.DrawString(this.fGame.FontPlayingSongScreenSongTitle,
                string.Format("Velocity: {0}%", this.fGameRoundController.Velocity),
                new Vector2(this.ScreenWidth - 200, this.ScreenHeight - 50), Color.White);


            RenderDebugInformation(this.SpriteBatch);

            this.SpriteBatch.End();
        }

        private void RenderGuitars(SpriteBatch pSpriteBatch)
        {
            if (this.fGameRoundController.SceneGuitars.Count() != 1)
                throw new Exception("Unexpected number of guitars: " + fGameRoundController.SceneGuitars.Count().ToString());

            int neckWidth = 245;
            int centralizedPosX = (this.ScreenWidth - neckWidth) / 2;

            var sceneGuitarRender = new GtSceneGuitarRender(this.fGame, centralizedPosX, this.ScreenHeight);
            sceneGuitarRender.Render(this.fGameRoundController.SceneGuitars[0], SpriteBatch);
        }

        private string notes = "";
        private void RenderDebugInformation(SpriteBatch pSpriteBatch)
        {
            //return;

            //draw the recognized notes
            var fft = this.fGame.GameController.AudioListener.FftData;

            if (fft.Where(p => p != 0).Count() > 0)
            {
                var spectrumAnalyzer = this.fGame.GameController.Factory.Instantiate<ISpectrumAnalyzer>();
                List<IMusicalNote> playingNotes = spectrumAnalyzer.GetMusicalNotes(fft);
                spectrumAnalyzer.DeleteUnusefulNotes(ref playingNotes);

                notes = "";
                foreach (var n in playingNotes)
                {
                    notes += n.ToString() + " ";
                }
            }

            pSpriteBatch.DrawString(this.fGame.FontSmallTexts,
                notes, new Vector2(60, 500), Color.White);
        }

    }
}
                
