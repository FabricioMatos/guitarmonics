using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Audio;

namespace Guitarmonics.GameLib.Controller
{
    public interface IAudioEffects
    {
        void PlaySingleHit();

        void PlayDoubleHit();

        void PlaySongTheme();

        void StopSongTheme();

    }

    public class AudioEffects : IAudioEffects
    {
        public AudioEffects()
        {
        }

        public SoundBank SoundBank { get; set; }
        private Cue SongThemeCue { get; set; }

        private void ValidateSoundBank()
        {
            if (this.SoundBank == null)
                throw new Exception("AudioEffects.SoundBank is null");
        }

        #region IAudioEffects Members

        public void PlaySingleHit()
        {
            ValidateSoundBank();

            this.SoundBank.PlayCue("SingleHit");
        }

        public void PlayDoubleHit()
        {
            ValidateSoundBank();

            this.SoundBank.PlayCue("DoubleHit");
        }

        public void PlaySongTheme()
        {
            ValidateSoundBank();

            this.SongThemeCue = this.SoundBank.GetCue("SongTheme");

            if (!this.SongThemeCue.IsPlaying)
                this.SongThemeCue.Play();

        }

        public void StopSongTheme()
        {
            if (this.SongThemeCue != null)
            {
                if (this.SongThemeCue.IsPlaying)
                    this.SongThemeCue.Stop(AudioStopOptions.Immediate);
            }
        }

        #endregion
    }
}
