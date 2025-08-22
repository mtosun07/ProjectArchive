using System;
using System.Data;
using System.Drawing;
using System.IO;
using System.Linq;
using System.Windows.Forms;

namespace TSN.Universe.DesktopApp
{
    internal partial class ExportRangeDialogForm : Form
    {
        static ExportRangeDialogForm()
        {
            _invalidFileNameChars = Path.GetInvalidFileNameChars();
            _defaultFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Regular, GraphicsUnit.Point, 162);
            _emptyFont = new Font("Microsoft Sans Serif", 8.25F, FontStyle.Italic, GraphicsUnit.Point, 162);
            _defaultColor = SystemColors.WindowText;
            _emptyColor = SystemColors.GrayText;
        }
        public ExportRangeDialogForm()
        {
            lock (_locker1)
            {
                FileNameWithExtension = false;
                FileName = string.Empty;
                InitializeComponent();
                CurrentModifiedEnabled = false;
                Program.ApplicationModelChanged += Program_ApplicationModelChanged;
                Program.ApplicationModel.UniverseChanged += ApplicationModel_UniverseChanged;
                var universe = Program.ApplicationModel.Universe;
                if (universe != null)
                    universe.Simulated += Universe_SimulationProcessEnded;
                ClearFileName();
            }
        }


        private const string _textboxPlaceHolder = "File names start with...";
        private const string _textboxPlaceHolderExt = _textboxPlaceHolder + " (Extension must be specified)";
        private const string _message_NoFileName = "File name should have been specified.";
        private const string _message_NoExtension = "File extension should have been specified.";

        private static readonly char[] _invalidFileNameChars;
        private static readonly Font _defaultFont;
        private static readonly Font _emptyFont;
        private static readonly Color _defaultColor;
        private static readonly Color _emptyColor;

        private readonly object _locker1 = new object(), _locker2 = new object();

        public bool CurrentModifiedEnabled
        {
            get => radCurrentModified.Enabled;
            set => radCurrentModified.Enabled = value;
        }
        public bool FileNameWithExtension { get; set; }
        public Options SelectedOption { get; private set; }
        public string FileName { get; private set; }



        private bool SetGenerations()
        {
            lock (_locker2)
            {
                var result = radAll.Enabled = radCurrent.Enabled = radRange.Enabled = Program.ApplicationModel.Universe?.CurrentGeneration.HasValue ?? false;
                nudRange1.Maximum = nudRange2.Maximum = result ? Program.ApplicationModel.Universe.CurrentGeneration.Value : 0M;
                return result;
            }
        }

        private void ClearFileName()
        {
            txtFileName.Font = _emptyFont;
            txtFileName.ForeColor = _emptyColor;
            txtFileName.Text = FileNameWithExtension ? _textboxPlaceHolderExt : _textboxPlaceHolder;
        }
        public void GetSelectedRange(out uint min, out uint max)
        {
            uint r1 = (uint)nudRange1.Value, r2 = (uint)nudRange2.Value;
            if (r1 < r2)
            {
                min = r1;
                max = r2;
            }
            else
            {
                min = r2;
                max = r1;
            }
        }

        private void ExportRangeForm_Load(object sender, EventArgs e)
        {
            lock (_locker1)
            {
                if (SetGenerations())
                    SelectedOption = Options.CurrentGeneraion;
                else if (CurrentModifiedEnabled)
                    radCurrentModified.Checked = true;
                else
                    SelectedOption = Options.NONE;
            }
        }
        private void btnExport_Click(object sender, EventArgs e)
        {
            lock (_locker1)
            {
                if (!txtFileName.Enabled)
                    return;
                var fileName = txtFileName.Font.Style == FontStyle.Italic ? string.Empty : txtFileName.Text;
                var msg = fileName.Equals(string.Empty) ? _message_NoFileName : (string.IsNullOrEmpty(Path.GetExtension(fileName)) ? _message_NoExtension : null);
                if (msg != null)
                {
                    MessageBox.Show(msg, "Warning", MessageBoxButtons.OK, MessageBoxIcon.Warning);
                    DialogResult = DialogResult.Cancel;
                }
                FileName = fileName;
            }
        }
        private void btnCancel_Click(object sender, EventArgs e)
        {
            lock (_locker1)
                Close();
        }
        private void txtFileName_Enter(object sender, EventArgs e)
        {
            if (txtFileName.Font.Style == FontStyle.Italic)
            {
                txtFileName.Clear();
                txtFileName.Font = _defaultFont;
                txtFileName.ForeColor = _defaultColor;
            }
        }
        private void txtFileName_Leave(object sender, EventArgs e)
        {
            if ((txtFileName.Text = new string(txtFileName.Text.Trim().Where(x => !_invalidFileNameChars.Contains(x)).ToArray())).Equals(string.Empty))
                ClearFileName();
        }
        private void radAll_CheckedChanged(object sender, EventArgs e)
        {
            if (radAll.Checked)
            {
                txtFileName.Enabled = true;
                SelectedOption = Options.AllGenerations;
            }
            else if (!radRange.Checked)
                ClearFileName();
        }
        private void radCurrent_CheckedChanged(object sender, EventArgs e)
        {
            if (radCurrent.Checked)
            {
                txtFileName.Enabled = false;
                SelectedOption = Options.CurrentGeneraion;
            }
        }
        private void radCurrentModified_CheckedChanged(object sender, EventArgs e)
        {
            if (radCurrentModified.Checked)
            {
                txtFileName.Enabled = false;
                SelectedOption = Options.CurrentModified;
            }
        }
        private void radRange_CheckedChanged(object sender, EventArgs e)
        {
            if (pnlRange.Enabled = radRange.Checked)
            {
                txtFileName.Enabled = true;
                SelectedOption = Options.SelectedRange;
            }
            else if (!radAll.Checked)
                ClearFileName();
            SetGenerations();
            nudRange1.Value = 0M;
            nudRange2.Value = radRange.Checked ? nudRange2.Maximum : 0M;
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
        private void Universe_SimulationProcessEnded(object sender, SimulationEventArgs e) => SetGenerations();



        public enum Options : byte
        {
            NONE = 0,
            AllGenerations = 1,
            CurrentGeneraion = 2,
            CurrentModified = 3,
            SelectedRange = 4
        }
    }
}