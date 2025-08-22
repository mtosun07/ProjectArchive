namespace TSN.Universe.DesktopApp
{
    partial class StatisticsForm
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
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.nudGeneration = new System.Windows.Forms.NumericUpDown();
            this.btnConsole = new System.Windows.Forms.Button();
            this.btnCharts = new System.Windows.Forms.Button();
            this.statisticsUC = new TSN.Universe.DesktopApp.StatisticsUserControl();
            this.btnExport = new TSN.Universe.DesktopApp.DropDownButton();
            this.cmsExport = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiExportAsImage = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExportAsText = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExportAsCsv = new System.Windows.Forms.ToolStripMenuItem();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.groupBox1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudGeneration)).BeginInit();
            this.cmsExport.SuspendLayout();
            this.SuspendLayout();
            // 
            // groupBox1
            // 
            this.groupBox1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.groupBox1.Controls.Add(this.nudGeneration);
            this.groupBox1.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold);
            this.groupBox1.ForeColor = System.Drawing.Color.SteelBlue;
            this.groupBox1.Location = new System.Drawing.Point(1029, 553);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(200, 55);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Representing Generation #";
            // 
            // nudGeneration
            // 
            this.nudGeneration.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.nudGeneration.Location = new System.Drawing.Point(6, 24);
            this.nudGeneration.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudGeneration.Name = "nudGeneration";
            this.nudGeneration.Size = new System.Drawing.Size(188, 25);
            this.nudGeneration.TabIndex = 0;
            this.nudGeneration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudGeneration.ValueChanged += new System.EventHandler(this.nudGeneration_ValueChanged);
            // 
            // btnConsole
            // 
            this.btnConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnConsole.BackColor = System.Drawing.Color.GhostWhite;
            this.btnConsole.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnConsole.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnConsole.Location = new System.Drawing.Point(285, 561);
            this.btnConsole.Name = "btnConsole";
            this.btnConsole.Size = new System.Drawing.Size(200, 38);
            this.btnConsole.TabIndex = 1;
            this.btnConsole.Text = "Progression Output Console...";
            this.btnConsole.UseVisualStyleBackColor = false;
            this.btnConsole.Click += new System.EventHandler(this.btnConsole_Click);
            // 
            // btnCharts
            // 
            this.btnCharts.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnCharts.BackColor = System.Drawing.Color.GhostWhite;
            this.btnCharts.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCharts.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnCharts.Location = new System.Drawing.Point(12, 561);
            this.btnCharts.Name = "btnCharts";
            this.btnCharts.Size = new System.Drawing.Size(267, 38);
            this.btnCharts.TabIndex = 3;
            this.btnCharts.Text = "Charts...";
            this.btnCharts.UseVisualStyleBackColor = false;
            this.btnCharts.Click += new System.EventHandler(this.btnCharts_Click);
            // 
            // statisticsUC
            // 
            this.statisticsUC.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.statisticsUC.BackColor = System.Drawing.Color.Transparent;
            this.statisticsUC.Location = new System.Drawing.Point(12, 12);
            this.statisticsUC.Name = "statisticsUC";
            this.statisticsUC.Size = new System.Drawing.Size(1217, 535);
            this.statisticsUC.TabIndex = 4;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.BackColor = System.Drawing.Color.GhostWhite;
            this.btnExport.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnExport.ForeColor = System.Drawing.Color.SteelBlue;
            this.btnExport.Location = new System.Drawing.Point(491, 561);
            this.btnExport.Menu = this.cmsExport;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 38);
            this.btnExport.TabIndex = 5;
            this.btnExport.Text = "Export...";
            this.btnExport.UseVisualStyleBackColor = false;
            // 
            // cmsExport
            // 
            this.cmsExport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExportAsImage,
            this.tsmiExportAsText,
            this.tsmiExportAsCsv});
            this.cmsExport.Name = "cmsExport";
            this.cmsExport.Size = new System.Drawing.Size(185, 70);
            // 
            // tsmiExportAsImage
            // 
            this.tsmiExportAsImage.Name = "tsmiExportAsImage";
            this.tsmiExportAsImage.Size = new System.Drawing.Size(184, 22);
            this.tsmiExportAsImage.Text = "Export as Image...";
            this.tsmiExportAsImage.Click += new System.EventHandler(this.tsmiExportAsImage_Click);
            // 
            // tsmiExportAsText
            // 
            this.tsmiExportAsText.Name = "tsmiExportAsText";
            this.tsmiExportAsText.Size = new System.Drawing.Size(184, 22);
            this.tsmiExportAsText.Text = "Export as Plain Text...";
            this.tsmiExportAsText.Click += new System.EventHandler(this.tsmiExportAsText_Click);
            // 
            // tsmiExportAsCsv
            // 
            this.tsmiExportAsCsv.Name = "tsmiExportAsCsv";
            this.tsmiExportAsCsv.Size = new System.Drawing.Size(184, 22);
            this.tsmiExportAsCsv.Text = "Export As .csv";
            this.tsmiExportAsCsv.Click += new System.EventHandler(this.tsmiExportAsCsv_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.Title = "Specify the target file to which the universe statistics would be saved";
            // 
            // StatisticsForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.GhostWhite;
            this.ClientSize = new System.Drawing.Size(1241, 620);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.statisticsUC);
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnConsole);
            this.Controls.Add(this.btnCharts);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(1257, 1109);
            this.MinimumSize = new System.Drawing.Size(1257, 659);
            this.Name = "StatisticsForm";
            this.ShowIcon = false;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Simulation Statistics";
            this.FormClosed += new System.Windows.Forms.FormClosedEventHandler(this.StatisticsForm_FormClosed);
            this.groupBox1.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudGeneration)).EndInit();
            this.cmsExport.ResumeLayout(false);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.NumericUpDown nudGeneration;
        private System.Windows.Forms.Button btnConsole;
        private System.Windows.Forms.Button btnCharts;
        private StatisticsUserControl statisticsUC;
        private DropDownButton btnExport;
        private System.Windows.Forms.ContextMenuStrip cmsExport;
        private System.Windows.Forms.ToolStripMenuItem tsmiExportAsImage;
        private System.Windows.Forms.ToolStripMenuItem tsmiExportAsText;
        private System.Windows.Forms.ToolStripMenuItem tsmiExportAsCsv;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
    }
}