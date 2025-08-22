namespace TSN.ColorSwitcher
{
    partial class MainForm
    {
        private System.ComponentModel.IContainer components = null;

        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Windows Form Designer generated code

        private void InitializeComponent()
        {
            this.txtSelectedColorR = new System.Windows.Forms.TextBox();
            this.txtSelectedColorG = new System.Windows.Forms.TextBox();
            this.txtSelectedColorB = new System.Windows.Forms.TextBox();
            this.txtSelectedColorA = new System.Windows.Forms.TextBox();
            this.txtSelectedColorHex = new System.Windows.Forms.TextBox();
            this.label13 = new System.Windows.Forms.Label();
            this.lblSelectedColor = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.btnSaveImage = new System.Windows.Forms.Button();
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.lblFilePath = new System.Windows.Forms.Label();
            this.grbColorsInImage = new System.Windows.Forms.GroupBox();
            this.btnClearSelectedImage = new System.Windows.Forms.Button();
            this.pnlSelectedColor = new System.Windows.Forms.Panel();
            this.label11 = new System.Windows.Forms.Label();
            this.btnOpenChangeColor = new System.Windows.Forms.Button();
            this.lstColorsInImage = new System.Windows.Forms.ListBox();
            this.colorDialog1 = new System.Windows.Forms.ColorDialog();
            this.grbChangeColor = new System.Windows.Forms.GroupBox();
            this.nudNewColorA = new System.Windows.Forms.NumericUpDown();
            this.nudNewColorB = new System.Windows.Forms.NumericUpDown();
            this.nudNewColorG = new System.Windows.Forms.NumericUpDown();
            this.nudNewColorR = new System.Windows.Forms.NumericUpDown();
            this.btnCancelChangeColor = new System.Windows.Forms.Button();
            this.btnChangeColor = new System.Windows.Forms.Button();
            this.btnColorPicker = new System.Windows.Forms.Button();
            this.txtNewColorHex = new System.Windows.Forms.TextBox();
            this.label1 = new System.Windows.Forms.Label();
            this.lblNewColor = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label8 = new System.Windows.Forms.Label();
            this.label14 = new System.Windows.Forms.Label();
            this.btnBrowseImage = new System.Windows.Forms.Button();
            this.picImage = new System.Windows.Forms.PictureBox();
            this.grbColorsInImage.SuspendLayout();
            this.pnlSelectedColor.SuspendLayout();
            this.grbChangeColor.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNewColorA)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNewColorB)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNewColorG)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNewColorR)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).BeginInit();
            this.SuspendLayout();
            // 
            // txtSelectedColorR
            // 
            this.txtSelectedColorR.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtSelectedColorR.Location = new System.Drawing.Point(46, 50);
            this.txtSelectedColorR.Name = "txtSelectedColorR";
            this.txtSelectedColorR.ReadOnly = true;
            this.txtSelectedColorR.Size = new System.Drawing.Size(104, 23);
            this.txtSelectedColorR.TabIndex = 2;
            this.txtSelectedColorR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSelectedColorG
            // 
            this.txtSelectedColorG.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtSelectedColorG.Location = new System.Drawing.Point(46, 79);
            this.txtSelectedColorG.Name = "txtSelectedColorG";
            this.txtSelectedColorG.ReadOnly = true;
            this.txtSelectedColorG.Size = new System.Drawing.Size(104, 23);
            this.txtSelectedColorG.TabIndex = 4;
            this.txtSelectedColorG.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSelectedColorB
            // 
            this.txtSelectedColorB.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtSelectedColorB.Location = new System.Drawing.Point(46, 108);
            this.txtSelectedColorB.Name = "txtSelectedColorB";
            this.txtSelectedColorB.ReadOnly = true;
            this.txtSelectedColorB.Size = new System.Drawing.Size(104, 23);
            this.txtSelectedColorB.TabIndex = 6;
            this.txtSelectedColorB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSelectedColorA
            // 
            this.txtSelectedColorA.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtSelectedColorA.Location = new System.Drawing.Point(46, 137);
            this.txtSelectedColorA.Name = "txtSelectedColorA";
            this.txtSelectedColorA.ReadOnly = true;
            this.txtSelectedColorA.Size = new System.Drawing.Size(104, 23);
            this.txtSelectedColorA.TabIndex = 8;
            this.txtSelectedColorA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // txtSelectedColorHex
            // 
            this.txtSelectedColorHex.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtSelectedColorHex.Location = new System.Drawing.Point(46, 166);
            this.txtSelectedColorHex.Name = "txtSelectedColorHex";
            this.txtSelectedColorHex.ReadOnly = true;
            this.txtSelectedColorHex.Size = new System.Drawing.Size(104, 23);
            this.txtSelectedColorHex.TabIndex = 10;
            this.txtSelectedColorHex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label13.Location = new System.Drawing.Point(0, 166);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(40, 23);
            this.label13.TabIndex = 9;
            this.label13.Text = "Hex:";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblSelectedColor
            // 
            this.lblSelectedColor.BackColor = System.Drawing.Color.Transparent;
            this.lblSelectedColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblSelectedColor.Location = new System.Drawing.Point(0, 0);
            this.lblSelectedColor.Name = "lblSelectedColor";
            this.lblSelectedColor.Size = new System.Drawing.Size(150, 47);
            this.lblSelectedColor.TabIndex = 0;
            this.lblSelectedColor.BackColorChanged += new System.EventHandler(this.lblSelectedColor_BackColorChanged);
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label12.Location = new System.Drawing.Point(0, 137);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(40, 23);
            this.label12.TabIndex = 7;
            this.label12.Text = "A:";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label9.Location = new System.Drawing.Point(0, 50);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(40, 23);
            this.label9.TabIndex = 1;
            this.label9.Text = "R:";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label10.Location = new System.Drawing.Point(0, 79);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(40, 23);
            this.label10.TabIndex = 3;
            this.label10.Text = "G:";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnSaveImage
            // 
            this.btnSaveImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnSaveImage.BackColor = System.Drawing.Color.White;
            this.btnSaveImage.Font = new System.Drawing.Font("Corbel", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnSaveImage.Location = new System.Drawing.Point(596, 723);
            this.btnSaveImage.Name = "btnSaveImage";
            this.btnSaveImage.Size = new System.Drawing.Size(150, 50);
            this.btnSaveImage.TabIndex = 4;
            this.btnSaveImage.Text = "SAVE IMAGE...";
            this.btnSaveImage.UseVisualStyleBackColor = false;
            this.btnSaveImage.Visible = false;
            this.btnSaveImage.Click += new System.EventHandler(this.btnSaveImage_Click);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "png";
            this.saveFileDialog1.Filter = "All Files|*.*";
            this.saveFileDialog1.Title = "Save Image";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.ShowReadOnly = true;
            this.openFileDialog1.Title = "Choose an image file to open.";
            // 
            // lblFilePath
            // 
            this.lblFilePath.Anchor = ((System.Windows.Forms.AnchorStyles)(((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.lblFilePath.Location = new System.Drawing.Point(168, 431);
            this.lblFilePath.Name = "lblFilePath";
            this.lblFilePath.Size = new System.Drawing.Size(578, 30);
            this.lblFilePath.TabIndex = 1;
            this.lblFilePath.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // grbColorsInImage
            // 
            this.grbColorsInImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grbColorsInImage.Controls.Add(this.btnClearSelectedImage);
            this.grbColorsInImage.Controls.Add(this.pnlSelectedColor);
            this.grbColorsInImage.Controls.Add(this.btnOpenChangeColor);
            this.grbColorsInImage.Controls.Add(this.lstColorsInImage);
            this.grbColorsInImage.Location = new System.Drawing.Point(12, 467);
            this.grbColorsInImage.Name = "grbColorsInImage";
            this.grbColorsInImage.Size = new System.Drawing.Size(400, 250);
            this.grbColorsInImage.TabIndex = 2;
            this.grbColorsInImage.TabStop = false;
            this.grbColorsInImage.Text = "Colors in Image";
            this.grbColorsInImage.Visible = false;
            // 
            // btnClearSelectedImage
            // 
            this.btnClearSelectedImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClearSelectedImage.Enabled = false;
            this.btnClearSelectedImage.Location = new System.Drawing.Point(6, 214);
            this.btnClearSelectedImage.Name = "btnClearSelectedImage";
            this.btnClearSelectedImage.Size = new System.Drawing.Size(150, 30);
            this.btnClearSelectedImage.TabIndex = 2;
            this.btnClearSelectedImage.Text = "Clear Selection";
            this.btnClearSelectedImage.UseVisualStyleBackColor = true;
            this.btnClearSelectedImage.Click += new System.EventHandler(this.btnClearSelectedImage_Click);
            // 
            // pnlSelectedColor
            // 
            this.pnlSelectedColor.Controls.Add(this.txtSelectedColorR);
            this.pnlSelectedColor.Controls.Add(this.txtSelectedColorG);
            this.pnlSelectedColor.Controls.Add(this.txtSelectedColorB);
            this.pnlSelectedColor.Controls.Add(this.txtSelectedColorA);
            this.pnlSelectedColor.Controls.Add(this.txtSelectedColorHex);
            this.pnlSelectedColor.Controls.Add(this.label13);
            this.pnlSelectedColor.Controls.Add(this.lblSelectedColor);
            this.pnlSelectedColor.Controls.Add(this.label12);
            this.pnlSelectedColor.Controls.Add(this.label9);
            this.pnlSelectedColor.Controls.Add(this.label11);
            this.pnlSelectedColor.Controls.Add(this.label10);
            this.pnlSelectedColor.Location = new System.Drawing.Point(244, 19);
            this.pnlSelectedColor.Name = "pnlSelectedColor";
            this.pnlSelectedColor.Size = new System.Drawing.Size(150, 189);
            this.pnlSelectedColor.TabIndex = 1;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label11.Location = new System.Drawing.Point(0, 108);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(40, 23);
            this.label11.TabIndex = 5;
            this.label11.Text = "B:";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnOpenChangeColor
            // 
            this.btnOpenChangeColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnOpenChangeColor.Enabled = false;
            this.btnOpenChangeColor.Location = new System.Drawing.Point(244, 214);
            this.btnOpenChangeColor.Name = "btnOpenChangeColor";
            this.btnOpenChangeColor.Size = new System.Drawing.Size(150, 30);
            this.btnOpenChangeColor.TabIndex = 3;
            this.btnOpenChangeColor.Text = "Change Color(s) with...";
            this.btnOpenChangeColor.UseVisualStyleBackColor = true;
            this.btnOpenChangeColor.Click += new System.EventHandler(this.btnOpenChangeColor_Click);
            // 
            // lstColorsInImage
            // 
            this.lstColorsInImage.Font = new System.Drawing.Font("Consolas", 9F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lstColorsInImage.FormattingEnabled = true;
            this.lstColorsInImage.ItemHeight = 14;
            this.lstColorsInImage.Location = new System.Drawing.Point(6, 19);
            this.lstColorsInImage.Name = "lstColorsInImage";
            this.lstColorsInImage.SelectionMode = System.Windows.Forms.SelectionMode.MultiExtended;
            this.lstColorsInImage.Size = new System.Drawing.Size(232, 186);
            this.lstColorsInImage.TabIndex = 0;
            this.lstColorsInImage.SelectedIndexChanged += new System.EventHandler(this.lstColorsInImage_SelectedIndexChanged);
            // 
            // colorDialog1
            // 
            this.colorDialog1.AnyColor = true;
            this.colorDialog1.FullOpen = true;
            // 
            // grbChangeColor
            // 
            this.grbChangeColor.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.grbChangeColor.Controls.Add(this.nudNewColorA);
            this.grbChangeColor.Controls.Add(this.nudNewColorB);
            this.grbChangeColor.Controls.Add(this.nudNewColorG);
            this.grbChangeColor.Controls.Add(this.nudNewColorR);
            this.grbChangeColor.Controls.Add(this.btnCancelChangeColor);
            this.grbChangeColor.Controls.Add(this.btnChangeColor);
            this.grbChangeColor.Controls.Add(this.btnColorPicker);
            this.grbChangeColor.Controls.Add(this.txtNewColorHex);
            this.grbChangeColor.Controls.Add(this.label1);
            this.grbChangeColor.Controls.Add(this.lblNewColor);
            this.grbChangeColor.Controls.Add(this.label6);
            this.grbChangeColor.Controls.Add(this.label7);
            this.grbChangeColor.Controls.Add(this.label8);
            this.grbChangeColor.Controls.Add(this.label14);
            this.grbChangeColor.Location = new System.Drawing.Point(418, 467);
            this.grbChangeColor.Name = "grbChangeColor";
            this.grbChangeColor.Size = new System.Drawing.Size(328, 250);
            this.grbChangeColor.TabIndex = 3;
            this.grbChangeColor.TabStop = false;
            this.grbChangeColor.Text = "Change Color(s) with...";
            this.grbChangeColor.Visible = false;
            this.grbChangeColor.VisibleChanged += new System.EventHandler(this.grbChangeColor_VisibleChanged);
            // 
            // nudNewColorA
            // 
            this.nudNewColorA.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.nudNewColorA.Location = new System.Drawing.Point(52, 156);
            this.nudNewColorA.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudNewColorA.Name = "nudNewColorA";
            this.nudNewColorA.Size = new System.Drawing.Size(270, 23);
            this.nudNewColorA.TabIndex = 8;
            this.nudNewColorA.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nudNewColorB
            // 
            this.nudNewColorB.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.nudNewColorB.Location = new System.Drawing.Point(52, 127);
            this.nudNewColorB.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudNewColorB.Name = "nudNewColorB";
            this.nudNewColorB.Size = new System.Drawing.Size(270, 23);
            this.nudNewColorB.TabIndex = 6;
            this.nudNewColorB.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudNewColorB.ValueChanged += new System.EventHandler(this.nudNewColor_ValueChanged);
            // 
            // nudNewColorG
            // 
            this.nudNewColorG.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.nudNewColorG.Location = new System.Drawing.Point(52, 98);
            this.nudNewColorG.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudNewColorG.Name = "nudNewColorG";
            this.nudNewColorG.Size = new System.Drawing.Size(270, 23);
            this.nudNewColorG.TabIndex = 4;
            this.nudNewColorG.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudNewColorG.ValueChanged += new System.EventHandler(this.nudNewColor_ValueChanged);
            // 
            // nudNewColorR
            // 
            this.nudNewColorR.Font = new System.Drawing.Font("Consolas", 9.75F);
            this.nudNewColorR.Location = new System.Drawing.Point(52, 69);
            this.nudNewColorR.Maximum = new decimal(new int[] {
            255,
            0,
            0,
            0});
            this.nudNewColorR.Name = "nudNewColorR";
            this.nudNewColorR.Size = new System.Drawing.Size(270, 23);
            this.nudNewColorR.TabIndex = 2;
            this.nudNewColorR.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudNewColorR.ValueChanged += new System.EventHandler(this.nudNewColor_ValueChanged);
            // 
            // btnCancelChangeColor
            // 
            this.btnCancelChangeColor.Location = new System.Drawing.Point(162, 214);
            this.btnCancelChangeColor.Name = "btnCancelChangeColor";
            this.btnCancelChangeColor.Size = new System.Drawing.Size(77, 30);
            this.btnCancelChangeColor.TabIndex = 12;
            this.btnCancelChangeColor.Text = "CANCEL";
            this.btnCancelChangeColor.UseVisualStyleBackColor = true;
            this.btnCancelChangeColor.Click += new System.EventHandler(this.btnCancelChangeColor_Click);
            // 
            // btnChangeColor
            // 
            this.btnChangeColor.Location = new System.Drawing.Point(245, 214);
            this.btnChangeColor.Name = "btnChangeColor";
            this.btnChangeColor.Size = new System.Drawing.Size(77, 30);
            this.btnChangeColor.TabIndex = 13;
            this.btnChangeColor.Text = "CHANGE";
            this.btnChangeColor.UseVisualStyleBackColor = true;
            this.btnChangeColor.Click += new System.EventHandler(this.btnChangeColor_Click);
            // 
            // btnColorPicker
            // 
            this.btnColorPicker.Location = new System.Drawing.Point(6, 214);
            this.btnColorPicker.Name = "btnColorPicker";
            this.btnColorPicker.Size = new System.Drawing.Size(150, 30);
            this.btnColorPicker.TabIndex = 11;
            this.btnColorPicker.Text = "Color Picker...";
            this.btnColorPicker.UseVisualStyleBackColor = true;
            this.btnColorPicker.Click += new System.EventHandler(this.btnColorPicker_Click);
            // 
            // txtNewColorHex
            // 
            this.txtNewColorHex.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtNewColorHex.Location = new System.Drawing.Point(52, 185);
            this.txtNewColorHex.Name = "txtNewColorHex";
            this.txtNewColorHex.ReadOnly = true;
            this.txtNewColorHex.Size = new System.Drawing.Size(270, 23);
            this.txtNewColorHex.TabIndex = 10;
            this.txtNewColorHex.Text = "#FFFFFF";
            this.txtNewColorHex.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label1
            // 
            this.label1.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label1.Location = new System.Drawing.Point(6, 185);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(40, 23);
            this.label1.TabIndex = 9;
            this.label1.Text = "Hex:";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // lblNewColor
            // 
            this.lblNewColor.BackColor = System.Drawing.Color.White;
            this.lblNewColor.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.lblNewColor.Location = new System.Drawing.Point(6, 19);
            this.lblNewColor.Name = "lblNewColor";
            this.lblNewColor.Size = new System.Drawing.Size(316, 47);
            this.lblNewColor.TabIndex = 0;
            this.lblNewColor.Click += new System.EventHandler(this.lblNewColor_Click);
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label6.Location = new System.Drawing.Point(6, 156);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(40, 23);
            this.label6.TabIndex = 7;
            this.label6.Text = "A:";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label7.Location = new System.Drawing.Point(6, 69);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(40, 23);
            this.label7.TabIndex = 1;
            this.label7.Text = "R:";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label8
            // 
            this.label8.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label8.Location = new System.Drawing.Point(6, 127);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(40, 23);
            this.label8.TabIndex = 5;
            this.label8.Text = "B:";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Consolas", 9.75F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label14.Location = new System.Drawing.Point(6, 98);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(40, 23);
            this.label14.TabIndex = 3;
            this.label14.Text = "G:";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnBrowseImage
            // 
            this.btnBrowseImage.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnBrowseImage.Location = new System.Drawing.Point(12, 431);
            this.btnBrowseImage.Name = "btnBrowseImage";
            this.btnBrowseImage.Size = new System.Drawing.Size(150, 30);
            this.btnBrowseImage.TabIndex = 0;
            this.btnBrowseImage.Text = "Browse Image...";
            this.btnBrowseImage.UseVisualStyleBackColor = true;
            this.btnBrowseImage.Click += new System.EventHandler(this.btnBrowseImage_Click);
            // 
            // picImage
            // 
            this.picImage.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picImage.BackColor = System.Drawing.Color.Transparent;
            this.picImage.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.picImage.Location = new System.Drawing.Point(12, 12);
            this.picImage.Name = "picImage";
            this.picImage.Size = new System.Drawing.Size(734, 413);
            this.picImage.SizeMode = System.Windows.Forms.PictureBoxSizeMode.Zoom;
            this.picImage.TabIndex = 44;
            this.picImage.TabStop = false;
            // 
            // MainForm
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(758, 785);
            this.Controls.Add(this.btnSaveImage);
            this.Controls.Add(this.lblFilePath);
            this.Controls.Add(this.grbColorsInImage);
            this.Controls.Add(this.grbChangeColor);
            this.Controls.Add(this.btnBrowseImage);
            this.Controls.Add(this.picImage);
            this.MinimumSize = new System.Drawing.Size(774, 824);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TSN Color Switcher";
            this.grbColorsInImage.ResumeLayout(false);
            this.pnlSelectedColor.ResumeLayout(false);
            this.pnlSelectedColor.PerformLayout();
            this.grbChangeColor.ResumeLayout(false);
            this.grbChangeColor.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudNewColorA)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNewColorB)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNewColorG)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudNewColorR)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.picImage)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.TextBox txtSelectedColorR;
        private System.Windows.Forms.TextBox txtSelectedColorG;
        private System.Windows.Forms.TextBox txtSelectedColorB;
        private System.Windows.Forms.TextBox txtSelectedColorA;
        private System.Windows.Forms.TextBox txtSelectedColorHex;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.Label lblSelectedColor;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Button btnSaveImage;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
        private System.Windows.Forms.Label lblFilePath;
        private System.Windows.Forms.GroupBox grbColorsInImage;
        private System.Windows.Forms.Button btnClearSelectedImage;
        private System.Windows.Forms.Panel pnlSelectedColor;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Button btnOpenChangeColor;
        private System.Windows.Forms.ListBox lstColorsInImage;
        private System.Windows.Forms.ColorDialog colorDialog1;
        private System.Windows.Forms.GroupBox grbChangeColor;
        private System.Windows.Forms.NumericUpDown nudNewColorA;
        private System.Windows.Forms.NumericUpDown nudNewColorB;
        private System.Windows.Forms.NumericUpDown nudNewColorG;
        private System.Windows.Forms.NumericUpDown nudNewColorR;
        private System.Windows.Forms.Button btnCancelChangeColor;
        private System.Windows.Forms.Button btnChangeColor;
        private System.Windows.Forms.Button btnColorPicker;
        private System.Windows.Forms.TextBox txtNewColorHex;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Label lblNewColor;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.Button btnBrowseImage;
        private System.Windows.Forms.PictureBox picImage;
    }
}

