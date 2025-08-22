using System;
using System.Linq;
using System.Windows.Forms;

namespace TSN.Universe.DesktopApp
{
    internal sealed partial class ProgressionOutputForm : Form
    {
        static ProgressionOutputForm()
        {
            _locker1 = new object();
            _locker2 = new object();
            _locker3 = new object();
            _instance = new ProgressionOutputForm();
            _previous = null;
            Program.ApplicationModelChanged += Program_ApplicationModelChanged;
            Program.ApplicationModel.UniverseChanged += ApplicationModel_UniverseChanged;
            Program.ApplicationModel.ConsoleEntryAdded += ApplicationModel_ConsoleEntryAdded;
            if (Program.ApplicationModel.Universe != null)
            {
                Program.ApplicationModel.Universe.Simulating += Universe_Simulating;
                Program.ApplicationModel.Universe.SimulationProcessEnded += Universe_SimulationProcessEnded;
            }
        }
        private ProgressionOutputForm() => InitializeComponent();


        private static readonly object _locker1, _locker2, _locker3;
        private static ProgressionOutputForm _instance;
        private static bool? _previous;



        private static void SetMaximumGeneration(uint? generation)
        {
            Action setGeneration = () =>
            {
                _instance.nudGeneration.ValueChanged -= _instance.RefreshRequested;
                _instance.nudGeneration.Maximum = generation ?? 0;
                _instance.nudGeneration.ValueChanged += _instance.RefreshRequested;
            };
            lock (_locker1)
                if (!_instance.nudGeneration.InvokeRequired)
                    setGeneration.Invoke();
                else
                    _instance.nudGeneration.Invoke(setGeneration);
        }
        private static void RepresentEntries(uint generation, bool previous, bool representAnyway = false)
        {
            Action<string> setText = text =>
            {
                _instance.txtConsole.Clear();
                _instance.txtConsole.Text = text;
                _instance.txtConsole.SelectionStart = text.Length;
                _instance.txtConsole.ScrollToCaret();
            };
            Action setGeneration = () =>
            {
                _instance.nudGeneration.ValueChanged -= _instance.RefreshRequested;
                _instance.nudGeneration.Value = generation;
                _instance.nudGeneration.ValueChanged += _instance.RefreshRequested;
            };
            lock (_locker2)
            {
                if (!representAnyway && !_instance.Visible)
                    return;
                string s, str = !previous ? (Program.ApplicationModel.Entries.TryGetValue(generation, out var list) && !string.IsNullOrEmpty(s = list?.ToString().Trim()) ? s : string.Empty) : string.Join(Environment.NewLine, Program.ApplicationModel.Entries.Keys.Where(x => x <= generation).OrderBy(x => x).Select(x => Program.ApplicationModel.Entries[x]?.ToString().Trim() ?? string.Empty).Where(x => !string.IsNullOrEmpty(x)) ?? new string[0]);
                if (!_instance.txtConsole.InvokeRequired)
                    setText.Invoke(str);
                else
                    _instance.txtConsole.Invoke(setText, str);
                if (!_instance.nudGeneration.InvokeRequired)
                    setGeneration.Invoke();
                else
                    _instance.nudGeneration.Invoke(setGeneration);
            }
        }
        public static void ClearInstance()
        {
            lock (_locker3)
            {
                if (!(_instance?.IsDisposed ?? true))
                {
                    _instance.txtConsole.Clear();
                    _instance.Close();
                }
                _instance = new ProgressionOutputForm();
            }
        }
        public static void ShowInstance(uint generation)
        {
            lock (_locker3)
            {
                if (Program.ApplicationModel.Universe == null)
                    return;
                _instance.nudGeneration.ValueChanged -= _instance.RefreshRequested;
                _instance.chkPrevious.CheckedChanged -= _instance.RefreshRequested;
                _instance.nudGeneration.Maximum = Program.ApplicationModel.Universe.CurrentGeneration.Value;
                _instance.nudGeneration.Value = generation;
                if (_previous.HasValue)
                {
                    _instance.chkPrevious.Checked = _previous.Value;
                    _previous = null;
                }
                _instance.nudGeneration.ValueChanged += _instance.RefreshRequested;
                _instance.chkPrevious.CheckedChanged += _instance.RefreshRequested;
                RepresentEntries(generation, _instance.chkPrevious.Checked, true);
                _instance.Show();
                _instance.Focus();
            }
        }

        protected override void OnFormClosed(FormClosedEventArgs e)
        {
            _previous = chkPrevious.Checked;
            base.OnFormClosed(e);
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
            {
                universe.Simulating += Universe_Simulating;
                universe.SimulationProcessEnded += Universe_SimulationProcessEnded;
            }
        }
        private static void ApplicationModel_ConsoleEntryAdded(object sender, ApplicationModel.ConsoleEntryAddedEventArgs e) { }
        private static void Universe_Simulating(object sender, SimulationEventArgs e) => SetMaximumGeneration(e.Generation);
        private static void Universe_SimulationProcessEnded(object sender, SimulationEventArgs e)
        {
            Func<bool> getChecked = () => _instance.chkPrevious.Checked;
            lock (_locker3)
            {
                if ((_instance?.IsDisposed ?? true) || !_instance.Visible)
                    return;
                SetMaximumGeneration(e.Generation);
                RepresentEntries(e.Generation, _instance.chkPrevious.InvokeRequired ? (bool)_instance.chkPrevious.Invoke(getChecked) : getChecked.Invoke());
            }
        }

        private void ProgressionOutputForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            if (e.CloseReason == CloseReason.UserClosing)
            {
                e.Cancel = true;
                Hide();
            }
        }
        private void RefreshRequested(object sender, EventArgs e) => RepresentEntries((uint)nudGeneration.Value, chkPrevious.Checked);
        private void btnClipboard_Click(object sender, EventArgs e)
        {
            if (string.IsNullOrEmpty(txtConsole.Text))
                return;
            Clipboard.SetText(txtConsole.Text, TextDataFormat.UnicodeText);
            MessageBox.Show("Copied to the clipboard as plain text.", "Information", MessageBoxButtons.OK, MessageBoxIcon.Information);
        }
    }
}