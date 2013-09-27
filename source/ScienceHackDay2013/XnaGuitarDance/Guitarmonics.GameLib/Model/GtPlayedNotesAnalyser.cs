using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib.Model;
using Guitarmonics.AudioLib.Analysis;

namespace Guitarmonics.GameLib.Model
{
    public interface IGtPlayedNotesAnalyser
    {
        int Analyse(List<GtSceneGuitarNote> pStartingNotes, List<GtSceneGuitarNote> pExpectedPlayingNotes);
        int AnalyseMaximum(List<GtSceneGuitarNote> pStartingNotes, List<GtSceneGuitarNote> pExpectedPlayingNotes);
    }

    public class GtPlayedNotesAnalyser : IGtPlayedNotesAnalyser
    {
        private GtFactory fFactory;
        private IGtPlayedNotesAnalyserHelper AnalyserHelper;

        public GtPlayedNotesAnalyser(GtFactory pFactory, IAudioListener pAudioListener)
        {
            this.fFactory = pFactory;

            this.AnalyserHelper = this.fFactory.Instantiate<IGtPlayedNotesAnalyserHelper>(pFactory, pAudioListener);
        }

        public int Analyse(List<GtSceneGuitarNote> pStartingNotes, List<GtSceneGuitarNote> pExpectedPlayingNotes)
        {
            int points = 0;

            foreach (var note in pStartingNotes)
            {
                if (AnalyserHelper.NoteIsPlaying(note))
                {
                    note.StartHited = true;
                    points += 100;
                }
            }

            foreach (var note in pExpectedPlayingNotes)
            {
                note.Playing = AnalyserHelper.NoteIsPlaying(note);

                if (note.Playing)
                {
                    points += 1;
                }
            }

            return points;
        }

        public int AnalyseMaximum(List<GtSceneGuitarNote> pStartingNotes, List<GtSceneGuitarNote> pExpectedPlayingNotes)
        {
            int points = 0;

            foreach (var note in pStartingNotes)
            {
                points += 100;
            }

            foreach (var note in pExpectedPlayingNotes)
            {
                points += 1;
            }

            return points;
        }
    }
}
