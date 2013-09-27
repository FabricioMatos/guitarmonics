using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib.Controller;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Guitarmonics.GameLib.View
{
    public class GtTuneScreen : GtScreenBase
    {

        private IGtTuneController GtTuneController;

        public GtTuneScreen(XnaGame pGame, IGtTuneController pGtTuneController)
            : base(pGame)
        {
            this.GtTuneController = pGtTuneController;
        }

        public override void Update(TimeSpan pTotalTime, TimeSpan pElapsedTime)
        {
            base.Update(pTotalTime, pElapsedTime);
        }

        public override void Render()
        {
            base.Render();

            this.SpriteBatch.Begin();

            RenderBackground(this.SpriteBatch);

            this.RenderTunner(this.SpriteBatch);

            this.SpriteBatch.End();
        }

        private void RenderTunner(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Draw(
                this.fGame.EqualizerOnePointTexture,
                new Rectangle(200,
                    400,
                    20,
                    10),
                Color.Red);
        }

        private void RenderBackground(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Draw(
                fGame.BackgroundTuneScreen,
                new Rectangle(0, 0, fGame.Window.ClientBounds.Width, fGame.Window.ClientBounds.Height),
                Color.White);
        }

    }
}
