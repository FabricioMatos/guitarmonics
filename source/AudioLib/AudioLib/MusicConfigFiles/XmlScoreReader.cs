using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Xml;
using Guitarmonics.AudioLib.Common;

namespace Guitarmonics.AudioLib.MusicConfigFiles
{
    //TODO: Apenas usar XmlScoreSyncReader e XmlScoreNotesReader e retornar o mesmo conteudo mesclado de notas e Sync

    public class XmlScoreReader : XmlScoreNotesReader
    {
        public XmlScoreReader(string pFileName)
            : base(pFileName)
        {
            AutoCompleteEmptyMoments(
                fScoreNotes.Cast<IBeatTickMoment>().Select(x => x as IBeatTickMoment).ToList());
        }

        public XmlScoreReader(string pXmlNotesFileName, string pXmlSyncFileName)
            : base(pXmlNotesFileName)
        {
            this.fXmlScoreSyncReader = new XmlScoreSyncReader(pXmlSyncFileName);

            MergeWithSyncFile(this.fXmlScoreSyncReader);

            AutoCompleteEmptyMoments(
                fScoreNotes.Cast<IBeatTickMoment>().Select(x => x as IBeatTickMoment).ToList());
        }

        private XmlScoreSyncReader fXmlScoreSyncReader;
        public List<BeatTickMoment> SyncElements
        {
            get { return fXmlScoreSyncReader.SyncElements; }
        }

        public void MergeWithSyncFile(XmlScoreSyncReader pXmlScoreSyncReader)
        {
            foreach (var elment in pXmlScoreSyncReader.SyncElements)
            {
                UpdateMoment(elment);
            }
        }

        private void UpdateMoment(IBeatTickMoment pBeatTickMoment)
        {
            foreach (var note in this.fScoreNotes)
            {
                if ((note.Beat == pBeatTickMoment.Beat) && (note.Tick == pBeatTickMoment.Tick))
                {
                    note.MomentInMiliseconds = pBeatTickMoment.MomentInMiliseconds;
                }

                if (note.Beat > pBeatTickMoment.Beat) 
                    break;
            }
        }


        protected void AutoCompleteEmptyMoments(List<IBeatTickMoment> pScoreNotes)
        {
            var beatTickMomentAutoCompleteHelper = new BeatTickMomentAutoCompleteHelper();

            beatTickMomentAutoCompleteHelper.AutoCompleteEmptyMoments(pScoreNotes);
        }
    }
}
