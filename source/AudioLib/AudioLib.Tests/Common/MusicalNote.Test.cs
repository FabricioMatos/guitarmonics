using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;

namespace Guitarmonics.AudioLib.Common.Tests
{
    [TestFixture]
    public class MusicalNoteTest
    {
        /// <summary>
        /// Note values like Gb should return "F#" when called .AsSharpedString()
        /// </summary>
        [Test]
        public void NoteValueToStringShouldReturnSharp()
        {
            Assert.AreEqual("C", NoteValue.C.AsSharpedString());
            Assert.AreEqual("C#", NoteValue.Db.AsSharpedString());
            Assert.AreEqual("D", NoteValue.D.AsSharpedString());
            Assert.AreEqual("D#", NoteValue.Eb.AsSharpedString());
            Assert.AreEqual("E", NoteValue.E.AsSharpedString());
            Assert.AreEqual("F", NoteValue.F.AsSharpedString());
            Assert.AreEqual("F#", NoteValue.Gb.AsSharpedString());
            Assert.AreEqual("G", NoteValue.G.AsSharpedString());
            Assert.AreEqual("G#", NoteValue.Ab.AsSharpedString());
            Assert.AreEqual("A", NoteValue.A.AsSharpedString());
            Assert.AreEqual("A#", NoteValue.Bb.AsSharpedString());
            Assert.AreEqual("B", NoteValue.B.AsSharpedString());
        }
    }
}
