using System;

namespace Guitarmonics.AudioLib.Player
{
    public enum SongPlayerStatus
    {
        NotInitialized,
        Stopped,
        Playing,
        Paused
    }

    public enum GtTimeSignature
    {
        Time2x4,
        Time3x4,
        Time4x4,
        Time6x8
        //etc...
    }

    public interface ISongPlayer : IDisposable
    {
        long CurrentPosition { get; }
        double CurrentPositionAsSeconds { get; }
        double DurationAsSeconds { get; }
        long Length { get; }
        void LoadStream();
        void Pause();
        void Play();
        void Play(int velocity);
        void Play(int velocity, float pitch);
        SongPlayerStatus Status { get; }
        void Stop();
        //double TickDurationAsSeconds { get; }
        GtTimeSignature TimeSignature { get; }
        TickNotifyEvent TickNotifyEvent { get; set; }
        //double TempoBPM { get; }
        //void SetupSong(string pFileName, double pTempoBPM, GtTimeSignature pTimeSignature);
        void SetupSong(string pFileName, GtTimeSignature pTimeSignature);

        void ChangeVelocity(int newVelocity);
    }
}
