namespace TSN.Universe.DesktopApp
{
    partial class FoodInfoUserControl
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

        #region Component Designer generated code

        /// <summary> 
        /// Required method for Designer support - do not modify 
        /// the contents of this method with the code editor.
        /// </summary>
        private void InitializeComponent()
        {
            this.label1 = new System.Windows.Forms.Label();
            this.llblReturnedFrom = new System.Windows.Forms.LinkLabel();
            this.llblEatenBy = new System.Windows.Forms.LinkLabel();
            this.label2 = new System.Windows.Forms.Label();
            this.SuspendLayout();
            // 
            // label1
            // 
            this.label1.BackColor = System.Drawing.Color.White;
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(0, 0);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(200, 25);
            this.label1.TabIndex = 0;
            this.label1.Text = "Returned From";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // llblReturnedFrom
            // 
            this.llblReturnedFrom.ActiveLinkColor = System.Drawing.Color.Red;
            this.llblReturnedFrom.BackColor = System.Drawing.Color.FloralWhite;
            this.llblReturnedFrom.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.llblReturnedFrom.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.llblReturnedFrom.LinkColor = System.Drawing.SystemColors.ControlText;
            this.llblReturnedFrom.Location = new System.Drawing.Point(200, 0);
            this.llblReturnedFrom.Name = "llblReturnedFrom";
            this.llblReturnedFrom.Size = new System.Drawing.Size(350, 25);
            this.llblReturnedFrom.TabIndex = 1;
            this.llblReturnedFrom.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.llblReturnedFrom.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            this.llblReturnedFrom.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblMatterId_LinkClicked);
            // 
            // llblEatenBy
            // 
            this.llblEatenBy.ActiveLinkColor = System.Drawing.Color.Red;
            this.llblEatenBy.BackColor = System.Drawing.Color.FloralWhite;
            this.llblEatenBy.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.llblEatenBy.Font = new System.Drawing.Font("Segoe UI", 8.25F);
            this.llblEatenBy.LinkColor = System.Drawing.SystemColors.ControlText;
            this.llblEatenBy.Location = new System.Drawing.Point(200, 25);
            this.llblEatenBy.Name = "llblEatenBy";
            this.llblEatenBy.Size = new System.Drawing.Size(350, 25);
            this.llblEatenBy.TabIndex = 3;
            this.llblEatenBy.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            this.llblEatenBy.VisitedLinkColor = System.Drawing.SystemColors.ControlText;
            this.llblEatenBy.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblMatterId_LinkClicked);
            // 
            // label2
            // 
            this.label2.BackColor = System.Drawing.Color.White;
            this.label2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label2.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(0, 25);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(200, 25);
            this.label2.TabIndex = 2;
            this.label2.Text = "Eaten By";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // FoodInfoUserControl
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.Transparent;
            this.Controls.Add(this.llblEatenBy);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.llblReturnedFrom);
            this.Controls.Add(this.label1);
            this.Name = "FoodInfoUserControl";
            this.Size = new System.Drawing.Size(550, 50);
            this.ResumeLayout(false);

        }

        #endregion
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.LinkLabel llblReturnedFrom;
        private System.Windows.Forms.LinkLabel llblEatenBy;
        private System.Windows.Forms.Label label2;
    }
}
