namespace AOE3_HomeCity.HelperForms
{
    partial class frmAddEdit_Prop
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
            this.txtName = new System.Windows.Forms.TextBox();
            this.label3 = new System.Windows.Forms.Label();
            this.label2 = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.panel1 = new System.Windows.Forms.Panel();
            this.radEnabled_false = new System.Windows.Forms.RadioButton();
            this.radEnabled_true = new System.Windows.Forms.RadioButton();
            this.radEnabled_empty = new System.Windows.Forms.RadioButton();
            this.panel1.SuspendLayout();
            this.SuspendLayout();
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(138, 39);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(302, 22);
            this.txtName.TabIndex = 3;
            // 
            // label3
            // 
            this.label3.Location = new System.Drawing.Point(12, 39);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(120, 22);
            this.label3.TabIndex = 2;
            this.label3.Text = "Name";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label2
            // 
            this.label2.Location = new System.Drawing.Point(12, 12);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(120, 22);
            this.label2.TabIndex = 0;
            this.label2.Text = "Enabled";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(365, 71);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 5;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(284, 71);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 4;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // panel1
            // 
            this.panel1.BackColor = System.Drawing.Color.Transparent;
            this.panel1.Controls.Add(this.radEnabled_false);
            this.panel1.Controls.Add(this.radEnabled_true);
            this.panel1.Controls.Add(this.radEnabled_empty);
            this.panel1.Location = new System.Drawing.Point(138, 12);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(302, 22);
            this.panel1.TabIndex = 1;
            // 
            // radEnabled_false
            // 
            this.radEnabled_false.AutoSize = true;
            this.radEnabled_false.Location = new System.Drawing.Point(0, 0);
            this.radEnabled_false.Name = "radEnabled_false";
            this.radEnabled_false.Size = new System.Drawing.Size(85, 21);
            this.radEnabled_false.TabIndex = 0;
            this.radEnabled_false.Text = "False (0)";
            this.radEnabled_false.UseVisualStyleBackColor = true;
            // 
            // radEnabled_true
            // 
            this.radEnabled_true.AutoSize = true;
            this.radEnabled_true.Location = new System.Drawing.Point(91, 0);
            this.radEnabled_true.Name = "radEnabled_true";
            this.radEnabled_true.Size = new System.Drawing.Size(81, 21);
            this.radEnabled_true.TabIndex = 1;
            this.radEnabled_true.Text = "True (1)";
            this.radEnabled_true.UseVisualStyleBackColor = true;
            // 
            // radEnabled_empty
            // 
            this.radEnabled_empty.AutoSize = true;
            this.radEnabled_empty.Location = new System.Drawing.Point(178, 0);
            this.radEnabled_empty.Name = "radEnabled_empty";
            this.radEnabled_empty.Size = new System.Drawing.Size(68, 21);
            this.radEnabled_empty.TabIndex = 2;
            this.radEnabled_empty.Text = "Empty";
            this.radEnabled_empty.UseVisualStyleBackColor = true;
            // 
            // frmAddEdit_Prop
            // 
            this.AcceptButton = this.btnOK;
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.CancelButton = this.btnCancel;
            this.ClientSize = new System.Drawing.Size(452, 113);
            this.Controls.Add(this.panel1);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.label3);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(470, 160);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(470, 160);
            this.Name = "frmAddEdit_Prop";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Prop";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAddEdit_Prop_FormClosing);
            this.Load += new System.EventHandler(this.frmAddEdit_Prop_Load);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtName;
        private System.Windows.Forms.Label label3;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.Button btnOK;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.RadioButton radEnabled_false;
        private System.Windows.Forms.RadioButton radEnabled_true;
        private System.Windows.Forms.RadioButton radEnabled_empty;
    }
}