using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib.Controller;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Guitarmonics.GameLib.View
{
    public class GtMainScreen : GtScreenBase
    {

        public GtMainScreen(XnaGame pGame)
            : base(pGame)
        {
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

            this.SpriteBatch.End();
        }

        private void RenderBackground(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Draw(
                fGame.BackgroundMainScreen,
                new Rectangle(0, 0, fGame.Window.ClientBounds.Width, fGame.Window.ClientBounds.Height),
                Color.White);
        }

    }
}
