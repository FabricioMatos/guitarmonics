using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib.Controller;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.AudioLib.Analysis;
using Guitarmonics.GameLib.Model;

namespace Guitarmonics.GameLib.View
{
    public class GtMainScreen : GtScreenBase
    {
        private const int FACTOR = 4;
        private GtEqualizer fEqualizer;
        private GtNotesLegend fGtNotesLegend;
        private List<IMusicalNote> playingNotes;
        private bool newPlayingNotes = false;
        private HappyGuyState happyGuyState = new HappyGuyState();

        public TimeSpan LastStatusChange = TimeSpan.MinValue;
        public int Bpm;
        public float beatDuration;
        private bool Step = false;
        private bool HalfStep = false;
        private int PosY;

        public GtMainScreen(XnaGame pGame)
            : base(pGame)
        {
            this.fEqualizer = new GtEqualizer(pGame, 10, 10);
            this.fGtNotesLegend = new GtNotesLegend(pGame);

            Bpm = 140;
            beatDuration = 60000.0f / Bpm;
        }

        public override void Update(TimeSpan pTotalTime, TimeSpan pElapsedTime)
        {
            base.Update(pTotalTime, pElapsedTime);

            this.fEqualizer.Update(this.fGame.GameController.AudioListener.FftData);

            this.newPlayingNotes = UpdatePlayingNotes();

            if ((pTotalTime.TotalMilliseconds - LastStatusChange.TotalMilliseconds) < (beatDuration / 2))
                HalfStep = false;
            else
                HalfStep = true;

            if ((pTotalTime.TotalMilliseconds - LastStatusChange.TotalMilliseconds) < beatDuration)
                return;

            Step = !Step;
            HalfStep = false;
            LastStatusChange = pTotalTime;

            if (this.newPlayingNotes)
            {
                this.happyGuyState.UpdateState(Step, playingNotes);
            }
        }

        private bool UpdatePlayingNotes()
        {
            var fft = this.fGame.GameController.AudioListener.FftData;

            if (fft.Where(p => p != 0).Count() > 0)
            {
                var spectrumAnalyzer = this.fGame.GameController.Factory.Instantiate<ISpectrumAnalyzer>();
                this.playingNotes = spectrumAnalyzer.GetMusicalNotes(fft);
                spectrumAnalyzer.DeleteUnusefulNotes(ref playingNotes);

            }

            return (playingNotes.Count > 0);
        }

        public override void Render()
        {
            base.Render();

            if (this.newPlayingNotes)
            {
                if (HalfStep)
                    PosY = 40;
                else
                    PosY = 60;
            }

            this.SpriteBatch.Begin();

            this.RenderBackground(this.SpriteBatch);

            this.fEqualizer.Render(this.SpriteBatch);

            this.fGtNotesLegend.Render(this.SpriteBatch, 10, 300);

            this.RenderNotesForDebug(this.SpriteBatch);

            this.RenderHappyGuy(this.SpriteBatch, this.happyGuyState);

            this.SpriteBatch.End();
        }

        private void RenderBackground(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Draw(
                fGame.BackgroundMainScreen,
                new Rectangle(0, 0, fGame.Window.ClientBounds.Width, fGame.Window.ClientBounds.Height),
                Color.White);
        }


        private void RenderNotesForDebug(SpriteBatch pSpriteBatch)
        {
            if (this.newPlayingNotes)
            {
                var notes = "";
                foreach (var n in playingNotes)
                {
                    notes += n.ToString() + " ";
                }

                pSpriteBatch.DrawString(this.fGame.FontSmallTexts,
                    notes, new Vector2(60, 200), Color.White);
            }
        }

        private void RenderHappyGuy(SpriteBatch pSpriteBatch, HappyGuyState happyGuyState)
        {
            RenderHead(pSpriteBatch, happyGuyState);
            RenderLeftArm(pSpriteBatch, happyGuyState);
            RenderRightArm(pSpriteBatch, happyGuyState);
            RenderLeftLeg(pSpriteBatch, happyGuyState);
            RenderRightLeg(pSpriteBatch, happyGuyState);

            RenderBody(pSpriteBatch, happyGuyState);
        }

        
        private void RenderHead(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch, HappyGuyState happyGuyState)
        {
            Texture2D texture = null;
            var deltaY = 0;

            switch (happyGuyState.HeadState)
            {
                case HeadState.Front:
                    texture = fGame.HeadFront;
                    break;
                case HeadState.Left:
                    texture = fGame.HeadLeft;
                    break;
                case HeadState.Right:
                    texture = fGame.HeadRight;
                    break;
                case HeadState.Up:
                    texture = fGame.HeadUp;
                    deltaY = 8;
                    break;
                case HeadState.Down:
                    texture = fGame.HeadDown;
                    deltaY = 10;
                    break;
                default:
                    texture = fGame.HeadFront;
                    break;
            }

            pSpriteBatch.Draw(texture,
                new Rectangle(500, PosY + deltaY, texture.Width, texture.Height),
                Color.White);
        }

        private void RenderBody(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch, HappyGuyState happyGuyState)
        {
            Texture2D texture = fGame.Body;

            pSpriteBatch.Draw(
                texture,
                new Rectangle(495, PosY + (fGame.HeadFront.Height) - 20, texture.Width, texture.Height),
                Color.White);
        }

        private void RenderLeftArm(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch, HappyGuyState happyGuyState)
        {
            Texture2D texture = null;
            int deltaY = 0;
            int deltaX = 0;

            switch (happyGuyState.LeftArmState)
            {
                case ArmState.Down1: //ok
                    texture = fGame.LeftArmDown1;
                    deltaY = 105;
                    deltaX = 5;
                    break;
                case ArmState.Up1: //ok
                    texture = fGame.LeftArmUp1;
                    deltaY = -75;
                    deltaX = 15;
                    break;
                case ArmState.Down2: //ok
                    texture = fGame.LeftArmDown2;
                    deltaY = 38;
                    deltaX = 10;
                    break;
                case ArmState.Up2: //ok
                    texture = fGame.LeftArmUp2;
                    deltaY = 80;
                    deltaX = 15;
                    break;
                case ArmState.Down3: //ok
                    texture = fGame.LeftArmDown3;
                    deltaY = 133;
                    deltaX = 10;
                    break;
                case ArmState.Up3: //ok
                    texture = fGame.LeftArmUp3;
                    deltaY = 105;
                    deltaX = 5;
                    break;
                default:
                    texture = fGame.LeftArmDown1;
                    deltaY = 105;
                    deltaX = 5;
                    break;
            }


            pSpriteBatch.Draw(
                texture,
                new Rectangle(495 - texture.Width + deltaX, PosY + deltaY, texture.Width, texture.Height),
                Color.White);
        }

        private void RenderRightArm(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch, HappyGuyState happyGuyState)
        {
            Texture2D texture = null;
            int deltaY = 0;
            int deltaX = 0;

            switch (happyGuyState.RightArmState)
            {
                case ArmState.Down1: //ok
                    texture = fGame.RightArmDown1;
                    deltaY = 105;
                    deltaX = -5;
                    break;
                case ArmState.Up1:
                    texture = fGame.RightArmUp1;
                    deltaY = -75;
                    deltaX = -15;
                    break;
                case ArmState.Down2:
                    texture = fGame.RightArmDown2;
                    deltaY = 38;
                    deltaX = -10;
                    break;
                case ArmState.Up2:
                    texture = fGame.RightArmUp2;
                    deltaY = 80;
                    deltaX = -15;
                    break;
                case ArmState.Down3:
                    texture = fGame.RightArmDown3;
                    deltaY = 133;
                    deltaX = -10;
                    break;
                case ArmState.Up3:
                    texture = fGame.RightArmUp3;
                    deltaY = 105;
                    deltaX = -5;
                    break;
                default:
                    texture = fGame.RightArmDown1;
                    deltaY = 105;
                    deltaX = -5;
                    break;
            }

            pSpriteBatch.Draw(
                texture,
                new Rectangle(495 + fGame.Body.Width + deltaX, PosY + deltaY, texture.Width, texture.Height),
                Color.White);
        }

        private void RenderLeftLeg(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch, HappyGuyState happyGuyState)
        {
            Texture2D texture = null;
            int deltaY = 0;
            int deltaX = 0;

            switch (happyGuyState.LeftLegState)
            {
                case LegState.Down1:
                    texture = fGame.LeftLegDown1;
                    deltaY = -80;
                    deltaX = -36;
                    break;
                case LegState.Up1:
                    texture = fGame.LeftLegUp1;
                    deltaY = -100;
                    deltaX = -270;
                    break;
                case LegState.Down2:
                    texture = fGame.LeftLegDown2;
                    deltaY = -80;
                    deltaX = -36;
                    break;
                case LegState.Up2:
                    texture = fGame.LeftLegUp2;
                    deltaY = -85;
                    deltaX = -145;
                    break;
                default:
                    texture = fGame.LeftLegDown1;
                    deltaY = -80;
                    deltaX = -36;
                    break;
            }

            pSpriteBatch.Draw(
                texture,
                new Rectangle(495 + deltaX, PosY + (fGame.HeadFront.Height) - 20 + fGame.Body.Height + deltaY, texture.Width, texture.Height),
                Color.White);
        }

        private void RenderRightLeg(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch, HappyGuyState happyGuyState)
        {
            Texture2D texture = null;
            int deltaY = 0;
            int deltaX = 0;

            switch (happyGuyState.RightLegState)
            {
                case LegState.Down1:
                    texture = fGame.RightLegDown1;
                    deltaY = -80;
                    deltaX = 36;
                    break;
                case LegState.Up1:
                    texture = fGame.RightLegUp1;
                    deltaY = -100;
                    deltaX = 68;
                    break;
                case LegState.Down2:
                    texture = fGame.RightLegDown2;
                    deltaY = -80;
                    deltaX = 36;
                    break;
                case LegState.Up2:
                    texture = fGame.RightLegUp2;
                    deltaY = -85;
                    deltaX = 68;
                    break;
                default:
                    texture = fGame.RightLegDown1;
                    deltaY = -80;
                    deltaX = 36;
                    break;
            }

            pSpriteBatch.Draw(
                texture,
                new Rectangle(495 + deltaX, PosY + (fGame.HeadFront.Height) - 20 + fGame.Body.Height + deltaY, texture.Width, texture.Height),
                Color.White);
        }

        #region Versao com Rascunho
        /*
        private void RenderRightLeg(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch, HappyGuyState happyGuyState)
        {
            Texture2D texture = null;

            switch (happyGuyState.RightLegState)
            {
                case LegState.Down1:
                    texture = fGame.RightLegDown1;
                    break;
                case LegState.Up1:
                    texture = fGame.RightLegUp1;
                    break;
                case LegState.Down2:
                    texture = fGame.RightLegDown2;
                    break;
                case LegState.Up2:
                    texture = fGame.RightLegUp2;
                    break;
                default:
                    texture = fGame.RightLegDown1;
                    break;
            }

            pSpriteBatch.Draw(
                texture,
                new Rectangle(495 + ((fGame.Body.Width / 2) / FACTOR), PosY + (fGame.HeadFront.Height / FACTOR) + (fGame.Body.Height / FACTOR), texture.Width / FACTOR, texture.Height / FACTOR),
                Color.White);

        }

        private void RenderLeftLeg(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch, HappyGuyState happyGuyState)
        {
            Texture2D texture = null;

            switch (happyGuyState.LeftLegState)
            {
                case LegState.Down1:
                    texture = fGame.LeftLegDown1;
                    break;
                case LegState.Up1:
                    texture = fGame.LeftLegUp1;
                    break;
                case LegState.Down2:
                    texture = fGame.LeftLegDown2;
                    break;
                case LegState.Up2:
                    texture = fGame.LeftLegUp2;
                    break;
                default:
                    texture = fGame.LeftLegDown1;
                    break;
            }

            pSpriteBatch.Draw(
                texture,
                new Rectangle(495 - (texture.Width / FACTOR) + ((fGame.Body.Width / 2) / FACTOR), PosY + (fGame.HeadFront.Height / FACTOR) + (fGame.Body.Height / FACTOR), texture.Width / FACTOR, texture.Height / FACTOR),
                Color.White);

        }

        private void RenderRightArm(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch, HappyGuyState happyGuyState)
        {
            Texture2D texture = null;

            switch (happyGuyState.RightArmState)
            {
                case ArmState.Down1:
                    texture = fGame.RightArmDown1;
                    break;
                case ArmState.Up1:
                    texture = fGame.RightArmUp1;
                    break;
                case ArmState.Down2:
                    texture = fGame.RightArmDown2;
                    break;
                case ArmState.Up2:
                    texture = fGame.RightArmUp2;
                    break;
                case ArmState.Down3:
                    texture = fGame.RightArmDown3;
                    break;
                case ArmState.Up3:
                    texture = fGame.RightArmUp3;
                    break;
                default:
                    texture = fGame.RightArmDown1;
                    break;
            }

            pSpriteBatch.Draw(
                texture,
                new Rectangle(495 + fGame.Body.Width / FACTOR, PosY + (fGame.HeadFront.Height / FACTOR), texture.Width / FACTOR, texture.Height / FACTOR),
                Color.White);

        }

        private void RenderLeftArm(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch, HappyGuyState happyGuyState)
        {
            Texture2D texture = null;

            switch (happyGuyState.LeftArmState)
            {
                case ArmState.Down1:
                    texture = fGame.LeftArmDown1;
                    break;
                case ArmState.Up1:
                    texture = fGame.LeftArmUp1;
                    break;
                case ArmState.Down2:
                    texture = fGame.LeftArmDown2;
                    break;
                case ArmState.Up2:
                    texture = fGame.LeftArmUp2;
                    break;
                case ArmState.Down3:
                    texture = fGame.LeftArmDown3;
                    break;
                case ArmState.Up3:
                    texture = fGame.LeftArmUp3;
                    break;
                default:
                    texture = fGame.LeftArmDown1;
                    break;
            }

            pSpriteBatch.Draw(
                texture,
                new Rectangle(495 - texture.Width / FACTOR, PosY + (fGame.HeadFront.Height / FACTOR), texture.Width / FACTOR, texture.Height / FACTOR),
                Color.White);
        }

        private void RenderHead(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch, HappyGuyState happyGuyState)
        {
            Texture2D texture = null;

            switch (happyGuyState.HeadState)
            {
                case HeadState.Front:
                    texture = fGame.HeadFront;
                    break;
                case HeadState.Left:
                    texture = fGame.HeadLeft;
                    break;
                case HeadState.Right:
                    texture = fGame.HeadRight;
                    break;
                case HeadState.Up:
                    texture = fGame.HeadUp;
                    break;
                case HeadState.Down:
                    texture = fGame.HeadDown;
                    break;
                default:
                    texture = fGame.HeadFront;
                    break;
            }

            pSpriteBatch.Draw(texture,
                new Rectangle(500, PosY, texture.Width / FACTOR, texture.Height / FACTOR),
                Color.White);
        }

        private void RenderBody(Microsoft.Xna.Framework.Graphics.SpriteBatch pSpriteBatch, HappyGuyState happyGuyState)
        {
            pSpriteBatch.Draw(
                fGame.Body,
                new Rectangle(495, PosY + (fGame.HeadFront.Height / FACTOR), fGame.Body.Width / FACTOR, fGame.Body.Height / FACTOR),
                Color.White);
        }
*/

        #endregion
    }
}
