using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using NUnit.Framework;
using Guitarmonics.GameLib.Controller;
using Guitarmonics.GameLib.View;
using Guitarmonics.GameLib.Model;
using Guitarmonics.AudioLib.Common;

namespace Guitarmonics.GameLib.ViewTest
{
    [TestFixture]
    public class GtSceneGuitarTest
    {
        [Test]
        public void Constructor()
        {
            var tickDataTable = new Double_GtTickDataTable_OneChord();
            var guitar = new GtSceneGuitar(tickDataTable);

            Assert.AreSame(tickDataTable, guitar.TickDataTable);
        }

        [ExpectedException(ExpectedException = typeof(GtSceneGuitarItemInvalidParameter))]
        public void ConstructorParametersValidation1()
        {
            var guitar = new GtSceneGuitar(null);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(GtSceneGuitarItemInvalidParameter))]
        public void ConstructorParametersValidation2()
        {
            var guitar = new GtSceneGuitar(new GtTickDataTable(0));
        }

        private static void AssertFSharpAt(GtSceneGuitar pGuitarItem, BeatTick pStartPosition, string pMessage)
        {
            {
                var notes = pGuitarItem.Notes.Where(p => (p.String == 6) && (p.Fret == 2) && (p.StartPosition == pStartPosition));

                Assert.AreEqual(1, notes.Count(), pMessage);

                Assert.AreEqual(pStartPosition, notes.ElementAt(0).StartPosition, pMessage);
                Assert.AreEqual(new BeatTick(pStartPosition.Beat, 470), notes.ElementAt(0).EndPosition, pMessage);
            }
            {
                var notes = pGuitarItem.Notes.Where(p => (p.String == 5) && (p.Fret == 4) && (p.StartPosition == pStartPosition));

                Assert.AreEqual(1, notes.Count(), pMessage);

                Assert.AreEqual(pStartPosition, notes.ElementAt(0).StartPosition, pMessage);
                Assert.AreEqual(new BeatTick(pStartPosition.Beat, 470), notes.ElementAt(0).EndPosition, pMessage);
            }
            {
                var notes = pGuitarItem.Notes.Where(p => (p.String == 4) && (p.Fret == 4) && (p.StartPosition == pStartPosition));

                Assert.AreEqual(1, notes.Count(), pMessage);

                Assert.AreEqual(pStartPosition, notes.ElementAt(0).StartPosition, pMessage);
                Assert.AreEqual(new BeatTick(pStartPosition.Beat, 470), notes.ElementAt(0).EndPosition, pMessage);
            }
        }

        [Test]
        public void PropertyNotes_OneChord()
        {
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_OneChord());

            AssertFSharpAt(guitar, (new BeatTick(1, 0)), "F# at 1:0");
        }

        [Test]
        public void PropertyNotes_TwoChords()
        {
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_TwoChords());

            AssertFSharpAt(guitar, (new BeatTick(1, 0)), "F# at 1:0");
            AssertFSharpAt(guitar, (new BeatTick(2, 0)), "F# at 2:0"); //Triangulation
        }

        [Test]
        public void VisibleNotes_Position1_0()
        {
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_32Notes(), 4);

            guitar.ForceCurrentPosition((new BeatTick(1, 0)));

            Assert.AreEqual((new BeatTick(1, 0)), guitar.CurrentPosition);

            var visibleNotes = guitar.GetVisibleNotes();

            Assert.AreEqual(24, visibleNotes.Count);

            //Eight Bs (string 5, fret 2)
            foreach (var note in visibleNotes)
            {
                switch (note.String)
                {
                    case 5:
                        Assert.AreEqual(2, note.Fret);
                        break;
                    case 4:
                        Assert.AreEqual(4, note.Fret);
                        break;
                    case 3:
                        Assert.AreEqual(4, note.Fret);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
        }

        [Test]
        public void VisibleNotes_Position1_0_Size1()
        {
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_32Notes(), 1);

            guitar.ForceCurrentPosition((new BeatTick(1, 0)).AddTicks(480));

            Assert.AreEqual((new BeatTick(1, 0)).AddTicks(480), guitar.CurrentPosition);

            var visibleNotes = guitar.GetVisibleNotes();

            Assert.AreEqual(6, visibleNotes.Count);

            //2 Bs (string 5, fret 2)
            foreach (var note in visibleNotes)
            {
                switch (note.String)
                {
                    case 5:
                        Assert.AreEqual(2, note.Fret);
                        break;
                    case 4:
                        Assert.AreEqual(4, note.Fret);
                        break;
                    case 3:
                        Assert.AreEqual(4, note.Fret);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
        }

        [Test]
        public void VisibleNotes_Position5_0()
        {
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_32Notes(), 4);

            guitar.ForceCurrentPosition((new BeatTick(5, 0)));

            Assert.AreEqual((new BeatTick(5, 0)), guitar.CurrentPosition);

            var visibleNotes = guitar.GetVisibleNotes();

            Assert.AreEqual(24, visibleNotes.Count);

            //Eight F#s (string 6, fret 2)
            foreach (var note in visibleNotes)
            {
                switch (note.String)
                {
                    case 6:
                        Assert.AreEqual(2, note.Fret);
                        break;
                    case 5:
                        Assert.AreEqual(4, note.Fret);
                        break;
                    case 4:
                        Assert.AreEqual(4, note.Fret);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
        }

        [Test]
        public void VisibleNotes_Position1_240()
        {
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_32Notes(), 4);

            guitar.ForceCurrentPosition((new BeatTick(1, 240)));

            Assert.AreEqual((new BeatTick(1, 240)), guitar.CurrentPosition);

            var visibleNotes = guitar.GetVisibleNotes();

            Assert.AreEqual(24, visibleNotes.Count);

            //8 B's
            for (int i = 0; i < 21; i++)
            {
                var note = visibleNotes[i];
                switch (note.String)
                {
                    case 5:
                        Assert.AreEqual(2, note.Fret);
                        break;
                    case 4:
                        Assert.AreEqual(4, note.Fret);
                        break;
                    case 3:
                        Assert.AreEqual(4, note.Fret);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }

            //1 F#
            Assert.AreEqual(4, visibleNotes[21].String);
            Assert.AreEqual(4, visibleNotes[21].Fret);
            Assert.AreEqual(5, visibleNotes[22].String);
            Assert.AreEqual(4, visibleNotes[22].Fret);
            Assert.AreEqual(6, visibleNotes[23].String);
            Assert.AreEqual(2, visibleNotes[23].Fret);
        }

        /// <summary>
        /// Note that tick 120 is the half life of the first note, but it's still visible.
        /// </summary>
        [Test]
        public void VisibleNotes_Position1_120()
        {
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_32Notes(), 4);

            guitar.ForceCurrentPosition((new BeatTick(1, 120)));

            Assert.AreEqual((new BeatTick(1, 120)), guitar.CurrentPosition);

            var visibleNotes = guitar.GetVisibleNotes();

            Assert.AreEqual(27, visibleNotes.Count);

            //8 B's
            for (int i = 0; i < 24; i++)
            {
                var note = visibleNotes[i];
                switch (note.String)
                {
                    case 5:
                        Assert.AreEqual(2, note.Fret);
                        break;
                    case 4:
                        Assert.AreEqual(4, note.Fret);
                        break;
                    case 3:
                        Assert.AreEqual(4, note.Fret);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }

            //1 F#
            for (int i = 24; i < 27; i++)
            {
                var note = visibleNotes[i];
                switch (note.String)
                {
                    case 6:
                        Assert.AreEqual(2, note.Fret);
                        break;
                    case 5:
                        Assert.AreEqual(4, note.Fret);
                        break;
                    case 4:
                        Assert.AreEqual(4, note.Fret);
                        break;
                    default:
                        Assert.Fail();
                        break;
                }
            }
        }


        /// <summary>
        /// Note that tick 120 is the half life of the first note, but it's still visible.
        /// </summary>
        [Test]
        public void VisibleNotes_Position5_120()
        {
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_1Note(), 4);

            guitar.ForceCurrentPosition(new BeatTick(5, 120));

            Assert.AreEqual(new BeatTick(5, 120), guitar.CurrentPosition);

            var visibleNotes = guitar.GetVisibleNotes();

            Assert.AreEqual(1, visibleNotes.Count);

            Assert.AreEqual(6, visibleNotes[0].String);
            Assert.AreEqual(0, visibleNotes[0].Fret);
        }

        [Test]
        public void UpdatePosition_1_0()
        {
            //1 note at 5:0
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_1Note(), 4);

            guitar.ForceCurrentPosition(new BeatTick(1, 0));

            Assert.AreEqual(4 * 480, guitar.Notes[0].DistanceFromCurrentPosition);
        }


        [Test]
        public void UpdatePosition_2_240()
        {
            //1 note at 5:0
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_1Note(), 4);

            guitar.ForceCurrentPosition(new BeatTick(2, 240));

            Assert.AreEqual(2 * 480 + 240, guitar.Notes[0].DistanceFromCurrentPosition);
        }

        [Test]
        public void UpdatePosition_5_0()
        {
            //1 note at 5:0
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_1Note(), 4);

            guitar.ForceCurrentPosition(new BeatTick(5, 0));

            Assert.AreEqual(0, guitar.Notes[0].DistanceFromCurrentPosition);
        }

        [Test]
        public void UpdatePosition_6_0()
        {
            //1 note at 5:0
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_1Note(), 4);

            guitar.ForceCurrentPosition(new BeatTick(6, 0));

            Assert.AreEqual(-480, guitar.Notes[0].DistanceFromCurrentPosition);
        }

        [Test]
        public void CurrentStartingNotes_OneNote()
        {
            //1 note at 5:0
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_1Note(), 4);

            guitar.ForceCurrentPosition(new BeatTick(1, 0));

            Assert.AreEqual(0, guitar.CurrentStartingNotes.Count);

            guitar.ForceCurrentPosition(new BeatTick(5, 0));

            Assert.AreEqual(1, guitar.CurrentStartingNotes.Count);
        }

        [Test]
        public void CurrentStartingNotes_Tolerance_InBounds()
        {
            //1 note at 5:0
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_1Note(), 30);

            var position = new BeatTick(5, 0);

            guitar.ForceCurrentPosition(position.AddTicks(GtSceneGuitar.PLAYER_TOLERANCE_IN_TICKS));
            Assert.AreEqual(1, guitar.CurrentStartingNotes.Count);

            guitar.ForceCurrentPosition(position.SubTicks(GtSceneGuitar.PLAYER_TOLERANCE_IN_TICKS));
            Assert.AreEqual(1, guitar.CurrentStartingNotes.Count);
        }

        [Test]
        public void CurrentStartingNotes_Tolerance_OutOfBounds()
        {
            //1 note at 5:0
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_1Note(), 30);

            var position = new BeatTick(5, 0);

            guitar.ForceCurrentPosition(position.AddTicks(GtSceneGuitar.PLAYER_TOLERANCE_IN_TICKS + 1));
            Assert.AreEqual(0, guitar.CurrentStartingNotes.Count);

            guitar.ForceCurrentPosition(position.SubTicks(GtSceneGuitar.PLAYER_TOLERANCE_IN_TICKS + 1));
            Assert.AreEqual(0, guitar.CurrentStartingNotes.Count);
        }

        [Test]
        public void CurrentExpectedPlayingNotes_Tolerance_InBounds()
        {
            //1 note at 5:0
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_1Note(), 30);

            var position = new BeatTick(5, 0);

            guitar.ForceCurrentPosition(position.SubTicks(GtSceneGuitar.PLAYER_TOLERANCE_IN_TICKS));
            Assert.AreEqual(1, guitar.CurrentStartingNotes.Count);

            //the note at (5:0) has 8 beats of duration.
            guitar.ForceCurrentPosition(position.AddTicks((8 * 480) + GtSceneGuitar.PLAYER_TOLERANCE_IN_TICKS));
            Assert.AreEqual(1, guitar.CurrentExpectedPlayingNotes.Count);
        }

        [Test]
        public void CurrentExpectedPlayingNotes_StartingAtFirstBeat()
        {
            //1 note at 1:0
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_1NoteFirstBeat(), 30);

            var position = new BeatTick(1, 0);

            guitar.ForceCurrentPosition(position);

            Assert.AreEqual(1, guitar.CurrentStartingNotes.Count);
            Assert.AreEqual(1, guitar.CurrentExpectedPlayingNotes.Count);
        }



        [Test]
        public void CurrentExpectedPlayingNotes_Tolerance_OutOfBounds()
        {
            //1 note at 5:0
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_1Note(), 30);

            var position = new BeatTick(5, 0);

            guitar.ForceCurrentPosition(position.SubTicks(GtSceneGuitar.PLAYER_TOLERANCE_IN_TICKS + 1));
            Assert.AreEqual(0, guitar.CurrentStartingNotes.Count);

            //the note at (5:0) has 8 beats of duration.
            guitar.ForceCurrentPosition(position.AddTicks((8 * 480) + GtSceneGuitar.PLAYER_TOLERANCE_IN_TICKS + 1));
            Assert.AreEqual(0, guitar.CurrentExpectedPlayingNotes.Count);

        }

        [Test]
        public void CurrentExpectedPlayingNotes_Tolerance_InBounds2()
        {
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_32Notes(), 30);

            var position = new BeatTick(2, 0);

            guitar.ForceCurrentPosition(position);
            Assert.AreEqual(3, guitar.CurrentStartingNotes.Count);

            guitar.ForceCurrentPosition(position.AddTicks(200));
            Assert.AreEqual(0, guitar.CurrentStartingNotes.Count);
        }

        [TestCase(1, 0, NoteValue.E, 5)]
        [TestCase(2, 0, NoteValue.B, 4)]
        [TestCase(3, 0, NoteValue.G, 4)]
        [TestCase(4, 0, NoteValue.D, 4)]
        [TestCase(5, 0, NoteValue.A, 3)]
        [TestCase(6, 0, NoteValue.E, 3)]

        [TestCase(1, 12, NoteValue.E, 6)]
        [TestCase(2, 12, NoteValue.B, 5)]
        [TestCase(3, 12, NoteValue.G, 5)]
        [TestCase(4, 12, NoteValue.D, 5)]
        [TestCase(5, 12, NoteValue.A, 4)]
        [TestCase(6, 12, NoteValue.E, 4)]

        [TestCase(1, 5, NoteValue.A, 5)]
        [TestCase(2, 5, NoteValue.E, 5)]
        [TestCase(3, 5, NoteValue.C, 5)]
        [TestCase(4, 5, NoteValue.G, 4)]
        [TestCase(5, 5, NoteValue.D, 4)]
        [TestCase(6, 5, NoteValue.A, 3)]
        public void GtSceneGuitarNoteValueAndNumber(int pString, int pFret, NoteValue pNoteValue, int pNoteNumber)
        {
            var guitarNote = new GtSceneGuitarNote(new BeatTick(1, 0), new BeatTick(2, 0), pString, pFret);

            Assert.AreEqual(pNoteValue, guitarNote.NoteValue);
            Assert.AreEqual(pNoteNumber, guitarNote.NoteNumber);
        }


        //Double_GtTickDataTable_32Notes has 60 bpm from (1:0) to (8:470) and 120bpm from (9:0) to (16:470)
        [TestCase(1, 0, 0)]
        [TestCase(2, 0, 1000)]      
        [TestCase(2, 240, 1500)]    
        [TestCase(9, 0, 8000)]      
        [TestCase(13, 0, 10000)]  //8 beats in first 8sec and 4 beats in the next 2sec
        public void ForceCurrentPositionInMiliseconds(long pBeat, long pTick, long pPosition)
        {
            var guitar = new GtSceneGuitar(new Double_GtTickDataTable_32Notes(), 30);

            guitar.ForceCurrentPositionInMiliseconds(pPosition);
            Assert.AreEqual(new BeatTick(pBeat, pTick), guitar.CurrentPosition);
        }
    }

    #region Data (GtTickDataTable) to test

    public class Double_GtTickDataTable_OneChord : GtTickDataTable
    {
        public Double_GtTickDataTable_OneChord()
            : base(4)
        {
            var tickData = new GtTickData();
            tickData.RemarkOrChordName = "F#";
            tickData.String6 = 2;
            tickData.String5 = 4;
            tickData.String4 = 4;

            this.AddTickData(new BeatTick(1, 0), new BeatTick(1, 470), tickData);
        }
    }

    public class Double_GtTickDataTable_TwoChords : GtTickDataTable
    {
        public Double_GtTickDataTable_TwoChords()
            : base(4)
        {
            var tickData = new GtTickData();
            tickData.RemarkOrChordName = "F#";
            tickData.String6 = 2;
            tickData.String5 = 4;
            tickData.String4 = 4;

            this.AddTickData(new BeatTick(1, 0), new BeatTick(1, 470), tickData);
            this.AddTickData(new BeatTick(2, 0), new BeatTick(2, 470), tickData);
        }
    }

    public class Double_GtTickDataTable_1Note : GtTickDataTable
    {
        public Double_GtTickDataTable_1Note()
            : base(16)
        {
            var tickData = new GtTickData();
            tickData.RemarkOrChordName = "E";
            tickData.String6 = 0;

            //var position = new BeatTick(4, 0); //GtSceneGuitar.SetSceneGuitarNote add 1 beat - I know, must be adjusted.
            var position = new BeatTick(5, 0);
            var duration = 8 * 480;

            this.AddTickData(position, position.AddTicks(duration), tickData);
        }
    }

    public class Double_GtTickDataTable_1NoteFirstBeat : GtTickDataTable
    {
        public Double_GtTickDataTable_1NoteFirstBeat()
            : base(16)
        {
            var tickData = new GtTickData();
            tickData.RemarkOrChordName = "E";
            tickData.String6 = 0;

            var position = new BeatTick(1, 0);
            var duration = 8 * 480;

            this.AddTickData(position, position.AddTicks(duration), tickData);
        }
    }

    public class Double_GtTickDataTable_32Notes : GtTickDataTable
    {
        public Double_GtTickDataTable_32Notes()
            : base(16)
        {
            var tickData1 = new GtTickData();
            tickData1.RemarkOrChordName = "B";
            tickData1.String5 = 2;
            tickData1.String4 = 4;
            tickData1.String3 = 4;

            var tickData2 = new GtTickData();
            tickData2.RemarkOrChordName = "F#";
            tickData2.String6 = 2;
            tickData2.String5 = 4;
            tickData2.String4 = 4;

            var tickData3 = new GtTickData();
            tickData3.RemarkOrChordName = "G#";
            tickData3.String6 = 4;
            tickData3.String5 = 6;
            tickData3.String4 = 6;

            var tickData4 = new GtTickData();
            tickData4.RemarkOrChordName = "E";
            tickData4.String6 = 0;
            tickData4.String5 = 2;
            tickData4.String4 = 2;

            var position = new BeatTick(1, 0);
            var duration = 120;
            var distance = 240;
            var distanceInMiliseconds = 500; //0.5 sec (60 BPM)
            var positionInMiliseconds = 0;

            //Insert 8 B (2 for each beat)
            for (int i = 0; i < 8; i++)
            {
                tickData1.MomentInMiliseconds = positionInMiliseconds;
                this.AddTickData(position, position.AddTicks(duration), tickData1);
                position = position.AddTicks(distance);
                positionInMiliseconds += distanceInMiliseconds;
            }

            //Insert 8 F# (2 for each beat)
            for (int i = 0; i < 8; i++)
            {
                tickData2.MomentInMiliseconds = positionInMiliseconds;
                this.AddTickData(position, position.AddTicks(duration), tickData2);
                position = position.AddTicks(distance);
                positionInMiliseconds += distanceInMiliseconds;
            }

            distanceInMiliseconds = 250; //0.25 sec (120 BPM)

            //Insert 8 G# (2 for each beat)
            for (int i = 0; i < 8; i++)
            {
                tickData3.MomentInMiliseconds = positionInMiliseconds;
                this.AddTickData(position, position.AddTicks(duration), tickData3);
                position = position.AddTicks(distance);
                positionInMiliseconds += distanceInMiliseconds;
            }

            //Insert 8 E (2 for each beat)
            for (int i = 0; i < 8; i++)
            {
                tickData4.MomentInMiliseconds = positionInMiliseconds;
                this.AddTickData(position, position.AddTicks(duration), tickData4);
                position = position.AddTicks(distance);
                positionInMiliseconds += distanceInMiliseconds;
            }
        }
    }


    public class Double_GtTickDataTable_ChromaticScale : GtTickDataTable
    {
        public Double_GtTickDataTable_ChromaticScale()
            : base(600)
        {
            var tickData1 = new GtTickData();
            tickData1.RemarkOrChordName = "C";
            tickData1.String5 = 3;

            var tickData2 = new GtTickData();
            tickData2.RemarkOrChordName = "C#";
            tickData2.String5 = 4;

            var tickData3 = new GtTickData();
            tickData3.RemarkOrChordName = "D";
            tickData3.String5 = 5;

            var tickData4 = new GtTickData();
            tickData4.RemarkOrChordName = "D#";
            tickData4.String5 = 6;

            var tickData5 = new GtTickData();
            tickData5.RemarkOrChordName = "E";
            tickData5.String4 = 2;

            var tickData6 = new GtTickData();
            tickData6.RemarkOrChordName = "F";
            tickData6.String4 = 3;

            var tickData7 = new GtTickData();
            tickData7.RemarkOrChordName = "F#";
            tickData7.String4 = 4;

            var tickData8 = new GtTickData();
            tickData8.RemarkOrChordName = "G";
            tickData8.String4 = 5;

            var tickData9 = new GtTickData();
            tickData9.RemarkOrChordName = "G#";
            tickData9.String3 = 1;

            var tickData10 = new GtTickData();
            tickData10.RemarkOrChordName = "A";
            tickData10.String3 = 2;

            var tickData11 = new GtTickData();
            tickData11.RemarkOrChordName = "A#";
            tickData11.String3 = 3;

            var tickData12 = new GtTickData();
            tickData12.RemarkOrChordName = "B";
            tickData12.String3 = 4;

            var position = new BeatTick(3, 0);
            var duration = 230;
            var distance = 240;

            var ntimes = 1;

            for (int j = 0; j < 50; j++)
            {
                for (int i = 0; i < ntimes; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData1);
                    position = position.AddTicks(distance);
                }

                for (int i = 0; i < ntimes; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData2);
                    position = position.AddTicks(distance);
                }

                for (int i = 0; i < ntimes; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData3);
                    position = position.AddTicks(distance);
                }

                for (int i = 0; i < ntimes; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData4);
                    position = position.AddTicks(distance);
                }

                for (int i = 0; i < ntimes; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData5);
                    position = position.AddTicks(distance);
                }

                for (int i = 0; i < ntimes; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData6);
                    position = position.AddTicks(distance);
                }

                for (int i = 0; i < ntimes; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData7);
                    position = position.AddTicks(distance);
                }

                for (int i = 0; i < ntimes; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData8);
                    position = position.AddTicks(distance);
                }

                for (int i = 0; i < ntimes; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData9);
                    position = position.AddTicks(distance);
                }

                for (int i = 0; i < ntimes; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData10);
                    position = position.AddTicks(distance);
                }

                for (int i = 0; i < ntimes; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData11);
                    position = position.AddTicks(distance);
                }

                for (int i = 0; i < ntimes; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData12);
                    position = position.AddTicks(distance);
                }
            }
        }
    }


    public class Double_GtTickDataTable_3200Notes : GtTickDataTable
    {
        public Double_GtTickDataTable_3200Notes()
            : base(3200)
        {
            var tickData1 = new GtTickData();
            tickData1.RemarkOrChordName = "B";
            tickData1.String5 = 2;
            tickData1.String4 = 4;
            tickData1.String3 = 4;

            var tickData2 = new GtTickData();
            tickData2.RemarkOrChordName = "F#";
            tickData2.String6 = 2;
            tickData2.String5 = 4;
            tickData2.String4 = 4;

            var tickData3 = new GtTickData();
            tickData3.RemarkOrChordName = "G#";
            tickData3.String6 = 4;
            tickData3.String5 = 6;
            tickData3.String4 = 6;

            var tickData4 = new GtTickData();
            tickData4.RemarkOrChordName = "E";
            tickData4.String6 = 0;
            tickData4.String5 = 2;
            tickData4.String4 = 2;

            var position = new BeatTick(1, 0);
            var duration = 120;
            var distance = 240;


            for (int j = 0; j < 100; j++)
            {
                //Insert 8 B (2 for each beat)
                for (int i = 0; i < 8; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData1);
                    position = position.AddTicks(distance);
                }

                //Insert 8 F# (2 for each beat)
                for (int i = 0; i < 8; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData2);
                    position = position.AddTicks(distance);
                }

                //Insert 8 G# (2 for each beat)
                for (int i = 0; i < 8; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData3);
                    position = position.AddTicks(distance);
                }

                //Insert 8 E (2 for each beat)
                for (int i = 0; i < 8; i++)
                {
                    this.AddTickData(position, position.AddTicks(duration), tickData4);
                    position = position.AddTicks(distance);
                }
            }
        }
    }

    #endregion

}
