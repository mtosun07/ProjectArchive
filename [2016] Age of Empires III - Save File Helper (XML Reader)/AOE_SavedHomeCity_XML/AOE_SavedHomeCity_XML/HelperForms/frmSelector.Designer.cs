namespace AOE3_HomeCity.HelperForms
{
    partial class frmSelector<T>
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
            this.pnlListBoxContainer = new System.Windows.Forms.Panel();
            this.lbx = new System.Windows.Forms.ListBox();
            this.llblSelectAll = new System.Windows.Forms.LinkLabel();
            this.pnlListBoxContainer.SuspendLayout();
            this.SuspendLayout();
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(249, 211);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(100, 30);
            this.btnOK.TabIndex = 2;
            this.btnOK.Text = "OK";
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(355, 211);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(100, 30);
            this.btnCancel.TabIndex = 3;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // pnlListBoxContainer
            // 
            this.pnlListBoxContainer.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlListBoxContainer.BackColor = System.Drawing.SystemColors.Window;
            this.pnlListBoxContainer.BorderStyle = System.Windows.Forms.BorderStyle.Fixed3D;
            this.pnlListBoxContainer.Controls.Add(this.lbx);
            this.pnlListBoxContainer.Location = new System.Drawing.Point(12, 12);
            this.pnlListBoxContainer.Name = "pnlListBoxContainer";
            this.pnlListBoxContainer.Size = new System.Drawing.Size(443, 193);
            this.pnlListBoxContainer.TabIndex = 0;
            // 
            // lbx
            // 
            this.lbx.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.lbx.Dock = System.Windows.Forms.DockStyle.Fill;
            this.lbx.FormattingEnabled = true;
            this.lbx.ItemHeight = 16;
            this.lbx.Location = new System.Drawing.Point(0, 0);
            this.lbx.Name = "lbx";
            this.lbx.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbx.Size = new System.Drawing.Size(439, 189);
            this.lbx.TabIndex = 0;
            // 
            // llblSelectAll
            // 
            this.llblSelectAll.AutoSize = true;
            this.llblSelectAll.Location = new System.Drawing.Point(9, 218);
            this.llblSelectAll.Name = "llblSelectAll";
            this.llblSelectAll.Size = new System.Drawing.Size(66, 17);
            this.llblSelectAll.TabIndex = 1;
            this.llblSelectAll.TabStop = true;
            this.llblSelectAll.Text = "Select All";
            this.llblSelectAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblSelectAll_LinkClicked);
            // 
            // frmSelector
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(467, 253);
            this.Controls.Add(this.llblSelectAll);
            this.Controls.Add(this.pnlListBoxContainer);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MinimizeBox = false;
            this.Name = "frmSelector";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.frmSelector_Load);
            this.pnlListBoxContainer.ResumeLayout(false);
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Panel pnlListBoxContainer;
        private System.Windows.Forms.ListBox lbx;
        private System.Windows.Forms.LinkLabel llblSelectAll;
    }
}