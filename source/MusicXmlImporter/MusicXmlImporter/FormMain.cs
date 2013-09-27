using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Linq;
using System.Text;
using System.Windows.Forms;
using Guitarmonics.AudioLib.MusicXml;
using System.Xml;
using Guitarmonics.AudioLib.Common;
using Guitarmonics.AudioLib.MusicConfigFiles;
using System.IO;

namespace MusicXmlImporter
{
    public partial class FormMain : Form
    {

        private GuitarMusicXmlImporter MusicXmlImporter = new GuitarMusicXmlImporter();
        private XmlDocument MusicXmlContent;
        private IList<TrackInfo> TrackInfoList;

        public FormMain()
        {
            InitializeComponent();


            openFileDialog1.InitialDirectory = Directory.GetCurrentDirectory();

            //Test:
            //txtMusicXmlFile.Text = @"D:\Guitarmonics\trunk\Documentacao\Songs Database\The Killers - When You Where Young\The Killers - When You Were Young.xml";
            //OpenSelectedFile();
        }

        private void btnSelectFile_Click(object sender, EventArgs e)
        {
            if (openFileDialog1.ShowDialog() == DialogResult.OK)
            {
                txtMusicXmlFile.Text = openFileDialog1.FileName;
                OpenSelectedFile();
            }
        }

        private void OpenSelectedFile()
        {
            RemoveDtdDocTypeLine(txtMusicXmlFile.Text);

            this.MusicXmlContent = this.MusicXmlImporter.OpenMusicXmlFile(txtMusicXmlFile.Text);

            this.TrackInfoList = MusicXmlImporter.ListTracks(this.MusicXmlContent);

            lstTracks.DataSource = this.TrackInfoList;

            lstTracks.Focus();
        }

        private void RemoveDtdDocTypeLine(string p)
        {
            //"<!DOCTYPE score-partwise PUBLIC \"-//Recordare//DTD MusicXML 1.0 Partwise//EN\" \"/musicxml/partwise.dtd\">"
        }

        private void btnGenerate_Click(object sender, EventArgs e)
        {
            if (lstTracks.SelectedItem == null)
            {
                MessageBox.Show("No track was selected!", "Warning");
                return;
            }

            SortedList<GuitarScoreNote, GuitarScoreNote> scoreNotes =
                this.MusicXmlImporter.Import(this.MusicXmlContent, (TrackInfo)lstTracks.SelectedItem);

            var artist = "Artist Name";
            var title = "Song Name";

            var xmlScoreWriter = new XmlScoreWriter(artist, title, PlayingMode.EletricGuitarScore, scoreNotes);

            saveFileDialog1.FileName = txtMusicXmlFile.Text.Replace(".xml", ".Guitarmonics.xml");

            if (saveFileDialog1.ShowDialog() == DialogResult.OK)
            {
                if (File.Exists(saveFileDialog1.FileName))
                    File.Delete(saveFileDialog1.FileName);

                xmlScoreWriter.SaveXmlNotesToFile(saveFileDialog1.FileName);
            }
        }
    }
}
