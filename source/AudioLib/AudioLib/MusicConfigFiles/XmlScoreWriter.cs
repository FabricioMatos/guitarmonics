using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.SongData;

namespace Guitarmonics.AudioLib.MusicConfigFiles
{
    /// <summary>
    /// Indicate if the format is "Eletric Guitar Socores", "Guitar Chords" or "Game"
    /// </summary>
    public enum PlayingMode
    {
        EletricGuitarScore,
        GuitarChords,
        Game
    }

    public class XmlScoreWriter
    {
        public XmlScoreWriter(string pArtist, string pTitle, PlayingMode pPlayingMode, SortedList<GuitarScoreNote, GuitarScoreNote> pScoreNotes)
        {
            Construct(pArtist, pTitle, pPlayingMode, pScoreNotes.Values.ToList());
        }

        public XmlScoreWriter(Song pSong, PlayingMode pPlayingMode, SortedList<GuitarScoreNote, GuitarScoreNote> pScoreNotes)
        {
            Construct(pSong.Author, pSong.Name, pPlayingMode, pScoreNotes.Values.ToList());
        }

        public XmlScoreWriter(string pArtist, string pTitle, PlayingMode pPlayingMode, SortedList<ScoreNote, ScoreNote> pScoreNotes)
        {
            Construct(pArtist, pTitle, pPlayingMode, pScoreNotes.Values.ToList());
        }

        private void Construct(string pArtist, string pTitle, PlayingMode pPlayingMode, IList pScoreNotes)
        {
            this.fArtist = pArtist;
            this.fTitle = pTitle;
            this.fPlayingMode = pPlayingMode;

            this.fXmlNotesStringBuilder = new StringBuilder();
            this.fXmlSyncStringBuilder = new StringBuilder();

            this.GenerateXmlNotes(pScoreNotes);
        }

        private StringBuilder fXmlNotesStringBuilder;
        private StringBuilder fXmlSyncStringBuilder;
        private string fArtist;
        private string fTitle;
        private PlayingMode fPlayingMode;

        private void GenerateXmlNotes(IList pScoreNotes)
        {
            fXmlNotesStringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");
            fXmlSyncStringBuilder.AppendLine("<?xml version=\"1.0\" encoding=\"utf-8\" ?>");

            fXmlNotesStringBuilder.AppendLine(this.GenerateSongHeader());
            fXmlSyncStringBuilder.AppendLine(this.GenerateSongSyncHeader());

            foreach (var scoreNote in pScoreNotes)
            {
                fXmlNotesStringBuilder.AppendLine(this.GenerateScoreNoteElement((ScoreNote)scoreNote));

                fXmlSyncStringBuilder.AppendLine(this.GenerateSyncElement((ScoreNote)scoreNote));
            }

            fXmlNotesStringBuilder.AppendLine("</Song>");
            fXmlSyncStringBuilder.AppendLine("</SongSync>");
        }

        public string GenerateScoreNoteElement(ScoreNote pScoreNote)
        {
            var result = string.Format(
                "\t<ScoreNote Beat=\"{0}\" Tick=\"{1}\" NoteId=\"{2}\" Duration=\"{3}\"",
                pScoreNote.Beat, pScoreNote.Tick, pScoreNote.NoteId, pScoreNote.DurationInTicks);

            if (pScoreNote is GuitarScoreNote)
            {
                NotePosition notePosition = ((GuitarScoreNote)pScoreNote).DefaultNotePosition;

                result += string.Format(" String=\"{0}\" Fret=\"{1}\" RemarkOrChordName=\"{2}\"", notePosition.String, notePosition.Fret, pScoreNote.RemarkOrChordName);
            }

            result += "/>";

            return result;
        }

        public string GenerateSyncElement(ScoreNote pScoreNote)
        {
            long momentMin = 0;
            long momentSec = 0;
            long momentMilisec = 0;

            //split the MomentInMiliseconds in Min:Sec:Milisec
            if (pScoreNote.MomentInMiliseconds != null)
            {
                momentMin = pScoreNote.MomentInMiliseconds.Value / 60000;
                momentSec = (pScoreNote.MomentInMiliseconds.Value - momentMin * 60000) / 1000;
                momentMilisec = pScoreNote.MomentInMiliseconds.Value % 1000;
            }

            var result = string.Format(
                "\t<ScoreNote Beat=\"{0}\" Tick=\"{1}\" SyncSongPin=\"{4}:{5}:{6}\"",
                pScoreNote.Beat, pScoreNote.Tick, pScoreNote.NoteId, pScoreNote.DurationInTicks, momentMin, momentSec, momentMilisec);

            result += "/>";

            return result;
        }

        public string GenerateSongHeader()
        {
            return string.Format("<Song Artist=\"{0}\" Title=\"{1}\" PlayingMode=\"{2}\">",
                fArtist, fTitle, fPlayingMode);
        }

        public string GenerateSongSyncHeader()
        {
            return string.Format("<SongSync Artist=\"{0}\" Title=\"{1}\" PlayingMode=\"{2}\">",
                fArtist, fTitle, fPlayingMode);
        }

        public string ToXmlNotes()
        {
            return fXmlNotesStringBuilder.ToString();
        }

        public string ToXmlSync()
        {
            return fXmlSyncStringBuilder.ToString();
        }

        public void SaveXmlNotesToFile(string pFileName)
        {
            var stream = File.CreateText(pFileName);
            stream.Write(this.ToXmlNotes());
            stream.Close();
        }
    }
}
