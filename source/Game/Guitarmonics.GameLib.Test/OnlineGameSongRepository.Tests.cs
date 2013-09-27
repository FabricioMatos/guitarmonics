using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Guitarmonics.WebServiceClient;

namespace Guitarmonics.GameLib.Test
{
    [TestFixture]
    public class OnlineGameSongRepositoryTests
    {
        [Test]
        public void ListAllSongVersionsSortedByArtistNameShouldReturnCorrectData()
        {
            var fileSystemGameSongRepository = new FileSystemGameSongRepository();

            var songList = fileSystemGameSongRepository.ListAllSongVersionsSortedByArtistName("", "");

            var firstSong = songList.Items.Where(s => s.Id == "Megadeth.RustInPeace.Hangar18").FirstOrDefault();

            Assert.AreEqual("Megadeth.RustInPeace.Hangar18", firstSong.Id);
            Assert.AreEqual("Megadeath", firstSong.Artist);
            Assert.AreEqual("Rust in Peace", firstSong.Album);
            Assert.AreEqual("Hangar 18", firstSong.Song);
        }

        [Test]
        public void ListAllSongVersionsSortedByArtistNameShouldReturnAllSongsInTheDataFolder()
        {
            var fileSystemGameSongRepository = new FileSystemGameSongRepository();

            var songList = fileSystemGameSongRepository.ListAllSongVersionsSortedByArtistName("", "");

            Assert.AreEqual(4, songList.Items.Count());
        }

    }
}
