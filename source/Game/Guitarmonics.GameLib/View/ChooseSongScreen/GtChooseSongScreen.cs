using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib.Controller;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Guitarmonics.GameLib.View
{
    public class GtChooseSongScreen : GtScreenBase
    {
        public GtChooseSongScreen(XnaGame pGame)
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

            RenderSongList(this.SpriteBatch);

            this.SpriteBatch.End();
        }

        private void RenderBackground(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Draw(
                fGame.BackgroundChooseSong,
                new Rectangle(0, 0, fGame.Window.ClientBounds.Width, fGame.Window.ClientBounds.Height),
                Color.Gray);
        }

        private void RenderSongList(SpriteBatch spriteBatch)
        {
            int height = 50;

            foreach (var songDescription in this.fGame.GameController.VisibleSongs)
            {
                Color color;
                if (this.fGame.GameController.SelectedSong == songDescription)
                    color = Color.Yellow;
                else
                    color = Color.White;


                spriteBatch.DrawString(this.fGame.FontSongDescription, songDescription.Song, new Vector2(100 + 1, height + 1), Color.Black);
                spriteBatch.DrawString(this.fGame.FontSongDescription, songDescription.Song, new Vector2(100, height), color);
                height += 20;

                spriteBatch.DrawString(this.fGame.FontSongDescription, songDescription.Artist, new Vector2(100 + 1, height + 1), Color.Black);
                spriteBatch.DrawString(this.fGame.FontSongDescription, songDescription.Artist, new Vector2(100, height), color);
                height += 50;
            }
        }
    }
}
