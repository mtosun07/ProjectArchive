namespace TSN.Universe.DesktopApp
{
    partial class ChartForm
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
            this.nudGeneration = new System.Windows.Forms.NumericUpDown();
            this.label16 = new System.Windows.Forms.Label();
            this.chart = new System.Windows.Forms.DataVisualization.Charting.Chart();
            this.cmsChart = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiExport2 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExport4 = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExport8 = new System.Windows.Forms.ToolStripMenuItem();
            this.cmbChartType = new System.Windows.Forms.ComboBox();
            this.label1 = new System.Windows.Forms.Label();
            this.chkPrevious = new System.Windows.Forms.CheckBox();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.label2 = new System.Windows.Forms.Label();
            this.cmbChartPalette = new System.Windows.Forms.ComboBox();
            this.btnAddChart = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudGeneration)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).BeginInit();
            this.cmsChart.SuspendLayout();
            this.SuspendLayout();
            // 
            // nudGeneration
            // 
            this.nudGeneration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.nudGeneration.Location = new System.Drawing.Point(12, 438);
            this.nudGeneration.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudGeneration.Name = "nudGeneration";
            this.nudGeneration.Size = new System.Drawing.Size(146, 20);
            this.nudGeneration.TabIndex = 2;
            this.nudGeneration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label16.Location = new System.Drawing.Point(12, 415);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(146, 20);
            this.label16.TabIndex = 1;
            this.label16.Text = "Representing Generation";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chart
            // 
            this.chart.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.chart.ContextMenuStrip = this.cmsChart;
            this.chart.Location = new System.Drawing.Point(12, 12);
            this.chart.Name = "chart";
            this.chart.Size = new System.Drawing.Size(800, 400);
            this.chart.TabIndex = 0;
            this.chart.DoubleClick += new System.EventHandler(this.chart_DoubleClick);
            this.chart.MouseMove += new System.Windows.Forms.MouseEventHandler(this.chart_MouseMove);
            // 
            // cmsChart
            // 
            this.cmsChart.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExport2,
            this.tsmiExport4,
            this.tsmiExport8});
            this.cmsChart.Name = "cmsChart";
            this.cmsChart.Size = new System.Drawing.Size(192, 70);
            // 
            // tsmiExport2
            // 
            this.tsmiExport2.Name = "tsmiExport2";
            this.tsmiExport2.Size = new System.Drawing.Size(191, 22);
            this.tsmiExport2.Text = "Export as Image (2K)...";
            this.tsmiExport2.Click += new System.EventHandler(this.tsmiExport2_Click);
            // 
            // tsmiExport4
            // 
            this.tsmiExport4.Name = "tsmiExport4";
            this.tsmiExport4.Size = new System.Drawing.Size(191, 22);
            this.tsmiExport4.Text = "Export as Image (4K)...";
            this.tsmiExport4.Click += new System.EventHandler(this.tsmiExport4_Click);
            // 
            // tsmiExport8
            // 
            this.tsmiExport8.Name = "tsmiExport8";
            this.tsmiExport8.Size = new System.Drawing.Size(191, 22);
            this.tsmiExport8.Text = "Export as Image (8K)...";
            this.tsmiExport8.Click += new System.EventHandler(this.tsmiExport8_Click);
            // 
            // cmbChartType
            // 
            this.cmbChartType.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChartType.FormattingEnabled = true;
            this.cmbChartType.Location = new System.Drawing.Point(612, 438);
            this.cmbChartType.Name = "cmbChartType";
            this.cmbChartType.Size = new System.Drawing.Size(200, 21);
            this.cmbChartType.TabIndex = 5;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(612, 415);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 20);
            this.label1.TabIndex = 4;
            this.label1.Text = "Charts Type";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // chkPrevious
            // 
            this.chkPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.chkPrevious.AutoSize = true;
            this.chkPrevious.Location = new System.Drawing.Point(164, 439);
            this.chkPrevious.Name = "chkPrevious";
            this.chkPrevious.Size = new System.Drawing.Size(89, 17);
            this.chkPrevious.TabIndex = 3;
            this.chkPrevious.Text = "And Previous";
            this.chkPrevious.UseVisualStyleBackColor = true;
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.DefaultExt = "png";
            this.saveFileDialog.Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp|Other Image Formats|*.*";
            this.saveFileDialog.Title = "Specify the target file to which the statistics chart image would be saved";
            // 
            // label2
            // 
            this.label2.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(406, 415);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 20);
            this.label2.TabIndex = 6;
            this.label2.Text = "Charts Palette";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // cmbChartPalette
            // 
            this.cmbChartPalette.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.cmbChartPalette.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChartPalette.FormattingEnabled = true;
            this.cmbChartPalette.Location = new System.Drawing.Point(406, 438);
            this.cmbChartPalette.Name = "cmbChartPalette";
            this.cmbChartPalette.Size = new System.Drawing.Size(200, 21);
            this.cmbChartPalette.TabIndex = 7;
            // 
            // btnAddChart
            // 
            this.btnAddChart.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnAddChart.Location = new System.Drawing.Point(300, 437);
            this.btnAddChart.Name = "btnAddChart";
            this.btnAddChart.Size = new System.Drawing.Size(100, 23);
            this.btnAddChart.TabIndex = 8;
            this.btnAddChart.Text = "Add Chart...";
            this.btnAddChart.UseVisualStyleBackColor = true;
            this.btnAddChart.Click += new System.EventHandler(this.btnAddChart_Click);
            // 
            // ChartForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(824, 471);
            this.Controls.Add(this.btnAddChart);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.cmbChartPalette);
            this.Controls.Add(this.chkPrevious);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.cmbChartType);
            this.Controls.Add(this.chart);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.nudGeneration);
            this.MinimumSize = new System.Drawing.Size(840, 510);
            this.Name = "ChartForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Statistics Charts";
            ((System.ComponentModel.ISupportInitialize)(this.nudGeneration)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.chart)).EndInit();
            this.cmsChart.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.NumericUpDown nudGeneration;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.DataVisualization.Charting.Chart chart;
        private System.Windows.Forms.ComboBox cmbChartType;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.CheckBox chkPrevious;
        private System.Windows.Forms.ContextMenuStrip cmsChart;
        private System.Windows.Forms.ToolStripMenuItem tsmiExport2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.ToolStripMenuItem tsmiExport4;
        private System.Windows.Forms.ToolStripMenuItem tsmiExport8;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.ComboBox cmbChartPalette;
        private System.Windows.Forms.Button btnAddChart;
    }
}