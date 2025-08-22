using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Threading;
using System.Windows.Forms;
using static TSN.Universe.DesktopApp.Utility;

namespace TSN.Universe.DesktopApp
{
    internal sealed partial class MainForm : Form
    {
        public MainForm()
        {
            _lockerClear = new object();
            _lockerParameters = new object();
            _lockerCanvas = new object();
            _lockerSimulation = new object();
            _initializing = false;
            InitializeComponent();
            btnSimulate.Text = SimulateButtonText_Initial;
            ttAlgorithm.SetToolTip(btnSimulate, Universe.Algorithm);
            _disabledControls = new Control[] { pnlUniverse, grSimulationParameters, nudSimulationCount, nudGeneration, btnExport, btnImport, btnClear, btnParametersClear, btnParametersDefault };
        }


        private const string SimulateButtonText = "Smiulate";
        private const string SimulateButtonText_Initial = "Initialize the Universe";
        private const string SimulateButtonText_Simulating = "Cancel - (Simulating...)";
        private const string SimulateButtonText_SimulatingFormat = "Cancel - (Simulating... {0}%)";
        private const string SimulateButtonText_Cancelling = "Cancelling...";

        private readonly object _lockerClear, _lockerParameters, _lockerCanvas, _lockerSimulation;
        private readonly Control[] _disabledControls;
        private bool _initializing;



        private bool ImportUniverse(string filePath) => TryLoadFromFile<ApplicationModel>(filePath, am =>
        {
            Program.ApplicationModel = am;
            if (am.Universe == null || !am.Universe.CurrentGeneration.HasValue)
                Clear();
            else
            {
                var generation = am.Universe.CurrentGeneration.Value;
                Enabled = false;
                nudM.Value = am.Universe.M;
                nudN.Value = am.Universe.N;
                nudM.Enabled = nudN.Enabled = false;
                nudSleep.Value = nudSleep.Minimum;
                nudSimulationCount.Value = 1M;
                btnSimulate.Text = SimulateButtonText;
                nudGeneration.ValueChanged -= nudGeneration_ValueChanged;
                nudGeneration.Value = nudGeneration.Maximum = generation;
                nudGeneration.ValueChanged += nudGeneration_ValueChanged;
                RepresentParameters(am.Universe.GetSimulationParameters(generation));
                RepresentCanvas(am.Universe.DrawUniverseCanvas(generation));
                Enabled = true;
            }
        });
        private bool ImportParameters(string filePath) => TryLoadFromFile<SimulationParameters>(filePath, sp => RepresentParameters(sp));
        private void Clear()
        {
            lock (_lockerClear)
            {
                Enabled = false;
                RepresentCanvas(null);
                nudSleep.Value = nudSleep.Minimum;
                nudSimulationCount.Value = 1M;
                nudM.Enabled = nudN.Enabled = true;
                btnSimulate.Text = SimulateButtonText_Initial;
                nudGeneration.ValueChanged -= nudGeneration_ValueChanged;
                nudGeneration.Maximum = 0M;
                nudGeneration.ValueChanged += nudGeneration_ValueChanged;
                Program.ApplicationModel.Universe = null;
                Enabled = true;
            }
        }
        private SimulationParameters GetParameters()
        {
            lock (_lockerParameters)
            {
                var options = SimulationOptions.SpawnThingsFirst;
                if (radSpawnFoodsFirst.Checked)
                    options |= SimulationOptions.SpawnFoodsFirst;
                else if (radSpawnBothSame.Checked)
                    options |= SimulationOptions.SpawnAtTheSameTime;
                if (chkSpawnThings.Checked)
                    options |= SimulationOptions.ThingsGetSpawned;
                if (chkSpawnFoods.Checked)
                    options |= SimulationOptions.FoodsGetSpawned;
                if (chkKillThings.Checked)
                    options |= SimulationOptions.ThingsGetKilled;
                if (chkKillFoods.Checked)
                    options |= SimulationOptions.FoodsGetKilled;
                if (chkReturnFood.Checked)
                    options |= SimulationOptions.ReturnFoodAfterDeath;
                if (chkReturnFood_ifNotHungry.Checked)
                    options |= SimulationOptions.ReturnFoodIfNotHungry;
                if (!chkDieIfHungry.Checked)
                    options |= SimulationOptions.DontDieIfHungry;
                if (chkReproduce.Checked)
                    options |= SimulationOptions.CanReproduce;
                if (chkReproduce_ifHungry.Checked)
                    options |= SimulationOptions.CanReproduceIfHungry;
                if (chkReproduce_multi.Checked)
                    options |= SimulationOptions.CanReproduceMoreThanOne;
                if (chkMove.Checked)
                    options |= SimulationOptions.MoveToEat;
                if (chkReplace_tt.Checked)
                    options |= SimulationOptions.ReplaceThingWithExistingThing;
                if (chkReplace_tf.Checked)
                    options |= SimulationOptions.ReplaceThingWithExistingFood;
                if (chkEatFood.Checked)
                    options |= SimulationOptions.EatFoodIfExists;
                if (chkReplace_ft.Checked)
                    options |= SimulationOptions.ReplaceFoodWithExistingThing;
                if (chkIgnoreThings.Checked)
                    options |= SimulationOptions.DecideLocationByIgnoringThings;
                if (chkIgnoreFoods.Checked)
                    options |= SimulationOptions.DecideLocationByIgnoringFoods;
                return new SimulationParameters((double)nudSpawnRate_Thing.Value / 100, (double)nudSpawnRate_Food.Value / 100, (double)nudReproduceRate_min.Value / 100, (double)nudReproduceRate_max.Value / 100, (double)nudDeathRate_min.Value / 100, (double)nudDeathRate_max.Value / 100, (ushort)nudSense_min.Value, (ushort)nudSense_max.Value, options);
            }
        }
        private void ClearParameters()
        {
            lock (_lockerParameters)
            {
                nudSleep.Value = nudSleep.Minimum;
                nudSpawnRate_Thing.Value = nudSpawnRate_Thing.Minimum;
                nudSpawnRate_Food.Value = nudSpawnRate_Food.Minimum;
                nudReproduceRate_min.Value = nudReproduceRate_min.Minimum;
                nudReproduceRate_max.Value = nudReproduceRate_max.Minimum;
                nudDeathRate_min.Value = nudDeathRate_min.Minimum;
                nudDeathRate_max.Value = nudDeathRate_max.Minimum;
                nudSense_min.Value = nudSense_min.Minimum;
                nudSense_max.Value = nudSense_max.Minimum;
                radSpawnBothSame.Checked = true;
                chkReplace_tt.Checked = chkReplace_tf.Checked = chkEatFood.Checked = chkReplace_ft.Checked = chkIgnoreThings.Checked = chkIgnoreFoods.Checked = chkSpawnThings.Checked = chkSpawnFoods.Checked = chkKillThings.Checked = chkKillFoods.Checked = chkReturnFood.Checked = chkReturnFood_ifNotHungry.Checked = chkDieIfHungry.Checked = chkReproduce.Checked = chkReproduce_ifHungry.Checked = chkReproduce_multi.Checked = chkMove.Checked = false;
            }
        }
        private void RepresentParameters(SimulationParameters? parameters = null)
        {
            if (!parameters.HasValue)
            {
                RepresentParameters(SimulationParameters.Default);
                return;
            }
            lock (_lockerParameters)
            {
                var p = parameters.Value;
                nudSpawnRate_Thing.Value = (decimal)p.SpawnRate_Thing * 100M;
                nudSpawnRate_Food.Value = (decimal)p.SpawnRate_Food * 100M;
                nudReproduceRate_min.Value = (decimal)p.ReproduceRate_Min * 100M;
                nudReproduceRate_max.Value = (decimal)p.ReproduceRate_Max * 100M;
                nudDeathRate_min.Value = (decimal)p.DeathRate_Min * 100M;
                nudDeathRate_max.Value = (decimal)p.DeathRate_Max * 100M;
                nudSense_min.Value = p.Sense_Min;
                nudSense_max.Value = p.Sense_Max;                
                ((p.Options & SimulationOptions.SpawnAtTheSameTime) != 0 ? radSpawnBothSame : ((p.Options & SimulationOptions.SpawnFoodsFirst) != 0 ? radSpawnFoodsFirst : radSpawnThingsFirst)).Checked = true;
                chkReplace_tt.Checked = (p.Options & SimulationOptions.ReplaceThingWithExistingThing) != 0;
                chkReplace_tf.Checked = (p.Options & SimulationOptions.ReplaceThingWithExistingFood) != 0;
                chkEatFood.Checked = (p.Options & SimulationOptions.EatFoodIfExists) != 0;
                chkReplace_ft.Checked = (p.Options & SimulationOptions.ReplaceFoodWithExistingThing) != 0;
                chkIgnoreThings.Checked = (p.Options & SimulationOptions.DecideLocationByIgnoringThings) != 0;
                chkIgnoreFoods.Checked = (p.Options & SimulationOptions.DecideLocationByIgnoringFoods) != 0;
                chkSpawnThings.Checked = (p.Options & SimulationOptions.ThingsGetSpawned) != 0;
                chkSpawnFoods.Checked = (p.Options & SimulationOptions.FoodsGetSpawned) != 0;
                chkKillThings.Checked = (p.Options & SimulationOptions.ThingsGetKilled) != 0;
                chkKillFoods.Checked = (p.Options & SimulationOptions.FoodsGetKilled) != 0;
                chkReturnFood.Checked = (p.Options & SimulationOptions.ReturnFoodAfterDeath) != 0;
                chkReturnFood_ifNotHungry.Checked = (p.Options & SimulationOptions.ReturnFoodIfNotHungry) != 0;
                chkDieIfHungry.Checked = (p.Options & SimulationOptions.DontDieIfHungry) == 0;
                chkReproduce.Checked = (p.Options & SimulationOptions.CanReproduce) != 0;
                chkReproduce_ifHungry.Checked = (p.Options & SimulationOptions.CanReproduceIfHungry) == SimulationOptions.CanReproduceIfHungry;
                chkReproduce_multi.Checked = (p.Options & SimulationOptions.CanReproduceMoreThanOne) == SimulationOptions.CanReproduceMoreThanOne;
                chkMove.Checked = (p.Options & SimulationOptions.MoveToEat) != 0;
            }
        }
        private void RepresentCanvas() => RepresentCanvas(Program.ApplicationModel.Universe.DrawUniverseCanvas());
        private void RepresentCanvas(Bitmap universe)
        {
            lock (_lockerCanvas)
            {
                if (universe == null)
                {
                    picCurrentUniverse.MouseClick -= picCurrentUniverse_MouseClick;
                    //picCurrentUniverse.MouseMove -= picCurrentUniverse_MouseMove;
                    KeyDown -= MainForm_KeyDown;
                }
                picCurrentUniverse.Image?.Dispose();
                picCurrentUniverse.Image = universe;
            }
        }

        private void MainForm_Load(object sender, EventArgs e) => RepresentParameters();
        private void MainForm_KeyDown(object sender, KeyEventArgs e)
        {
            if (e.KeyCode == Keys.F1)
                FullScreenImageForm.ShowInstance(
                    this,
                    Program.ApplicationModel.Universe.DrawUniverseCanvas((uint)nudGeneration.Value),
                    (sender_, e_) =>
                    {
                        var matter = Program.ApplicationModel.UniverseIllustrator.GetMatter(sender_.Size, e_.Location, (uint)nudGeneration.Value);
                        if (matter != null)
                            MatterInfoForm.ShowInstantce(matter);
                    },
                    (sender_, e_) =>
                    {
                        if (Program.ApplicationModel.Universe == null)
                            return;
                        picCurrentUniverse.Cursor = Program.ApplicationModel.UniverseIllustrator.GetMatter(sender_.Size, e_.Location, (uint)nudGeneration.Value) == null ? Cursors.Default : Cursors.Hand;
                    });
        }
        private void MainForm_DragOver(object sender, DragEventArgs e)
        {
            string[] files;
            string ext;
            e.Effect = (e.Data.GetDataPresent(DataFormats.FileDrop) && (files = (string[])e.Data.GetData(DataFormats.FileDrop)).Length == 1 && ((ext = GetDoubleExtenstion(files[0])).Equals(ApplicationModel.FileExtension_Parameters) || ext.Equals(ApplicationModel.FileExtension_Universe))) ? DragDropEffects.Copy : DragDropEffects.None;
        }
        private void MainForm_DragDrop(object sender, DragEventArgs e)
        {
            if ((e.Effect & DragDropEffects.Copy) == 0)
                return;
            var filePath = ((string[])e.Data.GetData(DataFormats.FileDrop))[0];
            switch (GetDoubleExtenstion(filePath))
            {
                case ApplicationModel.FileExtension_Parameters:
                    ImportParameters(filePath);
                    break;
                case ApplicationModel.FileExtension_Universe:
                    if ((Program.ApplicationModel.Universe?.CurrentGeneration.HasValue ?? false) && MessageBox.Show("Current simulation progress is going to be deleted permanently. Do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                        break;
                    ImportUniverse(filePath);
                    break;
            }
        }
        private void chkReturnFood_CheckedChanged(object sender, EventArgs e)
        {
            chkReturnFood_ifNotHungry.Checked = false;
            chkReturnFood_ifNotHungry.Enabled = ((CheckBox)sender).Checked;
        }
        private void chkReproduce_CheckedChanged(object sender, EventArgs e)
        {
            chkReproduce_ifHungry.Checked = chkReproduce_multi.Checked = false;
            chkReproduce_ifHungry.Enabled = chkReproduce_multi.Enabled = ((CheckBox)sender).Checked;
        }
        private void chkReplace_tf_CheckedChanged(object sender, EventArgs e)
        {
            chkEatFood.Checked = false;
            chkEatFood.Enabled = ((CheckBox)sender).Checked;
        }
        private void picCurrentUniverse_MouseClick(object sender, MouseEventArgs e)
        {
            var matter = Program.ApplicationModel.UniverseIllustrator.GetMatter(picCurrentUniverse.Size, e.Location, (uint)nudGeneration.Value);
            if (matter != null)
                MatterInfoForm.ShowInstantce(matter);
        }
        private void tsmiExportUniverse_Click(object sender, EventArgs e)
        {
            if (Program.ApplicationModel.Universe?.CurrentGeneration == null)
            {
                MessageBox.Show("Universe has not been initialized yet.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            saveFileDialog.DefaultExt = ApplicationModel.FileExtension_Universe;
            var ext = $"*{ApplicationModel.FileExtension_Universe}";
            saveFileDialog.Filter = $"SOE Binary Files for Universes ({ext})|{ext}";
            saveFileDialog.Title = "Specify the target file to which the universe would be saved";
            saveFileDialog.FileName = $"SOE Universe {DateTime.Now.ToString("yyyyMMddHHmmss")}";
            if (saveFileDialog.ShowDialog() == DialogResult.OK)
                Program.ApplicationModel.SaveToFile(saveFileDialog.FileName);
        }
        private void tsmiExportParameters_Click(object sender, EventArgs e)
        {
            using (var dialog = new ExportRangeDialogForm { CurrentModifiedEnabled = true })
            {
                if (dialog.ShowDialog() != DialogResult.OK || dialog.SelectedOption == ExportRangeDialogForm.Options.NONE)
                    return;
                bool ShowSaveFileDialog()
                {
                    saveFileDialog.DefaultExt = ApplicationModel.FileExtension_Parameters;
                    var ext = $"*{ApplicationModel.FileExtension_Parameters}";
                    saveFileDialog.Filter = $"SOE Binary Files for Simulation Parameters ({ext})|{ext}";
                    saveFileDialog.Title = "Specify the target file to which the simulation parameters would be saved";
                    saveFileDialog.FileName = $"SOE Simuation Parameters {DateTime.Now.ToString("yyyyMMddHHmmss")}";
                    return saveFileDialog.ShowDialog() == DialogResult.OK;
                }
                uint min = 0, max = 0;
                switch (dialog.SelectedOption)
                {
                    case ExportRangeDialogForm.Options.AllGenerations:
                        min = 0;
                        max = Program.ApplicationModel.Universe.CurrentGeneration.Value;
                        break;
                    case ExportRangeDialogForm.Options.CurrentGeneraion:
                        if (ShowSaveFileDialog())
                            Program.ApplicationModel.Universe.GetSimulationParameters(Program.ApplicationModel.Universe.CurrentGeneration.Value).SaveToFile(saveFileDialog.FileName);
                        return;
                    case ExportRangeDialogForm.Options.CurrentModified:
                        if (ShowSaveFileDialog())
                        {
                            SimulationParameters prm;
                            try
                            {
                                prm = GetParameters();
                            }
                            catch (Exception ex)
                            {
                                MessageBox.Show($"An error occured while reading simulation parameters:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                                return;
                            }
                            prm.SaveToFile(saveFileDialog.FileName);
                        }
                        return;
                    case ExportRangeDialogForm.Options.SelectedRange:
                        dialog.GetSelectedRange(out min, out max);
                        break;
                }
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                    return;
                var parameters = new List<(string, SimulationParameters)>();
                var padWidth = max.ToString().Length;
                for (uint i = min; i <= max; i++)
                    parameters.Add((Path.Combine(folderBrowserDialog.SelectedPath, $"{dialog.FileName}_Generation{i.ToString().PadLeft(padWidth, '0')}{ApplicationModel.FileExtension_Parameters}"), Program.ApplicationModel.Universe.GetSimulationParameters(i)));
                SaveToFile(parameters);
                parameters.Clear();
                parameters.TrimExcess();
            }
        }
        private void tsmiExportCanvas_Click(object sender, EventArgs e)
        {
            if (!Program.ApplicationModel.Universe.CurrentGeneration.HasValue)
            {
                MessageBox.Show("Universe has not been initialized yet.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            using (var dialog = new ExportRangeDialogForm
            {
                CurrentModifiedEnabled = true,
                FileNameWithExtension = true
            })
            {
                if (dialog.ShowDialog() != DialogResult.OK || dialog.SelectedOption == ExportRangeDialogForm.Options.NONE)
                    return;
                void SaveImage<T>(uint generation, T image) where T : Image
                {
                    saveFileDialog.DefaultExt = ".png";
                    saveFileDialog.Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp|Other Image Formats|*.*";
                    saveFileDialog.Title = "Specify the target file to which the simulation canvas would be saved";
                    saveFileDialog.FileName = $"SOE Simuation Canvas (Generation {generation}) {DateTime.Now.ToString("yyyyMMddHHmmss")}";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        switch (saveFileDialog.FilterIndex)
                        {
                            case 1:
                                image.SaveImage(saveFileDialog.FileName, ImageFormat.Png);
                                break;
                            case 2:
                                image.SaveImage(saveFileDialog.FileName, ImageFormat.Jpeg);
                                break;
                            case 3:
                                image.SaveImage(saveFileDialog.FileName, ImageFormat.Bmp);
                                break;
                            default:
                                image.SaveImage(saveFileDialog.FileName);
                                break;
                        }
                }
                uint min = 0, max = 0;
                switch (dialog.SelectedOption)
                {
                    case ExportRangeDialogForm.Options.AllGenerations:
                        min = 0;
                        max = Program.ApplicationModel.Universe.CurrentGeneration.Value;
                        break;
                    case ExportRangeDialogForm.Options.CurrentGeneraion:
                        var generation = Program.ApplicationModel.Universe.CurrentGeneration.Value;
                        SaveImage(generation, Program.ApplicationModel.Universe.DrawUniverseCanvas(generation));
                        return;
                    case ExportRangeDialogForm.Options.CurrentModified:
                        SaveImage((uint)nudGeneration.Value, picCurrentUniverse.Image);
                        return;
                    case ExportRangeDialogForm.Options.SelectedRange:
                        dialog.GetSelectedRange(out min, out max);
                        break;
                }
                if (folderBrowserDialog.ShowDialog() != DialogResult.OK)
                    return;
                var images = new List<(string, Bitmap, ImageFormat)>();
                var padWidth = max.ToString().Length;
                for (uint i = min; i <= max; i++)
                    images.Add((Path.Combine(folderBrowserDialog.SelectedPath, $"{Path.GetFileNameWithoutExtension(dialog.FileName)}_Generation{i.ToString().PadLeft(padWidth, '0')}{Path.GetExtension(dialog.FileName)}"), Program.ApplicationModel.Universe.DrawUniverseCanvas(i), null));
                Utility.SaveImage(images);
                images.Clear();
                images.TrimExcess();
            }
        }
        private void tsmiImportUniverse_Click(object sender, EventArgs e)
        {
            if ((Program.ApplicationModel.Universe?.CurrentGeneration.HasValue ?? false) && MessageBox.Show("Current simulation progress is going to be deleted permanently. Do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) != DialogResult.Yes)
                return;
            openFileDialog.Title = "Select the universe file that would be loaded";
            openFileDialog.DefaultExt = ApplicationModel.FileExtension_Universe;
            var ext = $"*{ApplicationModel.FileExtension_Universe}";
            openFileDialog.Filter = $"SOE Binary Files for Universes ({ext})|{ext}";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                ImportUniverse(openFileDialog.FileName);
        }
        private void tsmiImportParameters_Click(object sender, EventArgs e)
        {
            openFileDialog.Title = "Select the simulation parameters file that would be loaded";
            openFileDialog.DefaultExt = ApplicationModel.FileExtension_Parameters;
            var ext = $"*{ApplicationModel.FileExtension_Parameters}";
            openFileDialog.Filter = $"SOE Binary Files for Simulation Parameters ({ext})|{ext}";
            if (openFileDialog.ShowDialog() == DialogResult.OK)
                ImportParameters(openFileDialog.FileName);
        }
        private void btnParametersDefault_Click(object sender, EventArgs e) => RepresentParameters(null);
        private void btnParametersClear_Click(object sender, EventArgs e) => ClearParameters();
        private void btnClear_Click(object sender, EventArgs e)
        {
            if (!Program.ApplicationModel.Universe?.CurrentGeneration.HasValue ?? true)
            {
                RepresentParameters();
                return;
            }
            if (MessageBox.Show("Current simulation progress is going to be deleted permanently. Do you wish to continue?", "Warning", MessageBoxButtons.YesNo, MessageBoxIcon.Warning) == DialogResult.Yes)
                Clear();
        }
        private void btnStatistics_Click(object sender, EventArgs e)
        {
            if (_initializing)
            {
                ProgressionOutputForm.ShowInstance(0);
                return;
            }
            var curr = Program.ApplicationModel.Universe?.CurrentGeneration;
            if (!curr.HasValue)
            {
                MessageBox.Show("The universe has not been initialized yet.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                return;
            }
            StatisticsForm.ShowInstance((uint)nudGeneration.Value);
        }
        private void btnSimulate_Click(object sender, EventArgs e)
        {
            lock (_lockerClear)
            {
                bool initialize = false;
                if (!btnSimulate.Text.Equals(SimulateButtonText) && !(initialize = btnSimulate.Text.Equals(SimulateButtonText_Initial)))
                {
                    bgwSimulate.CancelAsync();
                    btnSimulate.Text = SimulateButtonText_Cancelling;
                    return;
                }
                if (initialize)
                {
                    nudM.Enabled = nudN.Enabled = false;
                    Program.ApplicationModel.Universe = new Universe((ushort)nudM.Value, (ushort)nudN.Value);
                    Program.ApplicationModel.Universe.Initializing += Universe_Initializing;
                    Program.ApplicationModel.Universe.SimulationProcessEnded += Universe_SimulationProcessEnded;
                    Program.ApplicationModel.Universe.SimulationErrorOccured += Universe_SimulationErrorOccured;
                }
                btnSimulate.Text = SimulateButtonText_Simulating;
                foreach (var c in _disabledControls)
                    c.Enabled = false;
                picCurrentUniverse.MouseClick -= picCurrentUniverse_MouseClick;
                KeyDown -= MainForm_KeyDown;
                var count = (int)nudSimulationCount.Value;
                bgwSimulate.RunWorkerAsync(count);
                progressbar.Maximum = count;
            }
        }
        private void bgwSimulate_DoWork(object sender, DoWorkEventArgs e)
        {
            var count = (int)e.Argument;
            void Work(int percentProgress = 0)
            {
                Program.ApplicationModel.Universe.Simulate(GetParameters());
                var generation = Program.ApplicationModel.Universe.CurrentGeneration.Value;
                bgwSimulate.ReportProgress(percentProgress, (generation, Program.ApplicationModel.Universe.DrawUniverseCanvas()));
                if (nudSleep.Value > 0M)
                    Thread.Sleep((int)nudSleep.Value);
            }
            if (count > 1 && Program.ApplicationModel.Universe.CurrentGeneration == null)
                Work();
            for (int i = 0; i < count; i++)
            {
                try
                {
                    Work((int)Math.Round(100D * (i + 1) / count));
                }
                catch (Exception ex)
                {
                    e.Cancel = true;
                    MessageBox.Show($"An error occured:\n{ex.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
                    break;
                }
                if (e.Cancel = bgwSimulate.CancellationPending)
                    break;
                if (i % 50 == 0)
                    GC.Collect();
            }
        }
        private void bgwSimulate_ProgressChanged(object sender, ProgressChangedEventArgs e)
        {
            btnSimulate.Text = string.Format(SimulateButtonText_SimulatingFormat, e.ProgressPercentage);
            var userState = ((uint Generation, Bitmap Canvas))e.UserState;
            RepresentCanvas(userState.Canvas);
            nudGeneration.ValueChanged -= nudGeneration_ValueChanged;
            nudGeneration.Value = nudGeneration.Maximum = userState.Generation;
            nudGeneration.ValueChanged += nudGeneration_ValueChanged;
            progressbar.PerformStep();
        }
        private void bgwSimulate_RunWorkerCompleted(object sender, RunWorkerCompletedEventArgs e)
        {
            lock (_lockerSimulation)
            {
                progressbar.Maximum = 0;
                btnSimulate.Enabled = true;
                if (Program.ApplicationModel.Universe?.CurrentGeneration.HasValue ?? false)
                {
                    btnSimulate.Text = SimulateButtonText;
                    picCurrentUniverse.MouseClick += picCurrentUniverse_MouseClick;
                    KeyDown += MainForm_KeyDown;
                    nudGeneration.ValueChanged -= nudGeneration_ValueChanged;
                    nudGeneration.Value = nudGeneration.Maximum = Program.ApplicationModel.Universe.CurrentGeneration.Value;
                    nudGeneration.ValueChanged += nudGeneration_ValueChanged;
                    RepresentCanvas();
                }
                else
                {
                    btnSimulate.Text = SimulateButtonText_Initial;
                    nudM.Enabled = nudN.Enabled = true;
                }
                foreach (var c in _disabledControls)
                    c.Enabled = true;
            }
            GC.Collect();
        }
        private void nudGeneration_ValueChanged(object sender, EventArgs e)
        {
            var generation = (uint)nudGeneration.Value;
            RepresentParameters(Program.ApplicationModel.Universe.GetSimulationParameters(generation));
            RepresentCanvas(Program.ApplicationModel.Universe.DrawUniverseCanvas(generation));
        }
        private void Universe_Initializing(object sender, SimulationEventArgs e) => _initializing = true;
        private void Universe_SimulationProcessEnded(object sender, SimulationEventArgs e) => _initializing = false;
        private void Universe_SimulationErrorOccured(object sender, SimulationErrorOccuredEventArgs e)
        {
            bgwSimulate.CancelAsync();
            MessageBox.Show($"An error occured while simulating generation #{e.Generation}.\nAll changes that made by the exceptional simulation, are going to be revoked.\nIMPORTANT: In order not to lose the current universe, please do not close the app until revoking process would be completed.\nError Rank: {e.Rank}, Error Source: {e.Exception.Source}\nError Message:\n{e.Exception.Message}", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error);
            e.CancelThrow = true;
        }
    }
}