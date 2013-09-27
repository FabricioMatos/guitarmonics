using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Analysis;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.GameLib.ControllerTest;
using Guitarmonics.GameLib.Model;
using Guitarmonics.GameLib.Model;
using NUnit.Framework;

namespace Guitarmonics.GameLib.Controller.Test.ModelTests
{
    [TestFixture]
    public class GtPlayedNotesAnalyserHelperTest
    {
        [Test]
        public void NoteIsPlayingSignature()
        {
            IGtPlayedNotesAnalyserHelper analyserHelper = new GtPlayedNotesAnalyserHelper(
                new GtFactory(),
                new DoubleAudioListenerDoNothing(40));

            var note = new GtSceneGuitarNote(
                new BeatTick(1, 0),
                new BeatTick(2, 0),
                6,
                0); //E3

            Assert.IsFalse(analyserHelper.NoteIsPlaying(note));
        }

        [Test]
        public void LastQueriedPlayedNotes()
        {
            IGtPlayedNotesAnalyserHelper analyserHelper = new GtPlayedNotesAnalyserHelper(
                new GtFactoryMockedClock(),
                new DoubleAudioListenerDoNothing(40));

            var note = new GtSceneGuitarNote(
                new BeatTick(1, 0),
                new BeatTick(2, 0),
                6,
                0); //E3

            Assert.IsFalse(analyserHelper.NoteIsPlaying(note));

            //Add the "E3" to the LastQueriedPlayedNotes list
            var playedNote = new MusicalNoteAndTimeStamp();
            playedNote.MusicalNote = new MusicalNote("E3");
            playedNote.TimeStamp = new DateTime(2010, 1, 1, 0, 0, 0, 0);

            analyserHelper.LastQueriedPlayedNotes.Add(playedNote);

            //50 ms after playedNote added to LastQueriedPlayedNotes
            GtFactoryMockedClock.MockCurrentDateTime = new DateTime(2010, 1, 1, 0, 0, 0, 
                GtPlayedNotesAnalyserHelper.DELAY_TIME_FOR_PLAYING_NOTES_RECOGNITION);
            Assert.IsTrue(analyserHelper.NoteIsPlaying(note));

            //51 ms after playedNote added to LastQueriedPlayedNotes
            GtFactoryMockedClock.MockCurrentDateTime = new DateTime(2010, 1, 1, 0, 0, 0, 
                GtPlayedNotesAnalyserHelper.DELAY_TIME_FOR_PLAYING_NOTES_RECOGNITION + 1);
            Assert.IsFalse(analyserHelper.NoteIsPlaying(note));

            //The expired playedNote was removed from analyserHelper.LastQueriedPlayedNotes
            Assert.AreEqual(0, analyserHelper.LastQueriedPlayedNotes.Count);
        }

        [Test]
        public void NoteIsPlayinAddInLastQueriedPlayedNotes()
        {
            IGtPlayedNotesAnalyserHelper analyserHelper = new GtPlayedNotesAnalyserHelperMock(
                new GtFactoryMockedClock(),
                new DoubleAudioListenerDoNothing(40));

            var note = new GtSceneGuitarNote(
                new BeatTick(1, 0),
                new BeatTick(2, 0),
                6,
                0); //E3

            GtFactoryMockedClock.MockCurrentDateTime = new DateTime(2010, 1, 1, 0, 0, 0, 0);
            GtPlayedNotesAnalyserHelperMock.ValueForNoteIsPlaying = true;
            Assert.IsTrue(analyserHelper.NoteIsPlaying(note)); //note is added to LastQueriedPlayedNotes list

            //50 ms after playedNote added to LastQueriedPlayedNotes
            GtFactoryMockedClock.MockCurrentDateTime = new DateTime(2010, 1, 1, 0, 0, 0,
                GtPlayedNotesAnalyserHelper.DELAY_TIME_FOR_PLAYING_NOTES_RECOGNITION);
            GtPlayedNotesAnalyserHelperMock.ValueForNoteIsPlaying = false;
            Assert.IsTrue(analyserHelper.NoteIsPlaying(note));

            //51 ms after playedNote added to LastQueriedPlayedNotes
            GtFactoryMockedClock.MockCurrentDateTime = new DateTime(2010, 1, 1, 0, 0, 0,
                GtPlayedNotesAnalyserHelper.DELAY_TIME_FOR_PLAYING_NOTES_RECOGNITION + 1);
            Assert.IsFalse(analyserHelper.NoteIsPlaying(note));

            //The expired playedNote was removed from analyserHelper.LastQueriedPlayedNotes
            Assert.AreEqual(0, analyserHelper.LastQueriedPlayedNotes.Count);
        }

    }

    public class GtFactoryMockedClock : GtFactory
    {
        public GtFactoryMockedClock()
        {
            this.Clock = new MockedSystemClock();
        }

        public static DateTime MockCurrentDateTime;

        public class MockedSystemClock : IClock
        {
            public DateTime CurrentDateTime
            {
                get
                {
                    return MockCurrentDateTime;
                }
            }
        }

    }

    public class GtPlayedNotesAnalyserHelperMock : GtPlayedNotesAnalyserHelper
    {
        public GtPlayedNotesAnalyserHelperMock(GtFactory pFactory, IAudioListener pAudioListener)
            : base(pFactory, pAudioListener)
        {

        }

        public static bool ValueForNoteIsPlaying = false;

        protected override bool CheckIfNoteIsPlaying(GtSceneGuitarNote pSceneGuitarNote)
        {
            //return base.CheckIfNoteIsPlaying(pSceneGuitarNote);
            return ValueForNoteIsPlaying;
        }
    }
}
