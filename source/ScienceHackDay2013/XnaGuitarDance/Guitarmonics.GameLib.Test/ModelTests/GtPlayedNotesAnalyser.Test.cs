using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib.ControllerTest;
using Guitarmonics.GameLib.Model;
using Guitarmonics.AudioLib.Analysis;
using NUnit.Framework;

namespace Guitarmonics.GameLib.Model.Tests
{
    [TestFixture]
    public class GtPlayedNotesAnalyserTest
    {
        [Test]
        public void ConstructUsingTheFactory()
        {
            var factory = new GtFactory();
            var p = factory.Instantiate<IGtPlayedNotesAnalyser>(factory, new DoubleAudioListenerDoNothing(40));

            Assert.IsNotNull(p);
        }

        [Test]
        public void StartHitedAndPlayingAreTrue()
        {
            //setup the factory to use mocks
            var factory = new GtFactory();
            factory.AddMapping<IGtPlayedNotesAnalyserHelper, MockGtPlayedNotesAnalyserHelperAlwaysIsPlaying>();

            var analyser = factory.Instantiate<IGtPlayedNotesAnalyser>(factory, new DoubleAudioListenerDoNothing(40));

            var note = new GtSceneGuitarNote(new BeatTick(1, 0), new BeatTick(1, 240), 6, 0);
            
            var startingNotes = new List<GtSceneGuitarNote>();
            startingNotes.Add(note);

            var expectedPlayingNotes = new List<GtSceneGuitarNote>();
            expectedPlayingNotes.Add(note);

            analyser.Analyse(startingNotes, expectedPlayingNotes);

            Assert.IsTrue(startingNotes[0].StartHited);
            Assert.IsTrue(startingNotes[0].Playing);
        }

        /// <summary>
        /// With StartHited is true, it remains true even if the user is not playing.
        /// </summary>
        [Test]
        public void StartHitedIsNotReset()
        {
            var factory = new GtFactory();
            factory.AddMapping<IGtPlayedNotesAnalyserHelper, MockGtPlayedNotesAnalyserHelperAlwaysIsNotPlaying>();

            var analyser = factory.Instantiate<IGtPlayedNotesAnalyser>(factory, new DoubleAudioListenerDoNothing(40));

            var note = new GtSceneGuitarNote(new BeatTick(1, 0), new BeatTick(1, 240), 6, 0);

            var startingNotes = new List<GtSceneGuitarNote>();
            startingNotes.Add(note);

            var expectedPlayingNotes = new List<GtSceneGuitarNote>();
            expectedPlayingNotes.Add(note);

            note.Playing = true;
            note.StartHited = true;

            analyser.Analyse(startingNotes, expectedPlayingNotes);

            Assert.IsTrue(startingNotes[0].StartHited);
            Assert.IsFalse(startingNotes[0].Playing);
        }

        /// <summary>
        /// When Analyse method don't find any correct note (hitted or playing) it returns zero.
        /// </summary>
        [Test]
        public void AnalyseMustReturnZeroIfNoNoteIsCorrect()
        {
            var factory = new GtFactory();
            factory.AddMapping<IGtPlayedNotesAnalyserHelper, MockGtPlayedNotesAnalyserHelperAlwaysIsNotPlaying>();

            var analyser = factory.Instantiate<IGtPlayedNotesAnalyser>(factory, new DoubleAudioListenerDoNothing(40));

            var note = new GtSceneGuitarNote(new BeatTick(1, 0), new BeatTick(1, 240), 6, 0);

            var startingNotes = new List<GtSceneGuitarNote>();
            startingNotes.Add(note);

            var expectedPlayingNotes = new List<GtSceneGuitarNote>();
            expectedPlayingNotes.Add(note);

            var points = analyser.Analyse(startingNotes, expectedPlayingNotes);
            var maxPoints = analyser.AnalyseMaximum(startingNotes, expectedPlayingNotes);

            Assert.AreEqual(0, points);
            Assert.AreEqual(101, maxPoints);
        }


        /// <summary>
        /// When one note is hitted method returns 100 points.
        /// </summary>
        [Test]
        public void OneHittedNoteShouldReturn100Points()
        {
            var factory = new GtFactory();
            factory.AddMapping<IGtPlayedNotesAnalyserHelper, MockGtPlayedNotesAnalyserHelperAlwaysIsPlaying>();

            var analyser = factory.Instantiate<IGtPlayedNotesAnalyser>(factory, new DoubleAudioListenerDoNothing(40));

            var note1 = new GtSceneGuitarNote(new BeatTick(1, 0), new BeatTick(1, 240), 6, 0);

            var startingNotes = new List<GtSceneGuitarNote>();
            startingNotes.Add(note1);

            var expectedPlayingNotes = new List<GtSceneGuitarNote>();

            var points = analyser.Analyse(startingNotes, expectedPlayingNotes);

            Assert.AreEqual(100, points);
        }

        /// <summary>
        /// When two notes are hitted method returns 2 * 100 points.
        /// </summary>
        [Test]
        public void TwoHittedNotesShouldReturn100Points()
        {
            var factory = new GtFactory();
            factory.AddMapping<IGtPlayedNotesAnalyserHelper, MockGtPlayedNotesAnalyserHelperAlwaysIsPlaying>();

            var analyser = factory.Instantiate<IGtPlayedNotesAnalyser>(factory, new DoubleAudioListenerDoNothing(40));

            var note1 = new GtSceneGuitarNote(new BeatTick(1, 0), new BeatTick(1, 240), 6, 0);
            var note2 = new GtSceneGuitarNote(new BeatTick(1, 0), new BeatTick(1, 240), 5, 2);

            var startingNotes = new List<GtSceneGuitarNote>();
            startingNotes.Add(note1);
            startingNotes.Add(note2);

            var expectedPlayingNotes = new List<GtSceneGuitarNote>();

            var points = analyser.Analyse(startingNotes, expectedPlayingNotes);

            Assert.AreEqual(2 * 100, points);
        }

        /// <summary>
        /// When one note is playing Analyse method returns 1 point.
        /// </summary>
        [Test]
        public void OnePlayingNoteShouldReturn1Point()
        {
            var factory = new GtFactory();
            factory.AddMapping<IGtPlayedNotesAnalyserHelper, MockGtPlayedNotesAnalyserHelperAlwaysIsPlaying>();

            var analyser = factory.Instantiate<IGtPlayedNotesAnalyser>(factory, new DoubleAudioListenerDoNothing(40));

            var note1 = new GtSceneGuitarNote(new BeatTick(1, 0), new BeatTick(1, 240), 6, 0);

            var startingNotes = new List<GtSceneGuitarNote>();

            var expectedPlayingNotes = new List<GtSceneGuitarNote>();
            expectedPlayingNotes.Add(note1);

            var points = analyser.Analyse(startingNotes, expectedPlayingNotes);

            Assert.AreEqual(1, points);
        }

        /// <summary>
        /// When two notes are playing Analyse method returns 2 points.
        /// </summary>
        [Test]
        public void TwoPlayingNotesShouldReturn2Points()
        {
            var factory = new GtFactory();
            factory.AddMapping<IGtPlayedNotesAnalyserHelper, MockGtPlayedNotesAnalyserHelperAlwaysIsPlaying>();

            var analyser = factory.Instantiate<IGtPlayedNotesAnalyser>(factory, new DoubleAudioListenerDoNothing(40));

            var note1 = new GtSceneGuitarNote(new BeatTick(1, 0), new BeatTick(1, 240), 6, 0);
            var note2 = new GtSceneGuitarNote(new BeatTick(1, 0), new BeatTick(1, 240), 5, 2);

            var startingNotes = new List<GtSceneGuitarNote>();

            var expectedPlayingNotes = new List<GtSceneGuitarNote>();
            expectedPlayingNotes.Add(note1);
            expectedPlayingNotes.Add(note2);

            var points = analyser.Analyse(startingNotes, expectedPlayingNotes);

            Assert.AreEqual(2, points);
        }

    }

    public class MockGtPlayedNotesAnalyserHelperAlwaysIsPlaying : IGtPlayedNotesAnalyserHelper
    {
        public MockGtPlayedNotesAnalyserHelperAlwaysIsPlaying(GtFactory pFactory, IAudioListener pAudioListener)
        {
        }

        public bool NoteIsPlaying(GtSceneGuitarNote pSceneGuitarNote)
        {
            return true;
        }

        public List<MusicalNoteAndTimeStamp> LastQueriedPlayedNotes
        {
            get { throw new NotImplementedException(); }
        }
    }

    public class MockGtPlayedNotesAnalyserHelperAlwaysIsNotPlaying : IGtPlayedNotesAnalyserHelper
    {
        public MockGtPlayedNotesAnalyserHelperAlwaysIsNotPlaying(GtFactory pFactory, IAudioListener pAudioListener)
        {
        }

        public bool NoteIsPlaying(GtSceneGuitarNote pSceneGuitarNote)
        {
            return false;
        }

        public List<MusicalNoteAndTimeStamp> LastQueriedPlayedNotes
        {
            get { throw new NotImplementedException(); }
        }
    }

}

