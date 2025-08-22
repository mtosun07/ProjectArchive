namespace TSN.Universe.DesktopApp
{
    partial class ExportRangeDialogForm
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
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnExport = new System.Windows.Forms.Button();
            this.groupBox1 = new System.Windows.Forms.GroupBox();
            this.radCurrentModified = new System.Windows.Forms.RadioButton();
            this.pnlRange = new System.Windows.Forms.Panel();
            this.nudRange1 = new System.Windows.Forms.NumericUpDown();
            this.txtFileName = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.nudRange2 = new System.Windows.Forms.NumericUpDown();
            this.radRange = new System.Windows.Forms.RadioButton();
            this.radCurrent = new System.Windows.Forms.RadioButton();
            this.radAll = new System.Windows.Forms.RadioButton();
            this.groupBox1.SuspendLayout();
            this.pnlRange.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudRange1)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRange2)).BeginInit();
            this.SuspendLayout();
            // 
            // btnCancel
            // 
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(12, 167);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(190, 30);
            this.btnCancel.TabIndex = 2;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnExport
            // 
            this.btnExport.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnExport.Location = new System.Drawing.Point(209, 167);
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(190, 30);
            this.btnExport.TabIndex = 1;
            this.btnExport.Text = "Export...";
            this.btnExport.UseVisualStyleBackColor = true;
            this.btnExport.Click += new System.EventHandler(this.btnExport_Click);
            // 
            // groupBox1
            // 
            this.groupBox1.Controls.Add(this.radCurrentModified);
            this.groupBox1.Controls.Add(this.txtFileName);
            this.groupBox1.Controls.Add(this.pnlRange);
            this.groupBox1.Controls.Add(this.radRange);
            this.groupBox1.Controls.Add(this.radCurrent);
            this.groupBox1.Controls.Add(this.radAll);
            this.groupBox1.Location = new System.Drawing.Point(12, 12);
            this.groupBox1.Name = "groupBox1";
            this.groupBox1.Size = new System.Drawing.Size(387, 149);
            this.groupBox1.TabIndex = 0;
            this.groupBox1.TabStop = false;
            this.groupBox1.Text = "Export Range";
            // 
            // radCurrentModified
            // 
            this.radCurrentModified.AutoSize = true;
            this.radCurrentModified.Enabled = false;
            this.radCurrentModified.Location = new System.Drawing.Point(6, 71);
            this.radCurrentModified.Name = "radCurrentModified";
            this.radCurrentModified.Size = new System.Drawing.Size(137, 17);
            this.radCurrentModified.TabIndex = 2;
            this.radCurrentModified.Text = "Current Modified/Visible";
            this.radCurrentModified.UseVisualStyleBackColor = true;
            this.radCurrentModified.CheckedChanged += new System.EventHandler(this.radCurrentModified_CheckedChanged);
            // 
            // pnlRange
            // 
            this.pnlRange.Controls.Add(this.nudRange1);
            this.pnlRange.Controls.Add(this.label1);
            this.pnlRange.Controls.Add(this.nudRange2);
            this.pnlRange.Enabled = false;
            this.pnlRange.Location = new System.Drawing.Point(154, 97);
            this.pnlRange.Name = "pnlRange";
            this.pnlRange.Size = new System.Drawing.Size(227, 20);
            this.pnlRange.TabIndex = 4;
            // 
            // nudRange1
            // 
            this.nudRange1.Location = new System.Drawing.Point(0, 0);
            this.nudRange1.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudRange1.Name = "nudRange1";
            this.nudRange1.Size = new System.Drawing.Size(100, 20);
            this.nudRange1.TabIndex = 0;
            // 
            // txtFileName
            // 
            this.txtFileName.Enabled = false;
            this.txtFileName.Location = new System.Drawing.Point(6, 123);
            this.txtFileName.Name = "txtFileName";
            this.txtFileName.Size = new System.Drawing.Size(375, 20);
            this.txtFileName.TabIndex = 3;
            this.txtFileName.Enter += new System.EventHandler(this.txtFileName_Enter);
            this.txtFileName.Leave += new System.EventHandler(this.txtFileName_Leave);
            // 
            // label1
            // 
            this.label1.Location = new System.Drawing.Point(106, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(15, 20);
            this.label1.TabIndex = 1;
            this.label1.Text = "-";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // nudRange2
            // 
            this.nudRange2.Location = new System.Drawing.Point(127, 0);
            this.nudRange2.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudRange2.Name = "nudRange2";
            this.nudRange2.Size = new System.Drawing.Size(100, 20);
            this.nudRange2.TabIndex = 2;
            // 
            // radRange
            // 
            this.radRange.AutoSize = true;
            this.radRange.Location = new System.Drawing.Point(6, 97);
            this.radRange.Name = "radRange";
            this.radRange.Size = new System.Drawing.Size(142, 17);
            this.radRange.TabIndex = 3;
            this.radRange.Text = "All Generations in Range";
            this.radRange.UseVisualStyleBackColor = true;
            this.radRange.CheckedChanged += new System.EventHandler(this.radRange_CheckedChanged);
            // 
            // radCurrent
            // 
            this.radCurrent.AutoSize = true;
            this.radCurrent.Checked = true;
            this.radCurrent.Location = new System.Drawing.Point(6, 45);
            this.radCurrent.Name = "radCurrent";
            this.radCurrent.Size = new System.Drawing.Size(114, 17);
            this.radCurrent.TabIndex = 1;
            this.radCurrent.TabStop = true;
            this.radCurrent.Text = "Current Generation";
            this.radCurrent.UseVisualStyleBackColor = true;
            this.radCurrent.CheckedChanged += new System.EventHandler(this.radCurrent_CheckedChanged);
            // 
            // radAll
            // 
            this.radAll.AutoSize = true;
            this.radAll.Location = new System.Drawing.Point(6, 19);
            this.radAll.Name = "radAll";
            this.radAll.Size = new System.Drawing.Size(96, 17);
            this.radAll.TabIndex = 0;
            this.radAll.Text = "All Generations";
            this.radAll.UseVisualStyleBackColor = true;
            this.radAll.CheckedChanged += new System.EventHandler(this.radAll_CheckedChanged);
            // 
            // ExportRangeForm
            // 
            this.AcceptButton = this.btnExport;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(421, 216);
            this.ControlBox = false;
            this.Controls.Add(this.groupBox1);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnCancel);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(427, 222);
            this.Name = "ExportRangeForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Select Generations to Export";
            this.TopMost = true;
            this.Load += new System.EventHandler(this.ExportRangeForm_Load);
            this.groupBox1.ResumeLayout(false);
            this.groupBox1.PerformLayout();
            this.pnlRange.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudRange1)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudRange2)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnExport;
        private System.Windows.Forms.GroupBox groupBox1;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.NumericUpDown nudRange2;
        private System.Windows.Forms.NumericUpDown nudRange1;
        private System.Windows.Forms.RadioButton radRange;
        private System.Windows.Forms.RadioButton radCurrent;
        private System.Windows.Forms.RadioButton radAll;
        private System.Windows.Forms.Panel pnlRange;
        private System.Windows.Forms.TextBox txtFileName;
        private System.Windows.Forms.RadioButton radCurrentModified;
    }
}