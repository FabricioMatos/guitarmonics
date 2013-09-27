using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Analysis;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.GameLib.Model;
using Guitarmonics.AudioLib.Analysis;

namespace Guitarmonics.GameLib.Model
{
    public interface IGtPlayedNotesAnalyserHelper
    {
        bool NoteIsPlaying(GtSceneGuitarNote pSceneGuitarNote);
        List<MusicalNoteAndTimeStamp> LastQueriedPlayedNotes { get; }
    }

    public class MusicalNoteAndTimeStamp
    {
        public IMusicalNote MusicalNote { get; set; }
        public DateTime TimeStamp { get; set; }
    }

    public class GtPlayedNotesAnalyserHelper : IGtPlayedNotesAnalyserHelper
    {
        public const int DELAY_TIME_FOR_PLAYING_NOTES_RECOGNITION = 300; //300ms
        private IAudioListener AudioListener;
        private ISpectrumAnalyzer SpectrumAnalyzer;
        private List<IMusicalNote> PlayingNotes = new List<IMusicalNote>();

        public List<MusicalNoteAndTimeStamp> LastQueriedPlayedNotes { get; private set; }
        public GtFactory Factory { get; private set; }

        public GtPlayedNotesAnalyserHelper(GtFactory pFactory, IAudioListener pAudioListener)
        {
            if (pAudioListener == null)
                throw new Exception("pAudioListener can't be null!");


            this.Factory = pFactory;
            this.AudioListener = pAudioListener;

            this.LastQueriedPlayedNotes = new List<MusicalNoteAndTimeStamp>();
            this.SpectrumAnalyzer = this.Factory.Instantiate<ISpectrumAnalyzer>();
        }

        public bool NoteIsPlaying(GtSceneGuitarNote pSceneGuitarNote)
        {
            //return true; //teste

            var fft = this.AudioListener.FftData;

            //if FFT is not invalid (all zero) update the PlayingNotes list.
            if (fft.Where(p => p != 0).Count() > 0)
            {
                //transform the FFT into a list of musical notes.
                PlayingNotes = this.SpectrumAnalyzer.GetMusicalNotes(fft);
                this.SpectrumAnalyzer.DeleteUnusefulNotes(ref PlayingNotes);
            }

            //query the LastQueriedPlayedNoteslist looking for the pSceneGuitarNote
            if (CheckIfNoteIsPlaying(pSceneGuitarNote))
            {
                 MusicalNoteAndTimeStamp playedNote = LastQueriedPlayedNotes.Where(p =>
                                         (p.MusicalNote.Value == pSceneGuitarNote.NoteValue) &&
                                         (p.MusicalNote.Number == pSceneGuitarNote.NoteNumber)).FirstOrDefault();
                
                if (playedNote != null)
                {
                    playedNote.TimeStamp = this.Factory.Clock.CurrentDateTime;
                }
                else
                {
                    playedNote = new MusicalNoteAndTimeStamp();
                    
                    playedNote.MusicalNote = new MusicalNote(pSceneGuitarNote.NoteValue.ToString() + pSceneGuitarNote.NoteNumber.ToString());
                    playedNote.TimeStamp = this.Factory.Clock.CurrentDateTime;

                    this.LastQueriedPlayedNotes.Add(playedNote);
                }

                return true;
            }

            bool noteFound = false;
            for (int i = LastQueriedPlayedNotes.Count - 1; i >= 0; i--)
            {
                var playedNote = LastQueriedPlayedNotes[i];

                //Delete all expired items
                if (playedNote.TimeStamp.AddMilliseconds(DELAY_TIME_FOR_PLAYING_NOTES_RECOGNITION) < this.Factory.Clock.CurrentDateTime)
                {
                    LastQueriedPlayedNotes.RemoveAt(i);                    
                }
                else
                {
                    //Check if the non expired note is the one we are looking for
                    if ((playedNote.MusicalNote.Value == pSceneGuitarNote.NoteValue) &&
                        (playedNote.MusicalNote.Number == pSceneGuitarNote.NoteNumber))
                    {
                        noteFound = true;   
                    }
                }
            }

            return noteFound;
        }

        virtual protected bool CheckIfNoteIsPlaying(GtSceneGuitarNote pSceneGuitarNote)
        {
            int qtd = PlayingNotes.Where(p =>
                                     (p.Value == pSceneGuitarNote.NoteValue) &&
                                     (p.Number == pSceneGuitarNote.NoteNumber)).Count();
            return (qtd == 1);
        }
    }
}
