using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Player;
using Guitarmonics.GameLib;
using Guitarmonics.GameLib.Model;
using Microsoft.Xna.Framework.Input;
using System.Configuration;

namespace Guitarmonics.GameLib.Controller
{
    public enum EnumGameRoundState
    {
        NotStarted,
        Playing,
        Paused,
        Aborting, //paused, waiting confirmation to abort
        Aborted,
        Finished
    }

    public interface IGtGameRoundController
    {
        void SetupSong(int pNumberOfPlayers, ISongPlayer pSongPlayer,
            string pArtist, string pAlbum, string pSong, float pPitch, params GtSceneGuitar[] pGtSceneGuitars);
        void PlaySong();

        /// <summary>
        /// If the Elapsed Time is bigger then 10 ms since the last update, update the game.
        /// Note that we update the game state based on the total time elapsed since the song begining
        /// and supose a constante BPM.
        /// </summary>
        /// <param name="pTotalTime"></param>
        /// <param name="pElapsedTime"></param>
        void UpdateProgress(TimeSpan pTotalTime);

        void ForceGameRoundState(EnumGameRoundState pGameRoundState);
        void InitializeProperties();

        ISongPlayer SongPlayer { get; }
        int NumberOfPlayers { get; }
        EnumGameRoundState GameRoundState { get; set; }
        GtSceneGuitar[] SceneGuitars { get; }
        int Velocity { get; }
        string Artist { get; }
        string Album { get; }
        string Song { get; }
        void Update(GtUserActionsListener pUser1ActionsListener);
    }

    /// <summary>
    /// Controla uma partida do jogo (uma música)
    /// </summary>
    public class GtGameRoundController : IGtGameRoundController
    {
        private IGtPlayedNotesAnalyser PlayedNotesAnalyser;
        private TimeSpan LastUpdatedTime;
        private TimeSpan BeginTime;
        private TimeSpan CurrentTime;
        private TimeSpan PauseBegin;
        private GtFactory Factory;
        private GtGameController GameController;
        private Dictionary<Keys, bool> KeyPressed;
        private decimal fVelocityValue;

        public decimal VelocityValue 
        {
            get
            {
                return fVelocityValue;
            }
            set
            {
                fVelocityValue = value;

                if (this.SongPlayer != null)
                    SongPlayer.ChangeVelocity((int)fVelocityValue);
            }
        }

        public int Velocity
        {
            get { return (int)decimal.Round(VelocityValue); }
        }
        public string Artist { get; private set; }
        public string Album { get; private set; }
        public string Song { get; private set; }
        public float Pitch { get; private set; }

        public GtGameRoundController(GtFactory pFactory, GtGameController pGameController)
        {
            //All used keys must be initialized to false here        
            KeyPressed = new Dictionary<Keys, bool>();
            KeyPressed.Add(Keys.Left, false);
            KeyPressed.Add(Keys.Right, false);
            KeyPressed.Add(Keys.Escape, false);

            Initialize(pFactory, pGameController);
        }

        private void Initialize(GtFactory pFactory, GtGameController pGameController)
        {
            InitializeProperties();

            this.Factory = pFactory;

            this.GameController = pGameController;

            this.PlayedNotesAnalyser = Factory.Instantiate<IGtPlayedNotesAnalyser>(this.Factory, pGameController.AudioListener);
        }

        public void InitializeProperties()
        {
            this.LastUpdatedTime = TimeSpan.MaxValue;
            this.BeginTime = TimeSpan.MaxValue;
            this.CurrentTime = TimeSpan.MaxValue;
            this.PauseBegin = TimeSpan.MaxValue;


            #region Define default Velocity
            
            this.VelocityValue = 100.0m;

            int defaultVelocityInConfigFile;
            if (int.TryParse(ConfigurationManager.AppSettings["DefaultVelocity"], out defaultVelocityInConfigFile))
                this.VelocityValue = defaultVelocityInConfigFile;

            #endregion

            this.fGameRoundState = EnumGameRoundState.NotStarted;

        }

        public void SetupSong(int pNumberOfPlayers, ISongPlayer pSongPlayer,
            string pArtist, string pAlbum, string pSong, float pPitch, params GtSceneGuitar[] pGtSceneGuitars)
        {
            this.fNumberOfPlayers = pNumberOfPlayers;

            if ((this.fNumberOfPlayers < 1) || (this.fNumberOfPlayers > 2))
                throw new InvalidGameRoundControlerParameters(string.Format("GtGameController must have one or two players: {0} found.", this.fNumberOfPlayers));

            if (pGtSceneGuitars == null)
                throw new InvalidGameRoundControlerParameters("null parameter is not allowed.");

            if (pGtSceneGuitars.Count() != this.fNumberOfPlayers)
                throw new InvalidGameRoundControlerParameters(string.Format("There are {0} players and {1} GtSceneGuitar in SetupSong(pGtSceneGuitars).", pGtSceneGuitars.Count(), this.fNumberOfPlayers));

            if (pSongPlayer == null)
                throw new InvalidGameRoundControlerParameters("null parameter is not allowed.");

            this.SongPlayer = pSongPlayer;

            this.Artist = pArtist;

            this.Album = pAlbum;

            this.Song = pSong;

            this.Pitch = pPitch;
            
            fSceneGuitars = pGtSceneGuitars;
        }

        public virtual void PlaySong()
        {
            this.SongPlayer.Play(Velocity, Pitch);
            this.fGameRoundState = EnumGameRoundState.Playing;

        }

        /// <summary>
        /// If the Elapsed Time is bigger then 10 ms since the last update, update the game.
        /// Note that we update the game state based on the total time elapsed since the song begining
        /// and supose a constante BPM.
        /// </summary>
        /// <param name="pTotalTime"></param>
        /// <param name="pElapsedTime"></param>
        public virtual void UpdateProgress(TimeSpan pTotalTime)
        {
            //TODO: Parei aqui. Preciso tratar o progresso com variacoes de velocidade.
            this.CurrentTime = pTotalTime;

            if (this.fGameRoundState != EnumGameRoundState.Playing)
                return;

            //Is the first call
            if (this.BeginTime == TimeSpan.MaxValue)
            {
                this.BeginTime = pTotalTime;
            }

            //Is the first call
            if (this.LastUpdatedTime == TimeSpan.MaxValue)
            {
                this.LastUpdatedTime = pTotalTime;
            }

            //Update the progress for each 10ms (100 frames per second - it's not necessary try more than that)
            if ((pTotalTime - this.LastUpdatedTime).TotalMilliseconds < 10)
                return;

            //calculate the "elapsed time"
            var elapsedTime = pTotalTime - this.BeginTime;

            //////////////////////////////////////////////////////

            //TODO: Tem um monte de código velho nesse metodo. Da pra dar uma enxugada pois agora não é mais necessário controlar o tempo decorrido, pois é utilizada
            //a informacao de posicao atual do próprio SongPlayer para sincronizar a tablatura.


            this.LastUpdatedTime = pTotalTime;

            var position = SongPlayer.CurrentPositionAsSeconds;
            int seconds = (int)position;
            //elapsedTime = new TimeSpan(0, 0, 0, seconds, (int)(1000 * (seconds - position)));
            elapsedTime = new TimeSpan((int)position * TimeSpan.TicksPerSecond);

            //////////////////////////////////////////////////////

            //Update all elements (SceneGuitars)
            foreach (var sceneGuitar in this.SceneGuitars)
            {
                //sceneGuitar.ForceCurrentPositionInMiliseconds((long)(elapsedTime.TotalSeconds * 1000));
                sceneGuitar.ForceCurrentPositionInMiliseconds((long)(position * 1000));

                sceneGuitar.Points += this.PlayedNotesAnalyser.Analyse(
                    sceneGuitar.CurrentStartingNotes,
                    sceneGuitar.CurrentExpectedPlayingNotes);

                sceneGuitar.MaxPoints += this.PlayedNotesAnalyser.AnalyseMaximum(
                    sceneGuitar.CurrentStartingNotes,
                    sceneGuitar.CurrentExpectedPlayingNotes);

                if (sceneGuitar.CurrentPosition.Beat == sceneGuitar.TickDataTable.NumberOfBeats)
                    this.GameRoundState = EnumGameRoundState.Finished;

            }
        }

        #region Properties

        public ISongPlayer SongPlayer { protected set; get; }
        private int fNumberOfPlayers;
        protected EnumGameRoundState fGameRoundState;
        private GtSceneGuitar[] fSceneGuitars = { };

        public int NumberOfPlayers
        {
            get { return fNumberOfPlayers; }
        }

        public void ForceGameRoundState(EnumGameRoundState pGameRoundState)
        {
            this.fGameRoundState = pGameRoundState;
        }

        public EnumGameRoundState GameRoundState
        {
            get { return fGameRoundState; }
            set
            {
                if ((fGameRoundState == EnumGameRoundState.Playing) && (value == EnumGameRoundState.Paused))
                {
                    Pause();
                }
                else if ((fGameRoundState == EnumGameRoundState.Playing) && (value == EnumGameRoundState.Aborting))
                {
                    Pause();
                }
                else if ((fGameRoundState == EnumGameRoundState.Paused) && (value == EnumGameRoundState.Playing))
                {
                    Resume();
                }
                else if ((fGameRoundState == EnumGameRoundState.Finished) && (value == EnumGameRoundState.Playing))
                {
                    Restart();
                }

                fGameRoundState = value;
            }
        }

        public virtual void Resume()
        {
            //Skip the paused duration to keep the sync with audio
            this.BeginTime += this.CurrentTime - this.PauseBegin;
            this.PauseBegin = TimeSpan.MaxValue;

            if (this.SongPlayer != null)
                this.SongPlayer.Play(Velocity, Pitch);
        }

        public virtual void Pause()
        {
            this.PauseBegin = this.CurrentTime;

            if (this.SongPlayer != null)
                this.SongPlayer.Pause();
        }

        public virtual void Restart()
        {
            this.BeginTime = TimeSpan.MaxValue;
            this.LastUpdatedTime = TimeSpan.MaxValue;
            this.PauseBegin = TimeSpan.MaxValue;

            if (this.SongPlayer != null)
            {
                this.SongPlayer.Stop();

                this.SongPlayer.Play(Velocity, Pitch);
            }

            foreach (var sceneGuitar in this.SceneGuitars)
            {
                sceneGuitar.ForceCurrentPosition(new BeatTick(1, 0));
            }
        }

        public GtSceneGuitar[] SceneGuitars
        {
            get { return fSceneGuitars; }
        }

        #endregion


        public void Update(GtUserActionsListener pUser1ActionsListener)
        {
            //KeyPressed is used to control key press (keeping down is ignored)

            #region Reset KeyPressed if key is up

            if (pUser1ActionsListener.CurrentKeyboardState.IsKeyUp(Keys.Left))
                KeyPressed[Keys.Left] = false;

            if (pUser1ActionsListener.CurrentKeyboardState.IsKeyUp(Keys.Right))
                KeyPressed[Keys.Right] = false;

            if (pUser1ActionsListener.CurrentKeyboardState.IsKeyUp(Keys.Escape))
                KeyPressed[Keys.Escape] = false;

            #endregion

            #region Change Velocity

            if (pUser1ActionsListener.CurrentKeyboardState.IsKeyDown(Keys.Left) && !KeyPressed[Keys.Left])
            {
                if (this.VelocityValue > 30.0m)
                    this.VelocityValue = this.VelocityValue - 5m;

                KeyPressed[Keys.Left] = true;
            }
            if (pUser1ActionsListener.CurrentKeyboardState.IsKeyDown(Keys.Right) && !KeyPressed[Keys.Right])
            {
                if (this.VelocityValue < 100.0m)
                    this.VelocityValue = this.VelocityValue + 5m;

                KeyPressed[Keys.Right] = true;
            }

            #endregion

            if (pUser1ActionsListener.CurrentKeyboardState.IsKeyDown(Keys.Escape))
            {
                //this.fGameRoundState = EnumGameRoundState.Aborted;
                this.GameRoundState = EnumGameRoundState.Finished;
            }
        }
    }

    public class InvalidGameRoundControlerParameters : Exception
    {
        public InvalidGameRoundControlerParameters(string pMessage)
            : base(pMessage)
        {
        }
    }
}
