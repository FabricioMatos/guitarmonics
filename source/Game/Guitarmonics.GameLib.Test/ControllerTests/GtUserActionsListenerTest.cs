using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework.Input;
using Guitarmonics.GameLib.Controller;

namespace Guitarmonics.GameLib.ControllerTest
{
    [TestFixture]
    public class GtUserActionsListenerTest
    {
        [Test]
        public void UpdateMethod()
        {
            var listener = new GtUserActionsListener();

            KeyboardState keyboardState = new KeyboardState(Keys.Escape); ;

            GamePadState gamePadState = new GamePadState(
                new GamePadThumbSticks(), 
                new GamePadTriggers(), 
                new GamePadButtons(Buttons.B), 
                new GamePadDPad());

            listener.Update(keyboardState, gamePadState);

            Assert.AreEqual(keyboardState, listener.CurrentKeyboardState);
            Assert.AreEqual(gamePadState, listener.CurrentGamePadState);

        }

        [Test]
        public void KeyDownNow()
        {
            var listener = new GtUserActionsListener();

            KeyboardState keyboardState = new KeyboardState(Keys.Escape); ;

            GamePadState gamePadState = new GamePadState(
                new GamePadThumbSticks(),
                new GamePadTriggers(),
                new GamePadButtons(Buttons.A),
                new GamePadDPad());

            listener.Update(keyboardState, gamePadState);

            Assert.IsTrue(listener.IsKeyDownNow(Keys.Escape));
            Assert.IsFalse(listener.IsKeyDownNow(Keys.Enter));

            Assert.IsTrue(listener.IsButtonDownNow(Buttons.A));
            Assert.IsFalse(listener.IsButtonDownNow(Buttons.B));

            //Update again with the same key pressed
            listener.Update(keyboardState, gamePadState);

            Assert.IsFalse(listener.IsKeyDownNow(Keys.Escape));
            Assert.IsFalse(listener.IsKeyDownNow(Keys.Enter));

            Assert.IsFalse(listener.IsButtonDownNow(Buttons.A));
            Assert.IsFalse(listener.IsButtonDownNow(Buttons.B));

       }

    }
}
