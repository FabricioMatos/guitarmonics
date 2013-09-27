using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.AudioLib.MusicConfigFiles;

namespace Guitarmonics.GameLib.Model
{
    public class InvalidEndPosition : Exception
    {
        public InvalidEndPosition(string pMessage)
            : base(pMessage)
        {
        }
    }

    public class InsufficientPins : Exception
    {
        public InsufficientPins(string pMessage)
            : base(pMessage)
        {
        }
    }

    /// <summary>
    /// Table used to represent all moments of some music.
    /// We choose represent each 10 ticks (1 beat was diveded in 48 moments)
    /// </summary>
    public class GtTickDataTable
    {
        public GtTickDataTable(int pNumberOfBeats)
        {
            Initialize(pNumberOfBeats);
        }

        public GtTickDataTable(int pNumberOfBeats, int pNumberOfTicks)
        {
            if (pNumberOfTicks > 0)
                pNumberOfBeats++;

            Initialize(pNumberOfBeats);
        }

        private long fNumberOfBeats = 0;

        public long NumberOfBeats
        {
            get
            {
                return fNumberOfBeats;
            }
        }

        /// <summary>
        /// Allocate memory for the table
        /// </summary>
        /// <param name="pNumberOfBeats"></param>
        private void Initialize(int pNumberOfBeats)
        {
            this.fNumberOfBeats = pNumberOfBeats;

            //instantiate the multidimensional array
            this.fItems = new GtTickData[pNumberOfBeats, 48];

            //instantiate each GtTickData
            for (long beat = 0; beat < pNumberOfBeats; beat++)
            {
                for (long tick = 0; tick < 48; tick++)
                {
                    this.fItems[beat, tick] = new GtTickData();
                    this.fItems[beat, tick].Beat = (int)beat + 1;
                    this.fItems[beat, tick].Tick = (int)tick * 10;
                }
            }
        }

        /// <summary>
        /// Internal field made public just for enable some unit tests.
        /// Shouldn't be used directly.
        /// </summary>
        public GtTickData[,] fItems;

        /// <summary>
        /// Return the GtTickData at position Beat:Tick.
        /// Note that Beat starts at 1 and Tick starts at 0.
        /// </summary>
        /// <param name="pBeat">Beat</param>
        /// <param name="pTick">Tick</param>
        /// <returns></returns>
        public GtTickData this[long pBeat, long pTick]
        {
            get 
            {
                if (pBeat <= 0)
                    throw new InvalidBeatValue(string.Format("The beat:tick {0}:{1} is invalid. The first beat value is 1.", pBeat, pTick));

                if (pBeat > this.fNumberOfBeats)
                    throw new InvalidBeatValue(string.Format("The beat:tick {0}:{1} is invalid. The last valid beat is {2}.", pBeat, pTick, this.fNumberOfBeats));

                if (pTick > 470)
                    throw new InvalidTickValue(string.Format("The beat:tick {0}:{1} is invalid. The last valid tick is 470.", pBeat, pTick));

                if ((pTick % 10) > 0)
                    throw new InvalidTickValue(string.Format("The beat:tick {0}:{1} is invalid. The tick value must be multiple of 10.", pBeat, pTick));

                return fItems[pBeat-1, pTick/10]; 
            }
        }

        public void AddTickData(BeatTick pStartPosition, BeatTick pEndPosition, GtTickData pTickData)
        {
            if (pStartPosition > pEndPosition)
                throw new InvalidEndPosition("StartPosition can't be greater then EndPosition");

            this[pStartPosition.Beat, pStartPosition.Tick].MomentInMiliseconds = pTickData.MomentInMiliseconds;

            for (BeatTick pos = pStartPosition; pos <= pEndPosition; pos = pos.AddTicks(10))
            {
                this[pos.Beat, pos.Tick].RemarkOrChordName = pTickData.RemarkOrChordName;

                this[pos.Beat, pos.Tick].String1 = pTickData.String1;
                this[pos.Beat, pos.Tick].String2 = pTickData.String2;
                this[pos.Beat, pos.Tick].String3 = pTickData.String3;
                this[pos.Beat, pos.Tick].String4 = pTickData.String4;
                this[pos.Beat, pos.Tick].String5 = pTickData.String5;
                this[pos.Beat, pos.Tick].String6 = pTickData.String6;

                this[pos.Beat, pos.Tick].IsStartTick = false;
                this[pos.Beat, pos.Tick].IsEndTick = false;
            }

            //mark the first as StartTick
            this[pStartPosition.Beat, pStartPosition.Tick].IsStartTick = true;

            //mark the last as EndTick
            this[pEndPosition.Beat, pEndPosition.Tick].IsEndTick = true;
        }

        public void AutoCompleteMomentInMiliseconds()
        {
            var listOfBeatTickMoment = ConvertToListOfIBeatTickMoment();

            var pinnedBeatTickMoments = listOfBeatTickMoment.Where(p => (p.MomentInMiliseconds != null) && (p.MomentInMiliseconds > 0)).ToArray();

            if (pinnedBeatTickMoments.Count() < 2)
                throw new InsufficientPins(
                    "Data table must have at least 2 pinned itens with not null MomentInMiliseconds.");

            var beatTickMomentAutoCompleteHelper = new BeatTickMomentAutoCompleteHelper();

            beatTickMomentAutoCompleteHelper.AutoCompleteEmptyMoments(listOfBeatTickMoment);

        }

        protected List<IBeatTickMoment> ConvertToListOfIBeatTickMoment()
        {
            var listOfBeatTickMoment = new List<IBeatTickMoment>();

            for (long beat = 0; beat < this.NumberOfBeats; beat++)
            {
                for (long tick = 0; tick < 48; tick++)
                {
                    {
                        //this.fItems[beat, tick].Beat = (int)beat+1;
                        //this.fItems[beat, tick].Tick = (int)tick * 10;
                        listOfBeatTickMoment.Add(this.fItems[beat, tick]);
                    }
                }
            }

            return listOfBeatTickMoment;
        }

        public void UpdateSync(List<BeatTickMoment> pSyncElements)
        {
            foreach (var element in pSyncElements)
            {
                this.fItems[element.Beat-1, element.Tick/10].MomentInMiliseconds = element.MomentInMiliseconds;
            }
        }
    }

    /// <summary>
    /// Represent the notes (and aditional information) of some music moment
    /// </summary>
    public class GtTickData : IBeatTickMoment
    {
        public GtTickData()
        {
            this.IsStartTick = false;
            this.IsEndTick = false;
            this.RemarkOrChordName = "";
            this.DrawChord = false;
        }

        public int? String1 { get; set; }
        public int? String2 { get; set; }
        public int? String3 { get; set; }
        public int? String4 { get; set; }
        public int? String5 { get; set; }
        public int? String6 { get; set; }
        public long? MomentInMiliseconds { get; set; }
        public int? Beat { get; set; }
        public int? Tick { get; set; }
        public bool DrawChord { get; set; }

        //TODO: Essas propriedades deveriam existir para cada corda. Pensar no problema de um arpejo cujas notas continuem soando.
        public bool IsStartTick { get; set; }
        public bool IsEndTick { get; set; }

        public string RemarkOrChordName { get; set; }

        public override string ToString()
        {
            return string.Format("GtTickData: [{0}, {1}] - {2}ms", this.Beat, this.Tick, this.MomentInMiliseconds);
        }

        #region IBeatTickMoment Members

        int IBeatTickMoment.Beat
        {
            get { return (this.Beat == null ? 0 : this.Beat.Value); }
        }

        int IBeatTickMoment.Tick
        {
            get { return (this.Tick == null ? 0 : this.Tick.Value); }
        }

        long? IBeatTickMoment.MomentInMiliseconds
        {
            get
            {
                return this.MomentInMiliseconds;
            }
            set
            {
                this.MomentInMiliseconds = value;
            }
        }

        #endregion
    }

    public class InvalidTickValue : Exception
    {
        public InvalidTickValue(string pMessage)
            : base(pMessage)
        {
        }
    }

    public class InvalidBeatValue : Exception
    {
        public InvalidBeatValue(string pMessage)
            : base(pMessage)
        {
        }
    }

}
