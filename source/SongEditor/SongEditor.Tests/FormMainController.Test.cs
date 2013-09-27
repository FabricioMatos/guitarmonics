using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace SongEditor.Tests
{
    [TestFixture]
    public class FormMainControllerTest
    {
        private const string MidiFileMetallica =
            @"D:\_GuitarMonics\svn-guitarmonics\trunk\Fontes\SongEditor\SongFilesForTest\Metallica - For Whom The Bell Tolls\metallica-for_whom_the_bell_tolls.mid";
        private const string XmlFileMetallica =
            @"D:\_GuitarMonics\svn-guitarmonics\trunk\Fontes\SongEditor\SongFilesForTest\Metallica - For Whom The Bell Tolls\metallica-for_whom_the_bell_tolls.song.xml";

        private const string MidiFileMegadeath =
            @"D:\_GuitarMonics\svn-guitarmonics\trunk\Fontes\SongEditor\SongFilesForTest\Megadeth-Hanger 18\Megadeth - Hanger 18 (final).mid";
        private const string XmlFileMegadeath =
            @"D:\_GuitarMonics\svn-guitarmonics\trunk\Fontes\SongEditor\SongFilesForTest\Megadeth-Hanger 18\Megadeth - Hanger 18 (final).song.xml";


        [Test]
        public void ListAllTracksWorks()
        {
            var controller = new FormMainController();
            var tracks = controller.ListAllTracks(MidiFileMetallica);

            Assert.AreEqual(9, tracks.Count);

            Assert.AreEqual("Vocals", tracks[0].Name);
            Assert.AreEqual("Guitar 1", tracks[1].Name);
            Assert.AreEqual("Guitar 2", tracks[2].Name);
            Assert.AreEqual("Guitar 3", tracks[3].Name);
            Assert.AreEqual("Guitar 4", tracks[4].Name);
            Assert.AreEqual("Bass 1", tracks[5].Name);
            Assert.AreEqual("Bass 2", tracks[6].Name);
            Assert.AreEqual("Bells", tracks[7].Name);
            Assert.AreEqual("Drums", tracks[8].Name);
        }


        //[TestCase(XmlFileMetallica, MidiFileMetallica, 1 /*Guitar 1*/, "Metallica", "Ride the Lightning", "For Whom The Bell Tolls")]
        [TestCase(XmlFileMegadeath, MidiFileMegadeath, 1, "Megadeath", "Rust in Peace", "Hangar 18")]
        public void CreateXmlFileFromMidi(string pXmlFile, string pMidiFile, int pTrack, 
            string pArtist, string pAlbum, string pSong)
        {
            if (File.Exists(pXmlFile))
                File.Delete(pXmlFile);

            var controller = new FormMainController();

            Assert.IsFalse(File.Exists(pXmlFile));

            controller.CreateXmlFileFromMidi(pMidiFile, pTrack, pXmlFile, pArtist, pAlbum, pSong);

            Assert.IsTrue(File.Exists(pXmlFile));
        }
  
    }
}
