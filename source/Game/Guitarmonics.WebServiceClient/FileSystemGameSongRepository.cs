using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.WebServiceClient.localhost;
using System.IO;

namespace Guitarmonics.WebServiceClient
{
    /// <summary>
    /// IGameSongRepository with online access to a webservice
    /// </summary>
    public class FileSystemGameSongRepository: IGameSongRepository
    {
        public string SongsFolder
        {
            get { return System.Configuration.ConfigurationManager.AppSettings["DataFolder"] + "Songs\\"; }
        }

        public SongVersionInformationList ListAllSongVersionsSortedByArtistName(string pUserName, string pPasswordHash)
        {
            string[] folders = Directory.GetDirectories(SongsFolder);

            var songs = new List<SongVersionInformation>();

            foreach (var folder in folders)
            {
                if (File.Exists(folder + "\\description.txt"))
                {
                    var songVersionInformation = ReadSongVersionInformation(folder + "\\description.txt");

                    songs.Add(songVersionInformation);
                }
            }

            var result = new SongVersionInformationList();

            result.Items = new SongVersionInformation[songs.Count];
            songs.CopyTo(result.Items);

            return result;
        }

        private SongVersionInformation ReadSongVersionInformation(string pFileName)
        {
            var folderName = Path.GetDirectoryName(pFileName).Split(new string[] { "\\" }, StringSplitOptions.RemoveEmptyEntries).Last<string>();

            var descriptionFile = File.ReadAllLines(pFileName);

            if (descriptionFile.Count() < 3)
                throw new Exception("File [" + pFileName + "] must have 3 lines with Artis, Album and Song Title.");

            return new SongVersionInformation()
            {
                Id = folderName,
                Artist = descriptionFile[0],
                Album = descriptionFile[1],
                Song = descriptionFile[2],
            };
        }

        public string GetSongVersionTablature(string pUserName, string pPasswordHash, Guid pOid)
        {
            throw new Exception("Off-line repository should not try to download tablature files");
        }

        public string GetSongVersionSynchronization(string pUserName, string pPasswordHash, Guid pOid)
        {
            throw new Exception("Off-line repository should not try to download sync files");
        }
    }
}
