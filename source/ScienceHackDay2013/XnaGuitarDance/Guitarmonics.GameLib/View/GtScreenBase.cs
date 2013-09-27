using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Guitarmonics.GameLib.Controller;

namespace Guitarmonics.GameLib.View
{
    public abstract class GtScreenBase
    {
        protected SpriteBatch SpriteBatch { get; private set; }

        protected XnaGame fGame = null;

        public GtScreenBase(XnaGame pGame)
        {
            if (pGame == null)
                throw new Exception("pGame can't be null");

            this.fGame = pGame;

            if (this.fGame != null)
            {
                this.SpriteBatch = new SpriteBatch(pGame.GraphicsDevice);
            }
        }

        public int ScreenWidth
        {
            get
            {
                int screenWidth;

                if (this.fGame.GraphicsDeviceManager.IsFullScreen)
                {
                    screenWidth = this.fGame.GraphicsDeviceManager.GraphicsDevice.DisplayMode.Width;
                }
                else
                {
                    screenWidth = this.fGame.Width;
                }

                return screenWidth;
            }
        }

        public int ScreenHeight
        {
            get
            {
                int screenHeight;

                if (this.fGame.GraphicsDeviceManager.IsFullScreen)
                {
                    screenHeight = this.fGame.GraphicsDeviceManager.GraphicsDevice.DisplayMode.Height;
                }
                else
                {
                    screenHeight = this.fGame.Height;
                }

                return screenHeight;
            }
        }

        public virtual void Update(TimeSpan pTotalTime, TimeSpan pElapsedTime)
        {
            //this.fGame.Window.Title = this.GetType().Name + ": " + pTotalTime.ToString();
        }

        public virtual void Render()
        {
        }
    }
}
