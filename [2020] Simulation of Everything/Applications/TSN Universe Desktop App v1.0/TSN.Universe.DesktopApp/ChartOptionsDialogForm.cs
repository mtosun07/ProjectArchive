using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Windows.Forms;

namespace TSN.Universe.DesktopApp
{
    public partial class ChartOptionsDialogForm : Form
    {
        static ChartOptionsDialogForm()
        {
            _titles = Statistics.GetOrderedTitles().ToList().AsReadOnly();
        }
        public ChartOptionsDialogForm()
        {
            lock (_locker1)
            {
                InitializeComponent();
                Program.ApplicationModelChanged += Program_ApplicationModelChanged;
                Program.ApplicationModel.UniverseChanged += ApplicationModel_UniverseChanged;
                var ext = $"*{saveFileDialog.DefaultExt = openFileDialog.DefaultExt = ApplicationModel.FileExtension_Charts}";
                saveFileDialog.Filter = openFileDialog.Filter = $"SOE Binary Files for Chart Options ({ext})|{ext}";
                _dontClose = false;
                Selection = new ChartOptions();
            }
        }


        private static readonly IReadOnlyList<Statistics.StatisticsTitle> _titles;

        private readonly object _locker1 = new object(), _locker2 = new object();
        private bool _dontClose;

        public ChartOptions Selection { get; private set; }



        private bool ValidateSelection()
        {
            lock (_locker2)
            {
                if (cmbChartType.SelectedIndex != 0 && cmbChartType.SelectedIndex != 1)
                {
                    MessageBox.Show("Chart Type should have been selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (cmbMagnitudes.SelectedIndex != 0 && cmbMagnitudes.SelectedIndex != 1 && cmbMagnitudes.SelectedIndex != 2)
                {
                    MessageBox.Show("Magnitude should have been selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                Statistics.StatisticsTitle[] titles;
                try
                {
                    titles = clbTitles.CheckedIndices.Cast<int>().Select(i => _titles[i]).ToArray();
                }
                catch
                {
                    MessageBox.Show("Some of selected titles were unexpected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                if (titles.Length == 0)
                {
                    MessageBox.Show("At least 1 title should have been selected.", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    return false;
                }
                Selection = new ChartOptions((ChartOptions.ChartTypes)cmbChartType.SelectedIndex, (ChartOptions.Magnitudes)cmbMagnitudes.SelectedIndex, titles);
                return true;
            }
        }

        private void ChartsSelectionForm_Load(object sender, EventArgs e)
        {
            lock (_locker1)
            {
                cmbChartType.Items.AddRange(new[] { "Single Generation", "Multi-Generation" });
                if (Program.ApplicationModel.Universe.CurrentGeneration.Value == 0)
                {
                    cmbChartType.Enabled = false;
                    cmbChartType.SelectedIndex = 0;
                }
                else
                    cmbChartType.SelectedIndex = 1;
                cmbMagnitudes.Items.AddRange(new[] { "New Borns", "Current Simulation", "Amongst All Simulations" });
                cmbMagnitudes.SelectedIndex = 1;
                clbTitles.Items.AddRange(_titles.Select(x => $"{x.Group} | \"{x.Title}\" | {x.SubTitle}").ToArray());
                for (int i = 0; i < _titles.Count; i++)
                    clbTitles.SetItemChecked(i, false);
            }
        }
        private void ChartsSelectionDialogForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.Cancel = _dontClose)
                _dontClose = false;
        }
        private void btnShow_Click(object sender, EventArgs e)
        {
            lock (_locker1)
                _dontClose = !ValidateSelection();
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            lock (_locker1)
                _dontClose = false;
        }
        private void btnAll_Click(object sender, EventArgs e)
        {
            lock (_locker1)
            {
                for (int i = 0; i < _titles.Count; i++)
                    clbTitles.SetItemChecked(i, true);
                _dontClose = true;
            }
        }
        private void btnClear_Click(object sender, EventArgs e)
        {
            lock (_locker1)
            {
                for (int i = 0; i < _titles.Count; i++)
                    clbTitles.SetItemChecked(i, false);
                _dontClose = true;
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            lock (_locker1)
            {
                if (ValidateSelection())
                {
                    saveFileDialog.FileName = $"SOE Chart Options {DateTime.Now.ToString("yyyyMMddHHmmss")}";
                    if (saveFileDialog.ShowDialog() == DialogResult.OK)
                        Selection.SaveToFile(saveFileDialog.FileName);
                }
                _dontClose = true;
            }
        }
        private void btnImport_Click(object sender, EventArgs e)
        {
            lock (_locker1)
            {
                if (openFileDialog.ShowDialog() == DialogResult.OK)
                    Utility.TryLoadFromFile<ChartOptions>(openFileDialog.FileName, s =>
                    {
                        cmbChartType.SelectedIndex = (int)s.ChartType;
                        var checkeds = s.GetSelectedTitles();
                        for (int i = 0; i < _titles.Count; i++)
                            clbTitles.SetItemChecked(i, checkeds.Contains(_titles[i]));
                    });
                _dontClose = true;
            }
        }
        private void Program_ApplicationModelChanged(object sender, EventArgs e)
        {
            lock (_locker1)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
        private void ApplicationModel_UniverseChanged(object sender, EventArgs e)
        {
            lock (_locker1)
            {
                DialogResult = DialogResult.Cancel;
                Close();
            }
        }
    }
}