
namespace TSN.EncryptDecrypt.WindowsForms
{
    partial class MainForm
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
            this._groupboxData = new System.Windows.Forms.GroupBox();
            this._textBoxData = new System.Windows.Forms.TextBox();
            this._labelPassword = new System.Windows.Forms.Label();
            this._textboxPassword = new System.Windows.Forms.TextBox();
            this._buttonEncrypt = new System.Windows.Forms.Button();
            this._buttonDecrypt = new System.Windows.Forms.Button();
            this._groupboxResult = new System.Windows.Forms.GroupBox();
            this._textBoxResult = new System.Windows.Forms.TextBox();
            this._groupboxData.SuspendLayout();
            this._groupboxResult.SuspendLayout();
            this.SuspendLayout();
            // 
            // _groupboxData
            // 
            this._groupboxData.Controls.Add(this._textboxPassword);
            this._groupboxData.Controls.Add(this._labelPassword);
            this._groupboxData.Controls.Add(this._textBoxData);
            this._groupboxData.Location = new System.Drawing.Point(12, 12);
            this._groupboxData.Name = "_groupboxData";
            this._groupboxData.Size = new System.Drawing.Size(450, 300);
            this._groupboxData.TabIndex = 0;
            this._groupboxData.TabStop = false;
            this._groupboxData.Text = "Data";
            // 
            // _textBoxData
            // 
            this._textBoxData.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textBoxData.Location = new System.Drawing.Point(6, 19);
            this._textBoxData.Multiline = true;
            this._textBoxData.Name = "_textBoxData";
            this._textBoxData.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textBoxData.Size = new System.Drawing.Size(438, 249);
            this._textBoxData.TabIndex = 0;
            // 
            // _labelPassword
            // 
            this._labelPassword.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this._labelPassword.Location = new System.Drawing.Point(6, 274);
            this._labelPassword.Name = "_labelPassword";
            this._labelPassword.Size = new System.Drawing.Size(100, 20);
            this._labelPassword.TabIndex = 1;
            this._labelPassword.Text = "Password:";
            this._labelPassword.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // _textboxPassword
            // 
            this._textboxPassword.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this._textboxPassword.Location = new System.Drawing.Point(112, 274);
            this._textboxPassword.Name = "_textboxPassword";
            this._textboxPassword.Size = new System.Drawing.Size(332, 20);
            this._textboxPassword.TabIndex = 2;
            // 
            // _buttonEncrypt
            // 
            this._buttonEncrypt.Location = new System.Drawing.Point(12, 318);
            this._buttonEncrypt.Name = "_buttonEncrypt";
            this._buttonEncrypt.Size = new System.Drawing.Size(150, 30);
            this._buttonEncrypt.TabIndex = 1;
            this._buttonEncrypt.Text = "Encrypt";
            this._buttonEncrypt.UseVisualStyleBackColor = true;
            this._buttonEncrypt.Click += new System.EventHandler(this.ButtonEncrypt_Click);
            // 
            // _buttonDecrypt
            // 
            this._buttonDecrypt.Location = new System.Drawing.Point(312, 318);
            this._buttonDecrypt.Name = "_buttonDecrypt";
            this._buttonDecrypt.Size = new System.Drawing.Size(150, 30);
            this._buttonDecrypt.TabIndex = 2;
            this._buttonDecrypt.Text = "Decrypt";
            this._buttonDecrypt.UseVisualStyleBackColor = true;
            this._buttonDecrypt.Click += new System.EventHandler(this.ButtonDecrypt_Click);
            // 
            // _groupboxResult
            // 
            this._groupboxResult.Controls.Add(this._textBoxResult);
            this._groupboxResult.Location = new System.Drawing.Point(468, 12);
            this._groupboxResult.Name = "_groupboxResult";
            this._groupboxResult.Size = new System.Drawing.Size(450, 336);
            this._groupboxResult.TabIndex = 3;
            this._groupboxResult.TabStop = false;
            this._groupboxResult.Text = "Result";
            // 
            // _textBoxResult
            // 
            this._textBoxResult.Location = new System.Drawing.Point(6, 19);
            this._textBoxResult.Multiline = true;
            this._textBoxResult.Name = "_textBoxResult";
            this._textBoxResult.ReadOnly = true;
            this._textBoxResult.ScrollBars = System.Windows.Forms.ScrollBars.Vertical;
            this._textBoxResult.Size = new System.Drawing.Size(438, 311);
            this._textBoxResult.TabIndex = 4;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(928, 360);
            this.Controls.Add(this._groupboxResult);
            this.Controls.Add(this._buttonDecrypt);
            this.Controls.Add(this._buttonEncrypt);
            this.Controls.Add(this._groupboxData);
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(944, 399);
            this.MinimumSize = new System.Drawing.Size(944, 399);
            this.Name = "MainForm";
            this.Text = "TSN - Encrypt & Decrypt";
            this._groupboxData.ResumeLayout(false);
            this._groupboxData.PerformLayout();
            this._groupboxResult.ResumeLayout(false);
            this._groupboxResult.PerformLayout();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.GroupBox _groupboxData;
        private System.Windows.Forms.TextBox _textboxPassword;
        private System.Windows.Forms.Label _labelPassword;
        private System.Windows.Forms.TextBox _textBoxData;
        private System.Windows.Forms.Button _buttonEncrypt;
        private System.Windows.Forms.Button _buttonDecrypt;
        private System.Windows.Forms.GroupBox _groupboxResult;
        private System.Windows.Forms.TextBox _textBoxResult;
    }
}

