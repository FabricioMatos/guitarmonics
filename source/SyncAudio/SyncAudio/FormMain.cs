using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Guitarmonics.AudioLib.Player;

namespace SyncAudio
{
    public partial class FormMain : Form
    {
        private ISongPlayer fSongPlayer;
        private List<string> fContent = new List<string>();
        private int fBeat;
        private Label[] LabelsForScoreNotes = new Label[6]; //TODO: Nao precisa mais desse array

        public FormMain()
        {
            InitializeComponent();

            fSongPlayer = new SongPlayer();

            timer1.Interval = 25;
            timer1.Enabled = true;

            btnClear_Click(null, null);

            LabelsForScoreNotes[0] = lblScoreNote1;
            //LabelsForScoreNotes[1] = lblScoreNote2;
            //LabelsForScoreNotes[2] = lblScoreNote3;
            //LabelsForScoreNotes[3] = lblScoreNote4;
            //LabelsForScoreNotes[4] = lblScoreNote5;
            //LabelsForScoreNotes[5] = lblScoreNote6;
        }

        private void btnSelectAudioFile_Click(object sender, EventArgs e)
        {
            if (openFileDialogAudio.ShowDialog() == DialogResult.OK)
            {
                edtAudioFile.Text = openFileDialogAudio.FileName;
            }

            if (edtAudioFile.Text.Trim() != "")
            {
                if ((fSongPlayer.Status != SongPlayerStatus.Stopped) && (fSongPlayer.Status != SongPlayerStatus.NotInitialized))
                    fSongPlayer.Stop();
                
                fSongPlayer.Dispose();
                
                fSongPlayer = new SongPlayer();

                fSongPlayer.SetupSong(edtAudioFile.Text, GtTimeSignature.Time4x4);
                fSongPlayer.LoadStream(); //start the driver (and show the BASSLib splash)
            }
        }

        private void btnPlayPause_Click(object sender, EventArgs e)
        {
            if (fSongPlayer.Status == SongPlayerStatus.Playing)
            {
                fSongPlayer.Stop();
                btnPlayPause.Text = "Play";
                btnSyncPoint.Enabled = false;
            }
            else
            {
                if (fSongPlayer.Status != SongPlayerStatus.Playing)
                {
                    fSongPlayer.SetupSong(edtAudioFile.Text, GtTimeSignature.Time4x4);
                }

                fBeat = (int)txtStartAt.Value;

                lblBeat.Text = fBeat.ToString();

                fSongPlayer.Play();

                btnPlayPause.Text = "Stop";

                btnSyncPoint.Enabled = true;
                btnSyncPoint.Focus();
            }
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

        private void FormMain_PreviewKeyDown(object sender, PreviewKeyDownEventArgs e)
        {
        }

        private void btnClear_Click(object sender, EventArgs e)
        {
            fContent.Clear();
            fBeat = (int)txtStartAt.Value;

            GenerateXmlContent();

            lblScoreNote1.Text = "";
            //lblScoreNote2.Text = "";
            //lblScoreNote3.Text = "";
            //lblScoreNote4.Text = "";
            //lblScoreNote5.Text = "";
            //lblScoreNote6.Text = "";

            btnSyncPoint.Focus();
        }

        private void GenerateXmlContent()
        {
            edtLogTemp.Clear();
            edtLogTemp.Text += "<?xml version=\"1.0\" encoding=\"utf-8\" ?>" + "\r\n";
            edtLogTemp.Text += "<Song Artist=\"\" Title=\"\" PlayingMode=\"EletricGuitarScore\">" + "\r\n";

            foreach (var item in fContent)
            {
                edtLogTemp.Text += item + "\r\n";
            }

            edtLogTemp.Text += "</Song>" + "\r\n";
        }

        private void btnSyncPoint_Click(object sender, EventArgs e)
        {
            long momentMin = 0;
            long momentSec = 0;
            long momentMilisec = 0;

            if (fSongPlayer.Status == SongPlayerStatus.Playing)
            {
                long moment = (long)Math.Truncate(fSongPlayer.CurrentPositionAsSeconds * 1000);

                //split the MomentInMiliseconds in Min:Sec:Milisec
                if (moment > 0)
                {
                    momentMin = moment / 60000;
                    momentSec = (moment - momentMin * 60000) / 1000;
                    momentMilisec = moment % 1000;
                }
            }

            var item = string.Format("  <ScoreNote Beat=\"{0}\" Tick=\"0\" SyncSongPin=\"{1}:{2}:{3}\"/>",
                fBeat, momentMin, momentSec, momentMilisec);

            lblBeat.Text = fBeat.ToString();

            fContent.Add(item);

            AddNewScoreNote(item);

            if (rdb1.Checked)
                fBeat += 1;

            if (rdb2.Checked)
                fBeat += 2;

        }

        private void AddNewScoreNote(string item)
        {
            lblScoreNote1.Text = item;

            //lblScoreNote1.Text = "";
            //lblScoreNote2.Text = "";
            //lblScoreNote3.Text = "";
            //lblScoreNote4.Text = "";
            //lblScoreNote5.Text = "";
            //lblScoreNote6.Text = "";

            //var contents = fContent.Skip(fContent.Count - 6);

            //for (int i = 0; i < contents.Count(); i++)
            //{
            //    LabelsForScoreNotes[i].Text = contents.ElementAt(i);
            //}
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            GenerateXmlContent();
        }

        private void rdb1_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb1.Checked)
                txtStartAt.Value = 2;
        }

        private void rdb2_CheckedChanged(object sender, EventArgs e)
        {
            if (rdb2.Checked)
                txtStartAt.Value = 3;
        }
    }
}
