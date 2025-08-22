namespace TSN.Universe.DesktopApp
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
            this.components = new System.ComponentModel.Container();
            this.picCurrentUniverse = new System.Windows.Forms.PictureBox();
            this.label1 = new System.Windows.Forms.Label();
            this.pnlUniverse = new System.Windows.Forms.Panel();
            this.nudSleep = new System.Windows.Forms.NumericUpDown();
            this.label15 = new System.Windows.Forms.Label();
            this.nudN = new System.Windows.Forms.NumericUpDown();
            this.nudM = new System.Windows.Forms.NumericUpDown();
            this.label2 = new System.Windows.Forms.Label();
            this.label3 = new System.Windows.Forms.Label();
            this.bgwSimulate = new System.ComponentModel.BackgroundWorker();
            this.cmsImport = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiImportUniverse = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiImportParameters = new System.Windows.Forms.ToolStripMenuItem();
            this.cmsExport = new System.Windows.Forms.ContextMenuStrip(this.components);
            this.tsmiExportUniverse = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExportParameters = new System.Windows.Forms.ToolStripMenuItem();
            this.tsmiExportCanvas = new System.Windows.Forms.ToolStripMenuItem();
            this.nudSimulationCount = new System.Windows.Forms.NumericUpDown();
            this.btnSimulate = new System.Windows.Forms.Button();
            this.label8 = new System.Windows.Forms.Label();
            this.grSimulationParameters = new System.Windows.Forms.GroupBox();
            this.panel1 = new System.Windows.Forms.Panel();
            this.chkIgnoreFoods = new System.Windows.Forms.CheckBox();
            this.chkIgnoreThings = new System.Windows.Forms.CheckBox();
            this.label14 = new System.Windows.Forms.Label();
            this.panel5 = new System.Windows.Forms.Panel();
            this.chkReplace_ft = new System.Windows.Forms.CheckBox();
            this.chkEatFood = new System.Windows.Forms.CheckBox();
            this.chkReplace_tf = new System.Windows.Forms.CheckBox();
            this.chkReplace_tt = new System.Windows.Forms.CheckBox();
            this.label13 = new System.Windows.Forms.Label();
            this.panel3 = new System.Windows.Forms.Panel();
            this.chkReturnFood = new System.Windows.Forms.CheckBox();
            this.chkReturnFood_ifNotHungry = new System.Windows.Forms.CheckBox();
            this.chkMove = new System.Windows.Forms.CheckBox();
            this.chkReproduce_multi = new System.Windows.Forms.CheckBox();
            this.chkReproduce_ifHungry = new System.Windows.Forms.CheckBox();
            this.chkReproduce = new System.Windows.Forms.CheckBox();
            this.chkDieIfHungry = new System.Windows.Forms.CheckBox();
            this.chkKillFoods = new System.Windows.Forms.CheckBox();
            this.chkSpawnFoods = new System.Windows.Forms.CheckBox();
            this.chkKillThings = new System.Windows.Forms.CheckBox();
            this.chkSpawnThings = new System.Windows.Forms.CheckBox();
            this.panel2 = new System.Windows.Forms.Panel();
            this.radSpawnBothSame = new System.Windows.Forms.RadioButton();
            this.radSpawnFoodsFirst = new System.Windows.Forms.RadioButton();
            this.radSpawnThingsFirst = new System.Windows.Forms.RadioButton();
            this.nudSense_max = new System.Windows.Forms.NumericUpDown();
            this.nudSense_min = new System.Windows.Forms.NumericUpDown();
            this.nudDeathRate_max = new System.Windows.Forms.NumericUpDown();
            this.nudDeathRate_min = new System.Windows.Forms.NumericUpDown();
            this.nudReproduceRate_max = new System.Windows.Forms.NumericUpDown();
            this.nudReproduceRate_min = new System.Windows.Forms.NumericUpDown();
            this.nudSpawnRate_Food = new System.Windows.Forms.NumericUpDown();
            this.nudSpawnRate_Thing = new System.Windows.Forms.NumericUpDown();
            this.label11 = new System.Windows.Forms.Label();
            this.label12 = new System.Windows.Forms.Label();
            this.label10 = new System.Windows.Forms.Label();
            this.label9 = new System.Windows.Forms.Label();
            this.label7 = new System.Windows.Forms.Label();
            this.label6 = new System.Windows.Forms.Label();
            this.label5 = new System.Windows.Forms.Label();
            this.label4 = new System.Windows.Forms.Label();
            this.btnClear = new System.Windows.Forms.Button();
            this.nudGeneration = new System.Windows.Forms.NumericUpDown();
            this.btnStatistics = new System.Windows.Forms.Button();
            this.label16 = new System.Windows.Forms.Label();
            this.ttAlgorithm = new System.Windows.Forms.ToolTip(this.components);
            this.saveFileDialog = new System.Windows.Forms.SaveFileDialog();
            this.openFileDialog = new System.Windows.Forms.OpenFileDialog();
            this.folderBrowserDialog = new System.Windows.Forms.FolderBrowserDialog();
            this.btnParametersClear = new System.Windows.Forms.Button();
            this.btnParametersDefault = new System.Windows.Forms.Button();
            this.progressbar = new System.Windows.Forms.ProgressBar();
            this.btnExport = new TSN.Universe.DesktopApp.DropDownButton();
            this.btnImport = new TSN.Universe.DesktopApp.DropDownButton();
            ((System.ComponentModel.ISupportInitialize)(this.picCurrentUniverse)).BeginInit();
            this.pnlUniverse.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSleep)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudN)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudM)).BeginInit();
            this.cmsImport.SuspendLayout();
            this.cmsExport.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSimulationCount)).BeginInit();
            this.grSimulationParameters.SuspendLayout();
            this.panel1.SuspendLayout();
            this.panel5.SuspendLayout();
            this.panel3.SuspendLayout();
            this.panel2.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSense_max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSense_min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeathRate_max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeathRate_min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudReproduceRate_max)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudReproduceRate_min)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpawnRate_Food)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpawnRate_Thing)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGeneration)).BeginInit();
            this.SuspendLayout();
            // 
            // picCurrentUniverse
            // 
            this.picCurrentUniverse.Anchor = ((System.Windows.Forms.AnchorStyles)((((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Bottom) 
            | System.Windows.Forms.AnchorStyles.Left) 
            | System.Windows.Forms.AnchorStyles.Right)));
            this.picCurrentUniverse.BackColor = System.Drawing.Color.White;
            this.picCurrentUniverse.Location = new System.Drawing.Point(12, 12);
            this.picCurrentUniverse.Name = "picCurrentUniverse";
            this.picCurrentUniverse.Size = new System.Drawing.Size(800, 755);
            this.picCurrentUniverse.SizeMode = System.Windows.Forms.PictureBoxSizeMode.StretchImage;
            this.picCurrentUniverse.TabIndex = 0;
            this.picCurrentUniverse.TabStop = false;
            // 
            // label1
            // 
            this.label1.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.label1.Font = new System.Drawing.Font("Segoe UI", 12F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label1.ForeColor = System.Drawing.Color.SandyBrown;
            this.label1.Location = new System.Drawing.Point(818, 12);
            this.label1.Name = "label1";
            this.label1.Size = new System.Drawing.Size(319, 50);
            this.label1.TabIndex = 0;
            this.label1.Text = "Universe";
            this.label1.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // pnlUniverse
            // 
            this.pnlUniverse.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.pnlUniverse.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.pnlUniverse.Controls.Add(this.nudSleep);
            this.pnlUniverse.Controls.Add(this.label15);
            this.pnlUniverse.Controls.Add(this.nudN);
            this.pnlUniverse.Controls.Add(this.nudM);
            this.pnlUniverse.Controls.Add(this.label2);
            this.pnlUniverse.Controls.Add(this.label3);
            this.pnlUniverse.Location = new System.Drawing.Point(818, 65);
            this.pnlUniverse.Name = "pnlUniverse";
            this.pnlUniverse.Size = new System.Drawing.Size(319, 86);
            this.pnlUniverse.TabIndex = 1;
            // 
            // nudSleep
            // 
            this.nudSleep.Location = new System.Drawing.Point(162, 58);
            this.nudSleep.Maximum = new decimal(new int[] {
            3600000,
            0,
            0,
            0});
            this.nudSleep.Name = "nudSleep";
            this.nudSleep.Size = new System.Drawing.Size(150, 20);
            this.nudSleep.TabIndex = 5;
            this.nudSleep.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label15
            // 
            this.label15.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label15.Location = new System.Drawing.Point(6, 54);
            this.label15.Name = "label15";
            this.label15.Size = new System.Drawing.Size(150, 25);
            this.label15.TabIndex = 4;
            this.label15.Text = "Sleep Time (ms)";
            this.label15.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // nudN
            // 
            this.nudN.Location = new System.Drawing.Point(162, 32);
            this.nudN.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudN.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudN.Name = "nudN";
            this.nudN.Size = new System.Drawing.Size(150, 20);
            this.nudN.TabIndex = 3;
            this.nudN.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudN.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // nudM
            // 
            this.nudM.Location = new System.Drawing.Point(162, 7);
            this.nudM.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudM.Minimum = new decimal(new int[] {
            4,
            0,
            0,
            0});
            this.nudM.Name = "nudM";
            this.nudM.Size = new System.Drawing.Size(150, 20);
            this.nudM.TabIndex = 1;
            this.nudM.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudM.Value = new decimal(new int[] {
            20,
            0,
            0,
            0});
            // 
            // label2
            // 
            this.label2.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label2.Location = new System.Drawing.Point(6, 3);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(150, 25);
            this.label2.TabIndex = 0;
            this.label2.Text = "M";
            this.label2.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label3
            // 
            this.label3.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label3.Location = new System.Drawing.Point(6, 28);
            this.label3.Name = "label3";
            this.label3.Size = new System.Drawing.Size(150, 25);
            this.label3.TabIndex = 2;
            this.label3.Text = "N";
            this.label3.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // bgwSimulate
            // 
            this.bgwSimulate.WorkerReportsProgress = true;
            this.bgwSimulate.WorkerSupportsCancellation = true;
            this.bgwSimulate.DoWork += new System.ComponentModel.DoWorkEventHandler(this.bgwSimulate_DoWork);
            this.bgwSimulate.ProgressChanged += new System.ComponentModel.ProgressChangedEventHandler(this.bgwSimulate_ProgressChanged);
            this.bgwSimulate.RunWorkerCompleted += new System.ComponentModel.RunWorkerCompletedEventHandler(this.bgwSimulate_RunWorkerCompleted);
            // 
            // cmsImport
            // 
            this.cmsImport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiImportUniverse,
            this.tsmiImportParameters});
            this.cmsImport.Name = "cmsImport";
            this.cmsImport.Size = new System.Drawing.Size(242, 48);
            // 
            // tsmiImportUniverse
            // 
            this.tsmiImportUniverse.Name = "tsmiImportUniverse";
            this.tsmiImportUniverse.Size = new System.Drawing.Size(241, 22);
            this.tsmiImportUniverse.Text = "Import Universe...";
            this.tsmiImportUniverse.Click += new System.EventHandler(this.tsmiImportUniverse_Click);
            // 
            // tsmiImportParameters
            // 
            this.tsmiImportParameters.Name = "tsmiImportParameters";
            this.tsmiImportParameters.Size = new System.Drawing.Size(241, 22);
            this.tsmiImportParameters.Text = "Import Simulation Parameters...";
            this.tsmiImportParameters.Click += new System.EventHandler(this.tsmiImportParameters_Click);
            // 
            // cmsExport
            // 
            this.cmsExport.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.tsmiExportUniverse,
            this.tsmiExportParameters,
            this.tsmiExportCanvas});
            this.cmsExport.Name = "cmsExport";
            this.cmsExport.Size = new System.Drawing.Size(269, 70);
            // 
            // tsmiExportUniverse
            // 
            this.tsmiExportUniverse.Name = "tsmiExportUniverse";
            this.tsmiExportUniverse.Size = new System.Drawing.Size(268, 22);
            this.tsmiExportUniverse.Text = "Export Universe...";
            this.tsmiExportUniverse.Click += new System.EventHandler(this.tsmiExportUniverse_Click);
            // 
            // tsmiExportParameters
            // 
            this.tsmiExportParameters.Name = "tsmiExportParameters";
            this.tsmiExportParameters.Size = new System.Drawing.Size(268, 22);
            this.tsmiExportParameters.Text = "Export Simulation Parameters...";
            this.tsmiExportParameters.Click += new System.EventHandler(this.tsmiExportParameters_Click);
            // 
            // tsmiExportCanvas
            // 
            this.tsmiExportCanvas.Name = "tsmiExportCanvas";
            this.tsmiExportCanvas.Size = new System.Drawing.Size(268, 22);
            this.tsmiExportCanvas.Text = "Export Simulation Canvas as Image...";
            this.tsmiExportCanvas.Click += new System.EventHandler(this.tsmiExportCanvas_Click);
            // 
            // nudSimulationCount
            // 
            this.nudSimulationCount.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.nudSimulationCount.Location = new System.Drawing.Point(818, 711);
            this.nudSimulationCount.Maximum = new decimal(new int[] {
            65535,
            0,
            0,
            0});
            this.nudSimulationCount.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSimulationCount.Name = "nudSimulationCount";
            this.nudSimulationCount.Size = new System.Drawing.Size(156, 20);
            this.nudSimulationCount.TabIndex = 4;
            this.nudSimulationCount.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSimulationCount.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // btnSimulate
            // 
            this.btnSimulate.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnSimulate.Location = new System.Drawing.Point(981, 693);
            this.btnSimulate.Name = "btnSimulate";
            this.btnSimulate.Size = new System.Drawing.Size(156, 38);
            this.btnSimulate.TabIndex = 5;
            this.btnSimulate.UseVisualStyleBackColor = true;
            this.btnSimulate.Click += new System.EventHandler(this.btnSimulate_Click);
            // 
            // label8
            // 
            this.label8.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.label8.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label8.Location = new System.Drawing.Point(818, 693);
            this.label8.Name = "label8";
            this.label8.Size = new System.Drawing.Size(156, 15);
            this.label8.TabIndex = 3;
            this.label8.Text = "Simulation Count";
            this.label8.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // grSimulationParameters
            // 
            this.grSimulationParameters.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.grSimulationParameters.Controls.Add(this.panel1);
            this.grSimulationParameters.Controls.Add(this.panel5);
            this.grSimulationParameters.Controls.Add(this.panel3);
            this.grSimulationParameters.Controls.Add(this.panel2);
            this.grSimulationParameters.Controls.Add(this.nudSense_max);
            this.grSimulationParameters.Controls.Add(this.nudSense_min);
            this.grSimulationParameters.Controls.Add(this.nudDeathRate_max);
            this.grSimulationParameters.Controls.Add(this.nudDeathRate_min);
            this.grSimulationParameters.Controls.Add(this.nudReproduceRate_max);
            this.grSimulationParameters.Controls.Add(this.nudReproduceRate_min);
            this.grSimulationParameters.Controls.Add(this.nudSpawnRate_Food);
            this.grSimulationParameters.Controls.Add(this.nudSpawnRate_Thing);
            this.grSimulationParameters.Controls.Add(this.label11);
            this.grSimulationParameters.Controls.Add(this.label12);
            this.grSimulationParameters.Controls.Add(this.label10);
            this.grSimulationParameters.Controls.Add(this.label9);
            this.grSimulationParameters.Controls.Add(this.label7);
            this.grSimulationParameters.Controls.Add(this.label6);
            this.grSimulationParameters.Controls.Add(this.label5);
            this.grSimulationParameters.Controls.Add(this.label4);
            this.grSimulationParameters.Location = new System.Drawing.Point(818, 157);
            this.grSimulationParameters.Name = "grSimulationParameters";
            this.grSimulationParameters.Size = new System.Drawing.Size(319, 530);
            this.grSimulationParameters.TabIndex = 2;
            this.grSimulationParameters.TabStop = false;
            this.grSimulationParameters.Text = "Simulation Parameters";
            // 
            // panel1
            // 
            this.panel1.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel1.Controls.Add(this.chkIgnoreFoods);
            this.panel1.Controls.Add(this.chkIgnoreThings);
            this.panel1.Controls.Add(this.label14);
            this.panel1.Location = new System.Drawing.Point(7, 445);
            this.panel1.Name = "panel1";
            this.panel1.Size = new System.Drawing.Size(150, 79);
            this.panel1.TabIndex = 18;
            // 
            // chkIgnoreFoods
            // 
            this.chkIgnoreFoods.AutoSize = true;
            this.chkIgnoreFoods.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkIgnoreFoods.Location = new System.Drawing.Point(4, 56);
            this.chkIgnoreFoods.Name = "chkIgnoreFoods";
            this.chkIgnoreFoods.Size = new System.Drawing.Size(124, 17);
            this.chkIgnoreFoods.TabIndex = 2;
            this.chkIgnoreFoods.Text = "Ignore Existing Foods";
            this.chkIgnoreFoods.UseVisualStyleBackColor = true;
            // 
            // chkIgnoreThings
            // 
            this.chkIgnoreThings.AutoSize = true;
            this.chkIgnoreThings.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkIgnoreThings.Location = new System.Drawing.Point(4, 33);
            this.chkIgnoreThings.Name = "chkIgnoreThings";
            this.chkIgnoreThings.Size = new System.Drawing.Size(127, 17);
            this.chkIgnoreThings.TabIndex = 1;
            this.chkIgnoreThings.Text = "Ignore Existing Things";
            this.chkIgnoreThings.UseVisualStyleBackColor = true;
            // 
            // label14
            // 
            this.label14.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label14.Location = new System.Drawing.Point(-1, -1);
            this.label14.Name = "label14";
            this.label14.Size = new System.Drawing.Size(149, 31);
            this.label14.TabIndex = 0;
            this.label14.Text = "Deciding Spawn Location";
            this.label14.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel5
            // 
            this.panel5.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel5.Controls.Add(this.chkReplace_ft);
            this.panel5.Controls.Add(this.chkEatFood);
            this.panel5.Controls.Add(this.chkReplace_tf);
            this.panel5.Controls.Add(this.chkReplace_tt);
            this.panel5.Controls.Add(this.label13);
            this.panel5.Location = new System.Drawing.Point(7, 307);
            this.panel5.Name = "panel5";
            this.panel5.Size = new System.Drawing.Size(150, 132);
            this.panel5.TabIndex = 17;
            // 
            // chkReplace_ft
            // 
            this.chkReplace_ft.AutoSize = true;
            this.chkReplace_ft.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkReplace_ft.Location = new System.Drawing.Point(3, 102);
            this.chkReplace_ft.Name = "chkReplace_ft";
            this.chkReplace_ft.Size = new System.Drawing.Size(129, 17);
            this.chkReplace_ft.TabIndex = 4;
            this.chkReplace_ft.Text = "Food with Thing if Any";
            this.chkReplace_ft.UseVisualStyleBackColor = true;
            // 
            // chkEatFood
            // 
            this.chkEatFood.AutoSize = true;
            this.chkEatFood.Enabled = false;
            this.chkEatFood.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkEatFood.Location = new System.Drawing.Point(3, 79);
            this.chkEatFood.Name = "chkEatFood";
            this.chkEatFood.Size = new System.Drawing.Size(129, 17);
            this.chkEatFood.TabIndex = 3;
            this.chkEatFood.Text = "Thing Eats Food if Any";
            this.chkEatFood.UseVisualStyleBackColor = true;
            // 
            // chkReplace_tf
            // 
            this.chkReplace_tf.AutoSize = true;
            this.chkReplace_tf.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkReplace_tf.Location = new System.Drawing.Point(3, 56);
            this.chkReplace_tf.Name = "chkReplace_tf";
            this.chkReplace_tf.Size = new System.Drawing.Size(129, 17);
            this.chkReplace_tf.TabIndex = 2;
            this.chkReplace_tf.Text = "Thing with Food if Any";
            this.chkReplace_tf.UseVisualStyleBackColor = true;
            this.chkReplace_tf.CheckedChanged += new System.EventHandler(this.chkReplace_tf_CheckedChanged);
            // 
            // chkReplace_tt
            // 
            this.chkReplace_tt.AutoSize = true;
            this.chkReplace_tt.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkReplace_tt.Location = new System.Drawing.Point(3, 33);
            this.chkReplace_tt.Name = "chkReplace_tt";
            this.chkReplace_tt.Size = new System.Drawing.Size(132, 17);
            this.chkReplace_tt.TabIndex = 1;
            this.chkReplace_tt.Text = "Thing with Thing if Any";
            this.chkReplace_tt.UseVisualStyleBackColor = true;
            // 
            // label13
            // 
            this.label13.Font = new System.Drawing.Font("Segoe UI", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label13.Location = new System.Drawing.Point(-1, -1);
            this.label13.Name = "label13";
            this.label13.Size = new System.Drawing.Size(150, 31);
            this.label13.TabIndex = 0;
            this.label13.Text = "Newborn Replacing";
            this.label13.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // panel3
            // 
            this.panel3.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel3.Controls.Add(this.chkReturnFood);
            this.panel3.Controls.Add(this.chkReturnFood_ifNotHungry);
            this.panel3.Controls.Add(this.chkMove);
            this.panel3.Controls.Add(this.chkReproduce_multi);
            this.panel3.Controls.Add(this.chkReproduce_ifHungry);
            this.panel3.Controls.Add(this.chkReproduce);
            this.panel3.Controls.Add(this.chkDieIfHungry);
            this.panel3.Controls.Add(this.chkKillFoods);
            this.panel3.Controls.Add(this.chkSpawnFoods);
            this.panel3.Controls.Add(this.chkKillThings);
            this.panel3.Controls.Add(this.chkSpawnThings);
            this.panel3.Location = new System.Drawing.Point(163, 221);
            this.panel3.Name = "panel3";
            this.panel3.Size = new System.Drawing.Size(150, 303);
            this.panel3.TabIndex = 19;
            // 
            // chkReturnFood
            // 
            this.chkReturnFood.AutoSize = true;
            this.chkReturnFood.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkReturnFood.Location = new System.Drawing.Point(3, 96);
            this.chkReturnFood.Name = "chkReturnFood";
            this.chkReturnFood.Size = new System.Drawing.Size(141, 17);
            this.chkReturnFood.TabIndex = 4;
            this.chkReturnFood.Text = "Return Food After Death";
            this.chkReturnFood.UseVisualStyleBackColor = true;
            this.chkReturnFood.CheckedChanged += new System.EventHandler(this.chkReturnFood_CheckedChanged);
            // 
            // chkReturnFood_ifNotHungry
            // 
            this.chkReturnFood_ifNotHungry.AutoSize = true;
            this.chkReturnFood_ifNotHungry.Enabled = false;
            this.chkReturnFood_ifNotHungry.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkReturnFood_ifNotHungry.Location = new System.Drawing.Point(4, 119);
            this.chkReturnFood_ifNotHungry.Name = "chkReturnFood_ifNotHungry";
            this.chkReturnFood_ifNotHungry.Size = new System.Drawing.Size(150, 17);
            this.chkReturnFood_ifNotHungry.TabIndex = 5;
            this.chkReturnFood_ifNotHungry.Text = "Return Food If Not Hungry";
            this.chkReturnFood_ifNotHungry.UseVisualStyleBackColor = true;
            // 
            // chkMove
            // 
            this.chkMove.AutoSize = true;
            this.chkMove.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkMove.Location = new System.Drawing.Point(4, 234);
            this.chkMove.Name = "chkMove";
            this.chkMove.Size = new System.Drawing.Size(142, 17);
            this.chkMove.TabIndex = 10;
            this.chkMove.Text = "Things Move to Eat Food";
            this.chkMove.UseVisualStyleBackColor = true;
            // 
            // chkReproduce_multi
            // 
            this.chkReproduce_multi.AutoSize = true;
            this.chkReproduce_multi.Enabled = false;
            this.chkReproduce_multi.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkReproduce_multi.Location = new System.Drawing.Point(4, 211);
            this.chkReproduce_multi.Name = "chkReproduce_multi";
            this.chkReproduce_multi.Size = new System.Drawing.Size(125, 17);
            this.chkReproduce_multi.TabIndex = 9;
            this.chkReproduce_multi.Text = "Can Reproduce Multi";
            this.chkReproduce_multi.UseVisualStyleBackColor = true;
            // 
            // chkReproduce_ifHungry
            // 
            this.chkReproduce_ifHungry.AutoSize = true;
            this.chkReproduce_ifHungry.Enabled = false;
            this.chkReproduce_ifHungry.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkReproduce_ifHungry.Location = new System.Drawing.Point(4, 188);
            this.chkReproduce_ifHungry.Name = "chkReproduce_ifHungry";
            this.chkReproduce_ifHungry.Size = new System.Drawing.Size(146, 17);
            this.chkReproduce_ifHungry.TabIndex = 8;
            this.chkReproduce_ifHungry.Text = "Can Reproduce If Hungry";
            this.chkReproduce_ifHungry.UseVisualStyleBackColor = true;
            // 
            // chkReproduce
            // 
            this.chkReproduce.AutoSize = true;
            this.chkReproduce.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkReproduce.Location = new System.Drawing.Point(4, 165);
            this.chkReproduce.Name = "chkReproduce";
            this.chkReproduce.Size = new System.Drawing.Size(132, 17);
            this.chkReproduce.TabIndex = 7;
            this.chkReproduce.Text = "Things Can Reproduce";
            this.chkReproduce.UseVisualStyleBackColor = true;
            this.chkReproduce.CheckedChanged += new System.EventHandler(this.chkReproduce_CheckedChanged);
            // 
            // chkDieIfHungry
            // 
            this.chkDieIfHungry.AutoSize = true;
            this.chkDieIfHungry.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkDieIfHungry.Location = new System.Drawing.Point(4, 142);
            this.chkDieIfHungry.Name = "chkDieIfHungry";
            this.chkDieIfHungry.Size = new System.Drawing.Size(122, 17);
            this.chkDieIfHungry.TabIndex = 6;
            this.chkDieIfHungry.Text = "Things Die If Hungry";
            this.chkDieIfHungry.UseVisualStyleBackColor = true;
            // 
            // chkKillFoods
            // 
            this.chkKillFoods.AutoSize = true;
            this.chkKillFoods.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkKillFoods.Location = new System.Drawing.Point(3, 73);
            this.chkKillFoods.Name = "chkKillFoods";
            this.chkKillFoods.Size = new System.Drawing.Size(99, 17);
            this.chkKillFoods.TabIndex = 3;
            this.chkKillFoods.Text = "Foods Get Killed";
            this.chkKillFoods.UseVisualStyleBackColor = true;
            // 
            // chkSpawnFoods
            // 
            this.chkSpawnFoods.AutoSize = true;
            this.chkSpawnFoods.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkSpawnFoods.Location = new System.Drawing.Point(3, 27);
            this.chkSpawnFoods.Name = "chkSpawnFoods";
            this.chkSpawnFoods.Size = new System.Drawing.Size(119, 17);
            this.chkSpawnFoods.TabIndex = 1;
            this.chkSpawnFoods.Text = "Foods Get Spawned";
            this.chkSpawnFoods.UseVisualStyleBackColor = true;
            // 
            // chkKillThings
            // 
            this.chkKillThings.AutoSize = true;
            this.chkKillThings.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkKillThings.Location = new System.Drawing.Point(3, 50);
            this.chkKillThings.Name = "chkKillThings";
            this.chkKillThings.Size = new System.Drawing.Size(102, 17);
            this.chkKillThings.TabIndex = 2;
            this.chkKillThings.Text = "Things Get Killed";
            this.chkKillThings.UseVisualStyleBackColor = true;
            // 
            // chkSpawnThings
            // 
            this.chkSpawnThings.AutoSize = true;
            this.chkSpawnThings.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.chkSpawnThings.Location = new System.Drawing.Point(3, 4);
            this.chkSpawnThings.Name = "chkSpawnThings";
            this.chkSpawnThings.Size = new System.Drawing.Size(122, 17);
            this.chkSpawnThings.TabIndex = 0;
            this.chkSpawnThings.Text = "Things Get Spawned";
            this.chkSpawnThings.UseVisualStyleBackColor = true;
            // 
            // panel2
            // 
            this.panel2.BorderStyle = System.Windows.Forms.BorderStyle.FixedSingle;
            this.panel2.Controls.Add(this.radSpawnBothSame);
            this.panel2.Controls.Add(this.radSpawnFoodsFirst);
            this.panel2.Controls.Add(this.radSpawnThingsFirst);
            this.panel2.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.panel2.Location = new System.Drawing.Point(7, 221);
            this.panel2.Name = "panel2";
            this.panel2.Size = new System.Drawing.Size(150, 80);
            this.panel2.TabIndex = 16;
            // 
            // radSpawnBothSame
            // 
            this.radSpawnBothSame.AutoSize = true;
            this.radSpawnBothSame.Checked = true;
            this.radSpawnBothSame.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.radSpawnBothSame.Location = new System.Drawing.Point(4, 49);
            this.radSpawnBothSame.Name = "radSpawnBothSame";
            this.radSpawnBothSame.Size = new System.Drawing.Size(111, 17);
            this.radSpawnBothSame.TabIndex = 2;
            this.radSpawnBothSame.TabStop = true;
            this.radSpawnBothSame.Text = "Spawn Both Same";
            this.radSpawnBothSame.UseVisualStyleBackColor = true;
            // 
            // radSpawnFoodsFirst
            // 
            this.radSpawnFoodsFirst.AutoSize = true;
            this.radSpawnFoodsFirst.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.radSpawnFoodsFirst.Location = new System.Drawing.Point(4, 26);
            this.radSpawnFoodsFirst.Name = "radSpawnFoodsFirst";
            this.radSpawnFoodsFirst.Size = new System.Drawing.Size(108, 17);
            this.radSpawnFoodsFirst.TabIndex = 1;
            this.radSpawnFoodsFirst.TabStop = true;
            this.radSpawnFoodsFirst.Text = "Spawn Foods First";
            this.radSpawnFoodsFirst.UseVisualStyleBackColor = true;
            // 
            // radSpawnThingsFirst
            // 
            this.radSpawnThingsFirst.AutoSize = true;
            this.radSpawnThingsFirst.Font = new System.Drawing.Font("Segoe UI Semilight", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.radSpawnThingsFirst.Location = new System.Drawing.Point(3, 3);
            this.radSpawnThingsFirst.Name = "radSpawnThingsFirst";
            this.radSpawnThingsFirst.Size = new System.Drawing.Size(111, 17);
            this.radSpawnThingsFirst.TabIndex = 0;
            this.radSpawnThingsFirst.TabStop = true;
            this.radSpawnThingsFirst.Text = "Spawn Things First";
            this.radSpawnThingsFirst.UseVisualStyleBackColor = true;
            // 
            // nudSense_max
            // 
            this.nudSense_max.Location = new System.Drawing.Point(163, 195);
            this.nudSense_max.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudSense_max.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSense_max.Name = "nudSense_max";
            this.nudSense_max.Size = new System.Drawing.Size(150, 20);
            this.nudSense_max.TabIndex = 15;
            this.nudSense_max.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSense_max.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudSense_min
            // 
            this.nudSense_min.Location = new System.Drawing.Point(163, 170);
            this.nudSense_min.Maximum = new decimal(new int[] {
            1000,
            0,
            0,
            0});
            this.nudSense_min.Minimum = new decimal(new int[] {
            1,
            0,
            0,
            0});
            this.nudSense_min.Name = "nudSense_min";
            this.nudSense_min.Size = new System.Drawing.Size(150, 20);
            this.nudSense_min.TabIndex = 13;
            this.nudSense_min.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudSense_min.Value = new decimal(new int[] {
            1,
            0,
            0,
            0});
            // 
            // nudDeathRate_max
            // 
            this.nudDeathRate_max.Location = new System.Drawing.Point(163, 145);
            this.nudDeathRate_max.Name = "nudDeathRate_max";
            this.nudDeathRate_max.Size = new System.Drawing.Size(150, 20);
            this.nudDeathRate_max.TabIndex = 11;
            this.nudDeathRate_max.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nudDeathRate_min
            // 
            this.nudDeathRate_min.Location = new System.Drawing.Point(163, 120);
            this.nudDeathRate_min.Name = "nudDeathRate_min";
            this.nudDeathRate_min.Size = new System.Drawing.Size(150, 20);
            this.nudDeathRate_min.TabIndex = 9;
            this.nudDeathRate_min.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nudReproduceRate_max
            // 
            this.nudReproduceRate_max.Location = new System.Drawing.Point(163, 95);
            this.nudReproduceRate_max.Name = "nudReproduceRate_max";
            this.nudReproduceRate_max.Size = new System.Drawing.Size(150, 20);
            this.nudReproduceRate_max.TabIndex = 7;
            this.nudReproduceRate_max.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nudReproduceRate_min
            // 
            this.nudReproduceRate_min.Location = new System.Drawing.Point(163, 70);
            this.nudReproduceRate_min.Name = "nudReproduceRate_min";
            this.nudReproduceRate_min.Size = new System.Drawing.Size(150, 20);
            this.nudReproduceRate_min.TabIndex = 5;
            this.nudReproduceRate_min.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nudSpawnRate_Food
            // 
            this.nudSpawnRate_Food.Location = new System.Drawing.Point(163, 45);
            this.nudSpawnRate_Food.Name = "nudSpawnRate_Food";
            this.nudSpawnRate_Food.Size = new System.Drawing.Size(150, 20);
            this.nudSpawnRate_Food.TabIndex = 3;
            this.nudSpawnRate_Food.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // nudSpawnRate_Thing
            // 
            this.nudSpawnRate_Thing.Location = new System.Drawing.Point(163, 20);
            this.nudSpawnRate_Thing.Name = "nudSpawnRate_Thing";
            this.nudSpawnRate_Thing.Size = new System.Drawing.Size(150, 20);
            this.nudSpawnRate_Thing.TabIndex = 1;
            this.nudSpawnRate_Thing.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            // 
            // label11
            // 
            this.label11.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label11.Location = new System.Drawing.Point(6, 191);
            this.label11.Name = "label11";
            this.label11.Size = new System.Drawing.Size(150, 25);
            this.label11.TabIndex = 14;
            this.label11.Text = "Max. Sense";
            this.label11.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label12
            // 
            this.label12.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label12.Location = new System.Drawing.Point(6, 166);
            this.label12.Name = "label12";
            this.label12.Size = new System.Drawing.Size(150, 25);
            this.label12.TabIndex = 12;
            this.label12.Text = "Min. Sense";
            this.label12.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label10
            // 
            this.label10.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label10.Location = new System.Drawing.Point(6, 141);
            this.label10.Name = "label10";
            this.label10.Size = new System.Drawing.Size(150, 25);
            this.label10.TabIndex = 10;
            this.label10.Text = "Max. Death Rate (%)";
            this.label10.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label9
            // 
            this.label9.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label9.Location = new System.Drawing.Point(6, 116);
            this.label9.Name = "label9";
            this.label9.Size = new System.Drawing.Size(150, 25);
            this.label9.TabIndex = 8;
            this.label9.Text = "Min. Death Rate (%)";
            this.label9.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label7
            // 
            this.label7.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label7.Location = new System.Drawing.Point(6, 91);
            this.label7.Name = "label7";
            this.label7.Size = new System.Drawing.Size(150, 25);
            this.label7.TabIndex = 6;
            this.label7.Text = "Max. Reproduce Rate (%)";
            this.label7.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label6
            // 
            this.label6.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label6.Location = new System.Drawing.Point(6, 66);
            this.label6.Name = "label6";
            this.label6.Size = new System.Drawing.Size(150, 25);
            this.label6.TabIndex = 4;
            this.label6.Text = "Min. Reproduce Rate (%)";
            this.label6.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label5
            // 
            this.label5.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label5.Location = new System.Drawing.Point(6, 41);
            this.label5.Name = "label5";
            this.label5.Size = new System.Drawing.Size(150, 25);
            this.label5.TabIndex = 2;
            this.label5.Text = "Food Spawn Rate (%)";
            this.label5.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // label4
            // 
            this.label4.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label4.Location = new System.Drawing.Point(6, 16);
            this.label4.Name = "label4";
            this.label4.Size = new System.Drawing.Size(150, 25);
            this.label4.TabIndex = 0;
            this.label4.Text = "Thing Spawn Rate (%)";
            this.label4.TextAlign = System.Drawing.ContentAlignment.MiddleLeft;
            // 
            // btnClear
            // 
            this.btnClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnClear.ForeColor = System.Drawing.Color.Crimson;
            this.btnClear.Location = new System.Drawing.Point(12, 777);
            this.btnClear.Name = "btnClear";
            this.btnClear.Size = new System.Drawing.Size(120, 30);
            this.btnClear.TabIndex = 13;
            this.btnClear.Text = "Clear";
            this.btnClear.UseVisualStyleBackColor = true;
            this.btnClear.Click += new System.EventHandler(this.btnClear_Click);
            // 
            // nudGeneration
            // 
            this.nudGeneration.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.nudGeneration.Location = new System.Drawing.Point(490, 791);
            this.nudGeneration.Maximum = new decimal(new int[] {
            0,
            0,
            0,
            0});
            this.nudGeneration.Name = "nudGeneration";
            this.nudGeneration.Size = new System.Drawing.Size(146, 20);
            this.nudGeneration.TabIndex = 10;
            this.nudGeneration.TextAlign = System.Windows.Forms.HorizontalAlignment.Center;
            this.nudGeneration.ValueChanged += new System.EventHandler(this.nudGeneration_ValueChanged);
            // 
            // btnStatistics
            // 
            this.btnStatistics.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.btnStatistics.Font = new System.Drawing.Font("Segoe UI Semibold", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnStatistics.ForeColor = System.Drawing.Color.DarkOliveGreen;
            this.btnStatistics.Location = new System.Drawing.Point(642, 773);
            this.btnStatistics.Name = "btnStatistics";
            this.btnStatistics.Size = new System.Drawing.Size(170, 38);
            this.btnStatistics.TabIndex = 8;
            this.btnStatistics.Text = "Statistics...";
            this.btnStatistics.UseVisualStyleBackColor = true;
            this.btnStatistics.Click += new System.EventHandler(this.btnStatistics_Click);
            // 
            // label16
            // 
            this.label16.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.label16.Font = new System.Drawing.Font("Segoe UI Semibold", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.label16.Location = new System.Drawing.Point(490, 773);
            this.label16.Name = "label16";
            this.label16.Size = new System.Drawing.Size(146, 15);
            this.label16.TabIndex = 9;
            this.label16.Text = "Representing Generation";
            this.label16.TextAlign = System.Drawing.ContentAlignment.MiddleCenter;
            // 
            // ttAlgorithm
            // 
            this.ttAlgorithm.ToolTipIcon = System.Windows.Forms.ToolTipIcon.Info;
            this.ttAlgorithm.ToolTipTitle = "Simulation Algorithm";
            // 
            // saveFileDialog
            // 
            this.saveFileDialog.SupportMultiDottedExtensions = true;
            // 
            // openFileDialog
            // 
            this.openFileDialog.SupportMultiDottedExtensions = true;
            // 
            // folderBrowserDialog
            // 
            this.folderBrowserDialog.SelectedPath = "Specify the destination folder in where all files would be saved";
            // 
            // btnParametersClear
            // 
            this.btnParametersClear.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnParametersClear.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnParametersClear.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnParametersClear.Location = new System.Drawing.Point(981, 737);
            this.btnParametersClear.Name = "btnParametersClear";
            this.btnParametersClear.Size = new System.Drawing.Size(156, 30);
            this.btnParametersClear.TabIndex = 7;
            this.btnParametersClear.Text = "Clear Parameters";
            this.btnParametersClear.UseVisualStyleBackColor = true;
            this.btnParametersClear.Click += new System.EventHandler(this.btnParametersClear_Click);
            // 
            // btnParametersDefault
            // 
            this.btnParametersDefault.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Top | System.Windows.Forms.AnchorStyles.Right)));
            this.btnParametersDefault.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnParametersDefault.ForeColor = System.Drawing.SystemColors.ControlText;
            this.btnParametersDefault.Location = new System.Drawing.Point(818, 737);
            this.btnParametersDefault.Name = "btnParametersDefault";
            this.btnParametersDefault.Size = new System.Drawing.Size(156, 30);
            this.btnParametersDefault.TabIndex = 6;
            this.btnParametersDefault.Text = "Parameters to Default";
            this.btnParametersDefault.UseVisualStyleBackColor = true;
            this.btnParametersDefault.Click += new System.EventHandler(this.btnParametersDefault_Click);
            // 
            // progressbar
            // 
            this.progressbar.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Right)));
            this.progressbar.Location = new System.Drawing.Point(818, 777);
            this.progressbar.Maximum = 0;
            this.progressbar.Name = "progressbar";
            this.progressbar.Size = new System.Drawing.Size(319, 30);
            this.progressbar.Step = 1;
            this.progressbar.TabIndex = 14;
            // 
            // btnExport
            // 
            this.btnExport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnExport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnExport.Location = new System.Drawing.Point(138, 777);
            this.btnExport.Menu = this.cmsExport;
            this.btnExport.Name = "btnExport";
            this.btnExport.Size = new System.Drawing.Size(120, 30);
            this.btnExport.TabIndex = 12;
            this.btnExport.Text = "Export...";
            this.btnExport.UseVisualStyleBackColor = true;
            // 
            // btnImport
            // 
            this.btnImport.Anchor = ((System.Windows.Forms.AnchorStyles)((System.Windows.Forms.AnchorStyles.Bottom | System.Windows.Forms.AnchorStyles.Left)));
            this.btnImport.Font = new System.Drawing.Font("Microsoft Sans Serif", 8.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(162)));
            this.btnImport.Location = new System.Drawing.Point(264, 777);
            this.btnImport.Menu = this.cmsImport;
            this.btnImport.Name = "btnImport";
            this.btnImport.Size = new System.Drawing.Size(120, 30);
            this.btnImport.TabIndex = 11;
            this.btnImport.Text = "Import...";
            this.btnImport.UseVisualStyleBackColor = true;
            // 
            // MainForm
            // 
            this.AcceptButton = this.btnSimulate;
            this.AllowDrop = true;
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1149, 823);
            this.Controls.Add(this.progressbar);
            this.Controls.Add(this.btnParametersDefault);
            this.Controls.Add(this.btnParametersClear);
            this.Controls.Add(this.label16);
            this.Controls.Add(this.btnStatistics);
            this.Controls.Add(this.nudGeneration);
            this.Controls.Add(this.btnClear);
            this.Controls.Add(this.grSimulationParameters);
            this.Controls.Add(this.nudSimulationCount);
            this.Controls.Add(this.btnSimulate);
            this.Controls.Add(this.label8);
            this.Controls.Add(this.btnExport);
            this.Controls.Add(this.btnImport);
            this.Controls.Add(this.pnlUniverse);
            this.Controls.Add(this.label1);
            this.Controls.Add(this.picCurrentUniverse);
            this.KeyPreview = true;
            this.MinimumSize = new System.Drawing.Size(1165, 862);
            this.Name = "MainForm";
            this.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen;
            this.Text = "TSN - Simulation of Everything";
            this.WindowState = System.Windows.Forms.FormWindowState.Maximized;
            this.Load += new System.EventHandler(this.MainForm_Load);
            this.DragDrop += new System.Windows.Forms.DragEventHandler(this.MainForm_DragDrop);
            this.DragOver += new System.Windows.Forms.DragEventHandler(this.MainForm_DragOver);
            ((System.ComponentModel.ISupportInitialize)(this.picCurrentUniverse)).EndInit();
            this.pnlUniverse.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudSleep)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudN)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudM)).EndInit();
            this.cmsImport.ResumeLayout(false);
            this.cmsExport.ResumeLayout(false);
            ((System.ComponentModel.ISupportInitialize)(this.nudSimulationCount)).EndInit();
            this.grSimulationParameters.ResumeLayout(false);
            this.panel1.ResumeLayout(false);
            this.panel1.PerformLayout();
            this.panel5.ResumeLayout(false);
            this.panel5.PerformLayout();
            this.panel3.ResumeLayout(false);
            this.panel3.PerformLayout();
            this.panel2.ResumeLayout(false);
            this.panel2.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.nudSense_max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSense_min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeathRate_max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudDeathRate_min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudReproduceRate_max)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudReproduceRate_min)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpawnRate_Food)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudSpawnRate_Thing)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.nudGeneration)).EndInit();
            this.ResumeLayout(false);

        }

        #endregion

        private System.Windows.Forms.PictureBox picCurrentUniverse;
        private System.Windows.Forms.Label label1;
        private System.Windows.Forms.Panel pnlUniverse;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.Label label3;
        private System.ComponentModel.BackgroundWorker bgwSimulate;
        private TSN.Universe.DesktopApp.DropDownButton btnImport;
        private TSN.Universe.DesktopApp.DropDownButton btnExport;
        private System.Windows.Forms.NumericUpDown nudSimulationCount;
        private System.Windows.Forms.Button btnSimulate;
        private System.Windows.Forms.Label label8;
        private System.Windows.Forms.GroupBox grSimulationParameters;
        private System.Windows.Forms.Label label7;
        private System.Windows.Forms.Label label6;
        private System.Windows.Forms.Label label5;
        private System.Windows.Forms.Label label4;
        private System.Windows.Forms.Panel panel3;
        private System.Windows.Forms.CheckBox chkReproduce;
        private System.Windows.Forms.CheckBox chkDieIfHungry;
        private System.Windows.Forms.CheckBox chkKillFoods;
        private System.Windows.Forms.CheckBox chkSpawnFoods;
        private System.Windows.Forms.CheckBox chkKillThings;
        private System.Windows.Forms.CheckBox chkSpawnThings;
        private System.Windows.Forms.Panel panel2;
        private System.Windows.Forms.RadioButton radSpawnBothSame;
        private System.Windows.Forms.RadioButton radSpawnFoodsFirst;
        private System.Windows.Forms.RadioButton radSpawnThingsFirst;
        private System.Windows.Forms.NumericUpDown nudSense_max;
        private System.Windows.Forms.NumericUpDown nudSense_min;
        private System.Windows.Forms.NumericUpDown nudDeathRate_max;
        private System.Windows.Forms.NumericUpDown nudDeathRate_min;
        private System.Windows.Forms.NumericUpDown nudReproduceRate_max;
        private System.Windows.Forms.NumericUpDown nudReproduceRate_min;
        private System.Windows.Forms.NumericUpDown nudSpawnRate_Food;
        private System.Windows.Forms.NumericUpDown nudSpawnRate_Thing;
        private System.Windows.Forms.Label label11;
        private System.Windows.Forms.Label label12;
        private System.Windows.Forms.Label label10;
        private System.Windows.Forms.Label label9;
        private System.Windows.Forms.Panel panel5;
        private System.Windows.Forms.CheckBox chkReplace_ft;
        private System.Windows.Forms.CheckBox chkEatFood;
        private System.Windows.Forms.CheckBox chkReplace_tf;
        private System.Windows.Forms.CheckBox chkReplace_tt;
        private System.Windows.Forms.Label label13;
        private System.Windows.Forms.CheckBox chkMove;
        private System.Windows.Forms.CheckBox chkReproduce_multi;
        private System.Windows.Forms.CheckBox chkReproduce_ifHungry;
        private System.Windows.Forms.NumericUpDown nudSleep;
        private System.Windows.Forms.Label label15;
        private System.Windows.Forms.NumericUpDown nudN;
        private System.Windows.Forms.NumericUpDown nudM;
        private System.Windows.Forms.Button btnClear;
        private System.Windows.Forms.NumericUpDown nudGeneration;
        private System.Windows.Forms.Button btnStatistics;
        private System.Windows.Forms.Panel panel1;
        private System.Windows.Forms.CheckBox chkIgnoreFoods;
        private System.Windows.Forms.CheckBox chkIgnoreThings;
        private System.Windows.Forms.Label label14;
        private System.Windows.Forms.CheckBox chkReturnFood;
        private System.Windows.Forms.CheckBox chkReturnFood_ifNotHungry;
        private System.Windows.Forms.Label label16;
        private System.Windows.Forms.ToolTip ttAlgorithm;
        private System.Windows.Forms.SaveFileDialog saveFileDialog;
        private System.Windows.Forms.OpenFileDialog openFileDialog;
        private System.Windows.Forms.ContextMenuStrip cmsExport;
        private System.Windows.Forms.ToolStripMenuItem tsmiExportUniverse;
        private System.Windows.Forms.ToolStripMenuItem tsmiExportParameters;
        private System.Windows.Forms.ToolStripMenuItem tsmiExportCanvas;
        private System.Windows.Forms.ContextMenuStrip cmsImport;
        private System.Windows.Forms.ToolStripMenuItem tsmiImportUniverse;
        private System.Windows.Forms.ToolStripMenuItem tsmiImportParameters;
        private System.Windows.Forms.FolderBrowserDialog folderBrowserDialog;
        private System.Windows.Forms.Button btnParametersClear;
        private System.Windows.Forms.Button btnParametersDefault;
        private System.Windows.Forms.ProgressBar progressbar;
    }
}

