using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.WebServiceClient.localhost;

namespace Guitarmonics.WebServiceClient
{
    /// <summary>
    /// Allow mock the GameService behaviour.
    /// </summary>
    public interface IGameSongRepository
    {
        //SongVersionInformationList  should be a ISongVersionInformationList
        SongVersionInformationList ListAllSongVersionsSortedByArtistName(string pUserName, string pPasswordHash);

        string GetSongVersionTablature(string pUserName, string pPasswordHash, Guid pOid);

        string GetSongVersionSynchronization(string pUserName, string pPasswordHash, Guid pOid);
    }
}
