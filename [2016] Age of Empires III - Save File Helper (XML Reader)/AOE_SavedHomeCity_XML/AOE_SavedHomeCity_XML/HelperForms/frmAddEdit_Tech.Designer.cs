namespace AOE3_HomeCity.HelperForms
{
    partial class frmAddEdit_Tech
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
            this.btnOK = new System.Windows.Forms.Button();
            this.btnCancel = new System.Windows.Forms.Button();
            this.chcDatabaseID = new System.Windows.Forms.CheckBox();
            this.nudDatabaseID = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudDatabaseID)).BeginInit();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(284, 71);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 5;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(365, 71);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 6;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // chcDatabaseID
            // 
            this.chcDatabaseID.Location = new System.Drawing.Point(315, 13);
            this.chcDatabaseID.Name = "chcDatabaseID";
            this.chcDatabaseID.Size = new System.Drawing.Size(125, 22);
            this.chcDatabaseID.TabIndex = 2;
            this.chcDatabaseID.Text = "Leave Empty";
            this.chcDatabaseID.UseVisualStyleBackColor = true;
            this.chcDatabaseID.CheckedChanged += new System.EventHandler(this.chcEmpty_CheckedChanged);
            // 
            // nudDatabaseID
            // 
            this.nudDatabaseID.Location = new System.Drawing.Point(138, 13);
            this.nudDatabaseID.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudDatabaseID.Name = "nudDatabaseID";
            this.nudDatabaseID.Size = new System.Drawing.Size(171, 22);
            this.nudDatabaseID.TabIndex = 1;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 22);
            this.label2.TabIndex = 0;
            this.label2.Text = "Database ID";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 22);
            this.label3.TabIndex = 3;
            this.label3.Text = "Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(138, 39);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(302, 22);
            this.txtName.TabIndex = 4;
            // 
            // frmAddEdit_Tech
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(452, 113);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.chcDatabaseID);
            this.Controls.Add(this.nudDatabaseID);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(470, 160);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(470, 160);
            this.Name = "frmAddEdit_Tech";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Tech";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAddEdit_Tech_FormClosing);
            this.Load += new System.EventHandler(this.frmAddEdit_Tech_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudDatabaseID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chcDatabaseID;
        private System.Windows.Forms.NumericUpDown nudDatabaseID;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.TextBox txtName;
    }
}