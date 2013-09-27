using System;
using Guitarmonics.GameLib.Controller;
//using Guitarmonics.GameLib.ControllerTest;
using Guitarmonics.AudioLib.Analysis;
using Guitarmonics.AudioLib.Player;
using Guitarmonics.GameLib.Model;
using Guitarmonics.GameLib;
using Un4seen.Bass;
using System.Threading;

namespace Guitarmonics
{
    static class Program
    {
        /// <summary>
        /// The main entry point for the application.
        /// </summary>
        static void Main(string[] args)
        {
            Bass.LoadMe();

            //Thread.Sleep(5000);

            var factory = GtFactory.Instance;
            //factory.AddMapping<ISpectrumAnalyzer, DoubleSpectrumAnalyzerAlwaysE>();            

            var fileLoader = new GtFileLoader(factory);
            ///var fileLoader = new GtFileLoaderDoubleWebService(factory);
            //var fileLoader = new GtFileLoaderDouble6();
            //var fileLoader = new GtFileLoaderDouble6_FakeTickDataTable();
            //var fileLoader = new GtFileLoaderDoubleChromaticScale_FakeTickDataTable();

            var audioEffects = new AudioEffects();


            var songPlayer = new SongPlayer();

            var gameController = new GtGameController(
                factory,
                fileLoader,
                songPlayer,
                audioEffects);

            try
            {
                using (var game = new Guitarmonics.GameLib.View.XnaGame(gameController))
                {
                    game.Run();
                }                
            }
            finally
            {
                gameController.Dispose();
            }
        }

        #region Mocks

        public class GtGameRoundControllerPaused : GtGameRoundController
        {
            public GtGameRoundControllerPaused(GtFactory pFactory): base(pFactory, null)
            {
            }

            public override void PlaySong()
            {
                this.fGameRoundState = EnumGameRoundState.Playing;
            }

            public override void UpdateProgress(TimeSpan pTotalTime)
            {
                var currentPosition = new BeatTick(1, 0);

                foreach (var sceneGuitar in this.SceneGuitars)
                {
                    sceneGuitar.ForceCurrentPosition(currentPosition);
                }
            }

        }

        #endregion

    }
}

