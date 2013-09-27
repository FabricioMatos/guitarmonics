using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.GameLib.View;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;

namespace Guitarmonics.GameLib.View
{
    public class GtEqualizer
    {
        public GtEqualizer(XnaGame pGame, int x, int y)
        {
            this.Game = pGame;
            this.Fft = new float[EQUALIZER_LENGTH];

            this.X = x;
            this.Y = y;
        }

        public XnaGame Game { get; protected set; }
        private float[] Fft;
        private int X;
        private int Y;

        //private const int EQUALIZER_LENGTH = 64;
        private const int EQUALIZER_LENGTH = 204;
        private const float FFT_MAX_VALUE = 20.0f;
        private const int WIDTH = EQUALIZER_LENGTH * (BAR_WIDTH + 0);
        private const int HEIGHT = 160;
        private const int BAR_WIDTH = 1;
        private const float REDUCTION_FACTOR = 20.0f;


        public void Update(float[] pFft)
        {
            CompactFft(pFft, ref this.Fft);
        }

        public void Render(SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Draw(
                this.Game.EqualizerBackgroundTexture,
                new Rectangle(this.X, this.Y, WIDTH, HEIGHT), Color.White);


            //Don't show the first 12 bars (just hammer)
            int skip = 12;

            for (int i = 0; i < this.Fft.Length - skip; i++)
            {
                int pos = i + skip;
                int barHeight = (int)((this.Fft[pos] / FFT_MAX_VALUE) * HEIGHT);
                int barWidth = WIDTH / EQUALIZER_LENGTH;

                int frequence = (int)(pos * (44100.0f / REDUCTION_FACTOR)) / EQUALIZER_LENGTH;

                pSpriteBatch.Draw(
                    this.Game.EqualizerOnePointTexture,
                    new Rectangle(this.X + (i * (barWidth + 0)) + 1,
                        this.Y + (HEIGHT - barHeight) + 1,
                        barWidth,
                        barHeight - 2),
                    GtNotesLegend.CalculateNoteColor(frequence));
            }

        }


        #region Compact FFT calculations

        /// <summary>
        /// Compacta a primeira metade de Fft em um array de TAMANHO_GRAFICO posicoes.
        /// A segunda parte (agudos) é ignorada.
        /// </summary>
        /// <param name="Fft"></param>
        /// <param name="pFftCompacta"></param>
        private void CompactFft(float[] Fft, ref float[] pFftCompacted)
        {
            //LowCut(ref Fft);

            //Fft.Length / (20 / 8) => 0Hz to 8KHz
            int groupLength = (int)(Fft.Length / REDUCTION_FACTOR) / EQUALIZER_LENGTH;
            for (int i = 0; i < EQUALIZER_LENGTH; i++)
            {
                pFftCompacted[i] = 0;
                float value = 0;

                for (int j = 0; j < groupLength; j++)
                {
                    value += Fft[groupLength * i + j];
                }
                pFftCompacted[i] = value / groupLength;
            }

            LimitFftMaxValue(ref pFftCompacted, FFT_MAX_VALUE);
        }

        /// <summary>
        /// Analyse only up to 8KHz
        /// </summary>
        /// <param name="Fft"></param>
        /// <param name="pFftCompacted"></param>
        private void HighPassFft(float[] Fft, ref float[] pFftCompacted)
        {
        }

        private void LimitFftMaxValue(ref float[] pFft, float pMaxValue)
        {
            for (int i = 0; i < pFft.Length; i++)
            {
                pFft[i] *= 500;

                if (pFft[i] > pMaxValue)
                    pFft[i] = pMaxValue;
            }

            //Normalize:

            //float maxFound = 0.0f;

            ////Found the greater value in pFft
            //for (int i = 0; i < pFft.Length; i++)
            //{
            //    if (pFft[i] > maxFound)
            //        maxFound = pFft[i];
            //}

            ////just noise (there is no relevant notes)
            //if (maxFound < 0.0001f)
            //{
            //    //clear all frequences
            //    for (int i = 0; i < pFft.Length; i++)
            //    {
            //        pFft[i] = 0.0f;
            //    }
            //}
            //else
            //{
            //    //Normalize pFft
            //    for (int i = 0; i < pFft.Length; i++)
            //    {
            //        pFft[i] = (pMaxValue * pFft[i]) / maxFound;
            //    }
            //}
        }

        private void LowCut(ref float[] pFft)
        {
            //Lowcut in 96,89941Hz to eliminate hammer. Note that the first interesting value is usually higher
            pFft[0] = 0;
            pFft[1] = 0;
            pFft[2] = 0;
            pFft[3] = 0;
            pFft[4] = 0;
            pFft[5] = 0;
            pFft[6] = 0;
            pFft[7] = 0;
            pFft[8] = 0;
            pFft[9] = 0;
        }

        #endregion

    }
}
