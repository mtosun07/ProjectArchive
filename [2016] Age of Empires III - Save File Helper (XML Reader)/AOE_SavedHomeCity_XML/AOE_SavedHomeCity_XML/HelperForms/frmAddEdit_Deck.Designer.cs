namespace AOE3_HomeCity.HelperForms
{
    partial class frmAddEdit_Deck
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
            this.llblCards_add = new System.Windows.Forms.LinkLabel();
            this.llblCards_deleteAll = new System.Windows.Forms.LinkLabel();
            this.llblCards_delete = new System.Windows.Forms.LinkLabel();
            this.lbxCards = new System.Windows.Forms.ListBox();
            this.lblCards = new System.Windows.Forms.Label();
            this.txtName = new System.Windows.Forms.TextBox();
            this.lblName = new System.Windows.Forms.Label();
            this.btnCancel = new System.Windows.Forms.Button();
            this.btnOK = new System.Windows.Forms.Button();
            this.chcGameID = new System.Windows.Forms.CheckBox();
            this.nudGameID = new System.Windows.Forms.NumericUpDown();
            this.lblGameID = new System.Windows.Forms.Label();
            this.toolTip1 = new System.Windows.Forms.ToolTip(this.components);
            this.chcGameIDExists = new System.Windows.Forms.CheckBox();
            ((System.ComponentModel.ISupportInitialize)(this.nudGameID)).BeginInit();
            this.SuspendLayout();
            // 
            // llblCards_add
            // 
            this.llblCards_add.AutoSize = true;
            this.llblCards_add.Location = new System.Drawing.Point(335, 202);
            this.llblCards_add.Name = "llblCards_add";
            this.llblCards_add.Size = new System.Drawing.Size(105, 17);
            this.llblCards_add.TabIndex = 8;
            this.llblCards_add.TabStop = true;
            this.llblCards_add.Text = "Add New Cards";
            this.llblCards_add.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblCards_add_LinkClicked);
            // 
            // llblCards_deleteAll
            // 
            this.llblCards_deleteAll.AutoSize = true;
            this.llblCards_deleteAll.Location = new System.Drawing.Point(135, 222);
            this.llblCards_deleteAll.Name = "llblCards_deleteAll";
            this.llblCards_deleteAll.Size = new System.Drawing.Size(101, 17);
            this.llblCards_deleteAll.TabIndex = 9;
            this.llblCards_deleteAll.TabStop = true;
            this.llblCards_deleteAll.Text = "Clear All Cards";
            this.llblCards_deleteAll.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblCards_deleteAll_LinkClicked);
            // 
            // llblCards_delete
            // 
            this.llblCards_delete.AutoSize = true;
            this.llblCards_delete.Location = new System.Drawing.Point(135, 202);
            this.llblCards_delete.Name = "llblCards_delete";
            this.llblCards_delete.Size = new System.Drawing.Size(115, 17);
            this.llblCards_delete.TabIndex = 7;
            this.llblCards_delete.TabStop = true;
            this.llblCards_delete.Text = "Delete Selecteds";
            this.llblCards_delete.LinkClicked += new System.Windows.Forms.LinkLabelLinkClickedEventHandler(this.llblCards_delete_LinkClicked);
            // 
            // lbxCards
            // 
            this.lbxCards.FormattingEnabled = true;
            this.lbxCards.ItemHeight = 16;
            this.lbxCards.Location = new System.Drawing.Point(138, 66);
            this.lbxCards.Name = "lbxCards";
            this.lbxCards.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lbxCards.Size = new System.Drawing.Size(302, 132);
            this.lbxCards.TabIndex = 6;
            this.lbxCards.SelectedIndexChanged += new System.EventHandler(this.lbxCards_SelectedIndexChanged);
            // 
            // lblCards
            // 
            this.lblCards.Location = new System.Drawing.Point(12, 66);
            this.lblCards.Name = "lblCards";
            this.lblCards.Size = new System.Drawing.Size(120, 22);
            this.lblCards.TabIndex = 5;
            this.lblCards.Text = "Cards";
            this.lblCards.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // txtName
            // 
            this.txtName.Location = new System.Drawing.Point(138, 12);
            this.txtName.Name = "txtName";
            this.txtName.Size = new System.Drawing.Size(302, 22);
            this.txtName.TabIndex = 1;
            // 
            // lblName
            // 
            this.lblName.Location = new System.Drawing.Point(12, 12);
            this.lblName.Name = "lblName";
            this.lblName.Size = new System.Drawing.Size(120, 22);
            this.lblName.TabIndex = 0;
            this.lblName.Text = "Name";
            this.lblName.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnCancel
            // 
            this.btnCancel.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel;
            this.btnCancel.Location = new System.Drawing.Point(365, 257);
            this.btnCancel.Name = "btnCancel";
            this.btnCancel.Size = new System.Drawing.Size(75, 30);
            this.btnCancel.TabIndex = 11;
            this.btnCancel.Text = "Cancel";
            this.btnCancel.UseVisualStyleBackColor = true;
            this.btnCancel.Click += new System.EventHandler(this.btnCancel_Click);
            // 
            // btnOK
            // 
            this.btnOK.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnOK.DialogResult = System.Windows.Forms.DialogResult.OK;
            this.btnOK.Location = new System.Drawing.Point(284, 257);
            this.btnOK.Name = "btnOK";
            this.btnOK.Size = new System.Drawing.Size(75, 30);
            this.btnOK.TabIndex = 10;
            this.btnOK.UseVisualStyleBackColor = true;
            this.btnOK.Click += new System.EventHandler(this.btnOK_Click);
            // 
            // chcGameID
            // 
            this.chcGameID.Location = new System.Drawing.Point(315, 40);
            this.chcGameID.Name = "chcGameID";
            this.chcGameID.Size = new System.Drawing.Size(125, 22);
            this.chcGameID.TabIndex = 4;
            this.chcGameID.Text = "Leave Empty";
            this.chcGameID.UseVisualStyleBackColor = true;
            this.chcGameID.CheckedChanged += new System.EventHandler(this.chcEmpty_CheckedChanged);
            // 
            // nudGameID
            // 
            this.nudGameID.Location = new System.Drawing.Point(138, 40);
            this.nudGameID.Maximum = new decimal(new int[] {
            2147483647,
            0,
            0,
            0});
            this.nudGameID.Name = "nudGameID";
            this.nudGameID.Size = new System.Drawing.Size(171, 22);
            this.nudGameID.TabIndex = 3;
            // 
            // lblGameID
            // 
            this.lblGameID.Location = new System.Drawing.Point(12, 39);
            this.lblGameID.Name = "lblGameID";
            this.lblGameID.Size = new System.Drawing.Size(120, 22);
            this.lblGameID.TabIndex = 2;
            this.lblGameID.Text = "Game ID";
            this.lblGameID.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // chcGameIDExists
            // 
            this.chcGameIDExists.AutoSize = true;
            this.chcGameIDExists.Checked = true;
            this.chcGameIDExists.CheckState = System.Windows.Forms.CheckState.Checked;
            this.chcGameIDExists.ForeColor = System.Drawing.Color.Crimson;
            this.chcGameIDExists.Location = new System.Drawing.Point(12, 263);
            this.chcGameIDExists.Name = "chcGameIDExists";
            this.chcGameIDExists.Size = new System.Drawing.Size(124, 21);
            this.chcGameIDExists.TabIndex = 12;
            this.chcGameIDExists.Text = "Game ID exists";
            this.toolTip1.SetToolTip(this.chcGameIDExists, "IMPORTANT:\r\nIf you are playing the game without any of its expantion packs, or yo" +
        "u are playing Extended Edition, you must leave this field CHECKED.\r\nIf not, you " +
        "must UNCHECK this field.");
            this.chcGameIDExists.UseVisualStyleBackColor = true;
            this.chcGameIDExists.Visible = false;
            this.chcGameIDExists.CheckedChanged += new System.EventHandler(this.chcGameIDExists_CheckedChanged);
            // 
            // frmAddEdit_Deck
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(8F, 16F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(452, 298);
            this.Controls.Add(this.chcGameIDExists);
            this.Controls.Add(this.chcGameID);
            this.Controls.Add(this.nudGameID);
            this.Controls.Add(this.lblGameID);
            this.Controls.Add(this.llblCards_add);
            this.Controls.Add(this.llblCards_deleteAll);
            this.Controls.Add(this.llblCards_delete);
            this.Controls.Add(this.lbxCards);
            this.Controls.Add(this.lblCards);
            this.Controls.Add(this.txtName);
            this.Controls.Add(this.lblName);
            this.Controls.Add(this.btnCancel);
            this.Controls.Add(this.btnOK);
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(470, 345);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(470, 345);
            this.Name = "frmAddEdit_Deck";
            this.ShowIcon = false;
            this.ShowInTaskbar = false;
            this.SizeGripStyle = System.Windows.Forms.SizeGripStyle.Hide;
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "Deck";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.frmAddEdit_Deck_FormClosing);
            this.Load += new System.EventHandler(this.frmAddEdit_Deck_Load);
            ((System.ComponentModel.ISupportInitialize)(this.nudGameID)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion
        private System.Windows.Forms.LinkLabel llblCards_add;
        private System.Windows.Forms.LinkLabel llblCards_deleteAll;
        private System.Windows.Forms.LinkLabel llblCards_delete;
        private System.Windows.Forms.ListBox lbxCards;
        private System.Windows.Forms.Label lblCards;
        private System.Windows.Forms.Label lblName;
        private System.Windows.Forms.Button btnCancel;
        private System.Windows.Forms.CheckBox chcGameID;
        private System.Windows.Forms.NumericUpDown nudGameID;
        private System.Windows.Forms.Label lblGameID;
        private System.Windows.Forms.ToolTip toolTip1;
        private System.Windows.Forms.CheckBox chcGameIDExists;
        public System.Windows.Forms.TextBox txtName;
        public System.Windows.Forms.Button btnOK;
    }
}