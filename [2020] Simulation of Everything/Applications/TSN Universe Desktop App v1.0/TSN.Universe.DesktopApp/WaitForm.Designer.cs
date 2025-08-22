namespace TSN.Universe.DesktopApp
{
    partial class WaitForm
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
            this.bgwAction = new System.ComponentModel.BackgroundWorker();
            this.progressbar = new System.Windows.Forms.ProgressBar();
            this.lblActionInfo = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // bgwAction
            // 
            this.bgwAction.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwAction_DoWork);
            this.bgwAction.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwAction_RunWorkerCompleted);
            // 
            // progressbar
            // 
            this.progressbar.Location = new System.Drawing.Point(12, 42);
            this.progressbar.Name = "progressbar";
            this.progressbar.Size = new System.Drawing.Size(500, 23);
            this.progressbar.Step = 1;
            this.progressbar.Style = System.Windows.Forms.ProgressBarStyle.Marquee;
            this.progressbar.TabIndex = 0;
            // 
            // lblActionInfo
            // 
            this.lblActionInfo.Font = new System.Drawing.Font("Segoe UI Semibold", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblActionInfo.ForeColor = System.Drawing.Color.DimGray;
            this.lblActionInfo.Location = new System.Drawing.Point(12, 12);
            this.lblActionInfo.Name = "lblActionInfo";
            this.lblActionInfo.Size = new System.Drawing.Size(500, 23);
            this.lblActionInfo.TabIndex = 1;
            this.lblActionInfo.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // WaitForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.White;
            this.ClientSize = new System.Drawing.Size(524, 100);
            this.ControlBox = false;
            this.Controls.Add(this.lblActionInfo);
            this.Controls.Add(this.progressbar);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(524, 100);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(524, 100);
            this.Name = "WaitForm";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Load += new System.EventHandler(this.WaitForm_Load);
            this.ResumeLayout(false);

        }

        #endregion

        private System.ComponentModel.BackgroundWorker bgwAction;
        private System.Windows.Forms.ProgressBar progressbar;
        private System.Windows.Forms.Label lblActionInfo;
    }
}