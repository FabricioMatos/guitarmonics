using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib.Model;
using Microsoft.Xna.Framework.Graphics;
using NUnit.Framework;
using Guitarmonics.AudioLib.Common;
using Microsoft.Xna.Framework;

namespace Guitarmonics.GameLib.Controller.Test.ModelTests
{
    [TestFixture]
    public class MusicalNoteExtensionTests
    {
        [Test]
        public void AllC()
        {
            var note3 = new MusicalNote("C3");
            var note4 = new MusicalNote("C4");
            var note5 = new MusicalNote("C5");
            var note6 = new MusicalNote("C6");
            var note7 = new MusicalNote("C7");

            Assert.AreEqual(note3.NoteColor(), note4.NoteColor());
            Assert.AreEqual(note4.NoteColor(), note5.NoteColor());
            Assert.AreEqual(note5.NoteColor(), note6.NoteColor());
            Assert.AreEqual(note6.NoteColor(), note7.NoteColor());
        }

        
        [Test]
        public void C()
        {
            var note = new MusicalNote("C3");

            Assert.AreEqual((Color)new HSLColor(160, 240, 100), note.NoteColor());
        }

        [Test]
        public void F()
        {
            var note = new MusicalNote("E3");

            Assert.AreEqual((Color)new HSLColor(80, 240, 100), note.NoteColor());
        }

        [Test]
        public void Ab()
        {
            var note = new MusicalNote("Ab3");

            Assert.AreEqual((Color)new HSLColor(0, 240, 100), note.NoteColor());
        }
    }
}
