using System;
using System.Drawing;
using System.Drawing.Imaging;
using System.Windows.Forms;
using static TSN.Universe.DesktopApp.Utility;

namespace TSN.Universe.DesktopApp
{
    internal sealed partial class StatisticsForm : Form
    {
        static StatisticsForm()
        {
            _locker1 = new object();
            _locker2 = new object();
            _instance = null;
            Program.ApplicationModelChanged += Program_ApplicationModelChanged;
            Program.ApplicationModel.UniverseChanged += ApplicationModel_UniverseChanged;
            if (Program.ApplicationModel.Universe != null)
                Program.ApplicationModel.Universe.SimulationProcessEnded += Universe_SimulationProcessEnded;
        }
        private StatisticsForm()
        {
            InitializeComponent();
            Size = new Size(statisticsUC.Width + 40, statisticsUC.Height + 124);
            statisticsUC.Anchor = AnchorStyles.Top | AnchorStyles.Left | AnchorStyles.Right | AnchorStyles.Bottom;
        }


        private const string _saveFileDialog_FileNameFormat = "SOE Statistics (Generation {0}) {1}";

        private static readonly object _locker1, _locker2;
        private static StatisticsForm _instance;



        private static void RepresentStatistics(uint? generation = null)
        {
            lock (_locker1)
            {
                if (!generation.HasValue)
                {
                    _instance.statisticsUC.Clear();
                    return;
                }
                _instance.nudGeneration.ValueChanged -= _instance.nudGeneration_ValueChanged;
                _instance.nudGeneration.Value = generation.Value;
                _instance.nudGeneration.ValueChanged += _instance.nudGeneration_ValueChanged;
                _instance.statisticsUC.RepresentStatistics(Program.ApplicationModel.Universe.GetStatistics(generation.Value));
            }
        }
        public static void ClearInstance(bool close = true)
        {
            lock (_locker2)
            {
                if (!(_instance?.IsDisposed ?? true))
                {
                    RepresentStatistics();
                    _instance.statisticsUC.Dispose();
                    if (close)
                        _instance.Close();
                }
                _instance = null;
            }
        }
        public static void ShowInstance(uint generation)
        {
            lock (_locker2)
            {
                if (Program.ApplicationModel.Universe == null)
                    throw new InvalidOperationException();
                if (_instance?.IsDisposed ?? true)
                    _instance = new StatisticsForm();
                _instance.nudGeneration.ValueChanged -= _instance.nudGeneration_ValueChanged;
                _instance.nudGeneration.Maximum = Program.ApplicationModel.Universe.CurrentGeneration.Value;
                _instance.nudGeneration.ValueChanged += _instance.nudGeneration_ValueChanged;
                RepresentStatistics(generation);
                _instance.Show();
                _instance.Focus();
            }
        }

        private void StatisticsForm_FormClosed(object sender, FormClosedEventArgs e) => ClearInstance(false);
        private void btnCharts_Click(object sender, EventArgs e)
        {
            lock (_locker1)
                using (var dialog = new ChartOptionsDialogForm())
                    if (dialog.ShowDialog() == DialogResult.OK)
                        ChartForm.ShowInstantce(dialog.Selection, (uint)nudGeneration.Value);
        }
        private void btnConsole_Click(object sender, EventArgs e)
        {
            lock (_locker1)
                ProgressionOutputForm.ShowInstance((uint)nudGeneration.Value);
        }
        private void nudGeneration_ValueChanged(object sender, EventArgs e) => RepresentStatistics((uint)nudGeneration.Value);
        private void tsmiExportAsImage_Click(object sender, EventArgs e)
        {
            lock (_locker1)
            {
                saveFileDialog.DefaultExt = ".png";
                saveFileDialog.Filter = "PNG|*.png|JPEG|*.jpg|BMP|*.bmp|Other Image Formats|*.*";
                saveFileDialog.FileName = string.Format(_saveFileDialog_FileNameFormat, (uint)nudGeneration.Value, DateTime.Now.ToString("yyyyMMddHHmmss"));
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    switch (saveFileDialog.FilterIndex)
                    {
                        case 1:
                            statisticsUC.TableToBitmap(true, true).SaveImage(saveFileDialog.FileName, ImageFormat.Png);
                            break;
                        case 2:
                            statisticsUC.TableToBitmap(true, true).SaveImage(saveFileDialog.FileName, ImageFormat.Jpeg);
                            break;
                        case 3:
                            statisticsUC.TableToBitmap(true, true).SaveImage(saveFileDialog.FileName, ImageFormat.Bmp);
                            break;
                        default:
                            statisticsUC.TableToBitmap(true, true).SaveImage(saveFileDialog.FileName);
                            break;
                    }
            }
        }
        private void tsmiExportAsText_Click(object sender, EventArgs e)
        {
            lock (_locker1)
            {
                saveFileDialog.DefaultExt = ".txt";
                saveFileDialog.Filter = "Text Files (*.txt)|*.txt";
                saveFileDialog.FileName = string.Format(_saveFileDialog_FileNameFormat, (uint)nudGeneration.Value, DateTime.Now.ToString("yyyyMMddHHmmss"));
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    SaveTextToFile(statisticsUC.TableToText(true), saveFileDialog.FileName);
            }
        }
        private void tsmiExportAsCsv_Click(object sender, EventArgs e)
        {
            lock (_locker1)
            {
                saveFileDialog.DefaultExt = ".csv";
                saveFileDialog.Filter = ".csv Files (*.csv)|*.csv";
                saveFileDialog.FileName = string.Format(_saveFileDialog_FileNameFormat, (uint)nudGeneration.Value, DateTime.Now.ToString("yyyyMMddHHmmss"));
                if (saveFileDialog.ShowDialog() == DialogResult.OK)
                    SaveTextToFile(statisticsUC.TableToCsv(true, true), saveFileDialog.FileName);
            }
        }

        private static void Program_ApplicationModelChanged(object sender, EventArgs e)
        {
            ClearInstance();
            Program.ApplicationModel.UniverseChanged += ApplicationModel_UniverseChanged;
        }
        private static void ApplicationModel_UniverseChanged(object sender, EventArgs e)
        {
            ClearInstance();
            var universe = ((ApplicationModel)sender).Universe;
            if (universe != null)
                universe.SimulationProcessEnded += Universe_SimulationProcessEnded;
        }
        private static void Universe_SimulationProcessEnded(object sender, SimulationEventArgs e)
        {
            lock (_locker2)
            {
                if ((_instance?.IsDisposed ?? true) || !_instance.Visible)
                    return;
                var gen = ((Universe)sender).CurrentGeneration;
                _instance.nudGeneration.ValueChanged -= _instance.nudGeneration_ValueChanged;
                _instance.nudGeneration.Maximum = gen ?? 0;
                _instance.nudGeneration.ValueChanged += _instance.nudGeneration_ValueChanged;
                if (!gen.HasValue)
                    _instance.Close();
            }
        }
    }
}