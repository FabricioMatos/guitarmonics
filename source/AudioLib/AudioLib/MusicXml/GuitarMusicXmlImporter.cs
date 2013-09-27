using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Guitarmonics.AudioLib.Common;

namespace Guitarmonics.AudioLib.MusicXml
{
    public class InvalidXmlMusicFile : Exception
    {
        public InvalidXmlMusicFile(string pMessage)
            : base(pMessage)
        {

        }
    }

    public class TrackInfo
    {
        public string Id { get; set; }
        public string Name { get; set; }

        public override string ToString()
        {
            return this.Name;
        }
    }

    public enum DottedType
    {
        Dotted,
        NonDotted
    }

    public class GuitarMusicXmlImporter
    {
        public GuitarMusicXmlImporter()
        {
            fScoreNotes = new SortedList<GuitarScoreNote, GuitarScoreNote>();
        }


        #region Properties

        private SortedList<GuitarScoreNote, GuitarScoreNote> fScoreNotes;
        public SortedList<GuitarScoreNote, GuitarScoreNote> ScoreNotes
        {
            get
            {
                return fScoreNotes;
            }
        }

        #endregion


        public XmlDocument OpenMusicXmlFile(string pFileName)
        {
            XmlDocument xmlDoc = new XmlDocument();
            xmlDoc.Load(pFileName);

            return xmlDoc;
        }

        public IList<TrackInfo> ListTracks(XmlDocument pXmlDoc)
        {
            var tracks = new List<TrackInfo>();

            XmlNodeList xmlScoreParts = pXmlDoc.GetElementsByTagName("score-part");

            foreach (XmlNode score in xmlScoreParts)
            {
                var xmlTrackIdNode = score.Attributes.GetNamedItem("id");
                var xmlTrackNameNode = score.SelectSingleNode("descendant::part-name");

                if (xmlTrackIdNode == null)
                    throw new InvalidXmlMusicFile("'id' attribute was not found in 'score-part' node of a MusicXml file.");

                if (xmlTrackNameNode == null)
                    throw new InvalidXmlMusicFile("'part-name' was not found in 'score-part' node of a MusicXml file.");

                tracks.Add(
                    new TrackInfo()
                        {
                            Id = xmlTrackIdNode.Value,
                            Name = xmlTrackNameNode.InnerText
                        });
            }

            return tracks;
        }

        public SortedList<GuitarScoreNote, GuitarScoreNote> Import(XmlDocument pXmlDoc, TrackInfo pTrackInfo)
        {
            XmlNodeList notes = this.ListAllNotesOfOneTrack(pXmlDoc, pTrackInfo);

            var scoreNotes = new SortedList<GuitarScoreNote, GuitarScoreNote>();

            this.ConvertNotesInGuitarScoreNote(scoreNotes, notes);

            return scoreNotes;
        }

        public XmlNodeList ListAllNotesOfOneTrack(XmlDocument pXmlDoc, TrackInfo pTrackInfo)
        {
            //select the first track, selecting the node "<part>" with the attribute id="P1"
            var selectedTrack = pXmlDoc.SelectSingleNode("descendant::part[@id=\"P1\"]");

            if (selectedTrack != null)
            {
                //treturn all child nodes <note> or <words>. Words are used to comment chord names or any other kind of hint.
                return selectedTrack.SelectNodes("descendant::*[name()=\"note\" or name()=\"words\"]");
            }

            return null;
        }

        public void ConvertNotesInGuitarScoreNote(SortedList<GuitarScoreNote, GuitarScoreNote> pScoreNotes,
            XmlNodeList pNotes)
        {
            int tick = 0;
            int durationInTicks = 0;
            string remarkOrChordName = null;

            foreach (XmlNode node in pNotes)
            {
                if (node.Name == "words")
                {
                    remarkOrChordName = node.InnerText;
                }
                else if (node.Name == "note")
                {
                    var chord = node.SelectSingleNode("descendant::chord");

                    if (chord != null)
                        tick -= durationInTicks;

                    var type = node.SelectSingleNode("descendant::type");
                    var dot = node.SelectSingleNode("descendant::dot");

                    if (dot == null)
                        durationInTicks = ConvertNoteTypeToTicks(type.InnerText, DottedType.NonDotted);
                    else
                        durationInTicks = ConvertNoteTypeToTicks(type.InnerText, DottedType.Dotted);

                    var step = node.SelectSingleNode("descendant::step");
                    if (step != null)
                    {
                        string noteId = step.InnerText;
                        var alter = node.SelectSingleNode("descendant::alter");

                        if (alter != null)
                        {
                            if (alter.InnerText == "1")
                                noteId += "#";
                        }

                        var octave = node.SelectSingleNode("descendant::octave");
                        noteId += octave.InnerText;

                        int beat = (tick / 480) + 1;

                        var guitarScoreNote = new GuitarScoreNote(noteId, beat, (tick % 480), durationInTicks, 0);


                        var stringNumber = node.SelectSingleNode("descendant::string");
                        if (stringNumber != null)
                            guitarScoreNote.DefaultNotePosition.String = int.Parse(stringNumber.InnerText);

                        var fret = node.SelectSingleNode("descendant::fret");
                        if (fret != null)
                            guitarScoreNote.DefaultNotePosition.Fret = int.Parse(fret.InnerText);

                        if (remarkOrChordName != null)
                        {
                            guitarScoreNote.RemarkOrChordName = remarkOrChordName;
                            remarkOrChordName = null;
                        }

                        pScoreNotes.Add(guitarScoreNote, guitarScoreNote);

                    }

                    tick += durationInTicks;
                }
            }
        }

        public int ConvertNoteTypeToTicks(string pNoteType, DottedType pDottedType)
        {
            int ticks;

            switch (pNoteType)
            {
                case "whole":
                    ticks = 1920;
                    break;
                case "half":
                    ticks = 960;
                    break;
                case "quarter":
                    ticks = 480;
                    break;
                case "eighth":
                    ticks = 240;
                    break;
                case "16th":
                    ticks = 120;
                    break;
                case "32th":
                    ticks = 60;
                    break;
                default:
                    throw new Exception(string.Format("Note type \"{0}\" is invalid!", pNoteType));
            }

            if (pDottedType == DottedType.Dotted)
                ticks += (ticks / 2);

            return ticks;
        }
    }
}
