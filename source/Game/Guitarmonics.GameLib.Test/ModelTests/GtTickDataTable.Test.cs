using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib.Controller;
using Guitarmonics.GameLib.ControllerTest;
using NUnit.Framework;
using Guitarmonics.GameLib.Model;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.AudioLib.MusicConfigFiles;

namespace Guitarmonics.GameLib.ModelTest
{
    [TestFixture]
    public class GtTickDataTableTest
    {
        [Test]
        public void TickData_Signature()
        {
            var tickData = new GtTickData();

            Assert.IsNull(tickData.String1);
            Assert.IsNull(tickData.String2);
            Assert.IsNull(tickData.String3);
            Assert.IsNull(tickData.String4);
            Assert.IsNull(tickData.String5);
            Assert.IsNull(tickData.String6);

            Assert.IsFalse(tickData.IsStartTick);
            Assert.IsFalse(tickData.IsEndTick);

            Assert.AreEqual("", tickData.RemarkOrChordName);

            Assert.IsFalse(tickData.DrawChord);

        }

        [Test]
        public void TickData_FilledExample()
        {
            var tickData = new GtTickData();

            tickData.IsStartTick = true;

            tickData.String6 = 2;
            tickData.String5 = 4;
            tickData.String4 = 4;

            tickData.RemarkOrChordName = "F#";


            Assert.IsNull(tickData.String1);
            Assert.IsNull(tickData.String2);
            Assert.IsNull(tickData.String3);
            Assert.AreEqual(4, tickData.String4);
            Assert.AreEqual(4, tickData.String5);
            Assert.AreEqual(2, tickData.String6);

            Assert.IsTrue(tickData.IsStartTick);
            Assert.IsFalse(tickData.IsEndTick);

            Assert.AreEqual("F#", tickData.RemarkOrChordName);
        }

        [Test]
        public void TickDataTable_ConstructionAndInitialization_1()
        {
            var tickDataTable = new GtTickDataTable(10);
            Assert.AreEqual(10 * 48, tickDataTable.fItems.Length);
        }

        [Test]
        public void TickDataTable_ConstructionAndInitialization_2()
        {
            var tickDataTable = new GtTickDataTable(10, 20);
            Assert.AreEqual(11 * 48, tickDataTable.fItems.Length);
        }

        [Test]
        public void TickDataTable_AccessTickData()
        {
            int musicLength = 100;
            var tickDataTable = new GtTickDataTable(musicLength);

            for (int beat = 1; beat <= musicLength; beat++)
            {
                for (int tick = 0; tick <= 470; tick += 10)
                {
                    Assert.AreEqual(typeof(GtTickData), tickDataTable[beat, tick].GetType());
                }
            }
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidBeatValue))]
        public void TickDataTable_InvalidBeat()
        {
            var tickDataTable = new GtTickDataTable(10);
            var tickData = tickDataTable[0, 0]; //the first beat is 1, not 0!
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidTickValue))]
        public void TickDataTable_InvalidTick()
        {
            var tickDataTable = new GtTickDataTable(10);
            var tickData = tickDataTable[1, 1]; //The tick value must be multiple of 10
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidBeatValue))]
        public void TickDataTable_BeatOutOfBounds()
        {
            var tickDataTable = new GtTickDataTable(10);
            var tickData = tickDataTable[11, 0]; //The last valid beat is 10
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidTickValue))]
        public void TickDataTable_TickOutOfBounds()
        {
            var tickDataTable = new GtTickDataTable(10);
            var tickData = tickDataTable[1, 471]; //The last valid tick is 470
        }


        [Test]
        public void TickDataTable_AddTickData_One()
        {
            var tickData = new GtTickData();
            tickData.RemarkOrChordName = "F#";
            tickData.String6 = 2;
            tickData.String5 = 4;
            tickData.String4 = 4;

            var tickDataTable = new GtTickDataTable(10);
            tickDataTable.AddTickData(new BeatTick(1, 0), new BeatTick(1, 0), tickData);

            Assert.AreEqual(tickData.RemarkOrChordName, tickDataTable[1, 0].RemarkOrChordName);

            Assert.AreEqual(tickData.String6, tickDataTable[1, 0].String6);
            Assert.AreEqual(tickData.String5, tickDataTable[1, 0].String5);
            Assert.AreEqual(tickData.String4, tickDataTable[1, 0].String4);
            Assert.AreEqual(tickData.String3, tickDataTable[1, 0].String3);
            Assert.AreEqual(tickData.String2, tickDataTable[1, 0].String2);
            Assert.AreEqual(tickData.String1, tickDataTable[1, 0].String1);

            Assert.IsTrue(tickDataTable[1, 0].IsStartTick);
            Assert.IsTrue(tickDataTable[1, 0].IsEndTick);
        }

        [Test]
        public void TickDataTable_AddTickData_DifferentReference()
        {
            var tickData = new GtTickData();
            var tickDataTable = new GtTickDataTable(10);
            tickDataTable.AddTickData(new BeatTick(1, 0), new BeatTick(1, 0), tickData);

            Assert.AreNotSame(tickData, tickDataTable[1, 0]);
        }

        [Test]
        public void TickDataTable_AddTickData_Many()
        {
            var tickData = new GtTickData();
            tickData.RemarkOrChordName = "F#";
            tickData.String6 = 2;
            tickData.String5 = 4;
            tickData.String4 = 4;

            var tickDataTable = new GtTickDataTable(10);
            tickDataTable.AddTickData(new BeatTick(1, 0), new BeatTick(1, 470), tickData);

            for (int tick = 0; tick < 480; tick += 10)
            {
                Assert.AreEqual(tickData.RemarkOrChordName, tickDataTable[1, tick].RemarkOrChordName);
                Assert.AreEqual(tickData.String6, tickDataTable[1, tick].String6);
                Assert.AreEqual(tickData.String5, tickDataTable[1, tick].String5);
                Assert.AreEqual(tickData.String4, tickDataTable[1, tick].String4);

                if (tick == 0)
                {
                    //first item
                    Assert.IsTrue(tickDataTable[1, tick].IsStartTick);
                    Assert.IsFalse(tickDataTable[1, tick].IsEndTick);
                }
                else if (tick == 470)
                {
                    //last item
                    Assert.IsFalse(tickDataTable[1, tick].IsStartTick);
                    Assert.IsTrue(tickDataTable[1, tick].IsEndTick);
                }
                else
                {
                    //other item
                    Assert.IsFalse(tickDataTable[1, tick].IsStartTick);
                    Assert.IsFalse(tickDataTable[1, tick].IsEndTick);
                }
            }

            Assert.AreEqual("", tickDataTable[2, 0].RemarkOrChordName);
            Assert.IsNull(tickDataTable[2, 0].String6);
            Assert.IsNull(tickDataTable[2, 0].String5);
            Assert.IsNull(tickDataTable[2, 0].String4);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InvalidEndPosition))]
        public void TickDataTable_AddTickData_Invalid()
        {
            var tickData = new GtTickData();

            var tickDataTable = new GtTickDataTable(10);

            tickDataTable.AddTickData(new BeatTick(2, 0), new BeatTick(1, 0), tickData);
        }

        [Test]
        [ExpectedException(ExpectedException = typeof(InsufficientPins))]
        public void AutoCompleteMomentInMilisecondsWithOnePinnedItemIsInvalid()
        {
            var tickDataTable = new GtTickDataTable(16);

            tickDataTable[2, 0].MomentInMiliseconds = 1000;

            tickDataTable.AutoCompleteMomentInMiliseconds();
        }

        [Test]
        public void AutoCompleteMomentInMilisecondsWithTwoPinnedItemsIsValid()
        {
            var tickDataTable = new GtTickDataTable(16);

            tickDataTable[2, 0].MomentInMiliseconds = 1000;
            tickDataTable[3, 0].MomentInMiliseconds = 2000;

            tickDataTable.AutoCompleteMomentInMiliseconds();

            //Assert.Pass();
        }

        [Test]
        public void AutoCompleteMomentInMilisecondsTest1()
        {
            var tickDataTable = new GtTickDataTable(16);

            tickDataTable[2, 0].MomentInMiliseconds = 3000;
            tickDataTable[5, 0].MomentInMiliseconds = 6000;

            tickDataTable.AutoCompleteMomentInMiliseconds();

            Assert.AreEqual(2000, tickDataTable[1, 0].MomentInMiliseconds);
            Assert.AreEqual(4000, tickDataTable[3, 0].MomentInMiliseconds);
            Assert.AreEqual(8000, tickDataTable[7, 0].MomentInMiliseconds);
        }

        //OBS: [Fabricio] Tem uns itens com Moment = 0 que ainda nao consegui ver o motivo.
        //[Test]
        //public void TestAutoCompleteMomentInMilisecondsForTheSong_ForWhomTheBellTolls()
        //{
        //    var fileLoader = new GtFileLoaderDouble6();

        //    var tickDataTable = fileLoader.LoadTickDataTable((fileLoader.ListAllSongs())[0]);

        //    BeatTick priorPosition = new BeatTick(1, 0);

        //    for (int beat = 1; beat <= tickDataTable.NumberOfBeats - GtFileLoader.NUMBER_ADITIONAL_BEATS; beat++)
        //    {
        //        for (int tick = 0; tick < 480; tick += 10)
        //        {
        //            //System.Diagnostics.Trace.WriteLine(string.Format("BeatTick({0}, {1}): {2}", beat, tick, tickDataTable[beat, tick].MomentInMiliseconds));

        //            if (priorPosition > new BeatTick(1, 0))
        //            {
        //                Assert.Greater(
        //                    tickDataTable[beat, tick].MomentInMiliseconds,
        //                    tickDataTable[priorPosition.Beat, priorPosition.Tick].MomentInMiliseconds,
        //                    string.Format("BeatTick({0}, {1})", beat, tick));
        //            }

        //            priorPosition = new BeatTick(beat, tick);
        //        }
        //    }
        //}

        [Ignore]
        [Test]
        public void TickDataAutoChordName()
        {
            //TODO: Em uma versao futura poderiamos inferir (opcao parametrizavel) o nome dos acordes.
            throw new NotImplementedException();
        }

        //start > end

        //start = end


        [Test]
        public void UpdateSync()
        {
            var tickDataTable = new GtTickDataTable(4);

            var syncElements = new List<BeatTickMoment>();
            syncElements.Add(new BeatTickMoment(2, 240, 1000));
            syncElements.Add(new BeatTickMoment(2, 260, 1000));

            tickDataTable.UpdateSync(syncElements);

            Assert.AreEqual(1000, tickDataTable[2, 240].MomentInMiliseconds);
            Assert.IsNull(tickDataTable[2, 250].MomentInMiliseconds);
            Assert.AreEqual(1000, tickDataTable[2, 260].MomentInMiliseconds);
            
        }

    }
}
