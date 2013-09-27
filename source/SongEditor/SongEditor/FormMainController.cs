using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Midi;
using Guitarmonics.AudioLib.MusicConfigFiles;
using Toub.Sound.Midi;

namespace SongEditor
{
    public class TrackName
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

    public class FormMainController
    {
        public List<TrackName> ListAllTracks(string pFileName)
        {
            var result = new List<TrackName>();

            var midiSequence = MidiSequence.Import(pFileName);
            var tracks = midiSequence.GetTracks();
            

            for (int i = 1; i < tracks.Count(); i++)
            {
                //pegar o evento que representao o trackName
                //var events = track.Events.Where(....)

                string nome = "";

                if (tracks[1].Events[0] is SequenceTrackName)
                    nome = ((SequenceTrackName)tracks[i].Events[0]).Text;
                else
                    nome = "Track " + i.ToString();

                result.Add(new TrackName()
                               {
                                   Id = i,
                                   Name = nome
                               });
            }


            return result;
        }


        public void CreateXmlFileFromMidi(string pMidiFileName, int pTrackNumber, string pXmlFileName, 
            string pArtist, string pAlbum, string pTitle)
        {
            var sequence = MidiSequence.Import(pMidiFileName);

            var track = sequence.GetTracks()[pTrackNumber];

            MidiEventCollection midiEvents = track.Events;

            var midiImporter = new GuitarMidiImporter(midiEvents, sequence.Division /*BPM*/, 0 /*pSkipBeats*/);

            //TODO: Add an "Album" parameter
            var xmlScoreWriter = new XmlScoreWriter(pArtist, /*pAlbum,*/ pTitle, PlayingMode.EletricGuitarScore, midiImporter.ScoreNotes);

            if (File.Exists(pXmlFileName))
                File.Delete(pXmlFileName);

            xmlScoreWriter.SaveXmlNotesToFile(pXmlFileName);
            
        }
    }
}
