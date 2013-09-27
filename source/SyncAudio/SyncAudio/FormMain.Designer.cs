namespace SyncAudio
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(FormMain));
            this.btnSelectAudioFile = new System.Windows.Forms.Button();
            this.edtAudioFile = new System.Windows.Forms.TextBox();
            this.label2 = new System.Windows.Forms.Label();
            this.openFileDialogAudio = new System.Windows.Forms.OpenFileDialog();
            this.btnPlayPause = new System.Windows.Forms.Button();
            this.label5 = new System.Windows.Forms.Label();
            this.lblSongTime = new System.Windows.Forms.Label();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.rdb1 = new System.Windows.Forms.RadioButton();
            this.rdb2 = new System.Windows.Forms.RadioButton();
            this.edtLogTemp = new System.Windows.Forms.TextBox();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnSyncPoint = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.txtStartAt = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.lblBeat = new System.Windows.Forms.Label();
            this.btnGenerate = new System.Windows.Forms.Button();
            this.lblScoreNote1 = new System.Windows.Forms.Label();
            ((System.ComponentModel.ISupportInitialize)(this.txtStartAt)).BeginInit();
            this.SuspendLayout();
            // 
            // btnSelectAudioFile
            // 
            this.btnSelectAudioFile.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSelectAudioFile.Location = new System.Drawing.Point(875, 12);
            this.btnSelectAudioFile.Margin = new System.Windows.Forms.Padding(4);
            this.btnSelectAudioFile.Name = "btnSelectAudioFile";
            this.btnSelectAudioFile.Size = new System.Drawing.Size(40, 28);
            this.btnSelectAudioFile.TabIndex = 10;
            this.btnSelectAudioFile.Text = "...";
            this.btnSelectAudioFile.UseVisualStyleBackColor = true;
            this.btnSelectAudioFile.Click += new System.EventHandler(this.btnSelectAudioFile_Click);
            // 
            // edtAudioFile
            // 
            this.edtAudioFile.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtAudioFile.Location = new System.Drawing.Point(93, 15);
            this.edtAudioFile.Margin = new System.Windows.Forms.Padding(4);
            this.edtAudioFile.Name = "edtAudioFile";
            this.edtAudioFile.Size = new System.Drawing.Size(772, 22);
            this.edtAudioFile.TabIndex = 9;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.BackColor = System.Drawing.Color.Transparent;
            this.label2.ForeColor = System.Drawing.Color.White;
            this.label2.Location = new System.Drawing.Point(11, 18);
            this.label2.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(74, 17);
            this.label2.TabIndex = 8;
            this.label2.Text = "Audio File:";
            // 
            // openFileDialogAudio
            // 
            this.openFileDialogAudio.Filter = "MP3 file|*.mp3|All files|*.*";
            // 
            // btnPlayPause
            // 
            this.btnPlayPause.Location = new System.Drawing.Point(16, 47);
            this.btnPlayPause.Margin = new System.Windows.Forms.Padding(4);
            this.btnPlayPause.Name = "btnPlayPause";
            this.btnPlayPause.Size = new System.Drawing.Size(100, 28);
            this.btnPlayPause.TabIndex = 11;
            this.btnPlayPause.Text = "Play";
            this.btnPlayPause.UseVisualStyleBackColor = true;
            this.btnPlayPause.Click += new System.EventHandler(this.btnPlayPause_Click);
            // 
            // label5
            // 
            this.label5.AutoSize = true;
            this.label5.BackColor = System.Drawing.Color.Transparent;
            this.label5.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label5.ForeColor = System.Drawing.Color.White;
            this.label5.Location = new System.Drawing.Point(127, 50);
            this.label5.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(67, 25);
            this.label5.TabIndex = 13;
            this.label5.Text = "Time:";
            // 
            // lblSongTime
            // 
            this.lblSongTime.AutoSize = true;
            this.lblSongTime.BackColor = System.Drawing.Color.Transparent;
            this.lblSongTime.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblSongTime.ForeColor = System.Drawing.Color.White;
            this.lblSongTime.Location = new System.Drawing.Point(198, 50);
            this.lblSongTime.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblSongTime.Name = "lblSongTime";
            this.lblSongTime.Size = new System.Drawing.Size(90, 25);
            this.lblSongTime.TabIndex = 12;
            this.lblSongTime.Text = "0:00:000";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // rdb1
            // 
            this.rdb1.AutoSize = true;
            this.rdb1.BackColor = System.Drawing.Color.Transparent;
            this.rdb1.ForeColor = System.Drawing.Color.White;
            this.rdb1.Location = new System.Drawing.Point(16, 94);
            this.rdb1.Margin = new System.Windows.Forms.Padding(4);
            this.rdb1.Name = "rdb1";
            this.rdb1.Size = new System.Drawing.Size(213, 21);
            this.rdb1.TabIndex = 14;
            this.rdb1.Text = "[1] Press space for each time";
            this.rdb1.UseVisualStyleBackColor = false;
            this.rdb1.CheckedChanged += new System.EventHandler(this.rdb1_CheckedChanged);
            // 
            // rdb2
            // 
            this.rdb2.AutoSize = true;
            this.rdb2.BackColor = System.Drawing.Color.Transparent;
            this.rdb2.Checked = true;
            this.rdb2.ForeColor = System.Drawing.Color.White;
            this.rdb2.Location = new System.Drawing.Point(15, 122);
            this.rdb2.Margin = new System.Windows.Forms.Padding(4);
            this.rdb2.Name = "rdb2";
            this.rdb2.Size = new System.Drawing.Size(245, 21);
            this.rdb2.TabIndex = 15;
            this.rdb2.TabStop = true;
            this.rdb2.Text = "[2] Press space for each two times";
            this.rdb2.UseVisualStyleBackColor = false;
            this.rdb2.CheckedChanged += new System.EventHandler(this.rdb2_CheckedChanged);
            // 
            // edtLogTemp
            // 
            this.edtLogTemp.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.edtLogTemp.BackColor = System.Drawing.Color.White;
            this.edtLogTemp.Location = new System.Drawing.Point(18, 197);
            this.edtLogTemp.Margin = new System.Windows.Forms.Padding(4);
            this.edtLogTemp.Multiline = true;
            this.edtLogTemp.Name = "edtLogTemp";
            this.edtLogTemp.ReadOnly = true;
            this.edtLogTemp.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this.edtLogTemp.Size = new System.Drawing.Size(892, 267);
            this.edtLogTemp.TabIndex = 16;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnClear.Location = new System.Drawing.Point(594, 114);
            this.btnClear.Margin = new System.Windows.Forms.Padding(4);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(100, 28);
            this.btnClear.TabIndex = 17;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnSyncPoint
            // 
            this.btnSyncPoint.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSyncPoint.Enabled = false;
            this.btnSyncPoint.Location = new System.Drawing.Point(702, 114);
            this.btnSyncPoint.Margin = new System.Windows.Forms.Padding(4);
            this.btnSyncPoint.Name = "btnSyncPoint";
            this.btnSyncPoint.Size = new System.Drawing.Size(100, 28);
            this.btnSyncPoint.TabIndex = 18;
            this.btnSyncPoint.Text = "Sync Point";
            this.btnSyncPoint.UseVisualStyleBackColor = true;
            this.btnSyncPoint.Click += new System.EventHandler(this.btnSyncPoint_Click);
            // 
            // label1
            // 
            this.label1.AutoSize = true;
            this.label1.BackColor = System.Drawing.Color.Transparent;
            this.label1.ForeColor = System.Drawing.Color.White;
            this.label1.Location = new System.Drawing.Point(351, 120);
            this.label1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(128, 17);
            this.label1.TabIndex = 19;
            this.label1.Text = "Start Sync at Time:";
            // 
            // txtStartAt
            // 
            this.txtStartAt.Location = new System.Drawing.Point(488, 118);
            this.txtStartAt.Margin = new System.Windows.Forms.Padding(4);
            this.txtStartAt.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.txtStartAt.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.txtStartAt.Name = "txtStartAt";
            this.txtStartAt.Size = new System.Drawing.Size(55, 22);
            this.txtStartAt.TabIndex = 21;
            this.txtStartAt.Value = new decimal(new int[] {
            3,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.AutoSize = true;
            this.label3.BackColor = System.Drawing.Color.Transparent;
            this.label3.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label3.ForeColor = System.Drawing.Color.White;
            this.label3.Location = new System.Drawing.Point(327, 50);
            this.label3.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(63, 25);
            this.label3.TabIndex = 23;
            this.label3.Text = "Beat:";
            // 
            // lblBeat
            // 
            this.lblBeat.AutoSize = true;
            this.lblBeat.BackColor = System.Drawing.Color.Transparent;
            this.lblBeat.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblBeat.ForeColor = System.Drawing.Color.White;
            this.lblBeat.Location = new System.Drawing.Point(398, 50);
            this.lblBeat.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblBeat.Name = "lblBeat";
            this.lblBeat.Size = new System.Drawing.Size(23, 25);
            this.lblBeat.TabIndex = 22;
            this.lblBeat.Text = "0";
            // 
            // btnGenerate
            // 
            this.btnGenerate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnGenerate.Location = new System.Drawing.Point(810, 114);
            this.btnGenerate.Margin = new System.Windows.Forms.Padding(4);
            this.btnGenerate.Name = "btnGenerate";
            this.btnGenerate.Size = new System.Drawing.Size(100, 28);
            this.btnGenerate.TabIndex = 24;
            this.btnGenerate.Text = "Generate";
            this.btnGenerate.UseVisualStyleBackColor = true;
            this.btnGenerate.Click += new System.EventHandler(this.btnGenerate_Click);
            // 
            // lblScoreNote1
            // 
            this.lblScoreNote1.AutoSize = true;
            this.lblScoreNote1.BackColor = System.Drawing.Color.Black;
            this.lblScoreNote1.Font = new System.Drawing.Font("Microsoft Sans Serif", 12F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblScoreNote1.ForeColor = System.Drawing.Color.White;
            this.lblScoreNote1.Location = new System.Drawing.Point(23, 162);
            this.lblScoreNote1.Margin = new System.Windows.Forms.Padding(4, 0, 4, 0);
            this.lblScoreNote1.Name = "lblScoreNote1";
            this.lblScoreNote1.Size = new System.Drawing.Size(503, 25);
            this.lblScoreNote1.TabIndex = 25;
            this.lblScoreNote1.Text = "<ScoreNote Beat=\"3\" Tick=\"0\" SyncSongPin=\"0:1:24\"/>";
            // 
            // FormMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackgroundImage = ((System.Drawing.Image)(resources.GetObject("$this.BackgroundImage")));
            this.ClientSize = new System.Drawing.Size(931, 477);
            this.Controls.Add(this.lblScoreNote1);
            this.Controls.Add(this.btnGenerate);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.lblBeat);
            this.Controls.Add(this.txtStartAt);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.btnSyncPoint);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.edtLogTemp);
            this.Controls.Add(this.rdb2);
            this.Controls.Add(this.rdb1);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.lblSongTime);
            this.Controls.Add(this.btnPlayPause);
            this.Controls.Add(this.btnSelectAudioFile);
            this.Controls.Add(this.edtAudioFile);
            this.Controls.Add(this.label2);
            this.Margin = new System.Windows.Forms.Padding(4);
            this.Name = "FormMain";
            this.Text = "Guitarmonics Sync Tool";
            this.PreviewKeyDown += new System.Windows.Forms.PreviewKeyDownEventHandler(this.FormMain_PreviewKeyDown);
            ((System.ComponentModel.ISupportInitialize)(this.txtStartAt)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnSelectAudioFile;
        private System.Windows.Forms.TextBox edtAudioFile;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.OpenFileDialog openFileDialogAudio;
        private System.Windows.Forms.Button btnPlayPause;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label lblSongTime;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.RadioButton rdb1;
        private System.Windows.Forms.RadioButton rdb2;
        private System.Windows.Forms.TextBox edtLogTemp;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnSyncPoint;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown txtStartAt;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label lblBeat;
        private System.Windows.Forms.Button btnGenerate;
        private System.Windows.Forms.Label lblScoreNote1;
    }
}

