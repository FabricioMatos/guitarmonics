using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib.Controller;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Guitarmonics.GameLib.View
{
    public class GtMenuScreen : GtScreenBase
    {

        public GtMenuScreen(XnaGame pGame)
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
            Texture2D background;

            switch (this.fGame.GameController.MenuScreenSelectedItem)
            {
                case EnumMenuScreenItems.QuickPlay:
                    background = this.fGame.BackgroundMenu_QuickPlay;
                    break;
                case EnumMenuScreenItems.Tune:
                    background = this.fGame.BackgroundMenu_Tune;
                    break;
                case EnumMenuScreenItems.Quit:
                    background = this.fGame.BackgroundMenu_Quit;
                    break;
                default:
                    background = this.fGame.BackgroundMenu_QuickPlay;
                    break;
            }

            pSpriteBatch.Draw(
                background,
                new Rectangle(0, 0, fGame.Window.ClientBounds.Width, fGame.Window.ClientBounds.Height),
                Color.White);
        }

    }
}
