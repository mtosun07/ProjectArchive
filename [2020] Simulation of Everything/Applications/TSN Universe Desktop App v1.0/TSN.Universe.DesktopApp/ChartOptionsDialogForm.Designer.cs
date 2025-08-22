namespace TSN.Universe.DesktopApp
{
    partial class ChartOptionsDialogForm
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
            this.btnAll = new System.Windows.Forms.Button();
            this.btnClear = new System.Windows.Forms.Button();
            this.btnShow = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.clbTitles = new System.Windows.Forms.CheckedListBox();
            this.cmbChartType = new System.Windows.Forms.ComboBox();
            this.btnExport = new System.Windows.Forms.Button();
            this.btnImport = new System.Windows.Forms.Button();
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.cmbMagnitudes = new System.Windows.Forms.ComboBox();
            this.SuspendLayout();
            // 
            // btnAll
            // 
            this.btnAll.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnAll.Location = new System.Drawing.Point(12, 313);
            this.btnAll.Name = "btnAll";
            this.btnAll.Size = new System.Drawing.Size(113, 30);
            this.btnAll.TabIndex = 5;
            this.btnAll.Text = "Select All";
            this.btnAll.UseVisualStyleBackColor = true;
            this.btnAll.Click += new System.EventHandler(this.btnAll_Click);
            // 
            // btnClear
            // 
            this.btnClear.Location = new System.Drawing.Point(131, 313);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(113, 30);
            this.btnClear.TabIndex = 6;
            this.btnClear.Text = "Clear Selection";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // btnShow
            // 
            this.btnShow.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnShow.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnShow.Location = new System.Drawing.Point(406, 313);
            this.btnShow.Name = "btnShow";
            this.btnShow.Size = new System.Drawing.Size(206, 30);
            this.btnShow.TabIndex = 3;
            this.btnShow.Text = "Show Chart";
            this.btnShow.UseVisualStyleBackColor = true;
            this.btnShow.Click += new System.EventHandler(this.btnShow_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(250, 313);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(150, 30);
            this.btnCancel.TabIndex = 4;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // clbTitles
            // 
            this.clbTitles.CheckOnClick = true;
            this.clbTitles.FormattingEnabled = true;
            this.clbTitles.Location = new System.Drawing.Point(12, 48);
            this.clbTitles.Name = "clbTitles";
            this.clbTitles.ScrollAlwaysVisible = true;
            this.clbTitles.Size = new System.Drawing.Size(600, 259);
            this.clbTitles.TabIndex = 2;
            // 
            // cmbChartType
            // 
            this.cmbChartType.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbChartType.FormattingEnabled = true;
            this.cmbChartType.Location = new System.Drawing.Point(12, 18);
            this.cmbChartType.Name = "cmbChartType";
            this.cmbChartType.Size = new System.Drawing.Size(150, 21);
            this.cmbChartType.TabIndex = 0;
            // 
            // btnExport
            // 
            this.btnExport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnExport.Location = new System.Drawing.Point(512, 12);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(100, 30);
            this.btnExport.TabIndex = 8;
            this.btnExport.Text = "Export...";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // btnImport
            // 
            this.btnImport.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnImport.Location = new System.Drawing.Point(406, 12);
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(100, 30);
            this.btnImport.TabIndex = 7;
            this.btnImport.Text = "Import...";
            this.btnImport.UseVisualStyleBackColor = true;
            this.btnImport.Click += new System.EventHandler(this.btnImport_Click);
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.FileName = "Specify the target file to which the chart options would be saved";
            // 
            // openFileDialog
            // 
            this.openFileDialog.Title = "Select the chart options file that would be loaded";
            // 
            // cmbMagnitudes
            // 
            this.cmbMagnitudes.DropDownStyle = System.Windows.Forms.ComboBoxStyle.DropDownList;
            this.cmbMagnitudes.FormattingEnabled = true;
            this.cmbMagnitudes.Location = new System.Drawing.Point(168, 18);
            this.cmbMagnitudes.Name = "cmbMagnitudes";
            this.cmbMagnitudes.Size = new System.Drawing.Size(150, 21);
            this.cmbMagnitudes.TabIndex = 1;
            // 
            // ChartOptionsDialogForm
            // 
            this.AcceptButton = this.btnShow;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(624, 355);
            this.ControlBox = false;
            this.Controls.Add(this.cmbMagnitudes);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.cmbChartType);
            this.Controls.Add(this.clbTitles);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnShow);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.btnAll);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedSingle;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(640, 394);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(640, 394);
            this.Name = "ChartOptionsDialogForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Statistics to Charts";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ChartsSelectionDialogForm_FormClosing);
            this.Load += new System.EventHandler(this.ChartsSelectionForm_Load);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Button btnAll;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.Button btnShow;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckedListBox clbTitles;
        private System.Windows.Forms.ComboBox cmbChartType;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.Button btnImport;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ComboBox cmbMagnitudes;
    }
}