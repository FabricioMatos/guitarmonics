using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Guitarmonics.Importer;
using Guitarmonics.SongData;

namespace ExperimentalSongPlayer
{
    public partial class Form1 : Form
    {
        public Form1()
        {
            InitializeComponent();
        }
        private PlayerManager _PlayerManager;
        private int HorizontalOffset = 0;
        const int STRING_SPACING = 10;

        private List<GpFileTestCase> TestSongs;
        private void Form1_Load(object sender, EventArgs e)
        {
            _PlayerManager = new PlayerManager(timer1);


            TestSongs = new List<GpFileTestCase>()
            {
                DataForSongTests.AsaBranca(),
                DataForSongTests.MeDeixa(),
                DataForSongTests.Stairway(),
                DataForSongTests.SmokeWater(),
                DataForSongTests.WhenYouWereYoung(),                
                DataForSongTests.AmericanIdiot(),                
                DataForSongTests.BasketCase(),                
            };

            foreach (var testCase in TestSongs)
            {
                comboBox1.Items.Add(testCase.ExpectedSong.Name);
            }

            comboBox1.SelectedIndex = 0;

        }

        private void DrawSong(Song song)
        {
            try
            {
                panel1.Controls.Clear();
                int x = 10;
                for (int m = 0; m < song.Measures.Count; m++)
                {
                    var pnlMeasure = PlaceMeasure(song, x, m);
                    _PlayerManager.MeasurePanels.Add(pnlMeasure);
                    x += pnlMeasure.Width;
                }

            }
            catch (Exception ex)
            {
                Text = ex.Message;
            }
        }

        private Panel PlaceMeasure(Song song, int x, int m)
        {

            var measure = song.Measures[m];
            var pnlMeasure = new Panel()
            {
                Left = x,
                Top = 10,
                Width = 10,
                AutoSize = true,
                BorderStyle = BorderStyle.FixedSingle,
            };

            var lblDescMeasure = new Label()
            {
                Left = 0,
                Top = 0,
                Width = 10,
                AutoSize = true,
                Text = "Measure " + m,
            };
            pnlMeasure.Controls.Add(lblDescMeasure);
            PlacePair(song, m, pnlMeasure, lblDescMeasure);
            panel1.Controls.Add(pnlMeasure);
            return pnlMeasure;
        }

        private static void PlacePair(Song song, int m, Panel pnlMeasure, Label lblDescMeasure)
        {

            int x2 = 0;
            int y = lblDescMeasure.Top + lblDescMeasure.Height + 10;
            var pairs = song.Measures[m].Pairs;
            for (int t = 0; t < pairs.Count; t++)
            {
                var pair = pairs[t];
                var pnlPair = new Panel()
                {
                    Left = 0,
                    Width = 10,
                    Top = y,
                    AutoSize = true,
                    BorderStyle = BorderStyle.FixedSingle,
                };

                pnlMeasure.Controls.Add(pnlPair);
                y += pnlPair.Height;
                x2 = PlaceBeats(x2, pair, pnlPair);
            }
        }

        private static int PlaceBeats(int x2, MeasureTrackPair pair, Panel pnlPair)
        {
            foreach (var beat in pair.Beats)
            {
                foreach (var note in beat.Notes)
                {
                    int noteTop = STRING_SPACING * 3;
                    string text = "$";
                    var strPlayed = note.StringPlayed;
                    if (strPlayed != null)
                    {
                        noteTop = STRING_SPACING * strPlayed.Value;
                        text = beat.Notes[0].Fret.ToString();
                    }
                    var l = new Label()
                    {
                        Left = x2,
                        Top = (6 * STRING_SPACING) - noteTop,
                        AutoSize = true,
                        Text = text,
                    };
                    pnlPair.Controls.Add(l);
                    x2 += l.Width;
                }
            }
            return x2;
        }

        private void comboBox1_SelectedIndexChanged(object sender, EventArgs e)
        {
            DrawSong(TestSongs[comboBox1.SelectedIndex].ExpectedSong);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            _PlayerManager.Start();
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            _PlayerManager.Step();
            HorizontalOffset = _PlayerManager.CurrentMeasurePanel.Left;
            _PlayerManager.CurrentMeasurePanel.BackColor = Color.Aqua;
            panel1.AutoScrollPosition = new Point(HorizontalOffset, 0);

        }


        private void button2_Click(object sender, EventArgs e)
        {
            _PlayerManager.Stop();
        }

    }
}
