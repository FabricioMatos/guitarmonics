namespace SongEditor
{
    partial class FormMain
    {
        /// <summary>
        /// Required designer variable.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Clean up any resources being used.
        /// </summary>
        /// <param name="disposing">true if managed resources should be disposed; otherwise, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        /// <summary>
        /// Required method for Designer support - do not modify
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.components = new System.ComponentModel.Container();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.tabControl1 = new System.Windows.Forms.TabControl();
            this.tabPageSelectFile = new System.Windows.Forms.TabPage();
            this.panel1 = new System.Windows.Forms.Panel();
            this.btnNext = new System.Windows.Forms.Button();
            this.btnSelectSongFile = new System.Windows.Forms.Button();
            this.edtSongFile = new System.Windows.Forms.TextBox();
            this.label4 = new System.Windows.Forms.Label();
            this.btnSelectAudioFile = new System.Windows.Forms.Button();
            this.btnSelectTabFile = new System.Windows.Forms.Button();
            this.label3 = new System.Windows.Forms.Label();
            this.comboBox1 = new System.Windows.Forms.ComboBox();
            this.edtAudioFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.edtTabFile = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.tabPageEdit = new System.Windows.Forms.TabPage();
            this.pnlTabPlaceHolder = new System.Windows.Forms.Panel();
            this.edtLogTemp = new System.Windows.Forms.TextBox();
            this.panelTop = new System.Windows.Forms.Panel();
            this.btnStop = new System.Windows.Forms.Button();
            this.btnPlayPause = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblSongTime = new System.Windows.Forms.Label();
            this.openFileDialogTab = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogSong = new System.Windows.Forms.OpenFileDialog();
            this.openFileDialogAudio = new System.Windows.Forms.OpenFileDialog();
            this.button1 = new System.Windows.Forms.Button();
            this.tabControl1.SuspendLayout();
            this.tabPageSelectFile.SuspendLayout();
            this.panel1.SuspendLayout();
            this.tabPageEdit.SuspendLayout();
            this.panelTop.SuspendLayout();
            this.SuspendLayout();
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // tabControl1
            // 
            this.tabControl1.Controls.Add(this.tabPageSelectFile);
            this.tabControl1.Controls.Add(this.tabPageEdit);
            this.tabControl1.Dock = System.Windows.Forms.DockStyle.Fill;
            this.tabControl1.Location = new System.Drawing.Point(0, 0);
            this.tabControl1.Name = "tabControl1";
            this.tabControl1.SelectedIndex = 0;
            this.tabControl1.Size = new System.Drawing.Size(740, 407);
            this.tabControl1.TabIndex = 1;
            this.tabControl1.SelectedIndexChanged += new System.EventHandler(this.tabControl1_SelectedIndexChanged);
            // 
            // tabPageSelectFile
            // 
            this.tabPageSelectFile.Controls.Add(this.panel1);
            this.tabPageSelectFile.Controls.Add(this.btnSelectSongFile);
            this.tabPageSelectFile.Controls.Add(this.edtSongFile);
            this.tabPageSelectFile.Controls.Add(this.label4);
            this.tabPageSelectFile.Controls.Add(this.btnSelectAudioFile);
            this.tabPageSelectFile.Controls.Add(this.btnSelectTabFile);
            this.tabPageSelectFile.Controls.Add(this.label3);
            this.tabPageSelectFile.Controls.Add(this.comboBox1);
            this.tabPageSelectFile.Controls.Add(this.edtAudioFile);
            this.tabPageSelectFile.Controls.Add(this.label2);
            this.tabPageSelectFile.Controls.Add(this.edtTabFile);
            this.tabPageSelectFile.Controls.Add(this.label1);
            this.tabPageSelectFile.Location = new System.Drawing.Point(4, 22);
            this.tabPageSelectFile.Name = "tabPageSelectFile";
            this.tabPageSelectFile.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageSelectFile.Size = new System.Drawing.Size(732, 381);
            this.tabPageSelectFile.TabIndex = 0;
            this.tabPageSelectFile.Text = "Select File";
            this.tabPageSelectFile.UseVisualStyleBackColor = true;
            // 
            // panel1
            // 
            this.panel1.Controls.Add(this.btnNext);
            this.panel1.Dock = System.Windows.Forms.DockStyle.Bottom;
            this.panel1.Location = new System.Drawing.Point(3, 352);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(726, 26);
            this.panel1.TabIndex = 11;
            // 
            // btnNext
            // 
            this.btnNext.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnNext.Location = new System.Drawing.Point(640, 3);
            this.btnNext.Name = "btnNext";
            this.btnNext.Size = new System.Drawing.Size(75, 23);
            this.btnNext.TabIndex = 0;
            this.btnNext.Text = "Next";
            this.btnNext.UseVisualStyleBackColor = true;
            this.btnNext.Click += new System.EventHandler(this.btnNext_Click);
            // 
            // btnSelectSongFile
            // 
            this.btnSelectSongFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectSongFile.Location = new System.Drawing.Point(688, 91);
            this.btnSelectSongFile.Name = "btnSelectSongFile";
            this.btnSelectSongFile.Size = new System.Drawing.Size(30, 23);
            this.btnSelectSongFile.TabIndex = 10;
            this.btnSelectSongFile.Text = "...";
            this.btnSelectSongFile.UseVisualStyleBackColor = true;
            this.btnSelectSongFile.Click += new System.EventHandler(this.btnSelectGmlFile_Click);
            // 
            // edtSongFile
            // 
            this.edtSongFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edtSongFile.Location = new System.Drawing.Point(69, 93);
            this.edtSongFile.Name = "edtSongFile";
            this.edtSongFile.Size = new System.Drawing.Size(616, 20);
            this.edtSongFile.TabIndex = 9;
            this.edtSongFile.Text = "D:\\_GuitarMonics\\svn-guitarmonics\\trunk\\Fontes\\SongEditor\\SongFilesForTest\\Metall" +
                "ica - For Whom The Bell Tolls\\metallica-for_whom_the_bell_tolls.song.xml";
            // 
            // label4
            // 
            this.label4.AutoSize = true;
            this.label4.Location = new System.Drawing.Point(11, 96);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(54, 13);
            this.label4.TabIndex = 8;
            this.label4.Text = "Song File:";
            // 
            // btnSelectAudioFile
            // 
            this.btnSelectAudioFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectAudioFile.Location = new System.Drawing.Point(688, 173);
            this.btnSelectAudioFile.Name = "btnSelectAudioFile";
            this.btnSelectAudioFile.Size = new System.Drawing.Size(30, 23);
            this.btnSelectAudioFile.TabIndex = 7;
            this.btnSelectAudioFile.Text = "...";
            this.btnSelectAudioFile.UseVisualStyleBackColor = true;
            this.btnSelectAudioFile.Click += new System.EventHandler(this.btnSelectAudioFile_Click);
            // 
            // btnSelectTabFile
            // 
            this.btnSelectTabFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectTabFile.Location = new System.Drawing.Point(688, 9);
            this.btnSelectTabFile.Name = "btnSelectTabFile";
            this.btnSelectTabFile.Size = new System.Drawing.Size(30, 23);
            this.btnSelectTabFile.TabIndex = 6;
            this.btnSelectTabFile.Text = "...";
            this.btnSelectTabFile.UseVisualStyleBackColor = true;
            this.btnSelectTabFile.Click += new System.EventHandler(this.btnSelectTabFile_Click);
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.Location = new System.Drawing.Point(25, 41);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(38, 13);
            this.label3.TabIndex = 5;
            this.label3.Text = "Track:";
            // 
            // comboBox1
            // 
            this.comboBox1.FormattingEnabled = true;
            this.comboBox1.Location = new System.Drawing.Point(69, 38);
            this.comboBox1.Name = "comboBox1";
            this.comboBox1.Size = new System.Drawing.Size(257, 21);
            this.comboBox1.TabIndex = 4;
            this.comboBox1.Enter += new System.EventHandler(this.comboBox1_Enter);
            // 
            // edtAudioFile
            // 
            this.edtAudioFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edtAudioFile.Location = new System.Drawing.Point(69, 175);
            this.edtAudioFile.Name = "edtAudioFile";
            this.edtAudioFile.Size = new System.Drawing.Size(616, 20);
            this.edtAudioFile.TabIndex = 3;
            this.edtAudioFile.Text = "D:\\_GuitarMonics\\svn-guitarmonics\\trunk\\Fontes\\SongEditor\\SongFilesForTest\\Metall" +
                "ica - For Whom The Bell Tolls\\metallica-for_whom_the_bell_tolls.mp3";
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Location = new System.Drawing.Point(7, 178);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(56, 13);
            this.label2.TabIndex = 2;
            this.label2.Text = "Audio File:";
            // 
            // edtTabFile
            // 
            this.edtTabFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edtTabFile.Location = new System.Drawing.Point(69, 11);
            this.edtTabFile.Name = "edtTabFile";
            this.edtTabFile.Size = new System.Drawing.Size(616, 20);
            this.edtTabFile.TabIndex = 1;
            this.edtTabFile.Text = "D:\\_GuitarMonics\\svn-guitarmonics\\trunk\\Fontes\\SongEditor\\SongFilesForTest\\Metall" +
                "ica - For Whom The Bell Tolls\\metallica-for_whom_the_bell_tolls.mid";
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.Location = new System.Drawing.Point(15, 14);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(48, 13);
            this.label1.TabIndex = 0;
            this.label1.Text = "Tab File:";
            // 
            // tabPageEdit
            // 
            this.tabPageEdit.Controls.Add(this.pnlTabPlaceHolder);
            this.tabPageEdit.Controls.Add(this.edtLogTemp);
            this.tabPageEdit.Controls.Add(this.panelTop);
            this.tabPageEdit.Location = new System.Drawing.Point(4, 22);
            this.tabPageEdit.Name = "tabPageEdit";
            this.tabPageEdit.Padding = new System.Windows.Forms.Padding(3);
            this.tabPageEdit.Size = new System.Drawing.Size(732, 381);
            this.tabPageEdit.TabIndex = 1;
            this.tabPageEdit.Text = "Edit/Synchronize";
            this.tabPageEdit.UseVisualStyleBackColor = true;
            // 
            // pnlTabPlaceHolder
            // 
            this.pnlTabPlaceHolder.Dock = System.Windows.Forms.DockStyle.Top;
            this.pnlTabPlaceHolder.Location = new System.Drawing.Point(3, 43);
            this.pnlTabPlaceHolder.Name = "pnlTabPlaceHolder";
            this.pnlTabPlaceHolder.Size = new System.Drawing.Size(726, 100);
            this.pnlTabPlaceHolder.TabIndex = 2;
            // 
            // edtLogTemp
            // 
            this.edtLogTemp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom)
                        | System.Windows.Forms.AnchorStyles.Left)
                        | System.Windows.Forms.AnchorStyles.Right)));
            this.edtLogTemp.Location = new System.Drawing.Point(14, 149);
            this.edtLogTemp.Multiline = true;
            this.edtLogTemp.Name = "edtLogTemp";
            this.edtLogTemp.Size = new System.Drawing.Size(754, 226);
            this.edtLogTemp.TabIndex = 1;
            // 
            // panelTop
            // 
            this.panelTop.Controls.Add(this.button1);
            this.panelTop.Controls.Add(this.btnStop);
            this.panelTop.Controls.Add(this.btnPlayPause);
            this.panelTop.Controls.Add(this.label5);
            this.panelTop.Controls.Add(this.lblSongTime);
            this.panelTop.Dock = System.Windows.Forms.DockStyle.Top;
            this.panelTop.Location = new System.Drawing.Point(3, 3);
            this.panelTop.Name = "panelTop";
            this.panelTop.Size = new System.Drawing.Size(726, 40);
            this.panelTop.TabIndex = 0;
            // 
            // btnStop
            // 
            this.btnStop.Location = new System.Drawing.Point(92, 9);
            this.btnStop.Name = "btnStop";
            this.btnStop.Size = new System.Drawing.Size(75, 23);
            this.btnStop.TabIndex = 3;
            this.btnStop.Text = "Stop";
            this.btnStop.UseVisualStyleBackColor = true;
            this.btnStop.Click += new System.EventHandler(this.btnStop_Click);
            // 
            // btnPlayPause
            // 
            this.btnPlayPause.Location = new System.Drawing.Point(11, 8);
            this.btnPlayPause.Name = "btnPlayPause";
            this.btnPlayPause.Size = new System.Drawing.Size(75, 23);
            this.btnPlayPause.TabIndex = 2;
            this.btnPlayPause.Text = "Play";
            this.btnPlayPause.UseVisualStyleBackColor = true;
            this.btnPlayPause.Click += new System.EventHandler(this.btnPlayPause_Click);
            // 
            // label5
            // 
            this.label5.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label5.AutoSize = true;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.Location = new System.Drawing.Point(597, 10);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(52, 20);
            this.label5.TabIndex = 1;
            this.label5.Text = "Time:";
            // 
            // lblSongTime
            // 
            this.lblSongTime.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.lblSongTime.AutoSize = true;
            this.lblSongTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSongTime.Location = new System.Drawing.Point(650, 10);
            this.lblSongTime.Name = "lblSongTime";
            this.lblSongTime.Size = new System.Drawing.Size(71, 20);
            this.lblSongTime.TabIndex = 0;
            this.lblSongTime.Text = "0:00:000";
            // 
            // openFileDialogTab
            // 
            this.openFileDialogTab.Filter = "MIDI files|*.midi|All files|*.*";
            // 
            // openFileDialogSong
            // 
            this.openFileDialogSong.Filter = "Guitarmonics Song file|*.song.xml|All files|*.*";
            // 
            // openFileDialogAudio
            // 
            this.openFileDialogAudio.Filter = "MP3 file|*.mp3|All files|*.*";
            // 
            // button1
            // 
            this.button1.Location = new System.Drawing.Point(173, 10);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(75, 23);
            this.button1.TabIndex = 4;
            this.button1.Text = "Pin (Key 1)";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(740, 407);
            this.Controls.Add(this.tabControl1);
            this.KeyPreview = true;
            this.Name = "FormMain";
            this.Text = "Guitarmonics Song Editor";
            this.Load += new System.EventHandler(this.Form3UsingAudioListener_Load);
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.Form3UsingAudioListener_FormClosed);
            this.KeyDown += new System.Windows.Forms.KeyEventHandler(this.FormMain_KeyDown);
            this.tabControl1.ResumeLayout(false);
            this.tabPageSelectFile.ResumeLayout(false);
            this.tabPageSelectFile.PerformLayout();
            this.panel1.ResumeLayout(false);
            this.tabPageEdit.ResumeLayout(false);
            this.tabPageEdit.PerformLayout();
            this.panelTop.ResumeLayout(false);
            this.panelTop.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.TabControl tabControl1;
        private System.Windows.Forms.TabPage tabPageSelectFile;
        private System.Windows.Forms.TabPage tabPageEdit;
        private System.Windows.Forms.Button btnSelectTabFile;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.ComboBox comboBox1;
        private System.Windows.Forms.TextBox edtAudioFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.TextBox edtTabFile;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnSelectAudioFile;
        private System.Windows.Forms.Button btnSelectSongFile;
        private System.Windows.Forms.TextBox edtSongFile;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.Button btnNext;
        private System.Windows.Forms.OpenFileDialog openFileDialogTab;
        private System.Windows.Forms.OpenFileDialog openFileDialogSong;
        private System.Windows.Forms.OpenFileDialog openFileDialogAudio;
        private System.Windows.Forms.Panel panelTop;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblSongTime;
        private System.Windows.Forms.Button btnStop;
        private System.Windows.Forms.Button btnPlayPause;
        private System.Windows.Forms.TextBox edtLogTemp;
        private System.Windows.Forms.Panel pnlTabPlaceHolder;
        private System.Windows.Forms.Button button1;
    }
}