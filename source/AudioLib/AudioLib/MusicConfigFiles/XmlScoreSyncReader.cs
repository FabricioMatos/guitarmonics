using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Common;
using System.Xml;

namespace Guitarmonics.AudioLib.MusicConfigFiles
{
    public class BeatTickMoment : IBeatTickMoment
    {
        public BeatTickMoment(int pBeat, int pTick, long? pMomentInMiliseconds)
        {
            this.Beat = pBeat;
            this.Tick = pTick;
            this.MomentInMiliseconds = pMomentInMiliseconds;
        }

        public int Beat { get; private set;  }
        public int Tick { get; private set;  }
        public long? MomentInMiliseconds { get; set; }
    }


    public class XmlScoreSyncReader : XmlScoreReaderBase
    {
        public XmlScoreSyncReader(string pFileName)
            : base(pFileName)
        {
            //AutoCompleteEmptyMoments(
            //    fScoreNotes.Cast<IBeatTickMoment>().Select(x => x as IBeatTickMoment).ToList());
        }

        protected long? fMomentInMiliseconds;

        private List<BeatTickMoment> fSyncElements = new List<BeatTickMoment>();
        public List<BeatTickMoment> SyncElements
        {
            get { return fSyncElements; }
        }

        protected override void ClearProperties()
        {
            base.ClearProperties();

            this.fMomentInMiliseconds = null;
        }

        protected override void ParseXmlProperties(XmlReader pXmlReader)
        {
            base.ParseXmlProperties(pXmlReader);

            if (pXmlReader.Name == "SyncSongPin")
            {
                if (pXmlReader.Value != null)
                {
                    var syncSongPinValue = pXmlReader.Value;
                    string[] values = syncSongPinValue.Split(new char[] { ':' });

                    fMomentInMiliseconds = 0;

                    fMomentInMiliseconds += long.Parse(values[0]) * 60000;  //min
                    fMomentInMiliseconds += long.Parse(values[1]) * 1000;   //sec
                    fMomentInMiliseconds += long.Parse(values[2]);          //milisec
                }
            }
        }

        protected override void AddNoteToCollection()
        {
            var scoreNote = new BeatTickMoment((int)fBeat, (int)fTick, fMomentInMiliseconds);

            this.fSyncElements.Add(scoreNote);
        }
    }
}
