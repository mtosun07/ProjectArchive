namespace TSN.FileToVideo
{
    partial class MainForm
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
            this.txtSourcePath = new System.Windows.Forms.TextBox();
            this.btnBrowse = new System.Windows.Forms.Button();
            this.btnConvert = new System.Windows.Forms.Button();
            this.ofdFile = new System.Windows.Forms.OpenFileDialog();
            this.fbdTargetFolder = new System.Windows.Forms.FolderBrowserDialog();
            this.prgConvertImages = new System.Windows.Forms.ProgressBar();
            this.lblProgress = new System.Windows.Forms.Label();
            this.bgwConvert = new System.ComponentModel.BackgroundWorker();
            this.lblElapsed = new System.Windows.Forms.Label();
            this.bgwElapsed = new System.ComponentModel.BackgroundWorker();
            this.label1 = new System.Windows.Forms.Label();
            this.nudPixelWidth = new System.Windows.Forms.NumericUpDown();
            this.nudPixelHeight = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.nudResolutionWidth = new System.Windows.Forms.NumericUpDown();
            this.label3 = new System.Windows.Forms.Label();
            this.nudResolutionHeight = new System.Windows.Forms.NumericUpDown();
            this.label4 = new System.Windows.Forms.Label();
            this.nudFramesPerSecond = new System.Windows.Forms.NumericUpDown();
            this.label5 = new System.Windows.Forms.Label();
            this.tmrElapsed = new System.Windows.Forms.Timer(this.components);
            this.prgConvertChecking = new System.Windows.Forms.ProgressBar();
            this.prgConvertAudios = new System.Windows.Forms.ProgressBar();
            this.nudSamplingFrequency = new System.Windows.Forms.NumericUpDown();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.nudChannels = new System.Windows.Forms.NumericUpDown();
            this.label8 = new System.Windows.Forms.Label();
            this.cmbBitsPerSample = new System.Windows.Forms.ComboBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudPixelWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPixelHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudResolutionWidth)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudResolutionHeight)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFramesPerSecond)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSamplingFrequency)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChannels)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSourcePath
            // 
            this.txtSourcePath.Location = new System.Drawing.Point(12, 18);
            this.txtSourcePath.Name = "txtSourcePath";
            this.txtSourcePath.ReadOnly = true;
            this.txtSourcePath.Size = new System.Drawing.Size(500, 20);
            this.txtSourcePath.TabIndex = 0;
            this.txtSourcePath.TabStop = false;
            // 
            // btnBrowse
            // 
            this.btnBrowse.Location = new System.Drawing.Point(518, 12);
            this.btnBrowse.Name = "btnBrowse";
            this.btnBrowse.Size = new System.Drawing.Size(100, 30);
            this.btnBrowse.TabIndex = 1;
            this.btnBrowse.Text = "Browse File...";
            this.btnBrowse.UseVisualStyleBackColor = true;
            this.btnBrowse.Click += new System.EventHandler(this.btnBrowse_Click);
            // 
            // btnConvert
            // 
            this.btnConvert.Location = new System.Drawing.Point(12, 183);
            this.btnConvert.Name = "btnConvert";
            this.btnConvert.Size = new System.Drawing.Size(276, 30);
            this.btnConvert.TabIndex = 18;
            this.btnConvert.UseVisualStyleBackColor = true;
            this.btnConvert.Click += new System.EventHandler(this.btnConvert_Click);
            // 
            // ofdFile
            // 
            this.ofdFile.Filter = "All Files|*.*";
            this.ofdFile.Title = "Select a File to Convert";
            // 
            // fbdTargetFolder
            // 
            this.fbdTargetFolder.Description = "Select Output Directory";
            // 
            // prgConvertImages
            // 
            this.prgConvertImages.Location = new System.Drawing.Point(12, 249);
            this.prgConvertImages.Maximum = 10000;
            this.prgConvertImages.Name = "prgConvertImages";
            this.prgConvertImages.Size = new System.Drawing.Size(606, 30);
            this.prgConvertImages.TabIndex = 21;
            // 
            // lblProgress
            // 
            this.lblProgress.Font = new System.Drawing.Font("Verdana", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblProgress.Location = new System.Drawing.Point(12, 216);
            this.lblProgress.Name = "lblProgress";
            this.lblProgress.Size = new System.Drawing.Size(400, 30);
            this.lblProgress.TabIndex = 19;
            this.lblProgress.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bgwConvert
            // 
            this.bgwConvert.WorkerReportsProgress = true;
            this.bgwConvert.WorkerSupportsCancellation = true;
            this.bgwConvert.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwConvert_DoWork);
            this.bgwConvert.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwConvert_ProgressChanged);
            this.bgwConvert.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwConvert_RunWorkerCompleted);
            // 
            // lblElapsed
            // 
            this.lblElapsed.Font = new System.Drawing.Font("Verdana", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblElapsed.Location = new System.Drawing.Point(418, 216);
            this.lblElapsed.Name = "lblElapsed";
            this.lblElapsed.Size = new System.Drawing.Size(200, 30);
            this.lblElapsed.TabIndex = 20;
            this.lblElapsed.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // bgwElapsed
            // 
            this.bgwElapsed.WorkerReportsProgress = true;
            this.bgwElapsed.WorkerSupportsCancellation = true;
            this.bgwElapsed.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwElapsed_DoWork);
            this.bgwElapsed.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwElapsed_ProgressChanged);
            this.bgwElapsed.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwElapsed_RunWorkerCompleted);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(12, 79);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(150, 20);
            this.label1.TabIndex = 2;
            this.label1.Text = "Pixel Width (px)";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudPixelWidth
            // 
            this.nudPixelWidth.Location = new System.Drawing.Point(168, 79);
            this.nudPixelWidth.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudPixelWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPixelWidth.Name = "nudPixelWidth";
            this.nudPixelWidth.Size = new System.Drawing.Size(120, 20);
            this.nudPixelWidth.TabIndex = 3;
            this.nudPixelWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPixelWidth.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // nudPixelHeight
            // 
            this.nudPixelHeight.Location = new System.Drawing.Point(498, 79);
            this.nudPixelHeight.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudPixelHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudPixelHeight.Name = "nudPixelHeight";
            this.nudPixelHeight.Size = new System.Drawing.Size(120, 20);
            this.nudPixelHeight.TabIndex = 5;
            this.nudPixelHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudPixelHeight.Value = new decimal(new int[] {
            120,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(342, 79);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 20);
            this.label2.TabIndex = 4;
            this.label2.Text = "Pixel Height (px)";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudResolutionWidth
            // 
            this.nudResolutionWidth.Location = new System.Drawing.Point(168, 105);
            this.nudResolutionWidth.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudResolutionWidth.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudResolutionWidth.Name = "nudResolutionWidth";
            this.nudResolutionWidth.Size = new System.Drawing.Size(120, 20);
            this.nudResolutionWidth.TabIndex = 7;
            this.nudResolutionWidth.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudResolutionWidth.Value = new decimal(new int[] {
            1920,
            0,
            0,
            0});
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 105);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 20);
            this.label3.TabIndex = 6;
            this.label3.Text = "Resolution Width (px)";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudResolutionHeight
            // 
            this.nudResolutionHeight.Location = new System.Drawing.Point(498, 105);
            this.nudResolutionHeight.Maximum = new decimal(new int[] {
            100000,
            0,
            0,
            0});
            this.nudResolutionHeight.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudResolutionHeight.Name = "nudResolutionHeight";
            this.nudResolutionHeight.Size = new System.Drawing.Size(120, 20);
            this.nudResolutionHeight.TabIndex = 9;
            this.nudResolutionHeight.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudResolutionHeight.Value = new decimal(new int[] {
            1080,
            0,
            0,
            0});
            // 
            // label4
            // 
            this.label4.Location = new System.Drawing.Point(342, 105);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 20);
            this.label4.TabIndex = 8;
            this.label4.Text = "Resolution Height (px)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudFramesPerSecond
            // 
            this.nudFramesPerSecond.Location = new System.Drawing.Point(498, 157);
            this.nudFramesPerSecond.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudFramesPerSecond.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudFramesPerSecond.Name = "nudFramesPerSecond";
            this.nudFramesPerSecond.Size = new System.Drawing.Size(120, 20);
            this.nudFramesPerSecond.TabIndex = 17;
            this.nudFramesPerSecond.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudFramesPerSecond.Value = new decimal(new int[] {
            25,
            0,
            0,
            0});
            // 
            // label5
            // 
            this.label5.Location = new System.Drawing.Point(342, 157);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 20);
            this.label5.TabIndex = 16;
            this.label5.Text = "Frames per Second";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // tmrElapsed
            // 
            this.tmrElapsed.Tick += new System.EventHandler(this.tmrElapsed_Tick);
            // 
            // prgConvertChecking
            // 
            this.prgConvertChecking.Location = new System.Drawing.Point(12, 285);
            this.prgConvertChecking.Maximum = 10000;
            this.prgConvertChecking.Name = "prgConvertChecking";
            this.prgConvertChecking.Size = new System.Drawing.Size(606, 30);
            this.prgConvertChecking.TabIndex = 22;
            // 
            // prgConvertAudios
            // 
            this.prgConvertAudios.Location = new System.Drawing.Point(12, 321);
            this.prgConvertAudios.Maximum = 10000;
            this.prgConvertAudios.Name = "prgConvertAudios";
            this.prgConvertAudios.Size = new System.Drawing.Size(606, 30);
            this.prgConvertAudios.TabIndex = 23;
            // 
            // nudSamplingFrequency
            // 
            this.nudSamplingFrequency.Location = new System.Drawing.Point(498, 131);
            this.nudSamplingFrequency.Maximum = new decimal(new int[] {
            100000000,
            0,
            0,
            0});
            this.nudSamplingFrequency.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSamplingFrequency.Name = "nudSamplingFrequency";
            this.nudSamplingFrequency.Size = new System.Drawing.Size(120, 20);
            this.nudSamplingFrequency.TabIndex = 13;
            this.nudSamplingFrequency.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSamplingFrequency.Value = new decimal(new int[] {
            96000,
            0,
            0,
            0});
            // 
            // label6
            // 
            this.label6.Location = new System.Drawing.Point(342, 131);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 20);
            this.label6.TabIndex = 12;
            this.label6.Text = "Audio Sampling Freq. (Hz)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Location = new System.Drawing.Point(12, 131);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(150, 20);
            this.label7.TabIndex = 10;
            this.label7.Text = "Audio Bits per Sample";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudChannels
            // 
            this.nudChannels.Location = new System.Drawing.Point(168, 157);
            this.nudChannels.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudChannels.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudChannels.Name = "nudChannels";
            this.nudChannels.Size = new System.Drawing.Size(120, 20);
            this.nudChannels.TabIndex = 15;
            this.nudChannels.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudChannels.Value = new decimal(new int[] {
            2,
            0,
            0,
            0});
            // 
            // label8
            // 
            this.label8.Location = new System.Drawing.Point(12, 157);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(150, 20);
            this.label8.TabIndex = 14;
            this.label8.Text = "Audio Channels";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // cmbBitsPerSample
            // 
            this.cmbBitsPerSample.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbBitsPerSample.FormattingEnabled = true;
            this.cmbBitsPerSample.Items.AddRange(new object[] {
            "8",
            "16",
            "32"});
            this.cmbBitsPerSample.Location = new System.Drawing.Point(167, 132);
            this.cmbBitsPerSample.Name = "cmbBitsPerSample";
            this.cmbBitsPerSample.Size = new System.Drawing.Size(121, 21);
            this.cmbBitsPerSample.TabIndex = 11;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(630, 363);
            this.Controls.Add(this.cmbBitsPerSample);
            this.Controls.Add(this.nudChannels);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.nudSamplingFrequency);
            this.Controls.Add(this.label6);
            this.Controls.Add(this.label7);
            this.Controls.Add(this.prgConvertAudios);
            this.Controls.Add(this.prgConvertChecking);
            this.Controls.Add(this.nudFramesPerSecond);
            this.Controls.Add(this.label5);
            this.Controls.Add(this.nudResolutionHeight);
            this.Controls.Add(this.label4);
            this.Controls.Add(this.nudResolutionWidth);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.nudPixelHeight);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.nudPixelWidth);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.lblElapsed);
            this.Controls.Add(this.lblProgress);
            this.Controls.Add(this.prgConvertImages);
            this.Controls.Add(this.btnConvert);
            this.Controls.Add(this.btnBrowse);
            this.Controls.Add(this.txtSourcePath);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(646, 402);
            this.MinimumSize = new System.Drawing.Size(646, 402);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TSN - File to Video Converter";
            ((System.ComponentModel.ISupportInitialize)(this.nudPixelWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudPixelHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudResolutionWidth)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudResolutionHeight)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudFramesPerSecond)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSamplingFrequency)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudChannels)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtSourcePath;
        private System.Windows.Forms.Button btnBrowse;
        private System.Windows.Forms.Button btnConvert;
        private System.Windows.Forms.OpenFileDialog ofdFile;
        private System.Windows.Forms.FolderBrowserDialog fbdTargetFolder;
        private System.Windows.Forms.ProgressBar prgConvertImages;
        private System.Windows.Forms.Label lblProgress;
        private System.ComponentModel.BackgroundWorker bgwConvert;
        private System.Windows.Forms.Label lblElapsed;
        private System.ComponentModel.BackgroundWorker bgwElapsed;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudPixelWidth;
        private System.Windows.Forms.NumericUpDown nudPixelHeight;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.NumericUpDown nudResolutionWidth;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.NumericUpDown nudResolutionHeight;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.NumericUpDown nudFramesPerSecond;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Timer tmrElapsed;
        private System.Windows.Forms.ProgressBar prgConvertChecking;
        private System.Windows.Forms.ProgressBar prgConvertAudios;
        private System.Windows.Forms.NumericUpDown nudSamplingFrequency;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.NumericUpDown nudChannels;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.ComboBox cmbBitsPerSample;
    }
}