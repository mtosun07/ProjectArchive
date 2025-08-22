using System;
using System.Windows.Forms;

namespace TSN.Universe.DesktopApp
{
    internal partial class OverwriteDialogForm : Form
    {
        public OverwriteDialogForm()
        {
            _targetFolderName = string.Empty;
            _fileName = string.Empty;
            SelectedOption = OverwritingOptions.NONE;
            InitializeComponent();
            SetText();
        }


        private string _targetFolderName;
        private string _fileName;

        public string TargetFolderName
        {
            get => _targetFolderName;
            set
            {
                _targetFolderName = value?.Trim() ?? string.Empty;
                SetText();
            }
        }
        public string FileName
        {
            get => _fileName;
            set
            {
                _fileName = value?.Trim() ?? string.Empty;
                SetText();
            }
        }
        public uint FileCount { get; set; }
        public OverwritingOptions SelectedOption { get; private set; }



        private void SetText()
        {
            lblCaptionText.Text = $"Exporting{(FileCount == 0 ? string.Empty : (FileCount == 1 ? " a file" : $" {FileCount} files"))}{(_targetFolderName.Equals(string.Empty) ? string.Empty : $" to \"{_targetFolderName}\"")}";
            lblDialogText.Text = $"The destination already has a file {(_fileName.Equals(string.Empty) ? "with the same name" : $"named \"{_fileName}\"")}";
        }

        private void btnYes_Click(object sender, EventArgs e) => SelectedOption = OverwritingOptions.Yes;
        private void btnNo_Click(object sender, EventArgs e) => SelectedOption = OverwritingOptions.No;
        private void btnYesAll_Click(object sender, EventArgs e) => SelectedOption = OverwritingOptions.YesToAll;
        private void btnNoAll_Click(object sender, EventArgs e) => SelectedOption = OverwritingOptions.NoToAll;



        public enum OverwritingOptions : byte
        {
            NONE = 0,
            Yes = 1,
            No = 2,
            YesToAll = 3,
            NoToAll = 4
        }
    }
}