using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.GameLib.Model;

namespace Guitarmonics.GameLib.View
{
    public class GtNotesLegend
    {
        private XnaGame Game;
        private int X;
        private int Y;

        public GtNotesLegend(XnaGame pGame)
        {
            this.Game = pGame;
        }

        private const int VERTICAL_DISTANCE = 24;

        public void Render(SpriteBatch pSpriteBatch, int x, int y)
        {
            DrawNoteLegend(pSpriteBatch, x, y, "C3", "C");
            y += VERTICAL_DISTANCE;

            DrawNoteLegend(pSpriteBatch, x, y, "C#3", "C#");
            y += VERTICAL_DISTANCE;

            DrawNoteLegend(pSpriteBatch, x, y, "D3", "D");
            y += VERTICAL_DISTANCE;

            DrawNoteLegend(pSpriteBatch, x, y, "D#3", "D#");
            y += VERTICAL_DISTANCE;

            DrawNoteLegend(pSpriteBatch, x, y, "E3", "E");
            y += VERTICAL_DISTANCE;

            DrawNoteLegend(pSpriteBatch, x, y, "F3", "F");
            y += VERTICAL_DISTANCE;

            DrawNoteLegend(pSpriteBatch, x, y, "F#3", "F#");
            y += VERTICAL_DISTANCE;

            DrawNoteLegend(pSpriteBatch, x, y, "G3", "G");
            y += VERTICAL_DISTANCE;

            DrawNoteLegend(pSpriteBatch, x, y, "G#3", "G#");
            y += VERTICAL_DISTANCE;

            DrawNoteLegend(pSpriteBatch, x, y, "A3", "A");
            y += VERTICAL_DISTANCE;

            DrawNoteLegend(pSpriteBatch, x, y, "A#3", "A#");
            y += VERTICAL_DISTANCE;

            DrawNoteLegend(pSpriteBatch, x, y, "B3", "B");
            y += VERTICAL_DISTANCE;

        }

        private void DrawNoteLegend(SpriteBatch pSpriteBatch, int x, int y, string pNoteId, string pNoteName)
        {
            var note = new MusicalNote(pNoteId);

            Color color = CalculateNoteColor((int)note.Frequence);

            pSpriteBatch.Draw(
                this.Game.EqualizerOnePointTexture,
                new Rectangle(x, y, 15, 15),
                color);

            pSpriteBatch.DrawString(
                this.Game.FontFretNumbers,
                pNoteName,
                new Vector2(x + VERTICAL_DISTANCE, y - 2),
                Color.White);
        }

        public static Color CalculateNoteColor(int pFrequence)
        {
            var note = new MusicalNote(pFrequence);

            Color color = Color.White;
            double delta = 0;
            switch (note.Value)
            {
                case NoteValue.C:
                    delta = -20.0f * note.Cents / 100.0f;
                    color = new HSLColor(delta + 160, 240, 100);
                    break;
                case NoteValue.Db:
                    delta = -20.0f * note.Cents / 100.0f;
                    color = new HSLColor(delta + 140, 240, 100);
                    break;
                case NoteValue.D:
                    delta = -20.0f * note.Cents / 100.0f;
                    color = new HSLColor(delta + 120, 240, 100);
                    break;
                case NoteValue.Eb:
                    delta = -20.0f * note.Cents / 100.0f;
                    color = new HSLColor(delta + 100, 240, 100);
                    break;
                case NoteValue.E:
                    delta = -20.0f * note.Cents / 100.0f;
                    color = new HSLColor(delta + 80, 240, 100);
                    break;
                case NoteValue.F:
                    delta = -20.0f * note.Cents / 100.0f;
                    color = new HSLColor(delta + 60, 240, 100);
                    break;
                case NoteValue.Gb:
                    delta = -20.0f * note.Cents / 100.0f;
                    color = new HSLColor(delta + 40, 240, 100);
                    break;
                case NoteValue.G:
                    delta = -20.0f * note.Cents / 100.0f;
                    color = new HSLColor(delta + 20, 240, 100);
                    break;
                case NoteValue.Ab:
                    delta = -20.0f * note.Cents / 100.0f;
                    color = new HSLColor(delta + 0, 240, 100);
                    break;
                case NoteValue.A:
                    delta = -20.0f * note.Cents / 100.0f;
                    color = new HSLColor(delta + 220, 240, 100);
                    break;
                case NoteValue.Bb:
                    delta = -20.0f * note.Cents / 100.0f;
                    color = new HSLColor(delta + 200, 240, 100);
                    break;
                case NoteValue.B:
                    delta = -20.0f * note.Cents / 100.0f;
                    color = new HSLColor(delta + 180, 240, 100);
                    break;
            }

            return color;
        }


    }
}
