using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.GameLib.Model;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Guitarmonics.AudioLib.Common
{
    public static class MusicalNoteExtension
    {
        public static Color NoteColor(this MusicalNote pMusicalNote)
        {
            Color color = Color.White;
            double delta = 0;
            switch (pMusicalNote.Value)
            {
                case NoteValue.C:
                    delta = -20.0f * pMusicalNote.Cents / 100.0f;
                    color = new HSLColor(delta + 160, 240, 100);
                    break;
                case NoteValue.Db:
                    delta = -20.0f * pMusicalNote.Cents / 100.0f;
                    color = new HSLColor(delta + 140, 240, 100);
                    break;
                case NoteValue.D:
                    delta = -20.0f * pMusicalNote.Cents / 100.0f;
                    color = new HSLColor(delta + 120, 240, 100);
                    break;
                case NoteValue.Eb:
                    delta = -20.0f * pMusicalNote.Cents / 100.0f;
                    color = new HSLColor(delta + 100, 240, 100);
                    break;
                case NoteValue.E:
                    delta = -20.0f * pMusicalNote.Cents / 100.0f;
                    color = new HSLColor(delta + 80, 240, 100);
                    break;
                case NoteValue.F:
                    delta = -20.0f * pMusicalNote.Cents / 100.0f;
                    color = new HSLColor(delta + 60, 240, 100);
                    break;
                case NoteValue.Gb:
                    delta = -20.0f * pMusicalNote.Cents / 100.0f;
                    color = new HSLColor(delta + 40, 240, 100);
                    break;
                case NoteValue.G:
                    delta = -20.0f * pMusicalNote.Cents / 100.0f;
                    color = new HSLColor(delta + 20, 240, 100);
                    break;
                case NoteValue.Ab:
                    delta = -20.0f * pMusicalNote.Cents / 100.0f;
                    color = new HSLColor(delta + 0, 240, 100);
                    break;
                case NoteValue.A:
                    delta = -20.0f * pMusicalNote.Cents / 100.0f;
                    color = new HSLColor(delta + 220, 240, 100);
                    break;
                case NoteValue.Bb:
                    delta = -20.0f * pMusicalNote.Cents / 100.0f;
                    color = new HSLColor(delta + 200, 240, 100);
                    break;
                case NoteValue.B:
                    delta = -20.0f * pMusicalNote.Cents / 100.0f;
                    color = new HSLColor(delta + 180, 240, 100);
                    break;
            }

            return color;
        }
    }
}
