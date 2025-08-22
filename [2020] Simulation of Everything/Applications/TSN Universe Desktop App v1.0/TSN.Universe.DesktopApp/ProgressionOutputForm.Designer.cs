namespace TSN.Universe.DesktopApp
{
    partial class ProgressionOutputForm
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
            this.txtConsole = new System.Windows.Forms.TextBox();
            this.nudGeneration = new System.Windows.Forms.NumericUpDown();
            this.chkPrevious = new System.Windows.Forms.CheckBox();
            this.btnClipboard = new System.Windows.Forms.Button();
            this.label1 = new System.Windows.Forms.Label();
            this.btnRefresh = new System.Windows.Forms.Button();
            ((System.ComponentModel.ISupportInitialize)(this.nudGeneration)).BeginInit();
            this.SuspendLayout();
            // 
            // txtConsole
            // 
            this.txtConsole.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.txtConsole.BackColor = System.Drawing.Color.Black;
            this.txtConsole.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtConsole.Font = new System.Drawing.Font("Consolas", 11.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtConsole.ForeColor = System.Drawing.Color.Lime;
            this.txtConsole.Location = new System.Drawing.Point(-1, 57);
            this.txtConsole.MaxLength = 2147483647;
            this.txtConsole.Multiline = true;
            this.txtConsole.Name = "txtConsole";
            this.txtConsole.ReadOnly = true;
            this.txtConsole.ScrollBars = System.Windows.Forms.ScrollBars.Both;
            this.txtConsole.Size = new System.Drawing.Size(785, 386);
            this.txtConsole.TabIndex = 0;
            this.txtConsole.WordWrap = false;
            // 
            // nudGeneration
            // 
            this.nudGeneration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudGeneration.BackColor = System.Drawing.Color.Black;
            this.nudGeneration.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.nudGeneration.ForeColor = System.Drawing.Color.Lime;
            this.nudGeneration.Location = new System.Drawing.Point(601, 34);
            this.nudGeneration.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudGeneration.Name = "nudGeneration";
            this.nudGeneration.Size = new System.Drawing.Size(76, 16);
            this.nudGeneration.TabIndex = 2;
            this.nudGeneration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // chkPrevious
            // 
            this.chkPrevious.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.chkPrevious.AutoSize = true;
            this.chkPrevious.BackColor = System.Drawing.Color.Transparent;
            this.chkPrevious.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkPrevious.ForeColor = System.Drawing.Color.Lime;
            this.chkPrevious.Location = new System.Drawing.Point(683, 34);
            this.chkPrevious.Name = "chkPrevious";
            this.chkPrevious.Size = new System.Drawing.Size(93, 17);
            this.chkPrevious.TabIndex = 3;
            this.chkPrevious.Text = "And Previous";
            this.chkPrevious.UseVisualStyleBackColor = false;
            // 
            // btnClipboard
            // 
            this.btnClipboard.BackColor = System.Drawing.Color.Black;
            this.btnClipboard.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnClipboard.ForeColor = System.Drawing.Color.Lime;
            this.btnClipboard.Location = new System.Drawing.Point(12, 12);
            this.btnClipboard.Name = "btnClipboard";
            this.btnClipboard.Size = new System.Drawing.Size(150, 39);
            this.btnClipboard.TabIndex = 4;
            this.btnClipboard.Text = "Copy To Clipboard";
            this.btnClipboard.UseVisualStyleBackColor = false;
            this.btnClipboard.Click += new System.EventHandler(this.btnClipboard_Click);
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.ForeColor = System.Drawing.Color.Lime;
            this.label1.Location = new System.Drawing.Point(601, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(171, 19);
            this.label1.TabIndex = 1;
            this.label1.Text = "Representing Generation #";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // btnRefresh
            // 
            this.btnRefresh.BackColor = System.Drawing.Color.Black;
            this.btnRefresh.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnRefresh.ForeColor = System.Drawing.Color.Lime;
            this.btnRefresh.Location = new System.Drawing.Point(168, 12);
            this.btnRefresh.Name = "btnRefresh";
            this.btnRefresh.Size = new System.Drawing.Size(150, 39);
            this.btnRefresh.TabIndex = 5;
            this.btnRefresh.Text = "Refresh";
            this.btnRefresh.UseVisualStyleBackColor = false;
            this.btnRefresh.Click += new System.EventHandler(this.RefreshRequested);
            // 
            // ProgressionOutputForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Black;
            this.ClientSize = new System.Drawing.Size(784, 443);
            this.Controls.Add(this.btnRefresh);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.nudGeneration);
            this.Controls.Add(this.chkPrevious);
            this.Controls.Add(this.btnClipboard);
            this.Controls.Add(this.txtConsole);
            this.MinimumSize = new System.Drawing.Size(800, 482);
            this.Name = "ProgressionOutputForm";
            this.ShowIcon = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Progression Output";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.ProgressionOutputForm_FormClosing);
            ((System.ComponentModel.ISupportInitialize)(this.nudGeneration)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.TextBox txtConsole;
        private System.Windows.Forms.NumericUpDown nudGeneration;
        private System.Windows.Forms.CheckBox chkPrevious;
        private System.Windows.Forms.Button btnClipboard;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Button btnRefresh;
    }
}