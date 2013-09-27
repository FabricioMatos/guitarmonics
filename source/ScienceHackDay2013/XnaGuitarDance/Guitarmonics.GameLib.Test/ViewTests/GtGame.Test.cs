using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Microsoft.Xna.Framework;
using Guitarmonics.GameLib.Controller;
using Guitarmonics.GameLib.View;
using Guitarmonics.GameLib.ControllerTest;

namespace Guitarmonics.GameLib.ViewTest
{
    //[TestFixture]
    //public class GtGameTest
    //{
    //    [Test]
    //    public void InitialGameState()
    //    {
    //    }

        //[Test]
        //public void GameStateAndTheRelatedScreen()
        //{
        //    var game = new GtGameFake_ExitWhenRun();

        //    Assert.AreEqual(EnumGameScreen.Undefined, game.CurrentState);
        //    Assert.IsNull(game.CurrentScreen);

        //    game.Run();

        //    //ChooseSong
        //    game.CurrentState = EnumGameScreen.ChooseSong;
        //    Assert.AreEqual(typeof(GtChooseSongScreen), game.CurrentScreen.GetType());            

        //    //PlayingSong
        //    game.CurrentState = EnumGameScreen.PlayingSong;
        //    Assert.AreEqual(typeof(GtPlayingSongScreen), game.CurrentScreen.GetType());

        //    //None
        //    var currentScreen = game.CurrentScreen;
        //    game.CurrentState = EnumGameScreen.Undefined;
        //    Assert.AreEqual(currentScreen, game.CurrentScreen);
        //}    
    //}

    /// <summary>
    /// Fake XnaGame
    /// </summary>
    public class GtGameFake_ExitWhenRun : Guitarmonics.GameLib.View.XnaGame
    {
        public GtGameFake_ExitWhenRun()
            : base(new GtGameController(new GtFileLoader()))
        {
        }

        protected override void Update(GameTime pGameTime)
        {
            base.Update(pGameTime);
            this.Exit();
        }
    }

}
