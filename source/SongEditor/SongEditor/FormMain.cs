using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.AudioLib.Analysis;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.AudioLib.Player;

namespace SongEditor
{

    public partial class FormMain : Form
    {
        internal class DoubleBufferedPanel : Panel
        {
            public DoubleBufferedPanel()
                : base()
            {
                //http://www.codeproject.com/KB/miscctrl/ScrollingTextControlArtic.aspx

                this.SetStyle(ControlStyles.DoubleBuffer, true);
                this.SetStyle(ControlStyles.AllPaintingInWmPaint, true);
                this.SetStyle(ControlStyles.UserPaint, true);
            }
        }

        public FormMain()
        {
            InitializeComponent();

            fSongPlayer = new SongPlayer();
        }

        private DoubleBufferedPanel pnlTab;
        private ISongPlayer fSongPlayer;

        private void Form3UsingAudioListener_Load(object sender, EventArgs e)
        {
            // pnlTab
            this.pnlTab = new DoubleBufferedPanel();
            pnlTabPlaceHolder.Controls.Add(pnlTab);
            this.pnlTab.BackColor = Color.Black;
            this.pnlTab.Dock = System.Windows.Forms.DockStyle.Fill;
            this.pnlTab.Location = new System.Drawing.Point(0, 0);
            this.pnlTab.Name = "pnlTab";
            this.pnlTab.TabIndex = 1;
            this.pnlTab.Paint += this.pnlMain_Paint;
            this.pnlTab.Height = 150;

            timer1.Interval = 25;
            timer1.Enabled = true;
        }

        private void timer1_Tick(object sender, EventArgs e)
        {
            if (fSongPlayer.Status == SongPlayerStatus.Playing)
            {
                long moment = (long)Math.Truncate(fSongPlayer.CurrentPositionAsSeconds * 1000);

                long momentMin = 0;
                long momentSec = 0;
                long momentMilisec = 0;

                //split the MomentInMiliseconds in Min:Sec:Milisec
                if (moment > 0)
                {
                    momentMin = moment / 60000;
                    momentSec = (moment - momentMin * 60000) / 1000;
                    momentMilisec = moment % 1000;
                }

                lblSongTime.Text = string.Format("{0}:{1}:{2}",
                    momentMin.ToString("D1"),
                    momentSec.ToString("D2"),
                    momentMilisec.ToString("D3"));
            }

        }

        private void pnlMain_Paint(object sender, PaintEventArgs e)
        {
            try
            {
                var brush = new SolidBrush(Color.White);
                var pen = new Pen(Brushes.Black, 1);

                e.Graphics.FillRectangle(brush, 0, 0, pnlTab.Width, pnlTab.Height);
                e.Graphics.DrawRectangle(pen, 0, 0, pnlTab.Width - pen.Width, pnlTab.Height - pen.Width);

                //e.Graphics.DrawString(pNoteName, new Font("Arial", 7), Brushes.White, x + 12, y);
            }
            catch (Exception)
            {
            }
        }

        private void Form3UsingAudioListener_FormClosed(object sender, FormClosedEventArgs e)
        {
        }

        private void btnSelectTabFile_Click(object sender, EventArgs e)
        {
            if (openFileDialogTab.ShowDialog() == DialogResult.OK)
            {
                edtTabFile.Text = openFileDialogTab.FileName;
            }
        }

        private void btnSelectGmlFile_Click(object sender, EventArgs e)
        {
            if (openFileDialogSong.ShowDialog() == DialogResult.OK)
            {
                edtSongFile.Text = openFileDialogSong.FileName;
            }
        }

        private void btnSelectAudioFile_Click(object sender, EventArgs e)
        {
            if (openFileDialogAudio.ShowDialog() == DialogResult.OK)
            {
                edtAudioFile.Text = openFileDialogAudio.FileName;
            }
        }

        private void tabControl1_SelectedIndexChanged(object sender, EventArgs e)
        {

        }

        private void btnPlayPause_Click(object sender, EventArgs e)
        {
            if (fSongPlayer.Status == SongPlayerStatus.Playing)
            {
                fSongPlayer.Pause();
                btnPlayPause.Text = "Play";
            }
            else
            {
                if (fSongPlayer.Status != SongPlayerStatus.Playing)
                {
                    fSongPlayer.SetupSong(edtAudioFile.Text, GtTimeSignature.Time4x4);
                }

                fSongPlayer.Play();
                btnPlayPause.Text = "Pause";
            }
        }

        private void btnStop_Click(object sender, EventArgs e)
        {
            fSongPlayer.Stop();
            btnPlayPause.Text = "Play";
        }

        private void FormMain_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.D1)
            {
                edtLogTemp.Text += lblSongTime.Text + "\r\n";
            }
        }

        private void btnNext_Click(object sender, EventArgs e)
        {

        }

        private void comboBox1_Enter(object sender, EventArgs e)
        {

        }

        private void button1_Click(object sender, EventArgs e)
        {
            edtLogTemp.Text += lblSongTime.Text + "\r\n";
        }
    }
}
