using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.AudioLib.Tests;
using NUnit.Framework;

namespace Guitarmonics.AudioLib.Analysis.Tests
{
    [TestFixture]
    public class SpectrumAnalysisTest
    {
        //protected static string RiffBlues1_Clean = TestConfig.AudioPath + "RiffBlues1-Clean.mp3";
        //protected static string IUseToLoveHer_Clean = TestConfig.AudioPath + "IUseToLoveHer-Clean.mp3";
        //protected static string FftVisualizationFile = TestConfig.TempOutputPath + "FftVisualizationFile.csv";

        [Test]
        public void EmptyFFT()
        {
            var spectrumAnalyser = new SpectrumAnalyzer();

            float[] fft = new float[4096];

            List<IMusicalNote> notes = spectrumAnalyser.GetMusicalNotes(fft);

            Assert.AreEqual(0, notes.Count);
        }

        [Test]
        public void OneNote_A4()
        {
            var spectrumAnalyser = new SpectrumAnalyzer();

            float[] fft = new float[4096];

            var level = 0.1f;
            SetFrequenceLevel(fft, 440.0f, level);

            List<IMusicalNote> notes = spectrumAnalyser.GetMusicalNotes(fft);

            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual("A4", notes[0].ToString());
            Assert.AreEqual(level, notes[0].Volume);
        }

        /// <summary>
        /// Notes under the limit noise level are discarted
        /// </summary>
        [Test]
        public void A4AtNoiseLevel()
        {
            var spectrumAnalyser = new SpectrumAnalyzer();

            float[] fft = new float[4096];

            SetFrequenceLevel(fft, 440.0f, 0.0249f); //A4 - just noise (level too low)
            SetFrequenceLevel(fft, 880.0f, 0.025f);  //A5 - valid

            List<IMusicalNote> notes = spectrumAnalyser.GetMusicalNotes(fft);

            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual("A5", notes[0].ToString());
        }


        /// <summary>
        /// frequences until 96,89941Hz are ignored
        /// </summary>
        [Test]
        public void LowCutTest()
        {
            var spectrumAnalyser = new SpectrumAnalyzer();

            float[] fft = new float[4096];

            SetFrequenceLevel(fft, 96.8f, 0.1f); //freq < 96,89941Hz
            SetFrequenceLevel(fft, 96.9f, 0.1f);  //freq > 96,89941Hz

            List<IMusicalNote> notes = spectrumAnalyser.GetMusicalNotes(fft);

            Assert.AreEqual(1, notes.Count);
        }

        [Test]
        public void ChordWithHarmonics()
        {
            List<IMusicalNote> notes = new List<IMusicalNote>();

            notes.Add(new MusicalNote("A3", 0.04f));  //0
            notes.Add(new MusicalNote("A4", 0.08f));  //1
            notes.Add(new MusicalNote("A5", 0.02f));  //2

            var spectrumAnalyser = new SpectrumAnalyzer();
            spectrumAnalyser.SortMusicalNotesByVolume(ref notes);

            Assert.AreEqual("A4", notes[0].ToString());
            Assert.AreEqual("A3", notes[1].ToString());
            Assert.AreEqual("A5", notes[2].ToString());
        }

        /// <summary>
        /// Real chord with harmonics (note the high level of them)
        /// </summary>
        [Test]
        public void RealExampleDChord()
        {
            var spectrumAnalyser = new SpectrumAnalyzer();

            float[] fft = new float[4096];

            SetFrequenceLevel(fft, 290.70f, 0.203f);	//D4
            SetFrequenceLevel(fft, 441.43f, 0.685f);	//A4
            SetFrequenceLevel(fft, 592.16f, 0.456f);	//D5
            SetFrequenceLevel(fft, 742.90f, 0.339f);	//F#5
            SetFrequenceLevel(fft, 882.86f, 0.9000f);	//A5 (harmonics)
            SetFrequenceLevel(fft, 1184.33f, 0.291f);	//D6 (harmonics)
            SetFrequenceLevel(fft, 1324.29f, 0.650f);	//E6 (harmonics)
            SetFrequenceLevel(fft, 1485.79f, 0.380f);	//F#6 (harmonics)
            SetFrequenceLevel(fft, 1765.72f, 0.166f);	//A6 (harmonics)
            SetFrequenceLevel(fft, 2217.92f, 0.460f);	//C#7 (harmonics)
            SetFrequenceLevel(fft, 2357.89f, 0.403f);	//D7 (harmonics)
            SetFrequenceLevel(fft, 2960.82f, 0.435f);	//F#7 (harmonics)
            SetFrequenceLevel(fft, 3703.71f, 0.297f);	//A#7 (harmonics)

            List<IMusicalNote> notes = spectrumAnalyser.GetMusicalNotes(fft);
            spectrumAnalyser.DeleteUnusefulNotes(ref notes);

            Assert.AreEqual(6, notes.Count);

            Assert.AreEqual("D4", notes[0].ToString());
            Assert.AreEqual("A4", notes[1].ToString());
            Assert.AreEqual("D5", notes[2].ToString());
            Assert.AreEqual("Gb5", notes[3].ToString());
            Assert.AreEqual("A5", notes[4].ToString());
            Assert.AreEqual("D6", notes[5].ToString());
        }

        [Test]
        public void ChordWithLowLevelHarmonics()
        {
            List<IMusicalNote> notes = new List<IMusicalNote>();

            notes.Add(new MusicalNote("A3", 0.0001f)); //low level (less then 1/10 of the greatest => will be ingored)
            notes.Add(new MusicalNote("A4", 0.0010f)); 
            notes.Add(new MusicalNote("A5", 0.0011f)); 

            var spectrumAnalyser = new SpectrumAnalyzer();
            spectrumAnalyser.DeleteUnusefulNotes(ref notes);

            Assert.AreEqual(2, notes.Count);
            Assert.AreEqual("A4", notes[0].ToString());
            Assert.AreEqual("A5", notes[1].ToString());
        }

        [Test]
        public void NotesUnder3AreIgnored()
        {
            List<IMusicalNote> notes = new List<IMusicalNote>();

            notes.Add(new MusicalNote("C1", 0.01f));
            notes.Add(new MusicalNote("D1", 0.01f));
            notes.Add(new MusicalNote("C2", 0.01f));
            notes.Add(new MusicalNote("D2", 0.01f));
            notes.Add(new MusicalNote("A2", 0.01f));
            notes.Add(new MusicalNote("Bb2", 0.01f));
            notes.Add(new MusicalNote("B2", 0.01f));
            notes.Add(new MusicalNote("C3", 0.01f));

            var spectrumAnalyser = new SpectrumAnalyzer();
            spectrumAnalyser.DeleteUnusefulNotes(ref notes);

            Assert.AreEqual(1, notes.Count);
            Assert.AreEqual("C3", notes[0].ToString());
        }


        #region Helpers

        private void SetFrequenceLevel(float[] pFft, float pFrequence, float pLevel)
        {
            //calculate the position of pFrequence in pFft
            int pos = (int)Math.Round((pFrequence * pFft.Length) / 44100.0f);

            //Set the level value
            pFft[pos] = pLevel;
        }

        
        #endregion
    }
}
