using System;

namespace Guitarmonics.AudioLib.Common
{
    public interface IMusicalNote
    {
        int Cents { get; }
        float Frequence { get; }
        int Number { get; }
        float PlayedFrequence { get; }
        string ToString();
        NoteValue Value { get; }
        float Volume { get; }
    }
}
