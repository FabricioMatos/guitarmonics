using System;
using System.Collections.Generic;
using System.Configuration;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Player;
using Guitarmonics.GameLib.Model;
using Guitarmonics.AudioLib.MusicConfigFiles;
using Guitarmonics.AudioLib.Common;
using System.ServiceModel;
using Guitarmonics.WebServiceClient;
using Guitarmonics.WebServiceClient.localhost;
using System.IO;

namespace Guitarmonics.GameLib.Controller
{

    public class GtFileLoaderDoubleWebService : GtFileLoader
    {
        internal static class TestConfig
        {
            public static string AudioPath = @"D:\Guitarmonics-OpenSource\trunk\source\AudioLib\AudioLib.Tests\_FilesForTest\";
        }

        public GtFileLoaderDoubleWebService(GtFactory pFactory)
            : base(pFactory)
        {
        }
    }



    /// <summary>
    /// Class responsible for read config XML files and provide structured data
    /// </summary>
    public class GtFileLoader
    {
        public GtFactory Factory { get; private set; }

        public GtFileLoader()
        {
            this.Factory = GtFactory.Instance;
        }

        public GtFileLoader(GtFactory pFactory)
        {
            this.Factory = pFactory;
        }

        public virtual IList<SongDescription> ListAllSongs()
        {
            var gameService = this.Factory.Instantiate<IGameSongRepository>();
            SongVersionInformationList list = gameService.ListAllSongVersionsSortedByArtistName("teste", "gu1t@rm0n1c5"); //TODO: user account hardcoded for test (note that doesn't make sense in offline mode)

            var result = new List<SongDescription>();

            foreach (var song in list.SongVersionInformationList1)
            {
                result.Add(new SongDescription()
                {
                    Id = song.Id,
                    Artist = song.Artist,
                    Album = song.Album,
                    Song = song.Song,

                    ConfigFileName = System.Configuration.ConfigurationManager.AppSettings["DataFolder"] + "Songs\\" + song.Id + "\\hard.xml",
                    SyncFileName = System.Configuration.ConfigurationManager.AppSettings["DataFolder"] + "Songs\\" + song.Id + "\\sync.xml",
                    AudioFileName = System.Configuration.ConfigurationManager.AppSettings["DataFolder"] + "Songs\\" + song.Id + "\\" + song.Id + ".mp3",

                    TimeSignature = GtTimeSignature.Time4x4,
                });
            }

            return result;
        }

        public virtual void DownloadSong(SongDescription pSelectedSong)
        {
            if (!File.Exists(pSelectedSong.ConfigFileName))
                this.DownloadContent(pSelectedSong);

            if (!File.Exists(pSelectedSong.SyncFileName))
                this.DownloadSynchronization(pSelectedSong);

            //Download audio file (if available)
        }

        public virtual void DownloadContent(SongDescription pSelectedSong)
        {
            var gameService = this.Factory.Instantiate<IGameSongRepository>();

            var hardTablature = gameService.GetSongVersionTablature("teste", "gu1t@rm0n1c5", pSelectedSong.OidHardTablature);

            File.WriteAllText(pSelectedSong.ConfigFileName, hardTablature);
        }

        public virtual void DownloadSynchronization(SongDescription pSelectedSong)
        {
            var gameService = this.Factory.Instantiate<IGameSongRepository>();

            var hardTablature = gameService.GetSongVersionSynchronization("teste", "gu1t@rm0n1c5", pSelectedSong.OidHardTablature);

            File.WriteAllText(pSelectedSong.SyncFileName, hardTablature);
        }


        public virtual void DownloadMp3(SongDescription pSelectedSong)
        {
            //not implemented!!!!
        }


        public virtual GtTickDataTable LoadTickDataTable(ref SongDescription pSongDescription)
        {
            //IList<GuitarScoreNote> scoreNotes = ReadXmlScores(pSongDescription);
            var xmlScoreReader = new XmlScoreReader(pSongDescription.ConfigFileName, pSongDescription.SyncFileName);

            pSongDescription.Pitch = xmlScoreReader.Pitch;

            var tickDataTable = ConvertScoreNotesInTickTable(xmlScoreReader.ScoreNotes);

            tickDataTable.UpdateSync(xmlScoreReader.SyncElements);

            tickDataTable.AutoCompleteMomentInMiliseconds();

            return tickDataTable;
        }

        public virtual int CalculateNumberOfBeats(IList<GuitarScoreNote> pScoreNotes)
        {
            if (pScoreNotes.Count == 0)
                return 1;

            float maxSongPosition = pScoreNotes.Max(p => ((p.Beat - 1) * 480f) + p.Tick + p.DurationInTicks).Value;

            int beats = (int)maxSongPosition / 480;
            if ((maxSongPosition % 480) > 0)
                beats++;

            return beats;
        }

        public const int NUMBER_ADITIONAL_BEATS = 4;
        public virtual GtTickDataTable ConvertScoreNotesInTickTable(IList<GuitarScoreNote> pScoreNotes)
        {
            var tickDataTable = new GtTickDataTable(this.CalculateNumberOfBeats(pScoreNotes) + NUMBER_ADITIONAL_BEATS);

            foreach (var note in pScoreNotes)
            {
                this.FillTickDataTable(tickDataTable, note);
            }

            //TODO: Falta Implementar ConvertScoreMomentsInTickTable

            return tickDataTable;
        }

        public void FillTickDataTable(GtTickDataTable pTickDataTable, GuitarScoreNote pGuitarScoreNote)
        {
            var pos = new BeatTick(pGuitarScoreNote.Beat, pGuitarScoreNote.Tick);

            pTickDataTable[pos.Beat, pos.Tick].IsStartTick = true;
            pTickDataTable[pos.Beat, pos.Tick].MomentInMiliseconds = pGuitarScoreNote.MomentInMiliseconds;
            if (pTickDataTable[pos.Beat, pos.Tick].RemarkOrChordName != string.Empty)
                pTickDataTable[pos.Beat, pos.Tick].RemarkOrChordName += " ";
            pTickDataTable[pos.Beat, pos.Tick].RemarkOrChordName += pGuitarScoreNote.RemarkOrChordName;

            int duration = (int)pGuitarScoreNote.DurationInTicks;

            for (int i = 0; i < duration; i += 10)
            {
                pos = (new BeatTick(pGuitarScoreNote.Beat, pGuitarScoreNote.Tick)).AddTicks(i);

                switch (pGuitarScoreNote.DefaultNotePosition.String)
                {
                    case 1:
                        pTickDataTable[pos.Beat, pos.Tick].String1 = pGuitarScoreNote.DefaultNotePosition.Fret;
                        break;
                    case 2:
                        pTickDataTable[pos.Beat, pos.Tick].String2 = pGuitarScoreNote.DefaultNotePosition.Fret;
                        break;
                    case 3:
                        pTickDataTable[pos.Beat, pos.Tick].String3 = pGuitarScoreNote.DefaultNotePosition.Fret;
                        break;
                    case 4:
                        pTickDataTable[pos.Beat, pos.Tick].String4 = pGuitarScoreNote.DefaultNotePosition.Fret;
                        break;
                    case 5:
                        pTickDataTable[pos.Beat, pos.Tick].String5 = pGuitarScoreNote.DefaultNotePosition.Fret;
                        break;
                    case 6:
                        pTickDataTable[pos.Beat, pos.Tick].String6 = pGuitarScoreNote.DefaultNotePosition.Fret;
                        break;
                }
            }

            pTickDataTable[pos.Beat, pos.Tick].IsEndTick = true;
        }

        //public IList<GuitarScoreNote> ReadXmlScores(SongDescription songDescription)
        //{
        //    var xmlScoreReader = new XmlScoreReader(songDescription.ConfigFileName, songDescription.SyncFileName);
        //    return xmlScoreReader.ScoreNotes;
        //}
    }
}
