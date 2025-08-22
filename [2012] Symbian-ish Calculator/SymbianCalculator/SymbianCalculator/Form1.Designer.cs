namespace SymbianCalculator
{
    partial class Form1
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
            System.ComponentModel.ComponentResourceManager resources = new System.ComponentModel.ComponentResourceManager(typeof(Form1));
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.sonSonuçToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hafızaToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.kaydetToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.geçerliDeğerToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.işlemListesiToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.hafızadanAlToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.geçerliDeğerToolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.işlemListesiToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.temizleToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.geçerliDeğerToolStripMenuItem2 = new System.Windows.Forms.ToolStripMenuItem();
            this.toolStripMenuItem1 = new System.Windows.Forms.ToolStripMenuItem();
            this.xToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.pnlHesaplar = new System.Windows.Forms.Panel();
            this.lblSonuc13 = new System.Windows.Forms.Label();
            this.lblSonuc12 = new System.Windows.Forms.Label();
            this.lblSonuc11 = new System.Windows.Forms.Label();
            this.lblSonuc10 = new System.Windows.Forms.Label();
            this.lblSonuc9 = new System.Windows.Forms.Label();
            this.lblSonuc8 = new System.Windows.Forms.Label();
            this.lblSonuc7 = new System.Windows.Forms.Label();
            this.lblSonuc6 = new System.Windows.Forms.Label();
            this.lblSonuc5 = new System.Windows.Forms.Label();
            this.lblSonuc4 = new System.Windows.Forms.Label();
            this.lblSonuc3 = new System.Windows.Forms.Label();
            this.lblSonuc2 = new System.Windows.Forms.Label();
            this.lblSonuc1 = new System.Windows.Forms.Label();
            this.txtbSayi = new System.Windows.Forms.TextBox();
            this.btnArti = new System.Windows.Forms.Button();
            this.btnEksi = new System.Windows.Forms.Button();
            this.btnCarpi = new System.Windows.Forms.Button();
            this.btnBolu = new System.Windows.Forms.Button();
            this.btnArtiEksi = new System.Windows.Forms.Button();
            this.btnAsagi = new System.Windows.Forms.Button();
            this.btnYukari = new System.Windows.Forms.Button();
            this.btnYuzde = new System.Windows.Forms.Button();
            this.btnKareKok = new System.Windows.Forms.Button();
            this.btnEsittir = new System.Windows.Forms.Button();
            this.btnCikis = new System.Windows.Forms.Button();
            this.button1 = new System.Windows.Forms.Button();
            this.statusStrip1 = new System.Windows.Forms.StatusStrip();
            this.toolStripStatusLabel1 = new System.Windows.Forms.ToolStripStatusLabel();
            this.toolStripStatusLabel2 = new System.Windows.Forms.ToolStripStatusLabel();
            this.timer1 = new System.Windows.Forms.Timer(this.components);
            this.saveFileDialog1 = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog1 = new System.Windows.Forms.OpenFileDialog();
            this.menuStrip1.SuspendLayout();
            this.pnlHesaplar.SuspendLayout();
            this.statusStrip1.SuspendLayout();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.BackColor = System.Drawing.Color.OliveDrab;
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.sonSonuçToolStripMenuItem,
            this.hafızaToolStripMenuItem,
            this.toolStripMenuItem1,
            this.xToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(500, 24);
            this.menuStrip1.TabIndex = 11;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // sonSonuçToolStripMenuItem
            // 
            this.sonSonuçToolStripMenuItem.Font = new System.Drawing.Font("Lucida Sans", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.sonSonuçToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.sonSonuçToolStripMenuItem.Name = "sonSonuçToolStripMenuItem";
            this.sonSonuçToolStripMenuItem.Size = new System.Drawing.Size(83, 20);
            this.sonSonuçToolStripMenuItem.Text = "Son Sonuç";
            this.sonSonuçToolStripMenuItem.Click += new System.EventHandler(this.sonSonuçToolStripMenuItem_Click);
            this.sonSonuçToolStripMenuItem.MouseEnter += new System.EventHandler(this.sonSonuçToolStripMenuItem_MouseEnter);
            this.sonSonuçToolStripMenuItem.MouseLeave += new System.EventHandler(this.sonSonuçToolStripMenuItem_MouseLeave);
            // 
            // hafızaToolStripMenuItem
            // 
            this.hafızaToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.kaydetToolStripMenuItem,
            this.hafızadanAlToolStripMenuItem,
            this.temizleToolStripMenuItem});
            this.hafızaToolStripMenuItem.Font = new System.Drawing.Font("Lucida Sans", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.hafızaToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.hafızaToolStripMenuItem.Name = "hafızaToolStripMenuItem";
            this.hafızaToolStripMenuItem.Size = new System.Drawing.Size(58, 20);
            this.hafızaToolStripMenuItem.Text = "Hafıza";
            this.hafızaToolStripMenuItem.MouseEnter += new System.EventHandler(this.hafızaToolStripMenuItem_MouseEnter);
            this.hafızaToolStripMenuItem.MouseLeave += new System.EventHandler(this.hafızaToolStripMenuItem_MouseLeave);
            // 
            // kaydetToolStripMenuItem
            // 
            this.kaydetToolStripMenuItem.BackColor = System.Drawing.Color.ForestGreen;
            this.kaydetToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.geçerliDeğerToolStripMenuItem,
            this.işlemListesiToolStripMenuItem});
            this.kaydetToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.kaydetToolStripMenuItem.Name = "kaydetToolStripMenuItem";
            this.kaydetToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.kaydetToolStripMenuItem.Text = "Kaydet";
            this.kaydetToolStripMenuItem.MouseEnter += new System.EventHandler(this.kaydetToolStripMenuItem_MouseEnter);
            this.kaydetToolStripMenuItem.MouseLeave += new System.EventHandler(this.kaydetToolStripMenuItem_MouseLeave);
            // 
            // geçerliDeğerToolStripMenuItem
            // 
            this.geçerliDeğerToolStripMenuItem.BackColor = System.Drawing.Color.ForestGreen;
            this.geçerliDeğerToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.geçerliDeğerToolStripMenuItem.Name = "geçerliDeğerToolStripMenuItem";
            this.geçerliDeğerToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.geçerliDeğerToolStripMenuItem.Text = "Geçerli Değer";
            this.geçerliDeğerToolStripMenuItem.Click += new System.EventHandler(this.geçerliDeğerToolStripMenuItem_Click);
            // 
            // işlemListesiToolStripMenuItem
            // 
            this.işlemListesiToolStripMenuItem.BackColor = System.Drawing.Color.ForestGreen;
            this.işlemListesiToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.işlemListesiToolStripMenuItem.Name = "işlemListesiToolStripMenuItem";
            this.işlemListesiToolStripMenuItem.Size = new System.Drawing.Size(157, 22);
            this.işlemListesiToolStripMenuItem.Text = "İşlem Listesi";
            this.işlemListesiToolStripMenuItem.Click += new System.EventHandler(this.işlemListesiToolStripMenuItem_Click);
            // 
            // hafızadanAlToolStripMenuItem
            // 
            this.hafızadanAlToolStripMenuItem.BackColor = System.Drawing.Color.ForestGreen;
            this.hafızadanAlToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.geçerliDeğerToolStripMenuItem1,
            this.işlemListesiToolStripMenuItem2});
            this.hafızadanAlToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.hafızadanAlToolStripMenuItem.Name = "hafızadanAlToolStripMenuItem";
            this.hafızadanAlToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.hafızadanAlToolStripMenuItem.Text = "Hafızadan Al";
            this.hafızadanAlToolStripMenuItem.MouseEnter += new System.EventHandler(this.hafızadanAlToolStripMenuItem_MouseEnter);
            this.hafızadanAlToolStripMenuItem.MouseLeave += new System.EventHandler(this.hafızadanAlToolStripMenuItem_MouseLeave);
            // 
            // geçerliDeğerToolStripMenuItem1
            // 
            this.geçerliDeğerToolStripMenuItem1.BackColor = System.Drawing.Color.ForestGreen;
            this.geçerliDeğerToolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.geçerliDeğerToolStripMenuItem1.Name = "geçerliDeğerToolStripMenuItem1";
            this.geçerliDeğerToolStripMenuItem1.Size = new System.Drawing.Size(157, 22);
            this.geçerliDeğerToolStripMenuItem1.Text = "Geçerli Değer";
            this.geçerliDeğerToolStripMenuItem1.Click += new System.EventHandler(this.geçerliDeğerToolStripMenuItem1_Click);
            // 
            // işlemListesiToolStripMenuItem2
            // 
            this.işlemListesiToolStripMenuItem2.BackColor = System.Drawing.Color.ForestGreen;
            this.işlemListesiToolStripMenuItem2.ForeColor = System.Drawing.Color.White;
            this.işlemListesiToolStripMenuItem2.Name = "işlemListesiToolStripMenuItem2";
            this.işlemListesiToolStripMenuItem2.Size = new System.Drawing.Size(157, 22);
            this.işlemListesiToolStripMenuItem2.Text = "İşlem Listesi";
            this.işlemListesiToolStripMenuItem2.Click += new System.EventHandler(this.işlemListesiToolStripMenuItem2_Click);
            // 
            // temizleToolStripMenuItem
            // 
            this.temizleToolStripMenuItem.BackColor = System.Drawing.Color.ForestGreen;
            this.temizleToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.geçerliDeğerToolStripMenuItem2});
            this.temizleToolStripMenuItem.ForeColor = System.Drawing.Color.White;
            this.temizleToolStripMenuItem.Name = "temizleToolStripMenuItem";
            this.temizleToolStripMenuItem.Size = new System.Drawing.Size(153, 22);
            this.temizleToolStripMenuItem.Text = "Temizle";
            this.temizleToolStripMenuItem.MouseEnter += new System.EventHandler(this.temizleToolStripMenuItem_MouseEnter);
            this.temizleToolStripMenuItem.MouseLeave += new System.EventHandler(this.temizleToolStripMenuItem_MouseLeave);
            // 
            // geçerliDeğerToolStripMenuItem2
            // 
            this.geçerliDeğerToolStripMenuItem2.BackColor = System.Drawing.Color.ForestGreen;
            this.geçerliDeğerToolStripMenuItem2.ForeColor = System.Drawing.Color.White;
            this.geçerliDeğerToolStripMenuItem2.Name = "geçerliDeğerToolStripMenuItem2";
            this.geçerliDeğerToolStripMenuItem2.Size = new System.Drawing.Size(157, 22);
            this.geçerliDeğerToolStripMenuItem2.Text = "Geçerli Değer";
            this.geçerliDeğerToolStripMenuItem2.Click += new System.EventHandler(this.geçerliDeğerToolStripMenuItem2_Click);
            // 
            // toolStripMenuItem1
            // 
            this.toolStripMenuItem1.Font = new System.Drawing.Font("Lucida Sans", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.toolStripMenuItem1.ForeColor = System.Drawing.Color.White;
            this.toolStripMenuItem1.Name = "toolStripMenuItem1";
            this.toolStripMenuItem1.Size = new System.Drawing.Size(126, 20);
            this.toolStripMenuItem1.Text = "İşlemleri Temizle";
            this.toolStripMenuItem1.Click += new System.EventHandler(this.toolStripMenuItem1_Click);
            // 
            // xToolStripMenuItem
            // 
            this.xToolStripMenuItem.Alignment = System.Windows.Forms.ToolStripItemAlignment.Right;
            this.xToolStripMenuItem.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.xToolStripMenuItem.ForeColor = System.Drawing.Color.DarkOrange;
            this.xToolStripMenuItem.Name = "xToolStripMenuItem";
            this.xToolStripMenuItem.Size = new System.Drawing.Size(33, 20);
            this.xToolStripMenuItem.Text = " X ";
            this.xToolStripMenuItem.ToolTipText = "Çıkış";
            this.xToolStripMenuItem.Click += new System.EventHandler(this.xToolStripMenuItem_Click);
            this.xToolStripMenuItem.MouseEnter += new System.EventHandler(this.xToolStripMenuItem_MouseEnter);
            this.xToolStripMenuItem.MouseLeave += new System.EventHandler(this.xToolStripMenuItem_MouseLeave);
            // 
            // pnlHesaplar
            // 
            this.pnlHesaplar.BackColor = System.Drawing.Color.Salmon;
            this.pnlHesaplar.Controls.Add(this.lblSonuc13);
            this.pnlHesaplar.Controls.Add(this.lblSonuc12);
            this.pnlHesaplar.Controls.Add(this.lblSonuc11);
            this.pnlHesaplar.Controls.Add(this.lblSonuc10);
            this.pnlHesaplar.Controls.Add(this.lblSonuc9);
            this.pnlHesaplar.Controls.Add(this.lblSonuc8);
            this.pnlHesaplar.Controls.Add(this.lblSonuc7);
            this.pnlHesaplar.Controls.Add(this.lblSonuc6);
            this.pnlHesaplar.Controls.Add(this.lblSonuc5);
            this.pnlHesaplar.Controls.Add(this.lblSonuc4);
            this.pnlHesaplar.Controls.Add(this.lblSonuc3);
            this.pnlHesaplar.Controls.Add(this.lblSonuc2);
            this.pnlHesaplar.Controls.Add(this.lblSonuc1);
            this.pnlHesaplar.Location = new System.Drawing.Point(0, 24);
            this.pnlHesaplar.Name = "pnlHesaplar";
            this.pnlHesaplar.Size = new System.Drawing.Size(385, 269);
            this.pnlHesaplar.TabIndex = 12;
            // 
            // lblSonuc13
            // 
            this.lblSonuc13.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSonuc13.ForeColor = System.Drawing.Color.White;
            this.lblSonuc13.Location = new System.Drawing.Point(1, 244);
            this.lblSonuc13.Name = "lblSonuc13";
            this.lblSonuc13.Size = new System.Drawing.Size(383, 20);
            this.lblSonuc13.TabIndex = 12;
            this.lblSonuc13.Tag = "";
            this.lblSonuc13.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSonuc12
            // 
            this.lblSonuc12.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSonuc12.ForeColor = System.Drawing.Color.White;
            this.lblSonuc12.Location = new System.Drawing.Point(1, 224);
            this.lblSonuc12.Name = "lblSonuc12";
            this.lblSonuc12.Size = new System.Drawing.Size(383, 20);
            this.lblSonuc12.TabIndex = 11;
            this.lblSonuc12.Tag = "";
            this.lblSonuc12.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSonuc11
            // 
            this.lblSonuc11.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSonuc11.ForeColor = System.Drawing.Color.White;
            this.lblSonuc11.Location = new System.Drawing.Point(1, 204);
            this.lblSonuc11.Name = "lblSonuc11";
            this.lblSonuc11.Size = new System.Drawing.Size(383, 20);
            this.lblSonuc11.TabIndex = 10;
            this.lblSonuc11.Tag = "";
            this.lblSonuc11.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSonuc10
            // 
            this.lblSonuc10.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSonuc10.ForeColor = System.Drawing.Color.White;
            this.lblSonuc10.Location = new System.Drawing.Point(1, 184);
            this.lblSonuc10.Name = "lblSonuc10";
            this.lblSonuc10.Size = new System.Drawing.Size(383, 20);
            this.lblSonuc10.TabIndex = 9;
            this.lblSonuc10.Tag = "";
            this.lblSonuc10.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSonuc9
            // 
            this.lblSonuc9.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSonuc9.ForeColor = System.Drawing.Color.White;
            this.lblSonuc9.Location = new System.Drawing.Point(1, 164);
            this.lblSonuc9.Name = "lblSonuc9";
            this.lblSonuc9.Size = new System.Drawing.Size(383, 20);
            this.lblSonuc9.TabIndex = 8;
            this.lblSonuc9.Tag = "";
            this.lblSonuc9.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSonuc8
            // 
            this.lblSonuc8.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSonuc8.ForeColor = System.Drawing.Color.White;
            this.lblSonuc8.Location = new System.Drawing.Point(1, 144);
            this.lblSonuc8.Name = "lblSonuc8";
            this.lblSonuc8.Size = new System.Drawing.Size(383, 20);
            this.lblSonuc8.TabIndex = 7;
            this.lblSonuc8.Tag = "";
            this.lblSonuc8.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSonuc7
            // 
            this.lblSonuc7.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSonuc7.ForeColor = System.Drawing.Color.White;
            this.lblSonuc7.Location = new System.Drawing.Point(1, 124);
            this.lblSonuc7.Name = "lblSonuc7";
            this.lblSonuc7.Size = new System.Drawing.Size(383, 20);
            this.lblSonuc7.TabIndex = 6;
            this.lblSonuc7.Tag = "";
            this.lblSonuc7.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSonuc6
            // 
            this.lblSonuc6.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSonuc6.ForeColor = System.Drawing.Color.White;
            this.lblSonuc6.Location = new System.Drawing.Point(1, 104);
            this.lblSonuc6.Name = "lblSonuc6";
            this.lblSonuc6.Size = new System.Drawing.Size(383, 20);
            this.lblSonuc6.TabIndex = 5;
            this.lblSonuc6.Tag = "";
            this.lblSonuc6.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSonuc5
            // 
            this.lblSonuc5.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSonuc5.ForeColor = System.Drawing.Color.White;
            this.lblSonuc5.Location = new System.Drawing.Point(1, 84);
            this.lblSonuc5.Name = "lblSonuc5";
            this.lblSonuc5.Size = new System.Drawing.Size(383, 20);
            this.lblSonuc5.TabIndex = 4;
            this.lblSonuc5.Tag = "";
            this.lblSonuc5.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSonuc4
            // 
            this.lblSonuc4.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSonuc4.ForeColor = System.Drawing.Color.White;
            this.lblSonuc4.Location = new System.Drawing.Point(1, 64);
            this.lblSonuc4.Name = "lblSonuc4";
            this.lblSonuc4.Size = new System.Drawing.Size(383, 20);
            this.lblSonuc4.TabIndex = 3;
            this.lblSonuc4.Tag = "";
            this.lblSonuc4.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSonuc3
            // 
            this.lblSonuc3.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSonuc3.ForeColor = System.Drawing.Color.White;
            this.lblSonuc3.Location = new System.Drawing.Point(1, 43);
            this.lblSonuc3.Name = "lblSonuc3";
            this.lblSonuc3.Size = new System.Drawing.Size(383, 20);
            this.lblSonuc3.TabIndex = 2;
            this.lblSonuc3.Tag = "";
            this.lblSonuc3.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSonuc2
            // 
            this.lblSonuc2.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSonuc2.ForeColor = System.Drawing.Color.White;
            this.lblSonuc2.Location = new System.Drawing.Point(1, 24);
            this.lblSonuc2.Name = "lblSonuc2";
            this.lblSonuc2.Size = new System.Drawing.Size(383, 20);
            this.lblSonuc2.TabIndex = 1;
            this.lblSonuc2.Tag = "";
            this.lblSonuc2.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // lblSonuc1
            // 
            this.lblSonuc1.Font = new System.Drawing.Font("Microsoft Sans Serif", 6.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.lblSonuc1.ForeColor = System.Drawing.Color.White;
            this.lblSonuc1.Location = new System.Drawing.Point(1, 4);
            this.lblSonuc1.Name = "lblSonuc1";
            this.lblSonuc1.Size = new System.Drawing.Size(383, 20);
            this.lblSonuc1.TabIndex = 0;
            this.lblSonuc1.Tag = "";
            this.lblSonuc1.TextAlign = System.Drawing.ContentAlignment.MiddleRight;
            // 
            // txtbSayi
            // 
            this.txtbSayi.BackColor = System.Drawing.Color.MediumPurple;
            this.txtbSayi.BorderStyle = System.Windows.Forms.BorderStyle.None;
            this.txtbSayi.Font = new System.Drawing.Font("Microsoft Sans Serif", 14.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.txtbSayi.ForeColor = System.Drawing.Color.White;
            this.txtbSayi.Location = new System.Drawing.Point(0, 294);
            this.txtbSayi.Multiline = true;
            this.txtbSayi.Name = "txtbSayi";
            this.txtbSayi.Size = new System.Drawing.Size(500, 25);
            this.txtbSayi.TabIndex = 0;
            this.txtbSayi.TextAlign = System.Windows.Forms.HorizontalAlignment.Right;
            this.txtbSayi.TextChanged += new System.EventHandler(this.txtbSayi_TextChanged);
            // 
            // btnArti
            // 
            this.btnArti.BackColor = System.Drawing.Color.White;
            this.btnArti.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnArti.ForeColor = System.Drawing.Color.DimGray;
            this.btnArti.Location = new System.Drawing.Point(392, 27);
            this.btnArti.Name = "btnArti";
            this.btnArti.Size = new System.Drawing.Size(45, 45);
            this.btnArti.TabIndex = 1;
            this.btnArti.Text = "+";
            this.btnArti.UseVisualStyleBackColor = false;
            this.btnArti.Click += new System.EventHandler(this.btnArti_Click);
            // 
            // btnEksi
            // 
            this.btnEksi.BackColor = System.Drawing.Color.White;
            this.btnEksi.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnEksi.ForeColor = System.Drawing.Color.DimGray;
            this.btnEksi.Location = new System.Drawing.Point(392, 78);
            this.btnEksi.Name = "btnEksi";
            this.btnEksi.Size = new System.Drawing.Size(45, 45);
            this.btnEksi.TabIndex = 2;
            this.btnEksi.Text = "−";
            this.btnEksi.UseVisualStyleBackColor = false;
            this.btnEksi.Click += new System.EventHandler(this.btnEksi_Click);
            // 
            // btnCarpi
            // 
            this.btnCarpi.BackColor = System.Drawing.Color.White;
            this.btnCarpi.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCarpi.ForeColor = System.Drawing.Color.DimGray;
            this.btnCarpi.Location = new System.Drawing.Point(392, 129);
            this.btnCarpi.Name = "btnCarpi";
            this.btnCarpi.Size = new System.Drawing.Size(45, 45);
            this.btnCarpi.TabIndex = 3;
            this.btnCarpi.Text = "×";
            this.btnCarpi.UseVisualStyleBackColor = false;
            this.btnCarpi.Click += new System.EventHandler(this.btnCarpi_Click);
            // 
            // btnBolu
            // 
            this.btnBolu.BackColor = System.Drawing.Color.White;
            this.btnBolu.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnBolu.ForeColor = System.Drawing.Color.DimGray;
            this.btnBolu.Location = new System.Drawing.Point(392, 180);
            this.btnBolu.Name = "btnBolu";
            this.btnBolu.Size = new System.Drawing.Size(45, 45);
            this.btnBolu.TabIndex = 4;
            this.btnBolu.Text = "÷";
            this.btnBolu.UseVisualStyleBackColor = false;
            this.btnBolu.Click += new System.EventHandler(this.btnBolu_Click);
            // 
            // btnArtiEksi
            // 
            this.btnArtiEksi.BackColor = System.Drawing.Color.White;
            this.btnArtiEksi.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnArtiEksi.ForeColor = System.Drawing.Color.DimGray;
            this.btnArtiEksi.Location = new System.Drawing.Point(392, 231);
            this.btnArtiEksi.Name = "btnArtiEksi";
            this.btnArtiEksi.Size = new System.Drawing.Size(45, 45);
            this.btnArtiEksi.TabIndex = 5;
            this.btnArtiEksi.Text = "±";
            this.btnArtiEksi.UseVisualStyleBackColor = false;
            this.btnArtiEksi.Click += new System.EventHandler(this.btnArtiEksi_Click);
            // 
            // btnAsagi
            // 
            this.btnAsagi.BackColor = System.Drawing.Color.White;
            this.btnAsagi.Enabled = false;
            this.btnAsagi.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnAsagi.ForeColor = System.Drawing.Color.DimGray;
            this.btnAsagi.Location = new System.Drawing.Point(443, 231);
            this.btnAsagi.Name = "btnAsagi";
            this.btnAsagi.Size = new System.Drawing.Size(45, 45);
            this.btnAsagi.TabIndex = 10;
            this.btnAsagi.Text = "»";
            this.btnAsagi.UseVisualStyleBackColor = false;
            this.btnAsagi.Click += new System.EventHandler(this.btnAsagi_Click);
            // 
            // btnYukari
            // 
            this.btnYukari.BackColor = System.Drawing.Color.White;
            this.btnYukari.Enabled = false;
            this.btnYukari.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYukari.ForeColor = System.Drawing.Color.DimGray;
            this.btnYukari.Location = new System.Drawing.Point(443, 180);
            this.btnYukari.Name = "btnYukari";
            this.btnYukari.Size = new System.Drawing.Size(45, 45);
            this.btnYukari.TabIndex = 9;
            this.btnYukari.Text = "«";
            this.btnYukari.UseVisualStyleBackColor = false;
            this.btnYukari.Click += new System.EventHandler(this.btnYukari_Click);
            // 
            // btnYuzde
            // 
            this.btnYuzde.BackColor = System.Drawing.Color.White;
            this.btnYuzde.Enabled = false;
            this.btnYuzde.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnYuzde.ForeColor = System.Drawing.Color.DimGray;
            this.btnYuzde.Location = new System.Drawing.Point(443, 129);
            this.btnYuzde.Name = "btnYuzde";
            this.btnYuzde.Size = new System.Drawing.Size(45, 45);
            this.btnYuzde.TabIndex = 8;
            this.btnYuzde.Text = "%";
            this.btnYuzde.UseVisualStyleBackColor = false;
            this.btnYuzde.Click += new System.EventHandler(this.btnYuzde_Click);
            // 
            // btnKareKok
            // 
            this.btnKareKok.BackColor = System.Drawing.Color.White;
            this.btnKareKok.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnKareKok.ForeColor = System.Drawing.Color.DimGray;
            this.btnKareKok.Location = new System.Drawing.Point(443, 78);
            this.btnKareKok.Name = "btnKareKok";
            this.btnKareKok.Size = new System.Drawing.Size(45, 45);
            this.btnKareKok.TabIndex = 7;
            this.btnKareKok.Text = "√";
            this.btnKareKok.UseVisualStyleBackColor = false;
            this.btnKareKok.Click += new System.EventHandler(this.btnKareKok_Click);
            // 
            // btnEsittir
            // 
            this.btnEsittir.BackColor = System.Drawing.Color.White;
            this.btnEsittir.Font = new System.Drawing.Font("Comic Sans MS", 20.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnEsittir.ForeColor = System.Drawing.Color.DimGray;
            this.btnEsittir.Location = new System.Drawing.Point(443, 27);
            this.btnEsittir.Name = "btnEsittir";
            this.btnEsittir.Size = new System.Drawing.Size(45, 45);
            this.btnEsittir.TabIndex = 6;
            this.btnEsittir.Text = "=";
            this.btnEsittir.UseVisualStyleBackColor = false;
            this.btnEsittir.Click += new System.EventHandler(this.btnEsittir_Click);
            // 
            // btnCikis
            // 
            this.btnCikis.Cursor = System.Windows.Forms.Cursors.Hand;
            this.btnCikis.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.btnCikis.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnCikis.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.btnCikis.Location = new System.Drawing.Point(443, 320);
            this.btnCikis.Name = "btnCikis";
            this.btnCikis.Size = new System.Drawing.Size(57, 23);
            this.btnCikis.TabIndex = 14;
            this.btnCikis.Text = "Çıkış";
            this.btnCikis.UseVisualStyleBackColor = true;
            this.btnCikis.Click += new System.EventHandler(this.button10_Click);
            // 
            // button1
            // 
            this.button1.Cursor = System.Windows.Forms.Cursors.Hand;
            this.button1.FlatStyle = System.Windows.Forms.FlatStyle.Flat;
            this.button1.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.button1.ForeColor = System.Drawing.SystemColors.ControlLight;
            this.button1.Location = new System.Drawing.Point(0, 320);
            this.button1.Name = "button1";
            this.button1.Size = new System.Drawing.Size(60, 23);
            this.button1.TabIndex = 13;
            this.button1.Text = "Temizle";
            this.button1.UseVisualStyleBackColor = true;
            this.button1.Click += new System.EventHandler(this.button1_Click);
            // 
            // statusStrip1
            // 
            this.statusStrip1.BackColor = System.Drawing.SystemColors.Control;
            this.statusStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.toolStripStatusLabel1,
            this.toolStripStatusLabel2});
            this.statusStrip1.Location = new System.Drawing.Point(0, 344);
            this.statusStrip1.Name = "statusStrip1";
            this.statusStrip1.Size = new System.Drawing.Size(500, 22);
            this.statusStrip1.SizingGrip = false;
            this.statusStrip1.TabIndex = 15;
            this.statusStrip1.Text = "statusStrip1";
            // 
            // toolStripStatusLabel1
            // 
            this.toolStripStatusLabel1.Font = new System.Drawing.Font("Segoe UI", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.toolStripStatusLabel1.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel1.Name = "toolStripStatusLabel1";
            this.toolStripStatusLabel1.Size = new System.Drawing.Size(192, 17);
            this.toolStripStatusLabel1.Text = "Copyright Mustafa Tosun ©  2012";
            // 
            // toolStripStatusLabel2
            // 
            this.toolStripStatusLabel2.ForeColor = System.Drawing.Color.Black;
            this.toolStripStatusLabel2.Name = "toolStripStatusLabel2";
            this.toolStripStatusLabel2.Size = new System.Drawing.Size(293, 17);
            this.toolStripStatusLabel2.Spring = true;
            this.toolStripStatusLabel2.Text = "www.mustafatosun.net";
            // 
            // timer1
            // 
            this.timer1.Tick += new System.EventHandler(this.timer1_Tick);
            // 
            // saveFileDialog1
            // 
            this.saveFileDialog1.DefaultExt = "csv";
            this.saveFileDialog1.FileName = "İşlem Dosyası.csv";
            this.saveFileDialog1.Filter = "İşlem Dosyaları|*.csv";
            this.saveFileDialog1.Title = "İşlem Dosyası Kaydet";
            // 
            // openFileDialog1
            // 
            this.openFileDialog1.DefaultExt = "csv";
            this.openFileDialog1.Filter = "İşlem Dosyaları|*.csv";
            this.openFileDialog1.Title = "İşlem Kaydı Al";
            // 
            // Form1
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.BackColor = System.Drawing.Color.DarkSeaGreen;
            this.ClientSize = new System.Drawing.Size(500, 366);
            this.Controls.Add(this.statusStrip1);
            this.Controls.Add(this.button1);
            this.Controls.Add(this.btnCikis);
            this.Controls.Add(this.btnAsagi);
            this.Controls.Add(this.btnYukari);
            this.Controls.Add(this.btnYuzde);
            this.Controls.Add(this.btnKareKok);
            this.Controls.Add(this.btnEsittir);
            this.Controls.Add(this.btnArtiEksi);
            this.Controls.Add(this.btnBolu);
            this.Controls.Add(this.btnCarpi);
            this.Controls.Add(this.btnEksi);
            this.Controls.Add(this.btnArti);
            this.Controls.Add(this.txtbSayi);
            this.Controls.Add(this.pnlHesaplar);
            this.Controls.Add(this.menuStrip1);
            this.ForeColor = System.Drawing.Color.White;
            this.FormBorderStyle = System.Windows.Forms.FormBorderStyle.None;
            this.Icon = ((System.Drawing.Icon)(resources.GetObject("$this.Icon")));
            this.MainMenuStrip = this.menuStrip1;
            this.MaximizeBox = false;
            this.MaximumSize = new System.Drawing.Size(500, 366);
            this.MinimizeBox = false;
            this.MinimumSize = new System.Drawing.Size(500, 366);
            this.Name = "Form1";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "CalculatorTSN";
            this.FormClosing += new System.Windows.Forms.FormClosingEventHandler(this.Form1_FormClosing);
            this.Load += new System.EventHandler(this.Form1_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            this.pnlHesaplar.ResumeLayout(false);
            this.statusStrip1.ResumeLayout(false);
            this.statusStrip1.PerformLayout();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem sonSonuçToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hafızaToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem kaydetToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem hafızadanAlToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem temizleToolStripMenuItem;
        private System.Windows.Forms.Panel pnlHesaplar;
        private System.Windows.Forms.TextBox txtbSayi;
        private System.Windows.Forms.Button btnArti;
        private System.Windows.Forms.Button btnEksi;
        private System.Windows.Forms.Button btnCarpi;
        private System.Windows.Forms.Button btnBolu;
        private System.Windows.Forms.Button btnArtiEksi;
        private System.Windows.Forms.Button btnAsagi;
        private System.Windows.Forms.Button btnYukari;
        private System.Windows.Forms.Button btnYuzde;
        private System.Windows.Forms.Button btnKareKok;
        private System.Windows.Forms.Button btnEsittir;
        private System.Windows.Forms.Button btnCikis;
        private System.Windows.Forms.Label lblSonuc12;
        private System.Windows.Forms.Label lblSonuc11;
        private System.Windows.Forms.Label lblSonuc10;
        private System.Windows.Forms.Label lblSonuc9;
        private System.Windows.Forms.Label lblSonuc8;
        private System.Windows.Forms.Label lblSonuc7;
        private System.Windows.Forms.Label lblSonuc6;
        private System.Windows.Forms.Label lblSonuc5;
        private System.Windows.Forms.Label lblSonuc4;
        private System.Windows.Forms.Label lblSonuc3;
        private System.Windows.Forms.Label lblSonuc2;
        private System.Windows.Forms.Label lblSonuc1;
        private System.Windows.Forms.Label lblSonuc13;
        private System.Windows.Forms.ToolStripMenuItem toolStripMenuItem1;
        private System.Windows.Forms.Button button1;
        private System.Windows.Forms.ToolStripMenuItem xToolStripMenuItem;
        private System.Windows.Forms.StatusStrip statusStrip1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel1;
        private System.Windows.Forms.ToolStripStatusLabel toolStripStatusLabel2;
        private System.Windows.Forms.Timer timer1;
        private System.Windows.Forms.ToolStripMenuItem geçerliDeğerToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem işlemListesiToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem geçerliDeğerToolStripMenuItem1;
        private System.Windows.Forms.ToolStripMenuItem işlemListesiToolStripMenuItem2;
        private System.Windows.Forms.ToolStripMenuItem geçerliDeğerToolStripMenuItem2;
        private System.Windows.Forms.SaveFileDialog saveFileDialog1;
        private System.Windows.Forms.OpenFileDialog openFileDialog1;
    }
}

