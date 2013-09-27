using System;
using System.Collections.Generic;
using System.Text;
using Un4seen.Bass;
using NUnit.Framework;
using System.IO;
using Un4seen.Bass.Misc;
using Guitarmonics.AudioLib.Tests;
using Un4seen.Bass.AddOn.Fx;

namespace Guitarmonics.AudioLib.Player.Tests
{
    [TestFixture]
    public class EstudoDaBibliotecaBASS
    {
        private static string MP3MattRedman = TestConfig.AudioPath + "Matt Redman.Facedown.Track 05.MP3";

        [Test]
        public void IniciarEFinalizarBASS()
        {
            bool ok = Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero);
            Assert.IsTrue(ok);

            ok = Bass.BASS_Free();
            Assert.IsTrue(ok);
        }

        [Test]
        public void TocarArquivoOGG()
        {
            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                throw new Exception("Erro ao inicializar BASS.");
            }

            try
            {
                int streamId = Bass.BASS_StreamCreateFile(TestConfig.AudioPath + "twibmpg.ogg", 0, 0,
                        BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN );

                Assert.AreNotEqual(0, streamId);

                bool ok = Bass.BASS_ChannelPlay(streamId, false);
                Assert.IsTrue(ok);
            }
            finally
            {
                Bass.BASS_Free();
            }
        }

        [Test]
        public void TocarArquivoMP3()
        {
            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                throw new Exception("Erro ao inicializar BASS.");
            }

            try
            {
                int streamId = Bass.BASS_StreamCreateFile(MP3MattRedman, 0, 0,
                        BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);

                Assert.AreNotEqual(0, streamId);

                bool ok = Bass.BASS_ChannelPlay(streamId, false);
                Assert.IsTrue(ok);
            }
            finally
            {
                Bass.BASS_Free();
            }
        }

        [Test]
        //O tamanho do arquivo nao quer dizer muito pois o formato é compactado.
        public void ObterTamanhoDaMusica()
        {
            FileStream arquivo = File.Open(MP3MattRedman, FileMode.Open, FileAccess.Read);
            var tamanhoArquivo = arquivo.Length;
            arquivo.Close();

            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                throw new Exception("Erro ao inicializar BASS.");
            }

            try
            {
                int streamId = Bass.BASS_StreamCreateFile(MP3MattRedman, 0, 0,
                        BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_STREAM_PRESCAN);

                var tamamnhoStream = Bass.BASS_StreamGetFilePosition(streamId, BASSStreamFilePosition.BASS_FILEPOS_END);

                Assert.AreEqual(4948636, tamamnhoStream);

                //Essa diferença parece algo relacionado ao header do arquivo
                Assert.AreEqual(4641, tamanhoArquivo - tamamnhoStream);

                arquivo.Close();

            }
            finally
            {
                Bass.BASS_Free();
            }
        }

        [Test]
        public void TocarArquivoMP3AlterandoTempo()
        {
            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                throw new Exception("Erro ao inicializar BASS.");
            }

            try
            {
                int streamHandle = Bass.BASS_StreamCreateFile(MP3MattRedman, 0L, 0L, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_MUSIC_NOSAMPLE | BASSFlag.BASS_MUSIC_DECODE);
                Assert.AreNotEqual(0, streamHandle, "BASS_StreamCreateFile");

                int streamFXHandle = BassFx.BASS_FX_TempoCreate(streamHandle, BASSFlag.BASS_FX_FREESOURCE | BASSFlag.BASS_SAMPLE_FLOAT);
                Assert.AreNotEqual(0, streamFXHandle, "BASS_FX_TempoCreate");

                Bass.BASS_ChannelSetAttribute(streamFXHandle, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_PREVENT_CLICK, 1);
                Bass.BASS_ChannelSetAttribute(streamFXHandle, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_SEQUENCE_MS, 82);
                Bass.BASS_ChannelSetAttribute(streamFXHandle, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_SEEKWINDOW_MS, 14);
                Bass.BASS_ChannelSetAttribute(streamFXHandle, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_OVERLAP_MS, 12);

                bool playing = Bass.BASS_ChannelPlay(streamFXHandle, false);
                Assert.IsTrue(playing, "BASS_ChannelPlay");

                bool tempo = Bass.BASS_ChannelSetAttribute(streamFXHandle, BASSAttribute.BASS_ATTRIB_TEMPO, -50f);
                Assert.IsTrue(tempo, "BASS_ChannelSetAttribute TEMPO");
            }
            finally
            {
                Bass.BASS_Free();
            }
        }


        [Test]
        public void TocarArquivoMP3AlterandoPitch()
        {
            if (!Bass.BASS_Init(-1, 44100, BASSInit.BASS_DEVICE_DEFAULT, IntPtr.Zero))
            {
                throw new Exception("Erro ao inicializar BASS.");
            }

            try
            {
                //var audioFile = @"C:\1-Pessoal\Guitarmonics-OpenSource\trunk\DataFolder\Songs\Metallica.RideTheLightning.ForWhomTheBellTolls\Metallica.RideTheLightning.ForWhomTheBellTolls.mp3";
                var audioFile = MP3MattRedman;

                int streamHandle = Bass.BASS_StreamCreateFile(audioFile, 0L, 0L, BASSFlag.BASS_STREAM_DECODE | BASSFlag.BASS_SAMPLE_FLOAT | BASSFlag.BASS_MUSIC_NOSAMPLE | BASSFlag.BASS_MUSIC_DECODE);
                Assert.AreNotEqual(0, streamHandle, "BASS_StreamCreateFile");

                int streamFXHandle = BassFx.BASS_FX_TempoCreate(streamHandle, BASSFlag.BASS_FX_FREESOURCE | BASSFlag.BASS_SAMPLE_FLOAT);
                Assert.AreNotEqual(0, streamFXHandle, "BASS_FX_TempoCreate");

                Bass.BASS_ChannelSetAttribute(streamFXHandle, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_PREVENT_CLICK, 1);
                Bass.BASS_ChannelSetAttribute(streamFXHandle, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_SEQUENCE_MS, 82);
                Bass.BASS_ChannelSetAttribute(streamFXHandle, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_SEEKWINDOW_MS, 14);
                Bass.BASS_ChannelSetAttribute(streamFXHandle, BASSAttribute.BASS_ATTRIB_TEMPO_OPTION_OVERLAP_MS, 12);

                bool playing = Bass.BASS_ChannelPlay(streamFXHandle, false);
                Assert.IsTrue(playing, "BASS_ChannelPlay");


                bool tempo = Bass.BASS_ChannelSetAttribute(streamFXHandle, BASSAttribute.BASS_ATTRIB_TEMPO_PITCH, 2f); //Sobe 2 semitons (1 tom)
                //bool tempo = Bass.BASS_ChannelSetAttribute(streamFXHandle, BASSAttribute.BASS_ATTRIB_TEMPO_PITCH, -0.5f); //desce 0.5 semitom.
                Assert.IsTrue(tempo, "BASS_ChannelSetAttribute TEMPO");
            }
            finally
            {
                Bass.BASS_Free();
            }
        }

    }
}
