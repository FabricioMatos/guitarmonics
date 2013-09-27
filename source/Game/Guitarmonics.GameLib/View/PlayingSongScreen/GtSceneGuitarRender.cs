using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.GameLib.Model;
using Guitarmonics.AudioLib.Analysis;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework;

namespace Guitarmonics.GameLib.View
{
    /// <summary>
    /// Class responsible for render (draw) the guitar and his elements.
    /// </summary>
    public class GtSceneGuitarRender
    {
        public XnaGame Game { get; protected set; }

        private int X;
        private int Y;
        private int ScreenHeight;
        private int Height;
        private int Distance;
        private int BallWidth;
        private int FretHeight;
        private int MatchAreaHeight;
        private float TickHeight;

        public GtSceneGuitarRender(XnaGame pGame, int x, int pScreenHeight)
        {
            this.Game = pGame;

            this.X = x;
            this.Y = 0;
            this.MatchAreaHeight = 40;
            this.Distance = 35;
            this.ScreenHeight = pScreenHeight;

            this.BallWidth = this.Distance - 5;

            this.Height = this.ScreenHeight - this.MatchAreaHeight;
        }

        public void Render(GtSceneGuitar pSceneGuitar, SpriteBatch pSpriteBatch)
        {
            RenderNeck(pSceneGuitar, pSpriteBatch);

            RenderFrets(pSceneGuitar, pSpriteBatch);

            RenderStrings(pSceneGuitar, pSpriteBatch);

            RenderVisibleNotes(pSceneGuitar, pSpriteBatch);

            RenderMatchBalls(pSceneGuitar, pSpriteBatch);

            DrawPanelWithAllNotesByFret(pSceneGuitar, pSpriteBatch);
        }

        private void DrawPanelWithAllNotesByFret(GtSceneGuitar pSceneGuitar, SpriteBatch pSpriteBatch)
        {
            for (var fret = 0; fret <= 21; fret++)
            {
                #region draw the ball/star/square

                Texture2D noteTexture;

                if (fret < 7)
                    noteTexture = this.Game.BallTexture;
                else if (fret < 14)
                    noteTexture = this.Game.StarTexture;
                else
                    noteTexture = this.Game.SquareTexture;

                var x = this.X + (7 * this.Distance);
                var y = 3 + (fret * this.BallWidth);

                pSpriteBatch.Draw(noteTexture, new Rectangle(x, y, this.BallWidth, this.BallWidth), CalculateColorByFret(fret));

                #endregion

                #region Draw the fret number

                var noteContent = fret.ToString();

                int shift;
                if (noteContent.Length == 1)
                    shift = 8; //1 digit
                else
                    shift = 2; //2 digits

                var x2 = x + 3; //2
                var y2 = y + 5; //4

                pSpriteBatch.DrawString(this.Game.FontFretNumbers, noteContent, new Vector2(x2 + shift - 2, y2), Color.Black);
                pSpriteBatch.DrawString(this.Game.FontFretNumbers, noteContent, new Vector2(x2 + shift, y2 - 2), Color.Black);
                pSpriteBatch.DrawString(this.Game.FontFretNumbers, noteContent, new Vector2(x2 + shift + 2, y2), Color.Black);
                pSpriteBatch.DrawString(this.Game.FontFretNumbers, noteContent, new Vector2(x2 + shift, y2 + 2), Color.Black);
                pSpriteBatch.DrawString(this.Game.FontFretNumbers, noteContent, new Vector2(x2 + shift, y2), Color.White);

                #endregion
            }
        }

        private void RenderNeck(GtSceneGuitar pSceneGuitar, SpriteBatch pSpriteBatch)
        {
            pSpriteBatch.Draw(this.Game.GuitarNeckTexture,
                new Rectangle(this.X - this.Distance, this.Y, 7 * this.Distance, this.Height + this.MatchAreaHeight), Color.Gray);
        }

        private void RenderFrets(GtSceneGuitar pSceneGuitar, SpriteBatch pSpriteBatch)
        {
            this.FretHeight = this.Height / pSceneGuitar.NumberOfVisibleBeats;
            this.TickHeight = this.FretHeight / 480.0f;

            int firstFretPosition = (int)(this.Height - ((480 - pSceneGuitar.CurrentPosition.Tick) * this.TickHeight));

            for (int i = 0; i < pSceneGuitar.NumberOfVisibleBeats; i++)
            {
                var position = firstFretPosition - (i * this.FretHeight);

                if (position >= this.Y)
                {
                    pSpriteBatch.Draw(this.Game.FretTexture,
                        new Rectangle(this.X - this.Distance, position, 7 * this.Distance, 2), Color.Gray);
                }
            }
        }

        private void RenderStrings(GtSceneGuitar pSceneGuitar, SpriteBatch pSpriteBatch)
        {
            var x = this.X;

            pSpriteBatch.Draw(this.Game.StringTexture,
                new Rectangle(x, this.Y, 4, this.Height + this.MatchAreaHeight), Color.White);
            x += this.Distance;

            pSpriteBatch.Draw(this.Game.StringTexture,
                new Rectangle(x, this.Y, 3, this.Height + this.MatchAreaHeight), Color.White);
            x += this.Distance;

            pSpriteBatch.Draw(this.Game.StringTexture,
                new Rectangle(x, this.Y, 3, this.Height + this.MatchAreaHeight), Color.White);
            x += this.Distance;

            pSpriteBatch.Draw(this.Game.StringTexture,
                new Rectangle(x, this.Y, 1, this.Height + this.MatchAreaHeight), Color.White);
            x += this.Distance;

            pSpriteBatch.Draw(this.Game.StringTexture,
                new Rectangle(x, this.Y, 1, this.Height + this.MatchAreaHeight), Color.White);
            x += this.Distance;

            pSpriteBatch.Draw(this.Game.StringTexture,
                new Rectangle(x, this.Y, 1, this.Height + this.MatchAreaHeight), Color.White);
            x += this.Distance;
        }

        private void RenderMatchBalls(GtSceneGuitar pSceneGuitar, SpriteBatch pSpriteBatch)
        {
            for (int iString = 1; iString <= 6; iString++)
            {
                var x = this.X - (this.Distance / 2); //start position - 6th string

                //positioning at the correct string
                x += (6 - iString) * this.Distance;

                x += 2;

                var color = Color.White;

                switch (iString)
                {
                    case 6:
                        x += 1; //this string is large
                        break;
                    case 5:
                        x += 1; //this string is large
                        break;
                    case 4:
                        x += 1; //this string is large
                        break;
                    case 3:
                        break;
                    case 2:
                        break;
                    case 1:
                        break;
                }

                pSpriteBatch.Draw(this.Game.MatchBallTexture,
                    new Rectangle(x, this.MatchBall_Y, this.BallWidth, this.BallWidth), Color.Silver);
            }

        }

        private int MatchBall_Y
        {
            get
            {
                return this.Height - (this.BallWidth / 2);
            }
        }

        private void RenderVisibleNotes(GtSceneGuitar pSceneGuitar, SpriteBatch pSpriteBatch)
        {
            IList<GtSceneGuitarNote> visibleNotes = pSceneGuitar.GetVisibleNotes();

            foreach (var note in visibleNotes)
            {
                RenderNote(pSceneGuitar, note, pSpriteBatch);
            }
        }

        private void RenderNote(GtSceneGuitar pSceneGuitar, GtSceneGuitarNote pSceneGuitarNote, SpriteBatch pSpriteBatch)
        {
            var x = this.X - (this.Distance / 2); //start position - 6th string

            //positioning at the correct string
            x += (6 - pSceneGuitarNote.String) * this.Distance;

            x += 2;

            var color = CalculateNoteColor(pSceneGuitarNote);

            switch (pSceneGuitarNote.String)
            {
                case 6:
                    x += 1; //this string is large
                    break;
                case 5:
                    x += 1; //this string is large
                    break;
                case 4:
                    x += 1; //this string is large
                    break;
                case 3:
                    break;
                case 2:
                    break;
                case 1:
                    break;
            }


            //Draw the boll
            int y = this.Height
                - (int)(this.TickHeight * pSceneGuitarNote.DistanceFromCurrentPosition)
                - (this.BallWidth / 2);


            if (y < 0) return;

            var noteRectangle = new Rectangle(x, y, this.BallWidth, this.BallWidth);
            this.RenderNoteDuration(pSceneGuitarNote, noteRectangle, color, pSpriteBatch);

            if (pSceneGuitarNote.Fret < 7)
                pSpriteBatch.Draw(this.Game.BallTexture, noteRectangle, color);
            else if (pSceneGuitarNote.Fret < 14)
                pSpriteBatch.Draw(this.Game.StarTexture, noteRectangle, color);
            else
                pSpriteBatch.Draw(this.Game.SquareTexture, noteRectangle, color);


            #region Draw the note content (note letter or fret number)

            string noteContent;
            int shift;

            noteContent = pSceneGuitarNote.Fret.ToString();
            //noteContent = pSceneGuitarNote.NoteValue.AsSharpedString();

            //Draw the fret number
            if (noteContent.Length == 1)
                shift = 8; //1 digit
            else
                shift = 2; //2 digits

            var x2 = x + 3;
            var y2 = y + 5;

            pSpriteBatch.DrawString(this.Game.FontFretNumbers, noteContent, new Vector2(x2 + shift - 2, y2), Color.Black);
            pSpriteBatch.DrawString(this.Game.FontFretNumbers, noteContent, new Vector2(x2 + shift, y2 - 2), Color.Black);
            pSpriteBatch.DrawString(this.Game.FontFretNumbers, noteContent, new Vector2(x2 + shift + 2, y2), Color.Black);
            pSpriteBatch.DrawString(this.Game.FontFretNumbers, noteContent, new Vector2(x2 + shift, y2 + 2), Color.Black);
            pSpriteBatch.DrawString(this.Game.FontFretNumbers, noteContent, new Vector2(x2 + shift, y2), Color.White);

            #endregion

            if (pSceneGuitarNote.Playing)
            {
                var fireHeight = 3 * this.BallWidth;
                var fireRectangle = new Rectangle(x, MatchBall_Y - fireHeight + (2 * this.BallWidth / 3), this.BallWidth, fireHeight);
                
                pSpriteBatch.Draw(this.Game.FireTexture,
                    fireRectangle, Color.White);
            }


            #region Draw the chord name

            var tickData = pSceneGuitar.TickDataTable[pSceneGuitarNote.StartPosition.Beat, pSceneGuitarNote.StartPosition.Tick];
            if (tickData.IsStartTick)
            {
                var chordName = tickData.RemarkOrChordName;
                pSpriteBatch.DrawString(
                    this.Game.FontFretNumbers,
                    " " + chordName,
                    new Vector2(this.X - (2 * this.Distance), y), //left
                    Color.Silver);

                if (tickData.DrawChord)
                {
                    RenderChordPicture(pSpriteBatch, tickData, y);
                }
            }

            #endregion

        }

        private static Color CalculateNoteColor(GtSceneGuitarNote pSceneGuitarNote)
        {
            //Color by Fret
            return CalculateColorByFret(pSceneGuitarNote.Fret);

            //Color by Note - Use the MusicalNote class to get the right color for this note
            //var auxNote = new MusicalNote(pSceneGuitarNote.NoteValue.ToString() + "3");
            //var color = auxNote.NoteColor();
            //return color;
        }

        private static Color CalculateColorByFret(int fret)
        {
            var index = fret % 7;

            switch (index)
            {
                case 0:
                    return new Color(100, 100, 255); //blue (a kind of blue)
                case 1:
                    return new Color(0, 255, 255); //cyan
                case 2:
                    return new Color(0, 255, 0); //green
                case 3:
                    return new Color(255, 255, 0); //yellow
                case 4:
                    return new Color(255, 127, 0); //orange
                case 5:
                    return new Color(255, 0, 0); //red
                case 6:
                    return new Color(255, 0, 255); //magenta
                default:
                    return new Color(255, 255, 255); //wihte (error!)
            }
        }

        private void RenderChordPicture(SpriteBatch pSpriteBatch, GtTickData pTickData, int pY)
        {
            int notesNumber = 0;
            int minorFret = int.MaxValue;

            #region Calculate Minor Fret

            if (pTickData.String1 != null)
            {
                notesNumber++;
                if (pTickData.String1 < minorFret) minorFret = pTickData.String1.Value;
            }
            if (pTickData.String2 != null)
            {
                notesNumber++;
                if (pTickData.String2 < minorFret) minorFret = pTickData.String2.Value;
            }
            if (pTickData.String3 != null)
            {
                notesNumber++;
                if (pTickData.String3 < minorFret) minorFret = pTickData.String3.Value;
            }
            if (pTickData.String4 != null)
            {
                notesNumber++;
                if (pTickData.String4 < minorFret) minorFret = pTickData.String4.Value;
            }
            if (pTickData.String5 != null)
            {
                notesNumber++;
                if (pTickData.String5 < minorFret) minorFret = pTickData.String5.Value;
            }
            if (pTickData.String6 != null)
            {
                notesNumber++;
                if (pTickData.String6 < minorFret) minorFret = pTickData.String6.Value;
            }

            #endregion

            int fretShift = 0; //real fret is the position (in picture) plus fretShift
            if (minorFret > 2)
                fretShift = minorFret - 2;

            var backgrouRectangle = new Rectangle(
                this.X + (6 * this.Distance) + 50,
                pY - 20,
                70,
                50);

            pSpriteBatch.Draw(
                this.Game.ChordPictureBackground,
                backgrouRectangle,
                Color.Gray);

            DrawPositionInChordPicture(pSpriteBatch, 6, pTickData.String6, backgrouRectangle, fretShift);
            DrawPositionInChordPicture(pSpriteBatch, 5, pTickData.String5, backgrouRectangle, fretShift);
            DrawPositionInChordPicture(pSpriteBatch, 4, pTickData.String4, backgrouRectangle, fretShift);
            DrawPositionInChordPicture(pSpriteBatch, 3, pTickData.String3, backgrouRectangle, fretShift);
            DrawPositionInChordPicture(pSpriteBatch, 2, pTickData.String2, backgrouRectangle, fretShift);
            DrawPositionInChordPicture(pSpriteBatch, 1, pTickData.String1, backgrouRectangle, fretShift);
        }

        private Color CalculateNoteColorByFretAndString(int pString, int pFret)
        {
            var noteValue = NoteValue.C; //any note...

            switch (pString)
            {
                case 1:
                    noteValue = NoteValue.E;
                    break;
                case 2:
                    noteValue = NoteValue.B;
                    break;
                case 3:
                    noteValue = NoteValue.G;
                    break;
                case 4:
                    noteValue = NoteValue.D;
                    break;
                case 5:
                    noteValue = NoteValue.A;
                    break;
                case 6:
                    noteValue = NoteValue.E;
                    break;
            }

            while (pFret > 0)
            {
                noteValue = noteValue.Shift(Interval.MinorSecond);
                pFret--;
            }

            //Use the MusicalNote class to get the right color for this note
            var auxNote = new MusicalNote(noteValue.ToString() + "3");
            return auxNote.NoteColor();
        }

        private void DrawPositionInChordPicture(SpriteBatch pSpriteBatch, int pString, int? pFret, Rectangle backgrouRectangle, int pfretShift)
        {
            if (pFret != null)
            {
                Color color = CalculateNoteColorByFretAndString(pString, pFret.Value);

                //if (pFret.Value == 0) return;

                var fret = pFret.Value - pfretShift;
                var x = backgrouRectangle.X + 3 + (5 - fret) * 13;
                var y = backgrouRectangle.Y - 4 + ((6 - pString) * 9);

                var noteRectangle = new Rectangle(
                    x,
                    y,
                    10,
                    10);

                pSpriteBatch.Draw(
                    this.Game.BallTexture,
                    noteRectangle,
                    color);
            }
        }

        private void RenderNoteDuration(GtSceneGuitarNote pSceneGuitarNote, Rectangle pNoteRectangle, Color pColor, SpriteBatch pSpriteBatch)
        {
            var width = pNoteRectangle.Width / 3;
            var x = pNoteRectangle.X + (pNoteRectangle.Width - width) / 2;

            //height
            var lengthInTicks = pSceneGuitarNote.EndPosition.AsTicks() - pSceneGuitarNote.StartPosition.AsTicks();
            var height = (int)(this.TickHeight * lengthInTicks);

            var y = (pNoteRectangle.Y - height) + (this.BallWidth / 2);

            Rectangle visibleArea = this.CutOffOutOfBoundArea(new Rectangle(x, y, width, height));

            //draw effect (back)
            if (pSceneGuitarNote.Playing)
            {
                Rectangle effectArea = this.CutOffOutOfBoundArea(new Rectangle(x - width / 2, y, 2 * width, height));

                //draw the duration bar
                pSpriteBatch.Draw(
                    this.Game.NoteSusteinTexture,
                    effectArea,
                    pColor);


                //draw the duration bar with playing effect
                pSpriteBatch.Draw(
                    this.Game.NoteSusteinPlayingEffect,
                    visibleArea,
                    pColor);
            }
            else
            {
                //draw the duration bar
                pSpriteBatch.Draw(
                    this.Game.NoteSusteinTexture,
                    visibleArea,
                    pColor);
            }

        }

        private Rectangle CutOffOutOfBoundArea(Rectangle pRectangle)
        {
            int topFat = 0;
            if (pRectangle.Y < this.Y)
                topFat = this.Y - pRectangle.Y;

            int bottomFat = 0;
            if ((pRectangle.Y + pRectangle.Height) > (this.Y + this.Height + this.MatchAreaHeight))
                bottomFat = (pRectangle.Y + pRectangle.Height) - (this.Y + this.Height + this.MatchAreaHeight);

            var newHeight = pRectangle.Height - topFat - bottomFat;
            var newY = pRectangle.Y + topFat;

            return new Rectangle(pRectangle.X, newY, pRectangle.Width, newHeight);
        }

    }
}
