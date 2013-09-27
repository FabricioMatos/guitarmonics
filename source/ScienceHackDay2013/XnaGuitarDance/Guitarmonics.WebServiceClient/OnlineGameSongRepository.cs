using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using Guitarmonics.WebServiceClient.localhost;

namespace Guitarmonics.WebServiceClient
{
    /// <summary>
    /// IGameSongRepository with online access to a webservice
    /// </summary>
    public class OnlineGameSongRepository : IGameSongRepository
    {
        public SongVersionInformationList ListAllSongVersionsSortedByArtistName(string pUserName, string pPasswordHash)
        {
            using (var gameService = new GameService())
            {
                return gameService.ListAllSongVersionsSortedByArtistName(pUserName, pPasswordHash);
            }
        }

        public string GetSongVersionTablature(string pUserName, string pPasswordHash, Guid pOid)
        {
            using (var service = new GameService())
            {
                return service.GetSongVersionTablature(pUserName, pPasswordHash, pOid);
            }
        }

        public string GetSongVersionSynchronization(string pUserName, string pPasswordHash, Guid pOid)
        {
            using (var service = new GameService())
            {
                return service.GetSongVersionSynchronization(pUserName, pPasswordHash, pOid);
            }
        }
    }
}
