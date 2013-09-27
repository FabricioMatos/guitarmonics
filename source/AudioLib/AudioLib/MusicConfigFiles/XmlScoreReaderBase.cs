using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Xml;
using Guitarmonics.AudioLib.Common;
using System.Globalization;

namespace Guitarmonics.AudioLib.MusicConfigFiles
{
    public class XmlScoreReaderBase
    {
        public XmlScoreReaderBase(string pFileName)
        {
            this.Pitch = 0;

            this.OpenXmlNotesFile(pFileName);

            this.LoadScoreNotes();
        }

        public float Pitch { get; private set; }
        public string Artist { get; private set; }
        public string Title { get; private set; }

        protected string fFileName;
        protected XmlReader fXmlReader;
        protected int? fBeat;
        protected int? fTick;

        protected virtual void OpenXmlNotesFile(string pFileName)
        {
            if (!File.Exists(pFileName))
            {
                throw new InvalidFileName(string.Format("Could't find the file \"{0}\".", pFileName));
            }

            this.fFileName = pFileName;

            var settings = new XmlReaderSettings();
            settings.ConformanceLevel = ConformanceLevel.Document;
            settings.IgnoreWhitespace = true;
            settings.IgnoreComments = true;

            fXmlReader = XmlReader.Create(pFileName, settings);
        }

        protected virtual void LoadScoreNotes()
        {
            // Read the first node and make sure it says "Song"
            if (!(fXmlReader.ReadToFollowing("Song")) && fXmlReader.Name != "Song")
                throw new InvalidSongFile(string.Format("Invalid Song XML document \"{0}\".", fFileName));


            //Read attributes of node "Song"
            fXmlReader.MoveToFirstAttribute();
            do
            {
                if (fXmlReader.Name == "Artist")
                    this.Artist = fXmlReader.Value;

                if (fXmlReader.Name == "Title")
                    this.Title = fXmlReader.Value;

                if (fXmlReader.Name == "Pitch")
                    this.Pitch = float.Parse(fXmlReader.Value, CultureInfo.InvariantCulture.NumberFormat);

            } while (fXmlReader.MoveToNextAttribute());


            //read the notes
            while (fXmlReader.Read())
            
            {
                if (fXmlReader.Name == "ScoreNote")
                {
                    ClearProperties();

                    fXmlReader.MoveToFirstAttribute();

                    do
                    {
                        ParseXmlProperties(fXmlReader);
                    } while (fXmlReader.MoveToNextAttribute());

                    //TODO: Validation: all fields found?

                    this.AddNoteToCollection();

                }
            }
        }

        protected virtual void ClearProperties()
        {
            this.fBeat = null;
            this.fTick = null;
            //this.fNoteId = "";
            //this.fDuration = null;
            //this.fString = null;
            //this.fFret = null;
            //this.fRemarkOrChordName = "";
        }

        protected virtual void ParseXmlProperties(XmlReader pXmlReader)
        {
            if (pXmlReader.Name == "Beat")
                this.fBeat = int.Parse(pXmlReader.Value);

            if (pXmlReader.Name == "Tick")
                this.fTick = int.Parse(pXmlReader.Value);

            //if (pXmlReader.Name == "NoteId")
            //    this.fNoteId = pXmlReader.Value;

            //if (pXmlReader.Name == "Duration")
            //    this.fDuration = int.Parse(pXmlReader.Value);

            //if (pXmlReader.Name == "String")
            //    this.fString = int.Parse(pXmlReader.Value);

            //if (pXmlReader.Name == "Fret")
            //    this.fFret = int.Parse(pXmlReader.Value);

            //if (pXmlReader.Name == "RemarkOrChordName")
            //    this.fRemarkOrChordName = pXmlReader.Value;
        }

        protected virtual void AddNoteToCollection()
        {
        }

        //protected void AutoCompleteEmptyMoments(List<IBeatTickMoment> pScoreNotes)
        //{
        //    var beatTickMomentAutoCompleteHelper = new BeatTickMomentAutoCompleteHelper();

        //    beatTickMomentAutoCompleteHelper.AutoCompleteEmptyMoments(pScoreNotes);
        //}
    }

    public class BeatTickMomentAutoCompleteHelper
    {
        public void AutoCompleteEmptyMoments(List<IBeatTickMoment> pScoreNotes)
        {
            var pinnedNotes = pScoreNotes.Where(p => (p.MomentInMiliseconds != null) && (p.MomentInMiliseconds > 0)).ToArray();

            //look for the sencond score note in pinnedNotes (pin B)
            int currentPinnedNotePosition = GoToNextPinnedPosition(0, pinnedNotes);;

            int currentNotePosition = 0; //first score note in pScoreNotes (can be before pin A)

            double tickTimeInMiliseconds = 0;

            long momentInMilisecondsPinA = 0;
            long momentInMilisecondsPinB = 0;
            long momentInTicksPinA = 0;
            long momentInTicksPinB = 0;

            //We need at least 2 pins in order to calculate the BPM
            while (currentPinnedNotePosition < pinnedNotes.Count())
            {
                //Pinned Position en Miliseconds
                momentInMilisecondsPinA =
                    pinnedNotes.ElementAt(currentPinnedNotePosition - 1).MomentInMiliseconds.Value;

                momentInMilisecondsPinB =
                    pinnedNotes.ElementAt(currentPinnedNotePosition).MomentInMiliseconds.Value;


                //Pinned Position en Ticks
                momentInTicksPinA = pinnedNotes.ElementAt(currentPinnedNotePosition - 1).MomentInTicks();

                momentInTicksPinB = pinnedNotes.ElementAt(currentPinnedNotePosition).MomentInTicks();

                //Deltas and Average BPM (from tickTimeInMiliseconds)
                long deltaTime = momentInMilisecondsPinB - momentInMilisecondsPinA;

                long deltaTicks = momentInTicksPinB - momentInTicksPinA;

                tickTimeInMiliseconds = (double)deltaTime / (double)deltaTicks;

                //Update ScoreNotes (until the last pinned note)
                while ((currentNotePosition < pScoreNotes.Count)
                    && (pScoreNotes[currentNotePosition] != pinnedNotes.ElementAt(currentPinnedNotePosition)))
                {
                    if ((pScoreNotes[currentNotePosition].MomentInMiliseconds == null) || 
                        (pScoreNotes[currentNotePosition].MomentInMiliseconds == 0))
                    {
                        pScoreNotes[currentNotePosition].MomentInMiliseconds =
                            (long)Math.Round(momentInMilisecondsPinA + tickTimeInMiliseconds *
                            (pScoreNotes[currentNotePosition].MomentInTicks() - momentInTicksPinA));
                    }
                    currentNotePosition++;
                }

                currentNotePosition++;
                currentPinnedNotePosition = GoToNextPinnedPosition(currentPinnedNotePosition, pinnedNotes); ;
                //currentPinnedNotePosition++;
            }

            //update notes from the last pinned not to the end
            while (currentNotePosition < pScoreNotes.Count)
            {
                if ((pScoreNotes[currentNotePosition].MomentInMiliseconds == null) &&
                    (tickTimeInMiliseconds > 0))
                {
                    pScoreNotes[currentNotePosition].MomentInMiliseconds =
                        (long)Math.Round(momentInMilisecondsPinA + tickTimeInMiliseconds *
                        (pScoreNotes[currentNotePosition].MomentInTicks() - momentInTicksPinA));
                }

                currentNotePosition++;
            }
        }

        private int GoToNextPinnedPosition(int pCurrentPinnedNotePosition, IBeatTickMoment[] pinnedNotes)
        {
            var originalPinnedNotePosition = pCurrentPinnedNotePosition;
            while ((pCurrentPinnedNotePosition < pinnedNotes.Count()) &&
                   (pinnedNotes[pCurrentPinnedNotePosition].MomentInTicks() == pinnedNotes[originalPinnedNotePosition].MomentInTicks()))
            {
                pCurrentPinnedNotePosition++;
            }
            return pCurrentPinnedNotePosition;
        }
    }

}
