namespace TSN.Universe.DesktopApp
{
    partial class OverwriteDialogForm
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
            this.btnYes = new System.Windows.Forms.Button();
            this.btnNo = new System.Windows.Forms.Button();
            this.btnYesAll = new System.Windows.Forms.Button();
            this.btnNoAll = new System.Windows.Forms.Button();
            this.lblCaptionText = new System.Windows.Forms.Label();
            this.lblDialogText = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // btnYes
            // 
            this.btnYes.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnYes.Location = new System.Drawing.Point(12, 72);
            this.btnYes.Name = "btnYes";
            this.btnYes.Size = new System.Drawing.Size(100, 30);
            this.btnYes.TabIndex = 2;
            this.btnYes.Text = "Replace This";
            this.btnYes.UseVisualStyleBackColor = true;
            this.btnYes.Click += new System.EventHandler(this.btnYes_Click);
            // 
            // btnNo
            // 
            this.btnNo.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNo.Location = new System.Drawing.Point(118, 72);
            this.btnNo.Name = "btnNo";
            this.btnNo.Size = new System.Drawing.Size(100, 30);
            this.btnNo.TabIndex = 3;
            this.btnNo.Text = "Skip This";
            this.btnNo.UseVisualStyleBackColor = true;
            this.btnNo.Click += new System.EventHandler(this.btnNo_Click);
            // 
            // btnYesAll
            // 
            this.btnYesAll.DialogResult = System.Windows.Forms.DialogResult.Yes;
            this.btnYesAll.Location = new System.Drawing.Point(224, 72);
            this.btnYesAll.Name = "btnYesAll";
            this.btnYesAll.Size = new System.Drawing.Size(100, 30);
            this.btnYesAll.TabIndex = 4;
            this.btnYesAll.Text = "Replace All";
            this.btnYesAll.UseVisualStyleBackColor = true;
            this.btnYesAll.Click += new System.EventHandler(this.btnYesAll_Click);
            // 
            // btnNoAll
            // 
            this.btnNoAll.DialogResult = System.Windows.Forms.DialogResult.No;
            this.btnNoAll.Location = new System.Drawing.Point(330, 72);
            this.btnNoAll.Name = "btnNoAll";
            this.btnNoAll.Size = new System.Drawing.Size(100, 30);
            this.btnNoAll.TabIndex = 5;
            this.btnNoAll.Text = "Skip All";
            this.btnNoAll.UseVisualStyleBackColor = true;
            this.btnNoAll.Click += new System.EventHandler(this.btnNoAll_Click);
            // 
            // lblCaptionText
            // 
            this.lblCaptionText.Font = new System.Drawing.Font("Segoe UI Semibold", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblCaptionText.Location = new System.Drawing.Point(12, 9);
            this.lblCaptionText.Name = "lblCaptionText";
            this.lblCaptionText.Size = new System.Drawing.Size(418, 20);
            this.lblCaptionText.TabIndex = 0;
            this.lblCaptionText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblDialogText
            // 
            this.lblDialogText.Font = new System.Drawing.Font("Segoe UI", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblDialogText.ForeColor = System.Drawing.Color.Firebrick;
            this.lblDialogText.Location = new System.Drawing.Point(12, 29);
            this.lblDialogText.Name = "lblDialogText";
            this.lblDialogText.Size = new System.Drawing.Size(418, 40);
            this.lblDialogText.TabIndex = 1;
            this.lblDialogText.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // OverwriteDialogForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(442, 114);
            this.ControlBox = false;
            this.Controls.Add(this.lblDialogText);
            this.Controls.Add(this.lblCaptionText);
            this.Controls.Add(this.btnNoAll);
            this.Controls.Add(this.btnYesAll);
            this.Controls.Add(this.btnNo);
            this.Controls.Add(this.btnYes);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(458, 153);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(458, 153);
            this.Name = "OverwriteDialogForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Replace or Skip Files";
            this.TopMost = true;
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.Button btnYes;
        private System.Windows.Forms.Button btnNo;
        private System.Windows.Forms.Button btnYesAll;
        private System.Windows.Forms.Button btnNoAll;
        private System.Windows.Forms.Label lblCaptionText;
        private System.Windows.Forms.Label lblDialogText;
    }
}